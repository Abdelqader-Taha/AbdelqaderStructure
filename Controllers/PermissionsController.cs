using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrphanSystem.ActionFilters;
using OrphanSystem.Helpers;
using OrphanSystem.Models.DTOs;
using OrphanSystem.Services;


namespace OrphanSystem.Controllers;

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
