using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Claims;
using YaleAccess.Models;

namespace YaleAccess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly Models.Options.AuthenticationOptions _authenticationOptions;

        public AuthenticationController(IOptions<Models.Options.AuthenticationOptions> authenticationOptions)
        {
            _authenticationOptions = authenticationOptions.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] string password)
        {
            try
            {
                // Check if the password is correct
                if (password != _authenticationOptions.Password)
                {
                    return Unauthorized(new ApiResponse("Incorrect password."));
                }

                // Log the user in 
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, "YaleAccess")
                };
                ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                 
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Return the response
                return Ok(new ApiResponse(true));
            }
            catch(Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred logging in.");
                return BadRequest(new ApiResponse("An error occurred logging in."));
            }
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Sign the user out
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Ok(new ApiResponse(true));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "An error occured logging out.");
                return BadRequest(new ApiResponse("An error occured logging out."));
            }
        }
    }
}