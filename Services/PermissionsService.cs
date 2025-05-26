using AutoMapper;
using AutoMapper.QueryableExtensions;
using OrphanSystem.Models.DTOs;
using OrphanSystem.Data;
using OrphanSystem.Extensions;
using OrphanSystem.Helpers;
using OrphanSystem.Models.Entities;

namespace OrphanSystem.Services;

public interface IPermissionsService
{
	Task<Response<PagedList<PermissionDTO>>> GetAll(PermissionFilter filter);
	Task<Response<PermissionDTO>> GetById(Guid id);
}

public class PermissionsService : BaseService, IPermissionsService
{
	public PermissionsService(MasterDbContext context, IMapper mapper) : base(context, mapper)
	{
	}

	public async Task<Response<PagedList<PermissionDTO>>> GetAll(PermissionFilter filter)
	{
		var permissions = await _context.Permissions
			.WhereBaseFilter(filter)
			.OrderByCreationDate()
			.ProjectTo<PermissionDTO>(_mapper.ConfigurationProvider)
			.Paginate(filter);

		return new Response<PagedList<PermissionDTO>>(permissions, null, 200);
	}

	public async Task<Response<PermissionDTO>> GetById(Guid id)
	{
		var dto = await _context.GetByIdOrException<Permission, PermissionDTO>(id);
		return new Response<PermissionDTO>(dto, null, 200);
	}
}