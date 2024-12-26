namespace BK_CoderHouse.Application.Services;

using BK_CoderHouse.Application.Interfaces;
using BK_CoderHouse.Domain.Entities;
using BK_CoderHouse.Domain.Payload;

public class AuthenticationService : IAuthenticationService
{

    private readonly IConfiguration _configuration;
    private readonly ITokenService _TokenService;

    public AuthenticationService(IConfiguration configuration, ITokenService tokenService)
    {
        _configuration = configuration;
        _TokenService = tokenService;
    }

    public async Task<LoginEntity> Login(LoginPayload payload)
    {

        var isValidUsername = payload.username == _configuration["User:Username"] ? true : false;

        if (!isValidUsername)
            throw new InvalidOperationException("Ingrese un usuario válido");

        var isValidPassword = BCrypt.Net.BCrypt
                    .Verify(payload.password, _configuration["User:Password"] ?? "");

        if (!isValidPassword)
            throw new InvalidOperationException("Ingrese una contraseña correcta");

        var tokenExpirationAt = DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["TokenManagement:JwtAccessTokenExpiration"])
                );
        var token = await _TokenService.generateUserToken(
            usuarioId: int.Parse(_configuration["User:Id"]),
            fechaExpiracion: tokenExpirationAt,
            type: "token"
        );

        var refreshTokenExpirationAt = DateTime.UtcNow.AddMinutes(
            int.Parse(_configuration["TokenManagement:JwtRefreshTokenExpiration"])
        );
        var refreshToken = await _TokenService.generateUserToken(
            usuarioId: int.Parse(_configuration["User:Id"]),
            fechaExpiracion: refreshTokenExpirationAt,
            type: "refresh"
        );

        var result = new LoginEntity() {
            username = payload.username,
            token =  token,
            tokenExpirationAt =  tokenExpirationAt.ToString("dd/MM/yyyy HH:mm:ss"),
            refreshToken = refreshToken,
            refreshTokenExpiration = refreshTokenExpirationAt.ToString("dd/MM/yyyy HH:mm:ss")
        };


        //var result = new LoginEntity();


        return result;
    }

    public async Task<RefleshLoginEntity> refleshLogin(RefleshTokenPayload payload)
    {

        var id = await _TokenService.validateRefleshToken(payload.refleshToken);

        if(id != "15")
            throw new InvalidOperationException("Token incorrecto");

        var tokenExpirationAt = DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["TokenManagement:JwtAccessTokenExpiration"])
                );
        var token = await _TokenService.generateUserToken(
            usuarioId: int.Parse(_configuration["User:Id"]),
            fechaExpiracion: tokenExpirationAt,
            type: "token"
        );

        var refreshTokenExpirationAt = DateTime.UtcNow.AddMinutes(
            int.Parse(_configuration["TokenManagement:JwtRefreshTokenExpiration"])
        );
        var refreshToken = await _TokenService.generateUserToken(
            usuarioId: int.Parse(_configuration["User:Id"]),
            fechaExpiracion: refreshTokenExpirationAt,
            type: "refresh"
        );


        var result = new RefleshLoginEntity()
        {
            token = token,
            tokenExpirationAt = tokenExpirationAt.ToString("dd/MM/yyyy HH:mm:ss"),
            refreshToken = refreshToken,
            refreshTokenExpiration = refreshTokenExpirationAt.ToString("dd/MM/yyyy HH:mm:ss")
        };


        return result;
    }



    public async Task<(IDictionary<string, object>?, string?)> ValidateToken(string token)
    {
        var result = await _TokenService.ValidateToken(token);
        string? userId;
        if (result == null)
        {
            return (null, null);
        }

        // Verifica que el valor "sub" existe y no es nulo. 
        if (
            result.TryGetValue(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out var subObj
            ) && subObj is string sub
        )
        {
            string id = sub;
            userId = id;
        }
        else
        {
            // Maneja el caso en que "sub" no está presente o es nulo.
            return (null, null);
        }

        return (result, userId);
    }
}
