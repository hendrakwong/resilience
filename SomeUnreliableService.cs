namespace resilience;

using System.Net;

public interface ISomeUnreliableService
{
	Task<HttpResponseMessage> PostSomething(string someParam);
}

public class SomeUnreliableService : ISomeUnreliableService
{
	private const int InstabilityQuotient = 4;
	
	public async Task<HttpResponseMessage> PostSomething(string someParam)
	{
		// Add some time delay to demonstrate the request
		await Task.Delay(1000);

		// Log message when the method is being called 
		Console.WriteLine($"SomeUnreliableService.PostSomething() method is called - Param: '{someParam}'");

		// Generate random exception to simulate the un-stability condition  
		if (new Random().Next(InstabilityQuotient) == 1)
			throw new HttpRequestException("Something wrong happened!!!");

		// Return result
		return new HttpResponseMessage()
		{
			StatusCode = HttpStatusCode.Created,
			Content = new StringContent($"Hello {someParam}")
		};
	}
}