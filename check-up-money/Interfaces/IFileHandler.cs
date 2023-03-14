using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using static check_up_money.CheckUpFile.CheckUpBlob;

namespace check_up_money.Interfaces
{
    public interface IFileHandler
    {
        void Init(IEnumerable<FileInfo> files, BudgetType budgetType);
        void UpdateFileName(string oldFileFullPath, string newFileFullPath, string newName);
        void UpdateFile(string fileFullPath, BudgetType budgetType);
        void CheckUpBlob_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
        void RemoveColumnFromDataGridView_Main(int cbItemIndex);
        void OpenFileFolder(int rowIndex);
        Task<bool> ProccessTheFileNew(int fileIndex, int foreignCurrencyOverride = 0);
    }
}