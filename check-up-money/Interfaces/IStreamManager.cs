using System.IO;
using System.Threading.Tasks;

namespace check_up_money.Interfaces
{
    interface IStreamManager
    {
        Task<FileStream> WaitForFileStream(string fullPath, FileMode mode, FileAccess access, FileShare share);
    }
}