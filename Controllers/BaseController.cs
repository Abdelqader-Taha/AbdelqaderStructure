using AbdelqaderStructure.ActionFilters;
using AbdelqaderStructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AbdelqaderStructure.Controllers;

[TypeFilter(typeof(ModelValidationActionFilter))]
[TypeFilter(typeof(AdminDynamicAuthActionFilter))]
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected string GetClaimValue(string claimType)
    {
        var claims = (User.Identity as ClaimsIdentity)?.Claims;
        var claim = claims?.FirstOrDefault(c =>
            string.Equals(c.Type, claimType, StringComparison.CurrentCultureIgnoreCase) &&
            !string.Equals(c.Type, "null", StringComparison.CurrentCultureIgnoreCase));
        var rr = claim?.Value!.Replace("\"", "");

        return rr ?? "";
    }

    protected Guid CurId => Guid.Parse(GetClaimValue(ClaimTypes.NameIdentifier));
    protected string CurRole => GetClaimValue(ClaimTypes.Role);
    protected IActionResult Response<T>(Response<T> result) =>
     StatusCode(result.StatusCode, result);
}