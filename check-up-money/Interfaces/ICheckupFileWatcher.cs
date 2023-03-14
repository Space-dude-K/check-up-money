using System.Threading.Tasks;
using System.Windows.Forms;
using static check_up_money.CheckUpFile.CheckUpBlob;

namespace check_up_money.Interfaces
{
    interface ICheckupFileWatcher
    {
        int CurrentFilesInFolder { get; }
        string CurrentFolder { get; set; }

        Task<bool> RunWatcher((string pathName, string labelTip) labelNameAndTip, string dirToWatch, BudgetType budgetType, Control tableLayoutControl, int bufferSize, string fileMask);
    }
}