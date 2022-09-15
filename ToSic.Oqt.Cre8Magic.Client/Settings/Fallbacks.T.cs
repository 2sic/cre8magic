namespace ToSic.Oqt.Cre8Magic.Client.Settings;

internal class Defaults<T> where T: class, new()
{
    public Defaults() {}

    public Defaults(T fallback, T? foundation)
    {
        Fallback = fallback;
        Foundation = foundation;
    }

    public T Fallback { get; set; } = new T();
    public T? Foundation { get; set; } = null;
}