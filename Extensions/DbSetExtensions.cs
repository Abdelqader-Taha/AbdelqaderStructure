using Microsoft.EntityFrameworkCore;
using OrphanSystem.Helpers;
using OrphanSystem.Models.DTOs;

namespace OrphanSystem.Extensions;

public static class DbSetExtensions
{
    public static async Task<PagedList<T>> Paginate<T>(this IQueryable<T> query, BaseFilter filter)
    {
        if (query == null)
            return null;

        return await PagedList<T>.Create(query, filter.PageNumber, filter.PageSize);
    }
}