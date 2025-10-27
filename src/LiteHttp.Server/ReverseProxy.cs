namespace LiteHttp.Server;

using LiteHttp.Models.Events;

public class ReverseProxy(
    ServerWorker[] workerPool
    ) : IReverseProxy<RequestReceivedEvent>
{
    public void Proxy(RequestReceivedEvent @event, CancellationToken ct)
    {
        while (true)
        {
            for (int i = 0; i < workerPool.Length; i++)
            {
                if (workerPool[i].Status == WorkerStatus.Waiting)
                {
                    workerPool[i].HandleEvent(@event, ct);
                    return;
                }
            }
        }
    }
}