using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AbdelqaderStructure.Helpers;
using AbdelqaderStructure.Models.DTOs.Auth;
using AbdelqaderStructure.Null;
using AbdelqaderStructure.Services;
using AbdelqaderStructure.Models.DTOs.Auth.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace AbdelqaderStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly PhoneNumberNormalizer _phoneNumberNormalizer;

        public AuthController(IAuthService authService, PhoneNumberNormalizer phoneNumberNormalizer)
        {
            _authService = authService;
            _phoneNumberNormalizer = phoneNumberNormalizer;
        }



        [HttpGet("Get-All-Users")]
        public async Task<IActionResult> GetAll([FromQuery] Filter filter)
        {
            var response = await _authService.GetAll(filter);
            return StatusCode(response.StatusCode, response);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserForm form)
        {
            form.Id = id; // Ensure the ID is assigned
            var result = await _authService.Update(form);
            return StatusCode(result.StatusCode, result);
        }


     
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _authService.Delete(id);
            if (result.StatusCode == 404)
                return NotFound(result.Msg);

            if (result.StatusCode == 401)
                return Forbid(result.Msg);

            return Ok(result.Msg);

        }
        #region Register
        [HttpPost("register")]
        public async Task<ActionResult<Response<string>>> Register([FromBody] RegisterFormDTO form)
        {
            form.Phone = _phoneNumberNormalizer.Normalize(form.Phone);
            form.PhoneCountryCode = _phoneNumberNormalizer.NormalizeCountryCode(form.PhoneCountryCode);
            return Ok(await _authService.Register(form));
        }
        #endregion

        #region Auth  
        [HttpPost("login")]
        public async Task<ActionResult<Response<string>>> Login([FromBody] LoginFormDTO form)
        {
            form.Phone = _phoneNumberNormalizer.Normalize(form.Phone);
            form.PhoneCountryCode = _phoneNumberNormalizer.NormalizeCountryCode(form.PhoneCountryCode);
            return Ok(await _authService.Login(form));
        }

        [HttpPost("reset-password/{id}")]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordFormDTO form)
        {
            try
            {
                await _authService.ResetPassword(id, form);
                return Ok(new { msg = "Password reset successful." });
            }
            catch (ErrResponseException ex) // This is your custom exception thrown by ErrResponseThrower
            {
                // Return your custom error message with appropriate HTTP status code
                if (ex.Message == "WRONG_PASSWORD")
                    return Unauthorized(new { msg = "Old password is incorrect." });
                else if (ex.Message == "USER_NOT_FOUND")
                    return NotFound(new { msg = "User not found." });

                // Other known error messages can be handled here
                return BadRequest(new { msg = ex.Message });
            }
            catch (Exception)
            {
                // Unexpected errors
                return StatusCode(500, new { msg = "Internal server error." });
            }
        }

        #endregion

        #region OAuth
        #endregion
    }
}
