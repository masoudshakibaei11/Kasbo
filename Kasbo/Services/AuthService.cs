using Kasbo.AppDbContext;
using Kasbo.Models;
using Kasbo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kasbo.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;

    public AuthService(IConfiguration configuration, ApplicationDbContext context, IEmailSender emailSender)
    {
        _configuration = configuration;
        _context = context;
        _emailSender = emailSender;
    }

    public async Task Register(string email)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
            throw new Exception("User already exists");
        Random random = new Random();
        int password = random.Next(1000, 9999);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password.ToString());
        var user = new User { Email = email, PasswordHash = passwordHash };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _emailSender.SendEmailAsync(email, "ثبت نام کسبوتک", $"رمز عبور شما {password}");

    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid email or password");

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.Now.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}