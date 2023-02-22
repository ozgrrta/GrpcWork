using Grpc.Net.Client;

namespace PerformanceClient
{
	public interface IGrpcPerformanceClient
	{
		Task<ResponseModel.PerformanceStatusModel> GetPerformanceStatus(string clientName);
	}

	internal class GrpcPerformanceClient : IGrpcPerformanceClient, IDisposable
	{
		private readonly GrpcChannel channel;

		public GrpcPerformanceClient(string serverUrl)
		{
			channel = GrpcChannel.ForAddress(serverUrl);
		}

		public async Task<ResponseModel.PerformanceStatusModel> GetPerformanceStatus(string clientName)
		{
			var client = new Performance.Monitor.MonitorClient(channel);

			var response = await client.GetPerformanceAsync(new Performance.PerformanceStatusRequest
			{
				ClientName = clientName
			});

			return new ResponseModel.PerformanceStatusModel
			{
				CpuPercentageUsage = response.CpuPercentageUsage,
				MemoryUsage = response.MemoryUsage,
				ProcessRunning = response.ProcessesRunning,
				ActiveConnections = response.ActiveConnections
			};
		}

		public void Dispose()
		{
			channel.Dispose();
		}
	}
}
