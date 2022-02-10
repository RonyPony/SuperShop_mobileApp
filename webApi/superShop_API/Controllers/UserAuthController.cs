using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Controllers.Base.Auth;
using superShop_API.Controllers.Base.Auth.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services.Constructor;
using superShop_API.Shared;

namespace superShop_API.Controllers;

[Authorize(Roles = Roles.Admin)]
public class UserAuthController : BaseAuthorizationController<User>
{
    public UserAuthController(IServiceConstructor _constructor, UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration) : base(_constructor, userManager, roleManager, configuration)
    {
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<Result>> Register([FromBody] RegisterUser userModel)
    {
        User userExists;

        if (new EmailAddressAttribute().IsValid(userModel.UserName))
        {
            userExists = await _userManager.FindByEmailAsync(userModel.UserName);
        }
        else
        {
            userExists = await _userManager.FindByNameAsync(userModel.UserName);
        }

        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("User already exists!"));

        var user = new User()
        {
            Name = userModel.Name,
            LastName = userModel.LastName,
            Email = userModel.Email,
            UserName = userModel.UserName,
            BirthDate = userModel.BirthDate,
            RegistrationDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, userModel.Password);

        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new Role(Roles.User));
            if (await _roleManager.RoleExistsAsync(Roles.User))
            {
                await _userManager.AddToRoleAsync(user, Roles.User);
            }
            return Ok(Result.Instance().Success($"User '{userModel.UserName} <{userModel.Email}>' created !"));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("User not created. Check the provided information and try again !"));
        }
    }

    [HttpPost]
    [Route("register/admin")]
    public async Task<ActionResult<Result>> RegisterAdmin([FromBody] RegisterUser userModel)
    {
        User userExists;

        if (new EmailAddressAttribute().IsValid(userModel.UserName))
        {
            userExists = await _userManager.FindByEmailAsync(userModel.UserName);
        }
        else
        {
            userExists = await _userManager.FindByNameAsync(userModel.UserName);
        }

        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("User already exists!"));

        var user = new User()
        {
            Name = userModel.Name,
            LastName = userModel.LastName,
            Email = userModel.Email,
            UserName = userModel.UserName,
            BirthDate = userModel.BirthDate,
            RegistrationDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, userModel.Password);

        if (result.Succeeded)
        {

            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new Role(Roles.Admin));

            if (await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin);
            }

            return Ok(Result.Instance().Success($"User '{userModel.UserName} <{userModel.Email}>' created !"));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("User not created. Check the provided information and try again !"));
        }
    }

    protected override async Task<(Result result, User? entity, string jwt, DateTime expiration)> AuthorizeAccess(Credentials credentials)
    {
        User userFinded;

        if (new EmailAddressAttribute().IsValid(credentials.UserName))
        {
            userFinded = await _userManager.FindByEmailAsync(credentials.UserName);
        }
        else
        {
            userFinded = await _userManager.FindByNameAsync(credentials.UserName);
        }

        if (userFinded != null && await _userManager.CheckPasswordAsync(userFinded, credentials.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(userFinded);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userFinded.UserName),
                    new Claim(ClaimTypes.Email, userFinded.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return (result: Result.Instance().Success("Login successful !"), entity: userFinded, jwt: new JwtSecurityTokenHandler().WriteToken(token), expiration: token.ValidTo);
        }
        return (result: Result.Instance().Fail("Login failed !"), entity: null, jwt: String.Empty, expiration: DateTime.Now);
    }

    protected override Task<(Result result, User? entity)> ChangePassword(string email, string newPassword, string actualPassword = null, string resetToken = null)
    {
        throw new NotImplementedException();
    }

    protected override Task<Claim> GetRoleClaims(User entity)
    {
        throw new NotImplementedException();
    }

    protected override Task<Result> LogOut(string token)
    {
        throw new NotImplementedException();
    }

    protected override Task<(Result result, User? entity, string confirmationToken)> RequestEmailValidation(string email)
    {
        throw new NotImplementedException();
    }

    protected override Task<(Result result, User? entity, string requestToken)> RequestPasswordRecovery(string username, IPAddress ip = null)
    {
        throw new NotImplementedException();
    }

    protected override Task<(Result result, User? entity)> ValidateEmail(string email, string confirmationToken)
    {
        throw new NotImplementedException();
    }
}