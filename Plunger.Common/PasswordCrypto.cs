using System.Text;
using Geralt;

namespace Plunger.Common;

public class PasswordCrypto
{
    private static class CryptoConstants
    {
        public static int MemorySize = 19456;
        public static int Iterations = 2;
    }
    
    public static bool CheckPassword(string textPassword, string storedHash)
    {
        var pwBytes = Encoding.UTF8.GetBytes(textPassword);
        var valid = Argon2id.VerifyHash(Encodings.FromBase64(storedHash), pwBytes);
        return valid;
    }
    
    public static string ComputePasswordHash(string password)
    {
        return ComputePasswordHash(Encoding.UTF8.GetBytes(password));
    }

    public static string ComputePasswordHash(Span<byte> password)
    {
        #warning TODO: Figure out how/if to truncate resulting hash
        var hash = new byte[Argon2id.MaxHashSize];
        Argon2id.ComputeHash(hash, password, CryptoConstants.Iterations, CryptoConstants.MemorySize);
        return Encodings.ToBase64(hash);
    }
}