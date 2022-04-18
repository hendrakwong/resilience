namespace resilience;

public class MyResilientFunction : IMyFunction
{
	private readonly ISomeUnreliableService _someUnreliableService;
	private const int MaxRetries = 4;
	
	
	// Constructor
	public MyResilientFunction(ISomeUnreliableService someUnreliableService)
	{
		_someUnreliableService = someUnreliableService;
	}

	public async Task<bool> CreateMessage(string message)
	{
		int retryCounter = MaxRetries;
		while (true)
		{
			try
			{
				// Request the call to the service
				HttpResponseMessage result = await _someUnreliableService.PostSomething(message);
		
				Console.WriteLine($"**  Status: {result.StatusCode} - Message: {result.Content.ReadAsStringAsync().Result}");

				break;
			}
			catch (HttpRequestException e)
			{
				if (retryCounter-- == 0) throw;
			}
		}
		
		return true;
	}
}