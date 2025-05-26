using OrphanSystem.Models.Entities;

namespace OrphanSystem.Extensions;

public static class RoleTypeExtensions
{
    public static string ToRoleString(this StaticRole staticRole)
    {
        switch (staticRole)
        {
            case StaticRole.SUPER_ADMIN:
                return "super-admin";
            default:
                return "UNDEFINED";
        }
    }
}