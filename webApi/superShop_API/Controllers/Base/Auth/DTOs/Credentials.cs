using System.ComponentModel.DataAnnotations;

namespace superShop_API.Controllers.Base.Auth.DTOs;

public class Credentials
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "RememberMe is required")]
    public bool RememberMe { get; set; }
}