using BK_CoderHouse.Domain.Entities;
using BK_CoderHouse.Domain.Payload;

namespace BK_CoderHouse.Application.Interfaces;

public interface IAuthenticationService
{
    Task<LoginEntity> Login(LoginPayload payload);

    Task<RefleshLoginEntity> refleshLogin(RefleshTokenPayload payload);

    Task<(IDictionary<string, object>?, string?)> ValidateToken(string token);
}
