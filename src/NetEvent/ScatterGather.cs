using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetEvent
{
    public static class ScatterGather<TRequest, TResponse>
    {
        internal static readonly ConcurrentBag<Func<TRequest, Task<TResponse>>> Subscriptions = new ConcurrentBag<Func<TRequest, Task<TResponse>>>();
        public static void SubscribeAsync(Func<TRequest, Task<TResponse>> subscription)
        {
            Subscriptions.Add(subscription);
        }
        public static void RegisterHandler(Func<TRequest, TResponse> subscription)
        {
            Task<TResponse> Wrapper(TRequest m) => Task.FromResult(subscription(m));
            Subscriptions.Add(Wrapper);
        }

        public static Task<TResponse[]> Request(TRequest request, TimeSpan timeout)
        {
            var cts = new CancellationTokenSource(timeout);
            return Request(request, cts.Token);
        }

        public static async Task<TResponse[]> Request(TRequest request, CancellationToken ct)
        {
            var l = Subscriptions.Select(sub => sub(request)).ToList();

            var all = Task.WhenAll(l);
            await Task.WhenAny(all, Task.Delay(Timeout.Infinite, ct));
            var results = l.Where(t => t.Status == TaskStatus.RanToCompletion).Select(t => t.Result).ToArray();
            return results;
        }
    }
}
