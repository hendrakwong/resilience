namespace resilience;

using Polly;
using Polly.Retry;

public class MyResilienceFunction
{
	private readonly ISomeUnreliableService _someUnreliableService;
	
	// Constructor
	public MyResilienceFunction(ISomeUnreliableService someUnreliableService)
	{
		_someUnreliableService = someUnreliableService;
	}

	public async Task<bool> CreateMessage(string message)
	{
		AsyncRetryPolicy policy = Policy
			.Handle<HttpRequestException>()
			.RetryAsync(4);

		bool result = await policy.ExecuteAsync(async () =>
		{
			HttpResponseMessage result = await _someUnreliableService.PostSomething(message);
		
			Console.WriteLine($"** Status: {result.StatusCode} - Message: {result.Content.ReadAsStringAsync().Result}");

			return true;
		});

		return result;
	}
}