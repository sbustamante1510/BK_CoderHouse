﻿namespace BK_CoderHouse.Domain.Entities;

public class RefleshLoginEntity
{
    public string token { get; set; }

    public string tokenExpirationAt { get; set; }

    public string refreshToken { get; set; }

    public string refreshTokenExpiration { get; set; }
}
