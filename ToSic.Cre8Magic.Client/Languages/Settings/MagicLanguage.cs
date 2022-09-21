using System.Globalization;

namespace ToSic.Oqt.Cre8Magic.Client.Languages.Settings;

public class MagicLanguage
{
    /// <summary>
    /// Empty constructor for deserialization
    /// </summary>
    public MagicLanguage() { }

    //public MagicLanguage(string culture, string? label = null, string? description = null)
    //{
    //    Culture = culture;
    //    Label = label;
    //    Description = description;
    //}

    public string? Culture { get; set; }

    /// <summary>
    /// Label to show for this culture.
    /// Will auto-default to first two characters. 
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Description to show for this language.
    /// Will auto-default to the system name for this language. 
    /// </summary>
    public string? Description { get; set; }

    public bool IsActive => CultureInfo.CurrentUICulture.Name == Culture;

    // TODO: MAYBE additional options to only enable on certain roles...?

}