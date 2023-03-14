using check_up_money.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static check_up_money.CheckUpFile.CheckUpBlob;

namespace check_up_money.CheckUpFile
{
    class FileParser : IFileParser
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");

        private readonly IFileLockHelper flh;
        readonly List<string> allowedFileExtensions;
        private readonly IStreamManager streamManager;
        public FileParser(IFileLockHelper flh, List<string> allowedFileExtensions, IStreamManager streamManager)
        {
            loggerDebug.Debug($"Init");

            this.flh = flh;
            this.allowedFileExtensions = allowedFileExtensions;
            this.streamManager = streamManager;
        }
        /// <summary>
        /// Основной парсер. Производит разбор файла с директории и возвращает готовый CheckUpBlob.
        /// </summary>
        /// <param name="directory">Директория-источник.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="budgetType">Тип бюджета.</param>
        /// <returns>
        /// Готовый <see cref="CheckUpBlob"/>
        /// </returns>
        public async Task<CheckUpBlob> ParseFile(string directory, string fileName, BudgetType budgetType)
        {
            CheckUpBlob cb = new CheckUpBlob();
            string filePath = Path.Combine(directory, fileName);

            try
            {
                logger.Info($"Trying to parse next file -> {filePath}");

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (FileStream sourceStream = await streamManager.WaitForFileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(sourceStream, Encoding.GetEncoding(866)))
                {
                    string s = String.Empty;

                    CheckUpDataStruct cds = null;

                    int dataStructCounter = 0;

                    cb.CheckUpFileType = FileType.Unknown;
                    cb.CurrencyType = "BYN";
                    cb.FilePath = filePath;
                    cb.FileName = fileName;
                    cb.CheckUpBudgetType = budgetType;
                    cb.FileDate = File.GetCreationTime(filePath).ToString();

                    var fileFi = new FileInfo(filePath);

                    cb.FileExtension = fileFi.Extension.ToUpper();
                    cb.FileSize = fileFi.Length;

                    logger.Info($"File {cb.FileName} was loaded {cb.FileSizeFormatted}, ext: {cb.FileExtension}");

                    bool isForeignCurrency = false;
                    var fileNameSuffix = new string(cb.FileName.Take(2).Select(c => char.ToLowerInvariant(c)).ToArray());

                    if(fileNameSuffix.Equals("pp"))
                    {
                        isForeignCurrency = true;
                        //fileNameSuffix = new string(cb.FileName.Substring(2).Take(2).ToArray());
                    }

                    logger.Info($"File suffix: {fileNameSuffix}");

                    // Switch on first 2 symbols.
                    switch (fileNameSuffix)
                    {
                        // Взыскание.
                        case "tp" when char.ToUpperInvariant(cb.FileExtension.Take(2).ElementAt(1)) == 'P':
                            cb.CheckUpFileType = FileType.Penalty;
                            break;
                        // Отзыв.
                        case "ot" when char.ToUpperInvariant(cb.FileExtension.Take(2).ElementAt(1)) == 'P':
                            cb.CheckUpFileType = FileType.Withdrawal;
                            break;
                        // Квитанция к-б.
                        case "cw" when cb.FileExtension == ".TXT":
                            cb.CheckUpFileType = FileType.CbTicket;

                            while ((s = sr.ReadLine()) != null)
                            {
                                if (s.StartsWith(":21:"))
                                {
                                    var rawType = s.Substring(4, 1);

                                    cb.CheckUpBudgetType = cb.GetBudgetTypeForCbTicket(rawType);
                                    logger.Info($"C-b ticket {rawType} -> {cb.CheckUpBudgetType}");
                                    break;
                                }
                            }

                            break;
                        // Закупка валюты.
                        case "zv":
                            cb.CheckUpFileType = FileType.CurrencyExchange;

                            while ((s = sr.ReadLine()) != null)
                            {
                                if(s.StartsWith("<803>"))
                                {
                                    var currencyTypeRaw = s.Substring(5, s.Length - 5);
                                    cb.CurrencyType = currencyTypeRaw;
                                }
                            }

                            break;
                        // Предписание, уведомление, default пачка.
                        default:
                            while ((s = sr.ReadLine()) != null)
                            {
                                if (s.StartsWith(":23E:"))
                                {
                                    cb.RawCheckUpFileType(s.Substring(5, 4), isForeignCurrency);
                                    break;
                                }

                                if(isForeignCurrency)
                                {
                                    // Type.
                                    if (s.StartsWith("12"))
                                    {
                                        cb.RawCheckUpFileType(s.Substring(2, s.Length - 2), isForeignCurrency);
                                        cds = new CheckUpDataStruct();
                                        cds.DataStructType = cb.GetBlobType(s.Substring(2, s.Length - 2), isForeignCurrency);
                                    }

                                    if (s.StartsWith("51"))
                                    {
                                        cb.CurrencyType = s.Substring(2, s.Length - 2);
                                    }

                                    // RawSumm.
                                    if (s.StartsWith("52"))
                                    {
                                        cds.SetRealSumm(s, 2, ",");
                                    }
                                }
                                else
                                {
                                    // Type.
                                    if (s.StartsWith("01"))
                                    {
                                        cb.RawCheckUpFileType(s.Substring(2, s.Length - 2), isForeignCurrency);
                                        cds = new CheckUpDataStruct();
                                        cds.DataStructType = cb.GetBlobType(s.Substring(2, s.Length - 2), isForeignCurrency);
                                    }

                                    // RawSumm.
                                    if (s.StartsWith("0A"))
                                    {
                                        cds.SetRealSumm(s, 2, ".");
                                    }
                                }

                                // End of struct.
                                if (s.Equals("~"))
                                {
                                    cb.Data.Add(cds);
                                    dataStructCounter++;
                                }
                            }

                            // Check for mixed type structs.
                            if ((cb.CheckUpFileType == FileType.Inc || cb.CheckUpFileType == FileType.Out) && cb.Data.Count > 1)
                            {
                                var dC = cb.Data
                                    .Select(d => d.DataStructType)
                                    .Distinct();
                                var b = dC
                                    .Count() > 1;

                                logger.Info($"Is file mixed? {b}.");

                                if(b)
                                    cb.CheckUpFileType = FileType.IncOut;
                            }

                            break;
                    }

                    if (dataStructCounter == 0)
                    {
                        logger.Info($"Data struct is empty for {cb.FileName}");
                    }

                    logger.Info($"Blob type: {cb.CheckUpFileType} Total parsed structs: {dataStructCounter} total sum: {cb.TotalSumm}");
                }
            }
            catch (IOException)
            {
                // Trying to get the processes causing the blocking.
                foreach (var proccess in flh.FindLockerProcesses(filePath))
                {
                    loggerError.Error($"File was blocked by: {proccess.strAppName} {proccess.strServiceShortName}");
                }

                throw new Exception("Ошибка доступа к файлу. Файл заблокирован сторонним процессом. Обратитесь к администратору.");
            }
            catch (Exception ex)
            {
                loggerError.Error($"File parse error for {filePath} {ex.Message}.");
                throw new Exception("Ошибка разбора файла. Обратитесь к администратору.");
            }

            return cb;
        }
        private CheckUpDataStruct SetBlobType(string lineStr, string startPrefix, CheckUpBlob cb, CheckUpDataStruct cds, bool isForeignCurrency)
        {
            if (lineStr.StartsWith(startPrefix))
            {
                cb.RawCheckUpFileType(lineStr.Substring(1, lineStr.Length - 1), isForeignCurrency);
                cds = new CheckUpDataStruct();
                cds.DataStructType = cb.GetBlobType(lineStr.Substring(1, lineStr.Length - 1), isForeignCurrency);
            }

            return cds;
        }
        private decimal GetSumm(string lineStr, string startSuffix)
        {
            if (lineStr.StartsWith(startSuffix))
            {
                return GetRealSumm(lineStr, 1, ".");
            }
            else
            {
                return decimal.Zero;
            }
        }
        private decimal GetRealSumm(string rawSummStr, int symbolsToSkip, string decimalSeparator)
        {
            // var x = decimal.Parse("18,285", new NumberFormatInfo() { NumberDecimalSeparator = "," });
            return decimal.Parse(rawSummStr.Substring(symbolsToSkip, rawSummStr.Length - symbolsToSkip),
                new NumberFormatInfo() { NumberDecimalSeparator = decimalSeparator });
        }
        private void SetCurrencyType(string lineStr, string startSuffix, CheckUpBlob cb, CheckUpDataStruct cds, bool isForeignCurrency)
        {
            if(isForeignCurrency && lineStr.StartsWith(startSuffix))
            {
                cb.CurrencyType = lineStr.Substring(startSuffix.Length);
            }
            else
            {
                cb.CurrencyType = "BYN";
            }
        }
    }
}