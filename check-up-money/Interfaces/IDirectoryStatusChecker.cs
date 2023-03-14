using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money.Interfaces
{
    interface IDirectoryStatusChecker
    {
        void CheckDirectories(List<(string pathType, Control label)> directoriesAndLabels);
        Task RunDirectoryStatusChecker(CancellationTokenSource source,
            List<(string pathType, Control label)> directoriesAndLabels, 
            int checkTimerInSeconds);
        List<(string pathType, int filesInDir)> GetFileCounterForOutFolders();
    }
}