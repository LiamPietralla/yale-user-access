using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using YaleAccess.Models;
using YaleAccess.Models.Options;
using YaleAccess.Services;
using YaleAccess.Services.Interfaces;

namespace YaleAccess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class YaleController : ControllerBase
    {
        private readonly IYaleAccessor _yaleAccessor;
        private readonly CodesOptions _codeOptions;

        public YaleController(IYaleAccessor yaleAccessor, IOptions<CodesOptions> codeOptions)
        {
            _yaleAccessor = yaleAccessor;
            _codeOptions = codeOptions.Value;
        }

        [HttpGet("codes")]
        public async Task<IActionResult> GetUserCodes()
        {
            try
            {
                // Get the home code first
                YaleUserCode homeCode = await _yaleAccessor.GetCodeInformationAsync(_codeOptions.Home);
                homeCode.IsHome = true;

                // Get the guest codes
                List<YaleUserCode> guestCodes = new();
                foreach (int code in Enumerable.Range(_codeOptions.GuestCodeRangeStart, _codeOptions.GuestCodeRangeCount))
                {
                    guestCodes.Add(await _yaleAccessor.GetCodeInformationAsync(code));
                }

                // Add the home code to the list
                guestCodes.Add(homeCode);

                // Return the codes
                return Ok(new ApiResponse(guestCodes));
            }
            catch(Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred retriving the codes.");
                return BadRequest(new ApiResponse("An error occurred retriving the codes."));
            }
        }

        [HttpPost("code/{id}")]
        public async Task<IActionResult> SetUserCode([FromRoute] int id, [FromBody] string newCode)
        {
            try
            {
                // First validate the user code
                string validCode = YaleAccessor.ValidateCode(newCode);
                if (validCode != string.Empty)
                {
                    return BadRequest(new ApiResponse(validCode));
                }

                // Set the new code
                bool result = await _yaleAccessor.SetUserCode(id, newCode);

                // Return the result
                if (result)
                {
                    Log.Logger.Information("Updated code for user {id} to {code}", id, newCode);
                    return Ok(new ApiResponse(true));
                }
                else
                {
                    Log.Logger.Information("Failed to update code for user {id} to {code}", id, newCode);
                    return BadRequest(new ApiResponse("An error occurred setting the code."));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred setting the code.");
                return BadRequest(new ApiResponse("An error occurred setting the code."));
            }
        }

        [HttpPost("code/{id}/status")]
        public async Task<IActionResult> SetUserCodeStatusAsAvailable([FromRoute] int id)
        {
            try
            {
                // First validate the user code
                string validCode = YaleAccessor.ValidateClearCode(id, _codeOptions.Home);
                if (validCode != string.Empty)
                {
                    return BadRequest(new ApiResponse(validCode));
                }

                // Set the available status
                bool result = await _yaleAccessor.SetCodeAsAvailable(id);

                // Return the result
                if (result)
                {
                    Log.Logger.Information("Updated code status for user {id} to available", id);
                    return Ok(new ApiResponse(true));
                }
                else
                {
                    Log.Logger.Information("Failed to update code status for user {id} to available", id);
                    return BadRequest(new ApiResponse("An error occurred setting the code status."));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred setting the code status.");
                return BadRequest(new ApiResponse("An error occurred setting the code status."));
            }
        }
    }
}