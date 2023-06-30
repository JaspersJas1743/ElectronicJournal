using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElectronicJournal.Utilities.Api
{
	public static class ApiClient
	{
		private const string _serverAddress = "https://localhost:7006/api/";

		private static readonly HttpClient _client = new HttpClient();
		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true,
			WriteIndented = true
		};

		static ApiClient()
		{
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Add("Accept", "application/json");
		}

		public static void SetTokenForAuthorization(string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				scheme: "Bearer", parameter: token.Trim(trimChars: " \"".ToCharArray())
			);
		}

		public static async Task<T> SendAndGetAsync<T>(string apiMethod, string arg = null)
		{
			Uri uri = new Uri(uriString: CreateUrl(apiMethod: apiMethod, arg: arg));
			HttpResponseMessage response = await _client.GetAsync(requestUri: uri);
			await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
			return await JsonSerializer.DeserializeAsync<T>(utf8Json: await response.Content.ReadAsStreamAsync(), options: _options);
		}

		private static string CreateUrl(string apiMethod, string arg = null)
			=> $"{_serverAddress}{apiMethod}/{arg}";
	}
}
