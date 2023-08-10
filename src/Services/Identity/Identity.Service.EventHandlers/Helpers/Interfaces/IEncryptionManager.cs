using System.Security.Cryptography;

namespace Identity.Service.EventHandlers.Helpers.Interfaces
{
    public interface IEncryptionManager
    {
        string Encrypt(string value, string key);
        string Encrypt<T>(string value, string key) where T : SymmetricAlgorithm, new();
        string Hash(string password);
        string Unscrypt(string value, string key);
        string Unscrypt<T>(string value, string key) where T : SymmetricAlgorithm, new();
    }
}