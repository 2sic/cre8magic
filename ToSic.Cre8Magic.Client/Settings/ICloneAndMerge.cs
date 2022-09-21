namespace ToSic.Cre8Magic.Client.Settings;

internal interface ICloneAndMerge<T> where T : class, new()
{
    public T Clone()
    {
        return JsonMerge.JsonMerger.Merge<T>(this as T, null as T);
    }

    public T CloneAndMerge(T merge)
    {
        return JsonMerge.JsonMerger.Merge<T>(this as T, merge);
    }
}