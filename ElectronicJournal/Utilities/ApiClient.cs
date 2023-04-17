using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.Utilities
{
	public class ApiClient
	{
		private const string _port = "https://localhost:7006/api/";
		private static readonly HttpClient _client;

		static ApiClient()
		{
			_client = Program.AppHost.Services.GetService<HttpClient>();
		}

		public static async Task<T> SendAsync<T>(string apiMethod, Dictionary<string, string> args = null)
		{
			StringBuilder builder = new StringBuilder(value: _port);
			builder.Append(value: apiMethod);
			if (!(args is null))
			{
				builder.Append($"?");
				foreach (var item in args)
					builder.Append($"{item.Key}={item.Value}&");
				builder.Remove(startIndex: builder.Length - 1, length: 1);
			}
			Uri uri = new Uri(uriString: builder.ToString());
			return await _client.GetFromJsonAsync<T>(requestUri: uri);
		}
	}
}
