using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace YottySuba;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddJwtBearerConfiguration(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            return builder.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = new TimeSpan(0, 0, 30),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = CreateSigningKey(configuration["Jwt:Secret"] ?? "")
                };
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = (context) =>
                    {
                        var jwtToken = (JwtSecurityToken)context.SecurityToken;

                        var name = jwtToken.GetUsername();

                        if (name is null)
                        {
                            context.Fail(new Exception("Invalid or Expired Token"));
                        }
                        else
                        {
                            context.Success();
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                        // Add some extra context for expired tokens.
                        if (context.AuthenticateFailure is not SecurityTokenExpiredException authenticationException)
                            return context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = context.Error,
                                error_description = context.ErrorDescription
                            }));
                        context.Response.Headers.Append("x-token-expired", authenticationException.Expires.ToString("o"));
                        context.Response.Headers.Append("token-expired", "true");
                        context.ErrorDescription = $"The token expired on {authenticationException.Expires:o}";

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        }));
                    }
                };
            });
        }
    
    private static SymmetricSecurityKey CreateSigningKey(string secret)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }
    
    
    /// <summary>
    /// Retrieves the Username from the current <see cref="JwtSecurityToken"/>
    /// </summary>
    /// <param name="token"><see cref="JwtSecurityToken"/></param>
    /// <returns>The Username</returns>
    private static string? GetUsername(this JwtSecurityToken token) => token.Claims.FirstOrDefault(claim => claim.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value;

    
}