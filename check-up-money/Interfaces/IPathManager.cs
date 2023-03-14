using check_up_money.CheckUpFile;
using System.Collections.Generic;
using System.Windows.Forms;

namespace check_up_money
{
    internal interface IPathManager
    {
        CheckUpBlob.BudgetType GetBudgetTypeFromSetting(string setting);
        (string labelName, string labelTip) GetCounterNameAndTipFromPathSetting(string pathName);
        string GetCounterSuffixForLabelFileType(bool isParentEnabled, bool isChildEnabled);
        List<(string pathType, Control unsentFilesLabel)> GetLabelsFromBudgetForDirectoriesStatusChecker(List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit);
        List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> GetPathsToInit();
        string GetPathTypeFromCheckUpFileTypeAndBudgetType(CheckUpBlob.FileType fileType, CheckUpBlob.BudgetType budgetType);
    }
}