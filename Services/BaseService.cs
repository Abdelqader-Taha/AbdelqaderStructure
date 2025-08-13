using AutoMapper;
using AbdelqaderStructure.Data;

namespace AbdelqaderStructure.Services;

public class BaseService
{
    protected readonly MasterDbContext _context;
    protected readonly IMapper _mapper;

    public BaseService(MasterDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
}