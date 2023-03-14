using System.Globalization;
using static check_up_money.CheckUpFile.CheckUpBlob;

namespace check_up_money.CheckUpFile
{
    class CheckUpDataStruct
    {
        private decimal summ;
        public decimal Summ
        {
            get { return summ; }
        }
        public FileType dataStructType;
        public FileType DataStructType
        {
            get { return dataStructType; }
            set { dataStructType = value; }
        }
        /// <summary>
        /// Разбор строки на правильную с нужным разделителем дробной части. Результат присваивается в Summ.
        /// </summary>
        /// <param name="rawSummStr">Входная "сырая" строка с суммой.</param>
        /// <param name="symbolsToSkip">Смещение от начала строки.</param>
        /// <param name="decimalSeparator">Разделитель дробной части.</param>
        public void SetRealSumm(string rawSummStr, int symbolsToSkip, string decimalSeparator)
        {
              summ = decimal.Parse(rawSummStr.Substring(symbolsToSkip, rawSummStr.Length - symbolsToSkip), 
                new NumberFormatInfo() { NumberDecimalSeparator = decimalSeparator });
        }
    }
}