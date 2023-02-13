using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StatusController : ControllerBase
	{
		private readonly IGrpcStatusClient _client;
		public StatusController(IGrpcStatusClient client)
		{
			_client = client;
		}

		[HttpGet]
		public async Task<IEnumerable<ClientStatusModel>> GetAllStatuses()
		{
			return await _client.GetAllStatuses();
		}

		[HttpGet("{clientName}")]
		public async Task<ClientStatusModel> GetClientstatus(string clientName)
		{
			return await _client.GetClientStatus(clientName);
		}

		[HttpPost("{clientName}/{status}")]
		public async Task<bool> UpdateClientStatus(string clientName, ClientStatus clientStatus)
		{
			return await _client.UpdateClientStatus(clientName, clientStatus);
		}
	}
}
