using System;

namespace Application.Services.Identities
{
    public interface IEncryptionService
    {


        string EncryptText(string plainText, string encryptionPrivateKey = "");

        string DecryptText(string cipherText, string encryptionPrivateKey = "");

        Guid CreateCryptographicallySecureGuid();

        string GetSha256Hash(string input);
    }
}
