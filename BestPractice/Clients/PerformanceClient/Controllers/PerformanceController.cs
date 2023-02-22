using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Performance;
using System.Diagnostics;
using static Grpc.Core.Metadata;
using System.Threading.Channels;

namespace PerformanceClient.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PerformanceController : ControllerBase
	{
		private readonly Performance.Monitor.MonitorClient _factoryClient;
		private readonly IGrpcPerformanceClient _clientWrapper;
		private readonly string serverUrl;


		public PerformanceController(Performance.Monitor.MonitorClient factoryClient, IGrpcPerformanceClient clientWrapper, IConfiguration configuration)
		{
			_factoryClient = factoryClient;
			_clientWrapper = clientWrapper;
			serverUrl = configuration["ServerUrl"];
		}

		//	Making a specified number of gRPC calls.
		//	Client is created by framework
		//	WORST PERFORMANCE
		[HttpGet("factory-client/{count}")]
		public async Task<ResponseModel> GetPerformanceFromFactoryClient(int count)
		{
			var stopWatch = Stopwatch.StartNew();
			var response = new ResponseModel();

			for (int i = 0; i < count; i++)
			{
				var grpcResponse = await _factoryClient.GetPerformanceAsync(new PerformanceStatusRequest
				{
					ClientName = $"client {i + 1}"
				});

				response.PerformanceStatuses.Add(new ResponseModel.PerformanceStatusModel
				{
					CpuPercentageUsage = grpcResponse.CpuPercentageUsage,
					MemoryUsage = grpcResponse.MemoryUsage,
					ProcessRunning = grpcResponse.ProcessesRunning,
					ActiveConnections = grpcResponse.ActiveConnections
				});
			}

			response.RequestProcessingTime = stopWatch.ElapsedMilliseconds;

			return response;
		}

		//	Making specified number of gRPC calls.
		//	Using client wrapper to create client for each call. However, each call uses the same channel.
		//	BEST PERFORMANCE
		[HttpGet("client-wrapper/{count}")]
		public async Task<ResponseModel> GetPerformanceFromClientWrapper(int count)
		{
			var stopWatch = Stopwatch.StartNew();
			var response = new ResponseModel();

			for (int i = 0; i < count; i++)
			{
				var grpcResponse = await _clientWrapper.GetPerformanceStatus($"client {i + 1}");

				response.PerformanceStatuses.Add(grpcResponse);
			}

			response.RequestProcessingTime = stopWatch.ElapsedMilliseconds;

			return response;
		}

		//	Here, we are, once again, making a specified number of gRPC calls.
		//	However, we are also creating a new channel and a new client for every call.
		[HttpGet("initialized-client/{count}")]
		public async Task<ResponseModel> GetPerformanceNewClient(int count)
		{
			var stopWatch = Stopwatch.StartNew();
			var response = new ResponseModel();

			for (int i = 0; i < count; i++)
			{
				using var channel = GrpcChannel.ForAddress(serverUrl);
				var client = new Performance.Monitor.MonitorClient(channel);
				var grpcResponse = await client.GetPerformanceAsync(new PerformanceStatusRequest
				{
					ClientName = $"client {count + 1}"
				});

				response.PerformanceStatuses.Add(new ResponseModel.PerformanceStatusModel
				{
					CpuPercentageUsage = grpcResponse.CpuPercentageUsage,
					MemoryUsage = grpcResponse.MemoryUsage,
					ProcessRunning = grpcResponse.ProcessesRunning,
					ActiveConnections = grpcResponse.ActiveConnections
				});
			}

			response.RequestProcessingTime = stopWatch.ElapsedMilliseconds;

			return response;
		}
	}
}