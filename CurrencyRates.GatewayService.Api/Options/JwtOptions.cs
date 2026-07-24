namespace CurrencyRates.GatewayService.Api.Options;

/// <summary>
/// Параметры JWT для валидации токенов на API-шлюзе.
/// </summary>
public sealed class JwtOptions
{
    /// <summary>Имя секции конфигурации (<c>Jwt</c>).</summary>
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;
}
