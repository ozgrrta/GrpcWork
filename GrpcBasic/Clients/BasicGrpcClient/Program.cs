using Greeter;
using Grpc.Net.Client;

// The port number(5001) must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new GreetingsManager.GreetingsManagerClient(channel);

while (true)
{
	Console.WriteLine("Type the name of the person you want to greet");
	var name = Console.ReadLine();
	var reply = await client.GenerateGreetingAsync(new GreetingRequest { Name = name });
	Console.WriteLine("Greeting: " + reply.GreetingMessage);
	
	Console.WriteLine("Press  key to exit...");
	var key = Console.ReadKey();
	if (key.Key == ConsoleKey.Escape)
	{
		break;
	}
}
