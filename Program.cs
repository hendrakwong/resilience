using resilience;

ISomeUnreliableService  someUnreliableService = new SomeUnreliableService();

IMyFunction myFunction = new MyFunction(someUnreliableService);
// IMyFunction myFunction = new MyResilientFunction(someUnreliableService);
// IMyFunction myFunction = new MyResilientFunctionWithPolly(someUnreliableService);

while (true) // Simulate continuous function calls
{
	await myFunction.CreateMessage($"Polly - {DateTime.UtcNow}");
}