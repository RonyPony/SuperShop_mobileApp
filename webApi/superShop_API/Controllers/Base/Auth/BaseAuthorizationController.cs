using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using superShop_API.Controllers.Base.Auth.DTOs;
using superShop_API.Database.Entities.Auth;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Services.Constructor;
using superShop_API.Shared;

namespace superShop_API.Controllers.Base.Auth;

/// <summary>
/// Controlador generico de manejo de controllers de autorizacion en el proyecto
/// </summary>
/// <typeparam name="Tentity">Tipo de entidad de modelo sobre la cual se hara la autorizacion</typeparam>
[ApiController]
[Route("api/auth/[controller]")]
public abstract class BaseAuthorizationController<Tentity> : ControllerBase where Tentity : class, IBaseEntity
{
    protected IServiceConstructor Constructor { get; set; }

    protected readonly UserManager<User> _userManager;
    protected readonly RoleManager<Role> _roleManager;
    protected readonly IConfiguration _configuration;


    /// <summary>
    /// Constructor generico de controlador autorizador
    /// </summary>
    /// <param name="_options">parametros sobre los cuales se hara la autorizacion</param>
    /// <param name="_constructor">Instancia de constructor de servicios</param>
    protected BaseAuthorizationController(
        IServiceConstructor _constructor,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration)
    {
        Constructor = _constructor;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    /// <summary>
    /// Ejerce el proceso de autorizacion en base a las credenciales recibidas
    /// </summary>
    /// <param name="credentials">Instancia de credenciales a validar para la autorizacion de acceso a sistema</param>
    /// <returns>retorna el resultado del proceso de autorizacion de acceso a sistema y su credencial de acceso generada</returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("login", Name = "Post[controller]Login")]
    public virtual async Task<ActionResult> PostLogin([FromBody] Credentials credentials)

    {
        var (result, entity, jwt, expiration) = await AuthorizeAccess(credentials);
        return Ok(new { Result = result, Token = jwt, Expiration = expiration });
    }

    /// <summary>
    /// Ejecuta el proceso de cierre de session del usuario actualmente autorizado
    /// </summary>
    /// <returns>retorna el resultado del proceso de cierre de session del usuario actualmente en uso</returns>
    [HttpGet]
    [Route("exit", Name = "Get[controller]LogOut")]
    public virtual async Task<ActionResult<Result>> GetLogOut()
    {
        return await LogOut(User.FindFirst("access_token").Value);
    }

    /// <summary>
    /// Ejecuta el proceso de requisicion de credenciales de acceso
    /// </summary>
    /// <param name="username">nombre de usuario o correo electronico del usuario a recuperar</param>
    /// <returns>retorna el resultado del proceso de recuperacion con el token de recuperacion temporal generado</returns>
    [AllowAnonymous]
    [Route("request-recover-token/{username}", Name = "GetRequestRecoverCredentialsOf[controller]")]
    [HttpGet]
    public virtual async Task<ActionResult> GetRequestRecoverCredentials([FromRoute] string username)
    {
        var remoteIP = Request.HttpContext.Connection.RemoteIpAddress;
        var (result, entity, recoverToken) = await RequestPasswordRecovery(username, remoteIP);
        return Ok(new { Result = result, RecoverToken = recoverToken });
    }

    /// <summary>
    /// Cambia las credenciales de acceso del usuario actualmente en uso
    /// </summary>
    /// <param name="credentials">Instancia de credenciales de recuperacion del usuario actual</param>
    /// <param name="token">Token de recuperacion de credenciales de usuario</param>
    /// <returns>retorna el resultado del proceso de cambio de credenciales</returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("change-credentials-per-token", Name = "PostChangeCredentialsByTokenOf[controller]")]
    public virtual async Task<ActionResult> PostChangeCredentialsByToken([FromBody] RecoverCredentials credentials, [FromQuery] string email, [FromQuery] string token)
    {
        if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(email))
        {
            return Ok((await ChangePassword(email, credentials.NewPassword, null, token)).result);
        }
        return Ok(Result.Instance().Fail("No se han recibido todos los datos necesarios para efectuar esta operacion"));
    }

    /// <summary>
    /// Cambia las credenciales de acceso del usuario actualmente en uso
    /// </summary>
    /// <param name="credentials">Instancia de credenciales de recuperacion del usuario actual</param>
    /// <returns>retorna el resultado del proceso de cambio de credenciales</returns>
    [HttpPost]
    [Route("change-credentials", Name = "PostChangeCredentialsOf[controller]")]
    public virtual async Task<ActionResult> PostChangeCredentials([FromBody] RecoverCredentials credentials)
    {
        var (result, entity) = await ChangePassword(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value, credentials.NewPassword, credentials.OldPassword, null);
        return Ok(result);
    }

