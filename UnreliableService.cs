namespace resilience;

using System.Net;

public interface IUnreliableService
{
	Task<HttpResponseMessage> PostSomething(string someParam);
}

public class UnreliableService : IUnreliableService
{
	private const int InstabilityQuotient = 3;
	
	public async Task<HttpResponseMessage> PostSomething(string someParam)
	{
		// Add some time delay to demonstrate the request
		await Task.Delay(1000);

		// Log message when the method is being called 
		Console.WriteLine($"** UnreliableService.PostSomething() method is called - Param: '{someParam}'");

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