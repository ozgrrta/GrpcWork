namespace PerformanceClient
{
	public class ResponseModel
	{
		public List<PerformanceStatusModel> PerformanceStatuses { get; } = new();
		public double RequestProcessingTime { get; set; }
		public class PerformanceStatusModel
		{
			public double CpuPercentageUsage { get; set; }
			public double MemoryUsage { get; set; }
			public int ProcessRunning { get; set; }
			public int ActiveConnections { get; set; }
		}
	}
}