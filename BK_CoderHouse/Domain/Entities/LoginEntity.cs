namespace BK_CoderHouse.Domain.Entities;

public class LoginEntity
{
    public string username {  get; set; }

    public string token { get; set; }

    public string tokenExpirationAt { get; set; }

    public string refreshToken {  get; set; }

    public string refreshTokenExpiration { get; set; }
}
