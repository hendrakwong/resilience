using resilience;

IUnreliableService unreliableService = new UnreliableService();

IMyFunction myFunction = new MyFunction(unreliableService);
// IMyFunction myFunction = new MyResilientFunction(unreliableService);
// IMyFunction myFunction = new MyResilientFunctionWithPolly(unreliableService);

while (true) // Simulate continuous function calls
{
	await myFunction.CreateMessage($"Polly - {DateTime.UtcNow}");
	Console.WriteLine();
}