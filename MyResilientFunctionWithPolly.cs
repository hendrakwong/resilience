namespace resilience;

using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;

public class MyResilientFunctionWithPolly : IMyFunction
{
	private readonly IUnreliableService _unreliableService;
	private const int MaxRetries = 4;
	
	public MyResilientFunctionWithPolly(IUnreliableService unreliableService)
	{
		_unreliableService = unreliableService;
	}

	public async Task<bool> CreateMessage(string message)
	{
		AsyncRetryPolicy retryPolicy = Policy
			.Handle<HttpRequestException>()
			.RetryAsync(MaxRetries, (exception, retryCounter) =>
			{
				// Ability to add action during OnRetry, e.g. log retry action
				Console.WriteLine($"** Retry #{retryCounter}");
			});

		AsyncRetryPolicy? waitAndRetryPolicy = Policy
			.Handle<HttpRequestException>()
			.WaitAndRetryAsync(MaxRetries, 
				retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
				(exception, timeSpan, retryCounter, context) => 
				{ 
					// Ability to add action during OnRetry, e.g. log retry action
					Console.WriteLine($"** Retry #{retryCounter} - TimeSpan: {timeSpan}");
				});
		
		var constantBackoff = 
			Backoff.ConstantBackoff(TimeSpan.FromMilliseconds(200), MaxRetries);
		var linearBackOff = 
			Backoff.LinearBackoff(TimeSpan.FromMilliseconds(200), MaxRetries, 2);
		var exponentialBackoff = 
			Backoff.ExponentialBackoff(TimeSpan.FromMilliseconds(200), MaxRetries, 2);
		var jitterBackoff =
			Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(200), MaxRetries);
		AsyncRetryPolicy? advancedWaitAndRetryPolicy = Policy
			.Handle<HttpRequestException>()
			.WaitAndRetryAsync(jitterBackoff, 
				(exception, timeSpan, retryCounter, context) => 
				{
					// Ability to add action during OnRetry, e.g. log retry action
					Console.WriteLine($"** Retry #{retryCounter} - TimeSpan: {timeSpan}");
				});
		
		AsyncTimeoutPolicy? timeOutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(2));

		AsyncPolicyWrap? retryWithBackOffAndOverallTimeOut = timeOutPolicy.WrapAsync(advancedWaitAndRetryPolicy);

		// Execution
		bool result = await retryPolicy.ExecuteAsync(async () =>
		{
			// Request the call to the service
			HttpResponseMessage result = await _unreliableService.PostSomething(message);
		
			Console.WriteLine($"Status: {result.StatusCode} - Message: {result.Content.ReadAsStringAsync().Result}");

			return true;
		});

		return result;
	}
}