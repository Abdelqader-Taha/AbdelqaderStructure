using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AbdelqaderStructure.Models.DTOs;
using AbdelqaderStructure.ActionFilters;
using AbdelqaderStructure.Helpers;
using AbdelqaderStructure.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace AbdelqaderStructure.Controllers;

public class RolesController : BaseController
{
	private readonly IRolesService _rolesService;
	public RolesController(IRolesService rolesService)
	{
		_rolesService = rolesService;
	}

	#region CRUDs
	[TypeFilter(typeof(SoftDeleteAccessFilterActionFilter))]
	[HttpGet]
	[Tags("Roles")]

    [HttpGet("{id}")]
    [SwaggerOperation(
            Summary = "get specific role by id ",
            Description = "this is the description  you type here what  you want here"
        )]
    public async Task<ActionResult<Response<PagedList<RoleDTO>>>> GetAll([FromQuery] RoleFilter filter) =>
		Ok(await _rolesService.GetAll(filter));

	[HttpGet("{id}")]
	public async Task<ActionResult<Response<RoleDTO>>> GetById(Guid id) =>
		Ok(await _rolesService.GetById(id));

	[HttpPost]
	public async Task<ActionResult<Response<RoleDTO>>> Create([FromBody] RoleFormDTO form) =>
		Ok(await _rolesService.Create(form));

	[HttpPut("{id}")]
	public async Task<ActionResult<Response<RoleDTO>>> Update([FromBody] RoleUpdateDTO update, Guid id)
	{
		update.Id = id;
		await _rolesService.Update(update);
			
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(204)]
	public async Task<IActionResult> Delete(Guid id)
	{
	    await _rolesService.Delete(id);
	    return Ok();
	}
	#endregion
}
