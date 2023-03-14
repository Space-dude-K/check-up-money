using System.ComponentModel;

namespace check_up_money.CheckUpFile
{
    public class CheckUpBlobHistory
    {
        private string fileName;
        private string fileDate;
        private string fileType;
        private string budgetType;
        private string totalDocs;
        private string totalSumm;
        private string currencyType;
        [DisplayName("Имя файла")]
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }
        [DisplayName("Дата файла")]
        public string FileDate
        {
            get
            {
                return fileDate;
            }

            set
            {
                fileDate = value;
            }
        }
        [DisplayName("Тип файла")]
        public string FileType
        {
            get
            {
                return fileType;
            }

            set
            {
                fileType = value;
            }
        }
        [DisplayName("Бюджет")]
        public string BudgetType
        {
            get
            {
                return budgetType;
            }

            set
            {
                budgetType = value;
            }
        }
        [DisplayName("Кол-во док-ов")]
        public string TotalDocs
        {
            get
            {
                return totalDocs;
            }

            set
            {
                totalDocs = value;
            }
        }
        [DisplayName("Сумма")]
        public string TotalSumm
        {
            get
            {
                return totalSumm;
            }

            set
            {
                totalSumm = value;
            }
        }
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
    }
}