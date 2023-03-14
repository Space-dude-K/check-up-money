using check_up_money.FileHelpers;

namespace check_up_money.Interfaces
{
    interface IFileLockHelper
    {
        RM_PROCESS_INFO[] FindLockerProcesses(string path);
    }
}