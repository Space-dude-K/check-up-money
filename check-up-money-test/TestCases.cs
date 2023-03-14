using System.Collections.Generic;

namespace check_up_money_test
{
    class TestCases
    {
        /// <summary>
        /// Retrieves data for file processing test cases.
        /// </summary>
        /// <returns>
        /// A tuple containing the following information:
        /// <list type="bullet">
        /// <item><see cref="Tuple{string,string,string,int}.pathType"/>: Setting path type.</item>
        /// <item><see cref="Tuple{string,string,string,int}.fileName"/>: File name.</item>
        /// <item><see cref="Tuple{string,string,string,int}.expectedDir"/>: Expected directory where the file will end up.</item>
        /// <item><see cref="Tuple{string,string,string,int}.foreignCurrencyOverride"/>: Foreign currency. -1 - value from TestSettings, 0 - disabled, 1 - enabled </item>
        /// </list>
        /// </returns>
        public static List<(string pathType, string fileName, string expectedFileType, string expectedDir, int foreignCurrencyOverride)> FileProcessingTestCases = new()
        {
            ("repIn", "ot120319.P02", "Отзыв", @"CbMisc\repPath", -1),
            ("repIn", "tp150319.P01", "Взыскание", @"CbMisc\repPath", -1),
            ("oblIn", "31122019.P09", "Доходно-расходная пачка", @"CbMain\oblPath", -1),
            ("cityIn", "01042020.P02", "Доходная пачка", @"CbMain\cityPath", -1),
            ("cityIn", "ps200005.36", "Предписание о приостановлении", @"CbMisc\cityPath", -1),
            ("regIn", "op200004.38", "Уведомление об отмене предписания", @"CbMisc\regPath", -1),
            ("regIn", "Zval230222.P01", "Закупка валюты", @"CbMiscCurrency\regPath", 1),
            ("uniIn", "pp230223.P02", "Валютная пачка в банк", @"CbMainCurrency\uniPath", 1),
            ("uniBankTicketIn", "CWCJ1CHW.txt", "Квитанция клиент-банка", @"CbTicketOut\uniPath", 0),
            ("extIn", "01082022.P01", "Расходная пачка", @"CbMain\extPath", 0),
            ("oblIn", "pp310322.P01", "Доходная пачка в валюте", @"CbMainCurrency\oblPath", 1)
        };
        /// <summary>
        /// Retrieves data for watcher test cases.
        /// </summary>
        /// <returns>
        /// A tuple containing the following information:
        /// <list type="bullet">
        /// <item><see cref="Tuple{int, int, int, int, int}.watcherIndex"/>: Watcher index.</item>
        /// <item><see cref="Tuple{int, int, int, int, int}.settingIndex"/>: Setting index.</item>
        /// <item><see cref="Tuple{int, int, int, int, int}.col"/>: Grid column.</item>
        /// <item><see cref="Tuple{int, int, int, int, int}.row"/>: Grid row. </item>
        /// <item><see cref="Tuple{int, int, int, int, int}.bufferSize"/>: Buffer size for file watcher, kb </item>
        /// </list>
        /// </returns>
        public static List<(int watcherIndex, int settingIndex, int col, int row, int bufferSize)> FileWatcherTestCases = new()
            {
                (0, 0, 0, 0, 65536)
            };
    }
}