using Grpc.Core;
using Grpc.Net.Client;
using Status;

namespace ApiGateway
{
	public interface IGrpcStatusClient
	{
		Task<IEnumerable<ClientStatusModel>> GetAllStatuses();
		Task<ClientStatusModel> GetClientStatus(string clientName);
		Task<bool> UpdateClientStatus(string clientName, ClientStatus clientStatus);
	}

	public class GrpcStatusClient : IGrpcStatusClient, IDisposable
	{
		private readonly GrpcChannel _channel;
		private readonly StatusManager.StatusManagerClient _client;

		public GrpcStatusClient(string serverUrl)
		{
			_channel = GrpcChannel.ForAddress(serverUrl);
			_client = new StatusManager.StatusManagerClient(_channel);
		}

		public void Dispose()
		{
			_channel.Dispose();
		}

		public async Task<IEnumerable<ClientStatusModel>> GetAllStatuses()
		{
			var statuses = new List<ClientStatusModel>();

			using var call = _client.GetAllStatuses(new ClientStatusesRequest());

			while (await call.ResponseStream.MoveNext())
			{
				var currentStatus = call.ResponseStream.Current;
				statuses.Add(new ClientStatusModel
				{
					Name = currentStatus.ClientName,
					Clientstatus = (ClientStatus)currentStatus.Status
				});
			}

			return statuses;
		}

		public async Task<ClientStatusModel> GetClientStatus(string clientName)
		{
			var response = await _client.GetClientStatusAsync(new ClientStatusReqeust
			{
				ClientName = clientName
			});

			return new ClientStatusModel
			{
				Name = response.ClientName,
				Clientstatus = (ClientStatus)response.Status
			};
		}

		public async Task<bool> UpdateClientStatus(string clientName, ClientStatus clientStatus)
		{
			var response = await _client.UpdateClientResponseAsync(new ClientStatusUpdateRequest
			{
				ClientName = clientName,
				Status = (Status.ClientStatus)clientStatus
			});

			return response.Success;
		}
	}
}
