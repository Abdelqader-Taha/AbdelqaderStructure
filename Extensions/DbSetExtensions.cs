using Microsoft.EntityFrameworkCore;
using AbdelqaderStructure.Helpers;
using AbdelqaderStructure.Models.DTOs;

namespace AbdelqaderStructure.Extensions;

public static class DbSetExtensions
{
    public static async Task<PagedList<T>> Paginate<T>(this IQueryable<T> query, BaseFilter filter)
    {
        if (query == null)
            return null;

        return await PagedList<T>.Create(query, filter.PageNumber, filter.PageSize);
    }
}