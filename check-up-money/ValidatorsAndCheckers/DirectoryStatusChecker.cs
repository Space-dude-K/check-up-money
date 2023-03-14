using check_up_money.Extensions;
using check_up_money.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money.ValidatorsAndCheckers
{
    class DirectoryStatusChecker : IDirectoryStatusChecker
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private readonly List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit;
        private readonly IMain main;
        private readonly ToolTip toolTipForLabels;
        private readonly IPathManager pMng;

        public DirectoryStatusChecker(IMain main, ToolTip toolTipForLabels, IPathManager pMng,
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit)
        {
            this.main = main;
            this.toolTipForLabels = toolTipForLabels;
            this.pMng = pMng;
            this.pathsToInit = pathsToInit;
        }
        public async Task RunDirectoryStatusChecker(CancellationTokenSource source, 
            List<(string pathType, Control label)> directoriesAndLabels,
            int checkTimerInSeconds)
        {
            logger.Info($"Starting directory status checker. Repeat time: {checkTimerInSeconds}");

            try
            {
                foreach(var dirAndLabels in directoriesAndLabels)
                {
                    // Set tooltip for label.
                    //SetToolTipForLabel(dirAndLabels.label, dirAndLabels.toolTipForLabel);
                    dirAndLabels.label.ThreadSafeInvokeForLabelToolTip(toolTipForLabels, GetToolTipTextFromPathType());
                }

                while (!source.Token.IsCancellationRequested)
                {
                    CheckDirectories(directoriesAndLabels);

                    // Wait for timer.
                    await Task.Delay(new TimeSpan(0, 0, checkTimerInSeconds));
                }
            }
            catch (OperationCanceledException)
            {
                logger.Info($"Directory status checker was stopped by cancelation request.");
            }
            catch (Exception ex)
            {
                loggerError.Error($"{ex.Message}");
            }
        }
        private string GetToolTipTextFromPathType()
        {
            string toolTipText = string.Empty;

            return toolTipText;
        }
        public void CheckDirectories(
            List<(string pathType, Control label)> directoriesAndLabels)
        {
            loggerDebug.Debug($"Total dir to init: {directoriesAndLabels.Count}");

            foreach(var dirAndLabel in directoriesAndLabels)
            {
                var data = GetFileCounterStringAndColorFromPathType(dirAndLabel.pathType);

                dirAndLabel.label.ThreadSafeInvokeForTextAssignment(data.textForLabel);
                dirAndLabel.label.ThreadSafeInvokeForLabelToolTip(toolTipForLabels, data.textForTip);
                dirAndLabel.label.ThreadSafeInvokeForColor(data.colorForLabel);
            }
        }
        // TODO. Рефакторинг.
        /// <summary>
        /// Этот метод получает текст, подсказку, цвет для информационной <see cref="Label"/> по типу пути.
        /// </summary>
        /// <param name="pathType">Тип пути.</param>
        /// <returns>
        /// <see cref="Tuple{string,string,Color}"/> с текстом, подсказкой и цветом для <see cref="Label"/>.
        /// </returns>
        private (string textForLabel, string textForTip, Color colorForLabel) GetFileCounterStringAndColorFromPathType(string pathType)
        {
            string fileCounterString = string.Empty;
            string textForTip = string.Empty;
            Color colorForLabel = Color.Red;

            (string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled) parent = 
                pathsToInit.Single(p => p.pathType.Equals(pathType));
            (string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled) child = 
                pathsToInit.SingleOrDefault(p => p.pathType.Equals(pathType + "Currency"));
            (string counterText, Color counterColor) checkedParent;
            (string counterText, Color counterColor) checkedChild;
            (string labelName, string labelTip) counterTextAndTip = pMng.GetCounterNameAndTipFromPathSetting(parent.pathType);

            if(!parent.isEnabled && child == default || child != default && !parent.isEnabled && !child.isEnabled)
            {
                fileCounterString = "-";
                colorForLabel = Color.Black;
            }
            else if(child != default && parent.isCurrencyEnabled)
            {
                checkedParent = CheckDir(parent.path, parent.fileMask, parent.isEnabled);
                checkedChild = CheckDir(child.path, child.fileMask, child.isEnabled);

                fileCounterString = counterTextAndTip.labelName + (checkedParent.counterText + checkedChild.counterText);
                textForTip = counterTextAndTip.labelTip + pMng.GetCounterSuffixForLabelFileType(parent.isEnabled, child.isEnabled);

                colorForLabel = Color.Black;
            }
            else
            {
                checkedParent = CheckDir(parent.path, parent.fileMask, parent.isEnabled);
                fileCounterString = counterTextAndTip.labelName + checkedParent.counterText;
                textForTip = pMng.GetCounterNameAndTipFromPathSetting(parent.pathType).labelTip + pMng.GetCounterSuffixForLabelFileType(parent.isEnabled, false);
                colorForLabel = checkedParent.counterColor;
            }

            return (fileCounterString, textForTip, colorForLabel);
        }
        private (string counterText, Color counterColor) CheckDir(string directory, string fileMask, bool isEnabled)
        {
            string counterText = string.Empty;
            Color counterColor = Color.Red;

            if(isEnabled)
            {
                if (!Directory.Exists(directory))
                {
                    counterText = "[ERR]";
                }
                else
                {
                    counterColor = Color.Black;
                    counterText = "[" + GetFilesCountInDir(directory, fileMask) + "]";
                }
            }
            else
            {
                counterColor = Color.Black;
                counterText = "-";
            }

            return (counterText, counterColor);
        }
        private int GetFilesCountInDir(string directory, string fileMask)
        {
            return Directory.GetFiles(directory, fileMask, SearchOption.TopDirectoryOnly).Length;
        }
        public List<(string pathType, int filesInDir)> GetFileCounterForOutFolders()
        {
            List<(string pathType, int filesInDir)> filesInDirs = new();

            foreach (var path in pathsToInit)
            {
                if(path.isEnabled)
                {
                    var fileCounterData = (path.pathType, GetFilesCountInDir(path.path, path.fileMask));
                    loggerDebug.Debug($"File counter data: {fileCounterData.pathType} - {fileCounterData.Item2}");
                    filesInDirs.Add(fileCounterData);
                }
            }

            return filesInDirs;
        }
    }
}