    /// <summary>
    /// Genera y envia el codigo de confirmacion necesario para la confirmacion de correo electronico del usuario quien solicita la misma
    /// </summary>
    /// <param name="email">Correo electronico del usuario a confirmar</param>
    /// <returns>retorna el resultado de la operacion de recuperacion de credenciales</returns>
    [HttpGet]
    [AllowAnonymous]
    [Route("request-email-confirmation")]
    public virtual async Task<ActionResult> GetRequestEmailConfirmation([FromQuery] string email)
    {
        var (result, entity, confirmationToken) = await RequestEmailValidation(email);
        return Ok(new { result, confirmationToken });
    }

    /// <summary>
    /// Confirma el correo electronico del usuario con el correo electronico recibido y valida los mismos
    /// </summary>
    /// <param name="email">Correo electronico del usuario a confirmar</param>
    /// <param name="confirmationToken">Codigo de confirmacion enviado al correo electronico del usuario</param>
    /// <returns>Retorna el resultado de la operacion de confirmacion de usuario</returns>
    [HttpGet]
    [AllowAnonymous]
    [Route("check-email", Name = "GetValidateEmailOf[controller]")]
    public virtual async Task<ActionResult> GetValidateEmail([FromQuery] string email, [FromQuery] string confirmationToken)
    {
        var (result, entity) = await ValidateEmail(email, confirmationToken);
        return Ok(result);
    }

    protected JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    /// <summary>
    /// Ejecuta el proceso de validacion de credenciales el usuario el cual se va a autentificar
    /// </summary>
    /// <param name="credentials">Instancia de credenciales de acceso del usuario a validar</param>
    /// <returns>Retorna el resultado de la operacion, la entidad del usuario resultante y el token de acceso de este usuario</returns>
    protected abstract Task<(Result result, Tentity? entity, string jwt, DateTime expiration)> AuthorizeAccess(Credentials credentials);

    /// <summary>
    /// Ejecuta el proceso de solicitud de confirmacion de credenciales de acceso al usuario registrado bajo el correo electronico suministrado
    /// </summary>
    /// <param name="email">Correo del usuario por confirmar</param>
    /// <returns>Retorna el resultado de la operacion y ademas devuelve el usuario registrado por el correo electronico recibido</returns>
    protected abstract Task<(Result result, Tentity? entity, string confirmationToken)> RequestEmailValidation(string email);

    /// <summary>
    /// Ejecuta el proceso de validacion de usuario mediante el codigo de confirmacion enviada a su correo electronico
    /// </summary>
    /// <param name="email">Correo electronice del usuario a validar</param>
    /// <param name="confirmationToken">Codigo de confirmacion recibido por el usuario a validar</param>
    /// <returns>Retorna el resultado de la operacion como ademas el usuario afectado por el mismo</returns>
    protected abstract Task<(Result result, Tentity? entity)> ValidateEmail(string email, string confirmationToken);

    /// <summary>
    /// Retorna los roles pertenecientes al usuario solicitado
    /// </summary>
    /// <param name="entity">Instancia del usuario a evaluar</param>
    /// <returns>Retorna la lista de roles concatenados pertenecientes a este usuario</returns>
    protected abstract Task<Claim> GetRoleClaims(Tentity entity);

    /// <summary>
    /// Ejecuta el procedimiento de cierre de session de usuario actualmente en uso
    /// </summary>
    /// <param name="token">token de usuario actual del usuario</param>
    /// <returns>Retorna el resultado de deslogueo del usuario actual</returns>
    protected abstract Task<Result> LogOut(string token);

    /// <summary>
    /// Ejecuta el procedimiento de requisicion de recuperacion de credenciales de usuarios
    /// </summary>
    /// <param name="username">Nombre de usuario o correo electronico a recuperar</param>
    /// <param name="ip">direccion IP del cliente HTTP el cual ha invocado este metodo</param>
    /// <returns>Retorna el resultado de la operacion de recuperacion, la entidad de usuario resultante y el token de recuperacion para este usuario</returns>
    protected abstract Task<(Result result, Tentity? entity, string requestToken)> RequestPasswordRecovery(string username, IPAddress ip = null);

    /// <summary>
    /// Ejecuta el proceso de cambio de credenciales de un usuario
    /// </summary>
    /// <param name="email">Correo electronico del usuario a modificar</param>
    /// <param name="newPassword">Nueva contraseña de usuario</param>
    /// <param name="actualPassword">Contraseña actual del usuario</param>
    /// <param name="resetToken">Token de recuperacion enviado al usuario</param>
    /// <param name="recoverable">Indica si el token de autorizacion actual es temporal o no</param>
    /// <returns>Retorna el resultado de la operacion y la entidad resultante de esta operacion</returns>
    protected abstract Task<(Result result, Tentity? entity)> ChangePassword(string email, string newPassword, string actualPassword = null, string resetToken = null);
}