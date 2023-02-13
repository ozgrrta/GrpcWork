namespace StatusMicroservice
{
	public interface IStateStore
	{
		IEnumerable<(string ClientName, ClientStatus ClientStatus)> GetAllStatuses();
		ClientStatus GetStatus(string clientName);
		bool UpdateStatus(string clientName, ClientStatus clientStatus);
	}

	internal class StateStore : IStateStore
	{
		private Dictionary<string, ClientStatus> _statuses;

		public StateStore()
		{
			_statuses = new Dictionary<string, ClientStatus>();
		}

		public IEnumerable<(string ClientName, ClientStatus ClientStatus)> GetAllStatuses()
		{
			var returnedStatuses = new List<(string ClientName, ClientStatus ClientStatus)>();

			foreach (var record in _statuses)
			{
				returnedStatuses.Add((record.Key, record.Value));
			}

			return returnedStatuses;
		}

		public ClientStatus GetStatus(string clientName)
		{
			if (!_statuses.ContainsKey(clientName))
			{
				return ClientStatus.OFFLINE;
			}

			return _statuses[clientName];
		}

		public bool UpdateStatus(string clientName, ClientStatus clientStatus)
		{
			_statuses[clientName] = clientStatus;

			return true;
		}
	}
}
