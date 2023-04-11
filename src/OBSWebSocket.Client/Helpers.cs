using System.Security.Cryptography;
using System.Text;

namespace OBSWebSocket.Client;

public static class Helpers
{
    public static string Sha256Hash(string text)
    {
        using var sha256 = SHA256.Create();

        byte[] buffer = Encoding.UTF8.GetBytes(text);

        byte[] hash = sha256.ComputeHash(buffer, 0, buffer.Length);

        return Convert.ToBase64String(hash);
    }
    
    public static string GenerateAuthenticationString(string password, string challenge, string salt)
    {
        string saltedPassword = password + salt;
        string secret = Sha256Hash(saltedPassword);
        string challengeWithSecret = secret + challenge;
        string authString = Sha256Hash(challengeWithSecret);
        return authString;
    }
}