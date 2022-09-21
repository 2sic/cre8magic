namespace ToSic.Cre8Magic.Client.Settings;

public class MagicDebugSettings
{
    public bool? Allowed { get; set; }
    private const bool AllowedDefault = false;
    public bool? Anonymous { get; set; }
    private const bool AnonymousDefault = false;
    public bool? Admin { get; set; }
    private const bool AdminDefault = true;

    public MagicDebugState GetState(object? target, bool isSuperUser)
        => (target is not IHasDebugSettings debugTarget
                ? this
                : Merge(this, debugTarget.Debug))
            .Parsed(isSuperUser);


    private MagicDebugSettings Merge(MagicDebugSettings master, MagicDebugSettings? slave)
    {
        if (slave == null) return master;
        return new()
        {
            Allowed = master.Allowed, // allowed can only come from master
            Anonymous = Merge(master.Anonymous, slave.Anonymous), // slave.Anonymous == true || (slave.Anonymous == null && master.Anonymous == true),
            Admin = Merge(master.Admin, slave.Admin), // slave.Admin == true || (slave.Admin == null && master.Admin == true),
        };
    }

    private bool Merge(bool? master, bool? slave) => slave == true || (slave == null && master == true);

    internal MagicDebugState Parsed(bool isSuperUser)
    {
        var settingForAnonymous = Anonymous ?? AnonymousDefault;
        var settingForAdmin = Admin ?? AdminDefault;
        var settingCurrentUser = isSuperUser ? settingForAdmin : settingForAnonymous;

        return new() { Show = settingCurrentUser && (Allowed ?? AllowedDefault) };
    }

    private static readonly MagicDebugSettings DefaultForAll = new()
    {
        Allowed = false,
        Anonymous = false,
        Admin = false,
    };

    internal static Defaults<MagicDebugSettings> Defaults = new ()
    {
        Foundation = DefaultForAll,
        Fallback = DefaultForAll,
    };
}