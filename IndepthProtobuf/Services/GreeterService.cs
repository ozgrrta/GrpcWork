using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IndepthProtobuf;

namespace IndepthProtobuf.Services
{
	public class GreeterService : Greeter.GreeterBase
	{
		private readonly ILogger<GreeterService> _logger;
		public GreeterService(ILogger<GreeterService> logger)
		{
			_logger = logger;
		}

		public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
		{
			//return Task.FromResult(new HelloReply
			//{
			//	Message = "Hello " + request.Name
			//});

			var message = new HelloReply
			{
				Message = "Hello " + request.Name,
				NestedMessageField = new HelloReply.Types.NestedMessage()
			};

			message.NestedMessageField.StringCollection.Add("entry 1");
			message.NestedMessageField.StringCollection.Add(new List<string>
				{
					"entry 2",
					"entry 3"
				});

			#region adding items to a MapField collection in various ways
			//	As a singular key-value pair
			message.NestedMessageField.StringToStringMap.Add("entry 1", "value 1");
			//	As a collection of key-value pairs
			message.NestedMessageField.StringToStringMap.Add(new Dictionary<string, string>
				{
					{ "entry 2", "value 2" },
					{ "entry 3", "value 3" },
				});
			//	Specifying a key and setting its value
			message.NestedMessageField.StringToStringMap["entry 4"] = "value 4";
			#endregion


			#region demonstration of setting oneof field
			message.BasicTypes2Field = new BasicTypes2
			{
				IntField = 1
			};
			#endregion


			#region MyRegion

			#endregion


			return Task.FromResult(message);
		}
	}
}