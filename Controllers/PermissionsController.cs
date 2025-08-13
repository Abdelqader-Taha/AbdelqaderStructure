using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AbdelqaderStructure.ActionFilters;
using AbdelqaderStructure.Helpers;
using AbdelqaderStructure.Models.DTOs;
using AbdelqaderStructure.Services;


namespace AbdelqaderStructure.Controllers;

[TypeFilter(typeof(ModelValidationActionFilter))]
public class PermissionsController : BaseController
{
	private readonly IPermissionsService _permissionsService;

	public PermissionsController(IPermissionsService permissionsService)
	{
		_permissionsService = permissionsService;
	}

	#region Read
	[TypeFilter(typeof(SoftDeleteAccessFilterActionFilter))]
	[HttpGet]
	public async Task<ActionResult<Response<PagedList<PermissionDTO>>>> GetAll([FromQuery] PermissionFilter filter) =>
		Ok(await _permissionsService.GetAll(filter));

	[HttpGet("{id}")]
	public async Task<ActionResult<Response<PermissionDTO>>> GetById(Guid id) =>
		Ok(await _permissionsService.GetById(id));
	#endregion
}
