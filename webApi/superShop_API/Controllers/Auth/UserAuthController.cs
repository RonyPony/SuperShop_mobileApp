using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Controllers.Base.Auth;
using superShop_API.Controllers.Base.Auth.DTOs;
using superShop_API.Database.Entities.Auth;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;
using superShop_API.Shared;

namespace superShop_API.Controllers.Auth;

[AllowAnonymous]
//[Authorize(Roles = Roles.Admin)]
public class UserAuthController : BaseAuthorizationController<User>
{
    public UserAuthController(IServiceConstructor _constructor, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration configuration) : base(_constructor, userManager, signInManager, roleManager, configuration)
    {
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<Result<Object>>> Register([FromBody] RegisterUser userModel)
    {
        try
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
                return StatusCode(StatusCodes.Status403Forbidden, Result.Instance().Fail($"User not created. Check the provided information and try again !.", data: result.Errors));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("There an internal error in the server, User not created. Check the provided information and try again !", exception: e));
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register/admin")]
    public async Task<ActionResult<Result<Object>>> RegisterAdmin([FromBody] RegisterUser userModel)
    {
        try
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
                return StatusCode(StatusCodes.Status403Forbidden, Result.Instance().Fail("User not created. Check the provided information and try again !", data: result.Errors));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("There an internal error in the server, User not created. Check the provided information and try again !", exception: e));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    //[Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
    [Route("user/{email}")]
    public async Task<ActionResult<Result<User>>> GetUser([FromQuery] string email)
    {
        try
        {
            var userFinded = await _userManager.FindByEmailAsync(email);

            if (userFinded != null)
            {
                return Result.Instance<User>().Success("user fined", userFinded);
            }
            else
            {
                return Result.Instance<User>().Fail($"User not finded. Please check your email");
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance<Object>().Fail("Unable to get the user", exception: e));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    //[Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
    [Route("users")]
    public async Task<ActionResult<Result<List<User>>>> GetUsers()
    {
        try
        {
            return Result.Instance<List<User>>().Success("Users list obtained", await this.Constructor.GetService<UserService, User>().GetAllAsync());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("Unable to get the user list", exception: e));
        }
    }

    [HttpDelete]
    [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
    [Route("remove")]
    public async Task<ActionResult<Result<Object>>> DeleteUser()
    {
        try
        {
            if (_signInManager.IsSignedIn(this.User))
            {
                var userFinded = await _userManager.FindByEmailAsync(this.User.FindFirst(c => c.Type == ClaimTypes.Email).Value);

                var result = await _userManager.DeleteAsync(userFinded);

                if (result.Succeeded)
                {
                    return Result.Instance().Success($"User <{userFinded.Email}> deleted !");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Result.Instance().Fail("Theres an error when trying to delete the user", data: result.Errors));
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Result.Instance().Fail("You don't logged in !"));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("Unable to delete the user", exception: e));
        }
    }

    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    [Route("remove/{userId}")]
    public async Task<ActionResult<Result<Object>>> DeleteByID([FromRoute] string userId)
    {
        try
        {
            var userFinded = await _userManager.FindByIdAsync(userId);

            if (userFinded != null)
            {
                var result = await _userManager.DeleteAsync(userFinded);

                if (result.Succeeded)
                {
                    return Result.Instance().Success($"User <{userFinded.Email}> deleted !");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Result.Instance().Fail("Theres an error when trying to delete the user", data: result.Errors));
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, Result.Instance().Fail("The requested user was't found !"));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("Unable to delete the user", exception: e));
        }
    }

    protected override async Task<(Result<Object> result, User? entity, string jwt, DateTime expiration)> AuthorizeAccess(Credentials credentials)
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

            await _signInManager.SignInWithClaimsAsync(userFinded, new AuthenticationProperties { ExpiresUtc = DateTimeOffset.FromUnixTimeMilliseconds(token.ValidTo.Millisecond), IssuedUtc = DateTimeOffset.FromUnixTimeMilliseconds(token.ValidFrom.Millisecond), IsPersistent = credentials.RememberMe }, authClaims);

            return (result: Result.Instance().Success("Login successful !"), entity: userFinded, jwt: new JwtSecurityTokenHandler().WriteToken(token), expiration: token.ValidTo);
        }
        return (result: Result.Instance().Fail("Login failed !"), entity: null, jwt: String.Empty, expiration: DateTime.Now);
    }

    protected override Task<(Result<Object> result, User? entity)> ChangePassword(string email, string newPassword, string actualPassword = null, string resetToken = null)
    {
        throw new NotImplementedException();
    }

    protected override Task<Claim> GetRoleClaims(User entity)
    {
        throw new NotImplementedException();
    }

    protected override async Task<Result<Object>> LogOut(string token)
    {
        await _signInManager.SignOutAsync();
        return Result.Instance().Success("User Logout !");
    }

    protected override Task<(Result<Object> result, User? entity, string confirmationToken)> RequestEmailValidation(string email)
    {
        throw new NotImplementedException();
    }

    protected override Task<(Result<Object> result, User? entity, string requestToken)> RequestPasswordRecovery(string username, IPAddress ip = null)
    {
        throw new NotImplementedException();
    }

    protected override Task<(Result<Object> result, User? entity)> ValidateEmail(string email, string confirmationToken)
    {
        throw new NotImplementedException();
    }
}