using check_up_money.CheckUpFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static check_up_money.CheckUpFile.CheckUpBlob;

namespace check_up_money.Interfaces
{
    interface IFileParser
    {
        Task<CheckUpBlob> ParseFile(string directory, string fileName, BudgetType budgetType);
    }
}
