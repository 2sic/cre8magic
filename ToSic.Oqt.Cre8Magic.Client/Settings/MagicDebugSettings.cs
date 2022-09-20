namespace ToSic.Oqt.Cre8Magic.Client.Settings;

public class MagicDebugSettings
{
    public bool? Allowed { get; set; }
    private const bool AllowedDefault = false;
    //public bool? Enabled { get; set; }
    public bool? Anonymous { get; set; }
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
            Anonymous = master.Anonymous == true || slave.Anonymous == true,
            //Enabled = master.Enabled == true || slave.Enabled == true,
            Admin = master.Admin == true || slave.Admin == true,
        };
    }

    internal MagicDebugState Parsed(bool isSuperUser)
    {
        var onForAnonymous = Anonymous ?? false;
        var onForCurrentUser = isSuperUser || onForAnonymous;

        var allow = onForCurrentUser && (Allowed ?? AllowedDefault);
        //var enable = allow && (Enabled ?? false);

        return new()
        {
            //Allowed = allow,
            //Enabled = enable,
            Show = allow && (Admin ?? AdminDefault),
            //Anonymous = enable && onForAnonymous,
        };
    }

    private static readonly MagicDebugSettings DefFandF = new()
    {
        Allowed = false,
        //Enabled = false,
        Anonymous = false,
        Admin = false,
    };

    internal static Defaults<MagicDebugSettings> Defaults = new ()
    {
        Foundation = DefFandF,
        Fallback = DefFandF,
    };
}