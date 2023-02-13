using Grpc.Core;
using Status;

namespace StatusMicroservice.Services
{
	public class StatusManagerService : StatusManager.StatusManagerBase
	{
		private readonly IStateStore _stateStore;

		public StatusManagerService(IStateStore stateStore)
		{
			_stateStore = stateStore;
		}

		public override async Task GetAllStatuses(ClientStatusesRequest request, IServerStreamWriter<ClientStatusResponse> responseStream, ServerCallContext context)
		{
			foreach (var record in _stateStore.GetAllStatuses())
			{
				await responseStream.WriteAsync(new ClientStatusResponse
				{
					ClientName = record.ClientName,
					Status = (Status.ClientStatus)record.ClientStatus,
					
				});
			}
		}

		public override Task<ClientStatusResponse> GetClientStatus(ClientStatusReqeust request, ServerCallContext context)
		{
			return Task.FromResult(new ClientStatusResponse
			{
				ClientName = request.ClientName,
				Status = (Status.ClientStatus)_stateStore.GetStatus(request.ClientName)
			});
		}

		public override Task<ClientStatusUpdateResponse> UpdateClientResponse(ClientStatusUpdateRequest request, ServerCallContext context)
		{
			return Task.FromResult(new ClientStatusUpdateResponse
			{
				Success = _stateStore.UpdateStatus(request.ClientName, (ClientStatus)request.Status)
			});
		}
	}
}
