using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrphanSystem.Helpers;
using OrphanSystem.Models.DTOs.Auth;
using OrphanSystem.Services;

namespace OrphanSystem.Controllers
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

        
        [Authorize]
        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordFormDTO form)
        {
            await _authService.ResetPassword(CurId, form);
            return Ok(new Response<object>(null, null, 200));
        }
        #endregion

        #region OAuth
        #endregion
    }
}
