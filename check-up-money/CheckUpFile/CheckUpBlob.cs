using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using check_up_money.Extensions;

namespace check_up_money.CheckUpFile
{
    public class CheckUpBlob : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public CheckUpBlob()
        {
            data = new List<CheckUpDataStruct>();
        }
        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        public enum FileType
        {
            [Description("Неизвестный тип")]
            Unknown,
            [Description("Предписание о приостановлении")]
            Stop,
            [Description("Уведомление об отмене предписания")]
            Star,
            [Description("Взыскание")]
            Penalty,
            [Description("Отзыв")]
            Withdrawal,
            [Description("Доходная пачка")]
            Inc,
            [Description("Расходная пачка")]
            Out,
            [Description("Доходно-расходная пачка")]
            IncOut,
            [Description("Квитанция клиент-банка")]
            CbTicket,
            [Description("Валютная пачка в банк")]
            OutForeign,
            [Description("Закупка валюты")]
            CurrencyExchange,
            [Description("Доходная пачка в валюте")]
            IncForeign
        };
        public enum BudgetType
        {
            [Description("Республиканский")]
            Rep,
            [Description("Областной")]
            Obl,
            [Description("Городской")]
            City,
            [Description("Районный")]
            Reg,
            [Description("Союзный")]
            Union,
            [Description("Внебюджет")]
            Extra,
            [Description("Неопределённый")]
            Undetermined
        };
        private string filePath;
        [Browsable(false)]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; NotifyPropertyChanged("FilePath"); }
        }

        private string fileName;
        [DisplayName("Имя")]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; NotifyPropertyChanged("FileName"); }
        }

        private string fileExtension;
        [Browsable(false)]
        public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; NotifyPropertyChanged("FileExtension"); }
        }

        private string fileDate;
        [DisplayName("Дата файла")]
        public string FileDate
        {
            get { return fileDate; }
            set { fileDate = value; NotifyPropertyChanged("FileDate"); }
        }

        private long fileSize;
        [Browsable(false)]
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; NotifyPropertyChanged("FileSize"); }
        }
        [DisplayName("Размер")]
        public string FileSizeFormatted
        {
            get { return fileSize.FormatSizeBytes(); }
        }
        public void RawCheckUpFileType(string rawStr, bool isForeignCurrency)
        {
            checkUpFileType = GetBlobType(rawStr, isForeignCurrency);
            NotifyPropertyChanged("RawCheckUpFileType");
        }
        private FileType checkUpFileType;
        [Browsable(false)]
        public FileType CheckUpFileType
        {
            get { return checkUpFileType; }
            set { checkUpFileType = value; NotifyPropertyChanged("CheckUpFileType"); }
        }
        private string currencyType;
        [DisplayName("Тип валюты")]
        public string CurrencyType
        {
            get
            {
                return currencyType;
            }

            set
            {
                currencyType = value;
            }
        }
        private string checkUpFileTypeFromDb;
        public string CheckUpFileTypeFromDb
        {
            get { return checkUpFileTypeFromDb; }
            set { checkUpFileTypeFromDb = value; }
        }
        [DisplayName("Тип файла")]
        public string CheckUpFileTypeString
        {
            get { return checkUpFileType.GetEnumDescription(); }
        }
        private BudgetType budgetType;
        [Browsable(false)]
        public BudgetType CheckUpBudgetType
        {
            get { return budgetType; }
            set { budgetType = value; NotifyPropertyChanged("CheckUpBudgetType"); }
        }
        [DisplayName("Бюджет")]
        public string CheckUpBudgetTypeString
        {
            get { return budgetType.GetEnumDescription(); }
        }
        [DisplayName("Кол-во док-ов")]
        public int TotalDocsInCheckUpDataStruct
        {
            get
            {
                return Data.Count;
            }
        }
        [DisplayName("Сумма")]
        public decimal TotalSumm
        {
            get { return data.Sum(d => d.Summ); }
        }
        private List<CheckUpDataStruct> data;
        [Browsable(false)]
        internal List<CheckUpDataStruct> Data
        {
            get { return data; }
            set { data = value; NotifyPropertyChanged("Data"); }
        }
        /// <summary>
        /// Получаем FileType из входящей строки.
        /// </summary>
        /// <returns>
        /// <see cref="FileType"/>
        /// </returns>
        internal FileType GetBlobType(string rawStr, bool isForeignCurrency)
        {
            FileType ft;

            switch (rawStr)
            {
                case "STAR":
                    ft = FileType.Star;
                    break;
                case "STOP":
                    ft = FileType.Stop;
                    break;
                default:
                    int number;
                    bool success = int.TryParse(rawStr, out number);
                    ft = success ? GetFileType(number, isForeignCurrency) : FileType.Unknown;
                    break;
            }

            return ft;
        }
        internal FileType GetFileType(int number, bool isForeign)
        {
            if(number < 800000 && isForeign)
            {
                return FileType.OutForeign;
            }
            else if(number < 800000 && !isForeign)
            {
                return FileType.Out;
            }
            else if(number > 800000 && isForeign)
            {
                return FileType.IncForeign;
            }
            else if(number > 800000 && !isForeign)
            {
                return FileType.Inc;
            }
            else
            {
                return FileType.Unknown;
            }
        }
        internal BudgetType GetBudgetTypeForCbTicket(string rawStr)
        {
            BudgetType bt;

            switch (rawStr)
            {
                case "1":
                    bt = BudgetType.Rep;
                    break;
                case "2":
                    bt = BudgetType.Obl;
                    break;
                case "3":
                    bt = BudgetType.Reg;
                    break;
                case "4":
                    bt = BudgetType.City;
                    break;
                case "5":
                    bt = BudgetType.Union;
                    break;
                case "6":
                    bt = BudgetType.Reg;
                    break;
                case "7":
                    bt = BudgetType.Extra;
                    break;
                default:
                    bt = BudgetType.Undetermined;
                    break;
            }

            return bt;
        }
    }
}