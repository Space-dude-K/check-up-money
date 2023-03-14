using check_up_money.Cypher;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money.Interfaces
{
    interface IRemoteHostAvailabilityChecker
    {
        bool CheckDbFromRi(List<(string budgetType, RequisiteInformation ri, Control control, bool isEnabled)> list);
        Task<bool> CheckHostAndBd(string connectionStr);
        Task<bool> CheckRealBd(string connStr);
        bool CheckRealServer(string serverIpOrDnsName);
        Task<bool> IsConnectionViable(string connectionStr);
        Task<bool> WaitForConnection(string connectionStr, Control control = null);
    }
}