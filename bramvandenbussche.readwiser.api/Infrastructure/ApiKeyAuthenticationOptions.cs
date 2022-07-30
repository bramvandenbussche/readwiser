using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace bramvandenbussche.readwiser.api.Infrastructure;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "API Key";
    public string Scheme => DefaultScheme;
    public string AuthenticationType = DefaultScheme;
}