using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Malmasp.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Malmasp.Services;

public class HasherService
{
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly JwtOptions _options;

    public HasherService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string CreateJwt(string id)
    {
        return _handler.CreateEncodedJwt(
            _options.Issuer,
            _options.Audience,
            new ClaimsIdentity(new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, id)
            }.AsEnumerable()),
            DateTime.Now,
            DateTime.Now.AddHours(24),
            DateTime.Now,
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                SecurityAlgorithms.HmacSha512Signature
                )
        );
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    public bool VerifyPassword(string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(hash: hashedPassword, text: password);
    }
}