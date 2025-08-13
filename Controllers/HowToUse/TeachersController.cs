using AbdelqaderStructure.ActionFilters;
using AbdelqaderStructure.Models.DTOs.TeacherDTOs;
using AbdelqaderStructure.Services.HowToUse;
using Microsoft.AspNetCore.Mvc;
//   Note : This code  is for Guide how to use this structure u can remove it after u learn  .

namespace AbdelqaderStructure.Controllers.HowToUse
{
    [TypeFilter(typeof(SoftDeleteAccessFilterActionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TeacherFilter filter)
        {
            var result = await _teacherService.GetAll(filter);
            return Response(result); 
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _teacherService.GetById(id);
            return Response(result); 
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeacherFrom form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _teacherService.Create(form);
            return Response(result); 
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeacherFrom form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _teacherService.Update(id, form);
            return Response(result); 
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _teacherService.Delete(id);
            return Response(result); 
        }
    }
}
