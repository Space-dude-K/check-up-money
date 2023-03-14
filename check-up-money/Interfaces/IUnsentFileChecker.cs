using check_up_money.Cypher;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace check_up_money.Interfaces
{
    interface IUnsentFileChecker
    {
        AsyncObservableCollection<int> UnsentFilesCounters { get; }

        List<(string budgetType, RequisiteInformation ri, Control counterLabel, bool isEnabled)> GetDataSetForUnsentfileChecker(List<(string budgetType, bool isEnabled)> unsentFileSettings, List<RequisiteInformation> databaseInfos, List<(string budgetType, Control unsentFilesLabel)> unsentFilesControls);
        void InitUnsentFileChecker(List<(string budgetType, bool isEnabled)> unsentFileSettings, List<(string budgetType, Control unsentFilesLabel)> unsentFilesControls,
            List<RequisiteInformation> databaseInfos, int delayInSeconds, CancellationTokenSource ctSource);
    }
}