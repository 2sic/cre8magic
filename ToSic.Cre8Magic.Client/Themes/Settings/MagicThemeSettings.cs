﻿using static ToSic.Cre8Magic.Client.MagicConstants;

namespace ToSic.Cre8Magic.Client.Themes.Settings;

public class MagicThemeSettings: SettingsWithInherit, IHasDebugSettings
{
    /// <summary>
    /// The logo to show, should be located in the assets subfolder
    /// </summary>
    public string? Logo { get; set; }

    public int LanguagesMin { get; set; }

    /// <summary>
    /// The parts of this theme, like breadcrumbs and various menu configs
    /// </summary>
    public NamedSettings<MagicThemePartSettings> Parts { get; set; } = new();


    public bool? MagicContextInBody { get; set; }

    public string? Design { get; set; }

    internal MagicThemeSettings Parse(ITokenReplace tokens)
    {
        Logo = tokens.Parse(Logo);
        return this;
    }

    public static MagicThemeSettings Fallback = new()
    {
        Logo = "unknown-logo.png",
        LanguagesMin = 2,
        MagicContextInBody = false,
        Design = InheritName,
    };

    internal static Defaults<MagicThemeSettings> Defaults = new()
    {
        Fallback = Fallback,
        Foundation = Fallback
    };

    public MagicDebugSettings? Debug { get; set; }
}