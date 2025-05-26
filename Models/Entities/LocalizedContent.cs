namespace OrphanSystem.Models.Entities;

public class LocalizedContent : BaseEntity
{
    public LocalizedContent()
    { }
    public LocalizedContent(string? en = null, string? ar = null, string? ku = null)
    {
        En = en;
        Ar = ar;
    }
    
    
    public string? En { get; set; }
    public string? Ar { get; set; }
    

    public string? GetLocalized(Lang lang) =>
        lang switch
        {
            Lang.AR => Ar ?? En,
            _ => Ar
        };
}

public enum Lang
{
    EN,
    AR,
}