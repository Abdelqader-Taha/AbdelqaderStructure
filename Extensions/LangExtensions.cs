using AbdelqaderStructure.Models.Entities;

namespace AbdelqaderStructure.Extensions;

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