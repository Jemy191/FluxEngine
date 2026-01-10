using System.Collections.Concurrent;

namespace Flux.Core;

public class MainThreadThrower
{
    readonly ConcurrentBag<Exception> exceptions = [];
    public void EnqueueException(Exception exception) => exceptions.Add(exception);
    public void ProcessExceptions()
    {
        if(exceptions.IsEmpty)
            return;
        
        throw new AggregateException("Last frame some exceptions where thrown", exceptions);
    }
}