using check_up_money.CheckUpFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money.Interfaces
{
    public interface IMain
    {
        List<Tuple<string, bool>> CheckedBudgets { get; }
        List<string> AllowedFileExtensions { get; set; }
        string AboutString { get; }
        List<(string pathType, string path)> PathSettings
        {
            get;
            set;
        }
        BindingList<CheckUpBlob> CheckUpBlobsBindingList { get; set; }
        List<(string budgetType, bool isEnabled)> BudgetSettings { get; set; }
        List<(string ticketType, bool isEnabled)> TicketSettings { get; set; }
        Task<bool> Add(string fullFilePath, CheckUpBlob.BudgetType budgetType, bool isInitAdd = false);
        [Obsolete]
        void Init();
        Task<bool> InitDir(string dir, CheckUpBlob.BudgetType budgetType, string fileMask);
        void InitUnsentFileChecker();
        void InitWatcher(int watcherIndex, int settingIndex, int col, int row, List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> settings, 
            IFileHandler fh, int bufferSize);
        void InitWatchers();
        void NotifyIconForMainStatus_BalloonTipClicked(object sender, EventArgs e);
        void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e);
        void Remove(string fullFilePath);
        void Start();
    }
}