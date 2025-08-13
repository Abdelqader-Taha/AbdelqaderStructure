using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AbdelqaderStructure.Models.DTOs;

namespace AbdelqaderStructure.ActionFilters;

public class SoftDeleteAccessFilterActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("filter", out var filterObj) && filterObj is BaseFilter filter)
        {
            if (filter.IsDeleted == null || filter.IsDeleted != false)
            {
                filter.IsDeleted = false;
            }
        }
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //*_*
    }
}