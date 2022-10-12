namespace ToSic.Cre8Magic.Client.Logging;

public class LogCall<T> : LogCallBase
{

    internal LogCall(ILog? log, CodeRef code, bool isProp, string parameters = null, string message = null, bool startTimer = false)
        : base(log, code, isProp, parameters, message, startTimer)
    {
    }

    public T Return(T result) => Return(result, null);

    public T Return(T result, string message)
    {
        this.DoneInternal(message);
        return result;
    }
    public T ReturnAndKeepData(T result, string message)
    {
        this.DoneInternal(message);
        if (Entry != null)
            Entry.Data = result;
        return result;
    }

    public T ReturnAndLog(T result) => Return(result, $"{result}");

    public T ReturnNull() => Return(default, null);

    public T ReturnNull(string message) => Return(default, message);
}