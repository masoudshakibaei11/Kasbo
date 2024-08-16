using Kasbo.Models;

namespace Kasbo.Services;

public interface IAuthService
{
    
    public Task Register(string email);
    public  Task<string> Login(string email, string password);


}