using Greeter;
using Grpc.Core;

namespace BasicGrpcService.Services
{
	public class GreetingsManagerService : GreetingsManager.GreetingsManagerBase
	{
		public override Task<GreetingResponse> GenerateGreeting(GreetingRequest request, ServerCallContext context)
		{
			return Task.FromResult(new GreetingResponse
			{
				GreetingMessage = "Hello " + request.Name
			});
		}
	}
}
