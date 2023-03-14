using check_up_money.Cypher;
using System.Security;

namespace check_up_money.Interfaces
{
    public interface ICypher
    {
        RequisiteInformation Encrypt(
            string driver, string host, string instance, string bd, SecureString user, SecureString pass);
        RequisiteInformation Decrypt(
            string driver,string host, string instance, string bd, SecureString userEncrypted, string uSalt, SecureString passEncrypted, string pSalt);
        string ToInsecureString(SecureString input);
        SecureString ToSecureString(string input);
        SecureString DecryptString(SecureString encryptedData, string salt);
    }
}