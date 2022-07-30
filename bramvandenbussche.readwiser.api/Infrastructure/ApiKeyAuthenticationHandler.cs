using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace bramvandenbussche.readwiser.api.Infrastructure;

/// <summary>
/// Handles API key security
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private const string ApiKeyHeaderName = "Authorization";
    private const string ProblemDetailsContentType = "application/problem+json";

    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!providedApiKey.ToLower().StartsWith("token "))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var apiKey = providedApiKey.Split(" ")[1];

        if (apiKey == "all-good")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Bram Vandenbussche")
            };

            // claims.AddRange(existingApiKey.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }

    //protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    //{
    //    Response.StatusCode = 401;
    //    Response.ContentType = ProblemDetailsContentType;
    //    var problemDetails = new UnauthorizedProblemDetails();

    //    await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
    //}

    //protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    //{
    //    Response.StatusCode = 403;
    //    Response.ContentType = ProblemDetailsContentType;
    //    var problemDetails = new ForbiddenProblemDetails();

    //    await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
    //}
}