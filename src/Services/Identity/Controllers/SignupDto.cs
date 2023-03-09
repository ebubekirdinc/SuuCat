using System.ComponentModel.DataAnnotations;

namespace Identity.Controllers;

public class SignupDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
 
}