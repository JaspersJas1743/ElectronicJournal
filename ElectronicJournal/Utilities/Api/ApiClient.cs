using ElectronicJournal.Resources.Windows;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElectronicJournal.Utilities.Api
{
	// https://stackoverflow.com/questions/5405895/how-to-check-the-internet-connection-with-net-c-and-wpf
	// Проверка подключения к интернету
	public static class ApiClient
	{
		#region Fields
		private const string _serverAddress = "https://localhost:7006/api/";

		private static readonly HttpClient _client = new HttpClient();

		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
		{
			WriteIndented = true,
			PropertyNamingPolicy = null
		};
		#endregion Fields

		#region Constructors
		static ApiClient()
		{
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Add("Accept", "application/json");
		}
		#endregion Constructors

		#region Methods
		public static void SetTokenForAuthorization(string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				scheme: "Bearer", parameter: token.Trim(trimChars: " \"".ToCharArray())
			);
		}

		public static void SetIdForUser(int id)
		{
			_client.DefaultRequestHeaders.Remove(name: "UserId");
			_client.DefaultRequestHeaders.Add(name: "UserId", value: id.ToString());
		}

		public static async Task<T> GetAsync<T>(Uri uri, string arg = null)
		{
			HttpResponseMessage response = await _client.GetAsync(requestUri: uri);
			await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
			return await response.Content.ReadFromJsonAsync<T>(options: _options);
		}

		public static async Task<T> GetAsync<T>(string apiMethod, string arg = null)
		{
			Uri uri = new Uri(uriString: CreateUri(apiMethod: apiMethod, arg: arg));
			HttpResponseMessage response = await _client.GetAsync(requestUri: uri);
			await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
			return await response.Content.ReadFromJsonAsync<T>(options: _options);
		}

		public static async Task<TOut> PostAsync<TOut, TIn>(string apiMethod, TIn arg)
		{
			Uri uri = new Uri(uriString: CreateUri(apiMethod: apiMethod));
			HttpResponseMessage response = await _client.PostAsync(requestUri: uri, content: JsonContent.Create<TIn>(inputValue: arg));
			await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
			return await response.Content.ReadFromJsonAsync<TOut>(options: _options);
		}

		public static async Task PostAsync<TIn>(string apiMethod, TIn arg)
		{
			Uri uri = new Uri(uriString: CreateUri(apiMethod: apiMethod));
			HttpResponseMessage response = await _client.PostAsync(requestUri: uri, content: JsonContent.Create<TIn>(inputValue: arg));
			await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
		}

		private static string CreateUri(string apiMethod, string arg = null)
			=> $"{_serverAddress}{apiMethod}{(arg is null ? String.Empty : "/" + arg)}";
		#endregion Methods
	}
}
