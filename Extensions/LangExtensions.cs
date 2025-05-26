using OrphanSystem.Models.Entities;

namespace OrphanSystem.Extensions;

public static class LangExtensions
{
    public static string ToNameString(this Lang lang)
    {
        return lang switch
        {
            Lang.EN => "en",
            Lang.AR => "ar",
        };
    }
}