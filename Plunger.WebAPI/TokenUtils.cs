using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Plunger.Data.DbModels;

namespace Plunger.WebApi;

public class TokenUtils
{
    public static string CreateToken(JwtConfig jwtConfig, User user, out string randomFingerprint)
    {
        randomFingerprint = GenerateRandomBase64UrlString();
        var issuer = jwtConfig.Issuer;
        var audience = jwtConfig.Audience;
        var key = Encoding.ASCII.GetBytes
            (jwtConfig.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                // new Claim(Constants.UserId, user.Id.ToString()),
                // Hash of RandomString
                new Claim(Constants.TokenFingerprint, HashBase64UrlString(randomFingerprint))
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public static bool VerifyTokenFingerprint(string fingerprintHash, string fingerprint)
    {
        return string.Equals(HashBase64UrlString(fingerprint), fingerprintHash);
    }

    private static string GenerateRandomBase64UrlString()
    {
        return Base64UrlEncoder.Encode(GenerateRandomBytes());
    }

    private static string GenerateRandomBase64String()
    {
        return Convert.ToBase64String(GenerateRandomBytes());
    }

    private static byte[] GenerateRandomBytes()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return bytes;
    }

    private static string HashBase64String(string str)
    {
        var bytes = Convert.FromBase64String(str);
        var hashedBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashedBytes);
    }

    private static string HashBase64UrlString(string str)
    {
        var bytes = Base64UrlEncoder.DecodeBytes(str);
        var hashedBytes = SHA256.HashData(bytes);
        return Base64UrlEncoder.Encode(hashedBytes);
    }
}