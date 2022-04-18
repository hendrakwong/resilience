namespace resilience;

using Polly;
using Polly.Retry;

public class MyResilientFunctionWithPolly : IMyFunction
{
	private readonly ISomeUnreliableService _someUnreliableService;
	private const int MaxRetries = 4;
	
	public MyResilientFunctionWithPolly(ISomeUnreliableService someUnreliableService)
	{
		_someUnreliableService = someUnreliableService;
	}

	public async Task<bool> CreateMessage(string message)
	{
		AsyncRetryPolicy policy = Policy
			.Handle<HttpRequestException>()
			.RetryAsync(MaxRetries);

		bool result = await policy.ExecuteAsync(async () =>
		{
			// Request the call to the service
			HttpResponseMessage result = await _someUnreliableService.PostSomething(message);
		
			Console.WriteLine($"** Status: {result.StatusCode} - Message: {result.Content.ReadAsStringAsync().Result}");

			return true;
		});

		return result;
	}
}