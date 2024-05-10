using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Tributech.SensorManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    // GET api/account/login
    [HttpGet("login")]
    public async Task<IActionResult> LoginCallback()
    {
        var authResult = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);
        if (authResult?.Succeeded != true)
        {
            // Handle failed authentication
            return RedirectToAction("Login");
        }

        // Get the access token and refresh token
        var accessToken = authResult.Properties.GetTokenValue("access_token");
        var refreshToken = authResult.Properties.GetTokenValue("refresh_token");

        // Save the tokens to the user's session or database
        HttpContext.Session.SetString("access_token", accessToken);
        HttpContext.Session.SetString("refresh_token", refreshToken);

        // Redirect the user to the desired page
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize]
    [Route("receive-token")]
    public IActionResult ReceiveToken()
    {
        try
        {
            string accessToken = ExtractAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized("No access token provided.");
            }

            // Process the token as needed
            // ...

            return Ok("Token received successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error receiving token: {ex.Message}");
        }
    }

    private string ExtractAccessToken()
    {
        if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var authHeaderVal = authHeader.ToString();
            if (authHeaderVal.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authHeaderVal.Substring("Bearer ".Length).Trim();
            }
        }

        return null;
    }
}