namespace resilience;

public class MyFunction
{
	private readonly ISomeUnreliableService _someUnreliableService;

	// Constructor
	public MyFunction(ISomeUnreliableService someUnreliableService)
	{
		_someUnreliableService = someUnreliableService;
	}

	public async Task<bool> CreateMessage(string message)
	{
		// Request the call to the service
		HttpResponseMessage result = await _someUnreliableService.PostSomething(message);
		
		Console.WriteLine($"**  Status: {result.StatusCode} - Message: {result.Content.ReadAsStringAsync().Result}");

		return true;
	}
}