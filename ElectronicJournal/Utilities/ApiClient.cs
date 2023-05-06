using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.Utilities
{
	public static class ApiClient
	{
		private const string _port = "https://localhost:7006/api/";
		private static readonly HttpClient _client = new HttpClient();
		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
		{
			//PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
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
			Stream content = await response.Content.ReadAsStreamAsync();
			if (new[] { HttpStatusCode.NotFound, HttpStatusCode.Unauthorized }.Contains(value: response.StatusCode))
			{
				Error error = await JsonSerializer.DeserializeAsync<Error>(utf8Json: content, options: _options);
				throw new ApiException(message: error.PropertyName);
			}

			return await JsonSerializer.DeserializeAsync<T>(utf8Json: content, options: _options);
		}

		public static async Task<string> SendAndGetStringAsync(string apiMethod, string arg = null)
		{
			Uri uri = new Uri(uriString: CreateUrl(apiMethod: apiMethod, arg: arg));
			HttpResponseMessage response = await _client.GetAsync(requestUri: uri);
			string content = await response.Content.ReadAsStringAsync();
			return content;
		}

		private static string CreateUrl(string apiMethod, string arg = null)
			=> $"{_port}{apiMethod}/{arg}";
	}
}
