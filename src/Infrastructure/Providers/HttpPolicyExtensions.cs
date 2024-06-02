using System.Net;
using Polly;

namespace Infrastructure.Providers;

public static class HttpPolicyExtensions
{
    public static IAsyncPolicy<HttpResponseMessage> GetRateLimitPolicy()
    {
        return Policy
            .HandleResult<
                HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.TooManyRequests) // HTTP 429 Too Many Requests
            .OrResult(r => r.Headers.Contains("X-MBX-USED-WEIGHT-1M"))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: (retryAttempt, response, context) =>
                {
                    if (response.Result.StatusCode == HttpStatusCode.TooManyRequests &&
                        response.Result.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                    {
                        // Use Retry-After header value if available
                        var retryAfter = retryAfterValues.First();
                        if (int.TryParse(retryAfter, out var retryAfterSeconds))
                        {
                            return TimeSpan.FromSeconds(retryAfterSeconds);
                        }
                    }

                    // Default delay for retry
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                },
                onRetryAsync: async (response, timespan, retryAttempt, context) =>
                {
                    // Log or handle retry logic here
                    var headers = response.Result.Headers;
                    if (headers.Contains("X-MBX-USED-WEIGHT-1M"))
                    {
                        var usedWeight = int.Parse(headers.GetValues("X-MBX-USED-WEIGHT-1M").First());
                        if (usedWeight >= 6000)
                        {
                            // Log or handle reaching the rate limit here
                            var waitTime = TimeSpan.FromMinutes(1);
                            await Task.Delay(waitTime);
                        }
                    }

                    await Task.Delay(timespan);
                });
    }
}