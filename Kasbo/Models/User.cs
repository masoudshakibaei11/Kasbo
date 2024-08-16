
using System.ComponentModel.DataAnnotations;

namespace Kasbo.Models;

public class User : BaseEntity<int>
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}