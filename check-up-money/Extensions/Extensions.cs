using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace check_up_money.Extensions
{
    public static class Extensions
    {
        static Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        static Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        static Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        public static string GetHostFromOdbcConnectionString(this string rawConnectionString)
        {
            var host = string.Empty;

            Regex regex = new Regex(@"(?<key>[^=;,]+)=(?<val>[^;,]+(,\d+)?)");

            foreach (Match m in regex.Matches(rawConnectionString))
            {
                if (m.Value.Contains("Server="))
                {
                    var val = m.Value.Substring(m.Value.IndexOf("=") + 1);
                    host = val.Contains(@"\") ? val.Substring(0, val.IndexOf(@"\")) : val;
                    break;
                }
            }

            if (string.IsNullOrEmpty(host))
                throw new Exception("Ошибка разбора хоста из строки подключения.");

            return host;
        }
        public static string GetDatabaseFromOdbcConnectionString(this string rawConnectionString)
        {
            var db = string.Empty;

            Regex regex = new Regex(@"(?<key>[^=;,]+)=(?<val>[^;,]+(,\d+)?)");

            foreach (Match m in regex.Matches(rawConnectionString))
            {
                if (m.Value.Contains("Database="))
                {
                    var val = m.Value.Substring(m.Value.IndexOf("=") + 1);
                    db = val;
                    break;
                }
            }

            if (string.IsNullOrEmpty(db))
                throw new Exception("Ошибка разбора строки базы данных из строки подключения.");

            return db;
        }
        public static IEnumerable<string> FilterFiles(this string path, params string[] exts)
        {
            return
                Directory
                .EnumerateFiles(path, "*.*")
                .Where(file => exts.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)));
        }
        public static List<(string path, string pathType)> ChangeBaseDirForPathSettings(this List<(string pathType, string path)> ps, 
            string newBaseDir)
        {
            List<(string pathType, string path)> newPs = new();

            foreach (var path in ps)
            {
                var newPath = Path.Combine(newBaseDir, string.Join("\\", path.path.Split('\\').Reverse().Take(2).Reverse()));
                newPs.Add((path.pathType, newPath));
            }

            return newPs;
        }
        public static string GetBudgetSuffixForLabelTip(this string rawBudget)
        {
            string budget = string.Empty;
            rawBudget = rawBudget.ToLower().Substring(0, 3);

            switch (rawBudget)
            {
                case "rep":
                    budget = " Республиканский бюджет.";
                    break;
                case "obl":
                    budget = " Областной бюджет.";
                    break;
                case "cit":
                    budget = " Городской бюджет.";
                    break;
                case "reg":
                    budget = " Районный бюджет.";
                    break;
                case "uni":
                    budget = " Союзный бюджет.";
                    break;
                case "ext":
                    budget = " Внебюджет.";
                    break;
            }

            return budget;
        }
        public static string GetUserNameFromIdentity(this WindowsIdentity windowsIdentity)
        {
            string userName =
                string.IsNullOrWhiteSpace(windowsIdentity.Name)
                ? "UnknownUser_" + Guid.NewGuid().ToString("N") : windowsIdentity.Name.Replace('\\', '_');

            return userName;
        }
        public static void ThreadSafeInvokeForTextAssignment(this Control control, string textToAssign)
        {
            if(control.IsHandleCreated)
            {
                // Thread safe invoke.
                if (control.InvokeRequired)
                {
                    control.Invoke(new MethodInvoker(delegate ()
                    {
                        control.Text = textToAssign;
                    }));
                }
                else
                {
                    control.Text = textToAssign;
                }
            }
            else
            {
                throw new Exception($"Missing handle for thread safe invoke. {control.GetType()}");
            }
        }
        public static void ThreadSafeInvokeForLabelToolTip(this Control control, ToolTip toolTip, string labelTooltipText)
        {
            if (control.IsHandleCreated)
            {
                // Thread safe invoke.
                if (control.InvokeRequired)
                {
                    control.Invoke(new MethodInvoker(delegate ()
                    {
                        SetToolTipForLabel(control, toolTip, labelTooltipText);
                    }));
                }
                else
                {
                    SetToolTipForLabel(control, toolTip, labelTooltipText);
                }
            }
            else
            {
                throw new Exception($"Missing handle for thread safe invoke. {control.GetType()}");
            }
        }
        private static void SetToolTipForLabel(Control label, ToolTip toolTip, string labelTooltipText)
        {
            //loggerDebug.Debug($"Set tooltip {labelTooltipText} for {label.Name}");

            toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.IsBalloon = true;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(label, labelTooltipText);
        }
        public static void ThreadSafeInvokeForColor(this Control control, Color colorName)
        {
            // Thread safe invoke.
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(delegate ()
                {
                    control.ForeColor = colorName;
                }));
            }
            else
            {
                control.ForeColor = colorName;
            }
        }
        public static IEnumerable<Control> GetAll(this Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }
        public static Control.ControlCollection FlattenAndAppend(this System.Windows.Forms.Control.ControlCollection collection, System.Windows.Forms.Control.ControlCollection newCollection)
        {
            List<Control> controlList = new List<Control>();
            FlattenControlTree(collection, controlList);
            newCollection.AddRange(controlList.ToArray());

            return newCollection;
        }

        public static void FlattenControlTree(System.Windows.Forms.Control.ControlCollection collection, List<Control> controlList)
        {
            foreach (Control control in collection)
            {
                controlList.Add(control);
                FlattenControlTree(control.Controls, controlList);
            }
        }
        public static string GetUniqueFilePathFromFolder(this string currentFolder)
        {
            var winIdentity = WindowsIdentity.GetCurrent();
            string userName =
                string.IsNullOrWhiteSpace(winIdentity.Name)
                ? "UnknownUser_" + Guid.NewGuid().ToString("N") : winIdentity.Name.Replace('\\', '_');

            var currentDt = DateTime.Now;
            var currentYear = currentDt.Year.ToString();
            var currentMonth = currentDt.Month.ToString();
            var currentDay = currentDt.Day.ToString();
            var currentTime = currentDt.ToString(@"HH-mm-ss-fff");

            return Path.Combine(currentFolder, userName, currentYear, currentMonth, currentDay, currentTime);
        }
        public static List<string> IterateFilesInDir(this string dirPath, string[] filesExt)
        {
            return new DirectoryInfo(dirPath).GetFilesByExtensions(filesExt).Select(f => f.FullName).ToList();
        }
        // Returns all controls of a certain type in all levels.
        public static IEnumerable<TheControlType> AllControls<TheControlType>(this Control theStartControl) where TheControlType : Control
        {
            var controlsInThisLevel = theStartControl.Controls.Cast<Control>();

            return controlsInThisLevel.SelectMany(AllControls<TheControlType>).Concat(controlsInThisLevel.OfType<TheControlType>());
        }
        // Get string description from enum.
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        /// <summary>
        /// Возвращает слова в падеже, зависимом от заданного числа 
        /// </summary>
        /// <param name="number">Число от которого зависит выбранное слово</param>
        /// <param name="nominativ">Именительный падеж слова. Например "день"</param>
        /// <param name="genetiv">Родительный падеж слова. Например "дня"</param>
        /// <param name="plural">Множественное число слова. Например "дней"</param>
        /// <returns></returns>
        public static string GetDeclension(this int number, string nominativ, string genetiv, string plural)
        {
            number = number % 100;

            if (number >= 11 && number <= 19)
            {
                return plural;
            }

            var i = number % 10;

            switch (i)
            {
                case 1:
                    return nominativ;
                case 2:
                case 3:
                case 4:
                    return genetiv;
                default:
                    return plural;
            }

        }
        // Filter dir by extensions
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("Extension filter exception");

            IEnumerable<FileInfo> files = dir.EnumerateFiles();

            return files.Where(f => extensions.Contains(f.Extension));
        }
        // Filter file
        public static string FilterFile(this string fullFilePath, List<string> allowedFileExtensions)
        {
            string filteredFullFilePath = string.Empty;

            if (allowedFileExtensions.Any(f => f.Equals(Path.GetExtension(fullFilePath))))
            {
                filteredFullFilePath = fullFilePath;
            }

            return filteredFullFilePath;
        }
        #region Size formatter
        private enum DiskSizeFormatType
        {
            Bytes = 0,
            KiloBytes = 1,
            MegaBytes = 2,
            GigaBytes = 3
        }
        private static decimal FormatSize(long freeBytes, DiskSizeFormatType type)
        {
            decimal formatedSizeFree;

            formatedSizeFree = (decimal)(freeBytes / Math.Pow(1024, (int)type));

            return formatedSizeFree;
        }
        public static string FormatSizeBytes(this long dlBytes)
        {
            if (dlBytes < 1000000) // Kb
            {
                return FormatSize(dlBytes, DiskSizeFormatType.KiloBytes).ToString("F") + " Kb";
            }
            else if (dlBytes > 999999 && dlBytes < 1000000000) // Mb
            {
                return FormatSize(dlBytes, DiskSizeFormatType.MegaBytes).ToString("F") + " Mb";
            }
            else  // Gb
            {
                return FormatSize(dlBytes, DiskSizeFormatType.GigaBytes).ToString("F") + " Gb"; ;
            }
        }
        #endregion
        #region Time formatters
        private enum TimeFormatType : long
        {
            [Description("ms")]
            Milliseconds = 0,
            [Description("sec")]
            Seconds = 1000,
            [Description("min")]
            Minutes = 60000,
            [Description("hr")]
            Hours = 3600000,
            [Description("day")]
            Days = 86400000,
            [Description("week")]
            Weeks = 604800000,
            [Description("mont")]
            Months = 262800000,
            [Description("year")]
            Years = 31557600000
        }
        private static long FormatTime(long milliseconds, TimeFormatType type)
        {
            long formatedTime;

            formatedTime = (milliseconds / (long)type);

            return formatedTime;
        }
        public static string FormatTimeMilliseconds(this double milliseconds)
        {
            TimeFormatType tft = GetTimeTypeFromMilliseconds((long)milliseconds);

            return FormatTime((long)milliseconds, tft).ToString("F") + " " + tft.GetEnumDescription();
        }
        private static TimeFormatType GetTimeTypeFromMilliseconds(long milliseconds)
        {
            switch (milliseconds)
            {
                case long m when (m > (long)TimeFormatType.Seconds && m < (long)TimeFormatType.Minutes):
                    return TimeFormatType.Seconds;
                case long m when (m > (long)TimeFormatType.Minutes && m < (long)TimeFormatType.Hours):
                    return TimeFormatType.Minutes;
                case long m when (m > (long)TimeFormatType.Hours && m < (long)TimeFormatType.Days):
                    return TimeFormatType.Hours;
                case long m when (m > (long)TimeFormatType.Days && m < (long)TimeFormatType.Weeks):
                    return TimeFormatType.Days;
                case long m when (m > (long)TimeFormatType.Weeks && m < (long)TimeFormatType.Months):
                    return TimeFormatType.Weeks;
                case long m when m > (long)TimeFormatType.Months:
                    return TimeFormatType.Months;
                case long m when (m > long.MaxValue):
                    return TimeFormatType.Years;
                default:
                    return TimeFormatType.Milliseconds;
            }
        }
        #endregion
    }
}