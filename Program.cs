using resilience;

ISomeUnreliableService  someUnreliableService = new SomeUnreliableService();

// MyFunction myFunction = new MyFunction(someUnreliableService);
MyResilienceFunction myResilienceFunction = new MyResilienceFunction(someUnreliableService);

while (true) // Simulate continuous function calls
{
	// await myFunction.CreateMessage($"Polly - {DateTime.UtcNow}");
	
	await myResilienceFunction.CreateMessage($"Polly - {DateTime.UtcNow}");
}
