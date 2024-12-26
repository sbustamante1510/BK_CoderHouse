namespace BK_CoderHouse.Application.Interfaces;

public interface ITokenService
{
        Task<string> generateUserToken(
            int usuarioId,
            DateTime fechaExpiracion,
            string type);

        Task<IDictionary<string, object>?> ValidateToken(
            string token);

        Task<string> validateRefleshToken (string refleshtoken);
    
}
