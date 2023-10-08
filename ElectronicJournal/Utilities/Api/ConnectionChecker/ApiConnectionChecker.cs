using ElectronicJournalAPI;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.Utilities.Api.ConnectionChecker
{
	public class ApiConnectionChecker : IConnectionChecker
	{
		private class HealthResponse
		{
			public enum HealthStatus
			{
				Healthy,
				Degraded,
				Unhealthy
			}

			[JsonConverter(converterType: typeof(JsonStringEnumConverter))]
			public HealthStatus Status { get; set; }
		}

		public async Task<bool> CheckConnection()
		{
			try
			{
				HealthResponse response = await ApiClient.GetAsync<HealthResponse>(new Uri(uriString: "https://localhost:7006/state/api"));
				return response.Status.Equals(HealthResponse.HealthStatus.Healthy);
			} catch (Exception ex)
			{
				return false;
			}
		}
	}
}
