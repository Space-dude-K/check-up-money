using check_up_money.Cypher;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace check_up_money.Interfaces
{
    public interface IPathsValidator
    {
        bool ValidatePathSettingsFromCfgs(List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathSettings);
        bool ValidatePathSettingsFromControls(List<(string pathType, string path)> pathSettings, Control.ControlCollection controls);
        bool ValidateBudgetSettingsFromControls(System.Windows.Forms.Control.ControlCollection controls);
        bool IsDbSettingsInputsFromControlsValid(System.Windows.Forms.Control.ControlCollection controls, List<RequisiteInformation> riList, bool isUpdateCheck = false);
    }
}