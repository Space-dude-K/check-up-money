using check_up_money.CheckUpFile;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace check_up_money.Interfaces
{
    interface INotificationManager
    {
        BindingList<CheckUpBlob> NotificationData { get; set; }
        void DisableTrayNotificationForMainState();
        Task RunNotificationReminderForFiles(CancellationTokenSource source,
            BindingList<CheckUpBlob> notificationData = null);
        void ShowTrayNotificationForMinimizedState();
        void ShowTrayNotificationForProcessingFile(string fileName);
        void ShowTrayNotificationForProcessingFiles(BindingList<CheckUpBlob> notificationData);
    }
}