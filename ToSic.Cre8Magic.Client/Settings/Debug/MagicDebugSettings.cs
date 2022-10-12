namespace ToSic.Cre8Magic.Client.Settings.Debug;

public class MagicDebugSettings
{
    public bool? Allowed { get; set; }
    private const bool AllowedDefault = false;
    public bool? Anonymous { get; set; }
    private const bool AnonymousDefault = false;
    public bool? Admin { get; set; }
    private const bool AdminDefault = true;

    public bool? Detailed { get; set; }
    private const bool DetailedDefault = false;

    public MagicDebugState GetState(object? target, bool isAdmin)
        => (target is not IHasDebugSettings debugTarget
                ? this
                : Merge(this, debugTarget.Debug))
            .Parsed(isAdmin);


    private MagicDebugSettings Merge(MagicDebugSettings master, MagicDebugSettings? slave)
    {
        if (slave == null) return master;
        return new()
        {
            Allowed = master.Allowed, // allowed can only come from master
            Anonymous = Merge(master.Anonymous, slave.Anonymous),
            Admin = Merge(master.Admin, slave.Admin),
        };
    }

    private bool Merge(bool? master, bool? slave) => slave == true || slave == null && master == true;

    internal MagicDebugState Parsed(bool isAdmin)
    {
        if (!(Allowed ?? AllowedDefault)) return new();

        return new() { Show = isAdmin ? Admin ?? AdminDefault : Anonymous ?? AnonymousDefault };
    }

    private static readonly MagicDebugSettings DefaultForAll = new()
    {
        Allowed = false,
        Anonymous = false,
        Admin = false,
    };

    internal static Defaults<MagicDebugSettings> Defaults = new()
    {
        Foundation = DefaultForAll,
        Fallback = DefaultForAll,
    };
}