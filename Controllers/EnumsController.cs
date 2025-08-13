using Microsoft.AspNetCore.Mvc;

namespace AbdelqaderStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllEnums()
        {
            var result = new
            {
               


            };


            return Ok(result);
        }
    }
}
