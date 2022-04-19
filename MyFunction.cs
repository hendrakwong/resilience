namespace resilience;

public interface IMyFunction
{
	Task<bool> CreateMessage(string message);
}

public class MyFunction : IMyFunction
{
	private readonly IUnreliableService _unreliableService;

	// Constructor
	public MyFunction(IUnreliableService unreliableService)
	{
		_unreliableService = unreliableService;
	}

	public async Task<bool> CreateMessage(string message)
	{
		// Request the call to the service
		HttpResponseMessage result = await _unreliableService.PostSomething(message);
		
		Console.WriteLine($"Status: {result.StatusCode} - Message: {result.Content.ReadAsStringAsync().Result}");

		return true;
	}
}