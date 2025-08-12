using OrphanSystem.Models.Entities;

namespace OrphanSystem.Extensions;

public static class RoleTypeExtensions
{
    public static string ToRoleString(this StaticRole staticRole)
    {
        switch (staticRole)
        {
            case StaticRole.SuperAdmin:
                return "super-admin";
            case StaticRole.Admin:
                return "Admin";
            
            default:
                return "UNKNOWN";
        }
    }
}