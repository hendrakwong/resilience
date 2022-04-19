namespace resilience;

public class MyResilientFunction : IMyFunction
{
	private readonly IUnreliableService _unreliableService;
	private const int MaxRetries = 4;
	
	
	// Constructor
	public MyResilientFunction(IUnreliableService unreliableService)
	{
		_unreliableService = unreliableService;
	}

	public async Task<bool> CreateMessage(string message)
	{
		int retryCounter = MaxRetries;
		while (true)
		{
			try
			{
				// Request the call to the service
				HttpResponseMessage result = await _unreliableService.PostSomething(message);
		
				Console.WriteLine($"Status: {result.StatusCode} - Message: {result.Content.ReadAsStringAsync().Result}");

				break;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"** Retry #{retryCounter}");
				if (retryCounter-- == 0) throw;
			}
		}
		
		return true;
	}
}