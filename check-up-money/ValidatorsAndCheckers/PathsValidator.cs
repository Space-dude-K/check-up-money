using check_up_money.Cypher;
using check_up_money.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using NLog;

namespace check_up_money.ValidatorsAndCheckers
{
    class PathsValidator : IPathsValidator
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");
        public PathsValidator()
        {
            loggerDebug.Debug("Init.");
        }
        public bool ValidatePathSettingsFromCfgs(List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathSettings)
        {
            List<bool> pathValidationStatuses = new List<bool>();
            bool isAllPathValid = false;

            foreach(var pathSetting in pathSettings)
            {
                loggerDebug.Debug($"Validate path {pathSetting.pathType} - {pathSetting.isCurrencyEnabled}");

                if (pathSetting.isEnabled)
                {
                    pathValidationStatuses.Add(CheckPath(pathSetting.pathType, pathSetting.path));
                }
            }

            if (!pathValidationStatuses.Any(p => p == false))
            {
                isAllPathValid = true;
            }
            else
            {
                loggerError.Error($"Invalid paths. Check paths settings in MainSettings.config.");
            }

            return isAllPathValid;
        }
        private bool CheckPath(string pathName, string path)
        {
            bool isPathValid = false;

            if (string.IsNullOrEmpty(path))
            {
                loggerError.Error($"{pathName} {path} is null or empty!");
            }
            else if (!Directory.Exists(path))
            {
                loggerError.Error($"{pathName} {path} didnt exist!");
            }
            else if (!IsDirectoryWritable(path))
            {
                loggerError.Error($"{pathName} {path} write access error, check privileges!");
            }
            else
            {
                loggerDebug.Debug($"Path {path} is OK.");
                isPathValid = true;
            }

            return isPathValid;
        }
        public bool ValidatePathSettingsFromControlsOld(System.Windows.Forms.Control.ControlCollection controls, List<Tuple<string, bool>> budgetSettings)
        {
            int controlCounter = 0;
            bool isAllPathsValid = false;

            List<Task> tasks = new List<Task>();

            foreach (Control x in controls)
            {
                if (x.Enabled && x is TextBox && x.Name.Contains("PS"))
                {
                    bool currentPathIsValid = false;
                    string textBoxStr = x.Text;

                    /*
                    // Skip bank path and unchecked budgets
                    if (controlCounter != 0 && !budgetSettings[controlCounter].Item2)
                    {
                        logger.Info($"{textBoxStr} skipped I");
                        continue;
                    }
                    */
                    // Skip first 2 bank indexes and unchecked budgets
                    if(controlCounter > 1 && !budgetSettings[controlCounter].Item2)
                    {
                        logger.Info($"{textBoxStr} has {budgetSettings[controlCounter].Item2} status.");
                        continue;
                    }
                        
                    if (string.IsNullOrEmpty(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Empty inputs!", 400, 8));

                        x.BackColor = Color.Red;
                        x.MouseEnter += eh;
                        x.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} is null or empty!");
                    }
                    else if (!Directory.Exists(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Directory didnt exists!", 400, 8));

                        x.BackColor = Color.Red;
                        x.MouseEnter += eh;
                        x.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} directory didnt exists!");
                    }
                    else if (!IsDirectoryWritable(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Access error!", 400, 8));

                        x.BackColor = Color.Red;
                        x.MouseEnter += eh;
                        x.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} Access error!");
                    }
                    else
                    {
                        currentPathIsValid = true;
                    }

                    if (isAllPathsValid == true && currentPathIsValid == false)
                    {
                        isAllPathsValid = false;
                    }
                    else if (isAllPathsValid == false && currentPathIsValid == true)
                    {
                        isAllPathsValid = true;
                    }

                    controlCounter++;
                }
            }

            if (isAllPathsValid)
                logger.Info($"Path status -> VALID.");

            return isAllPathsValid;
        }
        public bool ValidatePathSettingsFromControls(List<(string pathType, string path)> pathSettings, Control.ControlCollection controls)
        {
            bool isSettingsValid = true;

            foreach(Control control in controls)
            {
                //loggerDebug.Debug($"Check control -> {control.Name} {control.Enabled} {control.GetType()}");

                // Control must be enabled, TextBox, marked as PS control.
                if (control.Enabled && control is TextBox && control.Name.Contains("PS"))
                {
                    string textBoxStr = control.Text;

                    loggerDebug.Debug($"Check path -> {textBoxStr}");

                    if (string.IsNullOrEmpty(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Empty inputs!", 400, 8));

                        control.BackColor = Color.Red;
                        control.MouseEnter += eh;
                        control.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} is null or empty!");

                        isSettingsValid = false;
                    }
                    else if (!Directory.Exists(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Directory didnt exists!", 400, 8));

                        control.BackColor = Color.Red;
                        control.MouseEnter += eh;
                        control.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} directory didnt exists!");

                        isSettingsValid = false;
                    }
                    else if (!IsDirectoryWritable(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Access error!", 400, 8));

                        control.BackColor = Color.Red;
                        control.MouseEnter += eh;
                        control.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} Access error!");

                        isSettingsValid = false;
                    }
                }
            }

            return isSettingsValid;
        }
        public bool ValidateBudgetSettingsFromControls(System.Windows.Forms.Control.ControlCollection controls)
        {
            bool isAllBudgetInputValid = false;

            foreach (Control x in controls)
            {
                if (x.Enabled && x is TextBox && x.Name.Contains("textBox_FBS"))
                {
                    bool currentPathIsValid = false;
                    string textBoxStr = x.Text;

                    if (string.IsNullOrWhiteSpace(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Empty inputs!", 100, 8));

                        x.BackColor = Color.Red;
                        x.MouseEnter += eh;
                        x.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));
                        x.EnabledChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} is null or empty!");
                    }
                    else
                    {
                        currentPathIsValid = true;
                    }

                    if (isAllBudgetInputValid == true && currentPathIsValid == false)
                    {
                        isAllBudgetInputValid = false;
                    }
                    else if (isAllBudgetInputValid == false && currentPathIsValid == true)
                    {
                        isAllBudgetInputValid = true;
                    }
                }
            }

            if (isAllBudgetInputValid)
                loggerError.Error($"Budget inputs is valid");

            return isAllBudgetInputValid;
        }
        public bool IsDbSettingsInputsFromControlsValid(System.Windows.Forms.Control.ControlCollection controls, List<RequisiteInformation> riList, bool isUpdateCheck = false)
        {
            bool isErrorRaised = false;

            foreach (Control x in controls)
            {
                if (x.Enabled && x is TextBox && x.Name.Contains("textBox_DBS"))
                {
                    string textBoxStr = x.Text;

                    if (string.IsNullOrWhiteSpace(textBoxStr))
                    {
                        EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Empty inputs!", 100, 8));

                        x.BackColor = Color.Red;
                        x.MouseEnter += eh;
                        x.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));
                        x.EnabledChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                        loggerError.Error($"{textBoxStr} is null or empty!");

                        isErrorRaised = true;
                    }
                    else
                    {
                        /*
                        // Check if host, bd setting alrdy exists
                        if (!isUpdateCheck && x.Name == "textBox_DBS_ServName" && riList.Where(r => r.Host.Equals(textBoxStr)).Any() 
                            && riList.Where(r => r.Db.Equals(textBoxStr)).Any())
                        {
                            EventHandler eh = new EventHandler((sender, e) => ShowToolTip((Control)sender, "Host alrdy exist!", 100, 8));

                            x.BackColor = Color.Red;
                            x.MouseEnter += eh;
                            x.TextChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));
                            x.EnabledChanged += new EventHandler((sender, e) => ResetToolTip((Control)sender, eh));

                            loggerError.Error($"{textBoxStr} host alrdy exist!");

                            isErrorRaised = true;
                        }
                        */
                    } 
                }
            }

            if (!isErrorRaised)
                logger.Info($"Db inputs is valid");

            return isErrorRaised ? false : true;
        }
        private void ShowToolTip(Control control, string msg, int pointOffSetX, int pointOffSetY)
        {
            MethodInvoker methodInvokerDelegate = delegate()
            {
                Point point = new Point(control.PointToScreen(Point.Empty).X + pointOffSetX, control.PointToScreen(Point.Empty).Y - pointOffSetY);

                Help.ShowPopup(control, msg, point);

                Debug.WriteLine("Help pp at: " + point.X + " " + point.Y);
            };

            control.Invoke(methodInvokerDelegate);
        }
        private void ResetToolTip(Control control, EventHandler eh)
        {
            control.MouseEnter -= eh;
            control.BackColor = Color.White;
        }
        private bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
        {
            try
            {
                using (FileStream fs = File.Create(Path.Combine(dirPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".test"), 1, FileOptions.DeleteOnClose))
                { }

                return true;
            }
            catch
            {
                if (throwIfFails)
                {
                    loggerError.Error($"Write access exception for: {dirPath}");
                    throw new UnauthorizedAccessException($"Write access exception for: {dirPath}");
                }
                else
                {
                    return false;
                }
            }
        }
    }
}