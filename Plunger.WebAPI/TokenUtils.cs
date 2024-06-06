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
        randomFingerprint = GenerateRandomBase64String();
        var issuer = jwtConfig.Issuer;
        var audience = jwtConfig.Audience;
        var key = Encoding.ASCII.GetBytes
            (jwtConfig.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                new Claim(Constants.UserId, user.Id.ToString()),
                // Hash of RandomString
                new Claim(Constants.TokenFingerprint, HashBase64String(randomFingerprint))
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

    private static string GenerateRandomBase64String()
    {
        var bytes = new byte[64];
        var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashBase64String(string str)
    {
        var hasher = SHA256.Create();
        var bytes = Convert.FromBase64String(str);
        var hashedBytes = hasher.ComputeHash(bytes);
        return Convert.ToBase64String(hashedBytes);
    }
}