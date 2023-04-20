using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Utilities
{
	public static class ApiClient
	{
		private const string _port = "https://localhost:7006/api/";
		private static readonly HttpClient _client = new HttpClient();

		public static async Task<T> SendAsync<T>(string apiMethod, Dictionary<string, string> args = null)
		{
			try
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
			catch (Exception ex)
			{
				throw new ApiException(message: "Ошибка при отправке запроса на сервер :(", inner: ex);
			}
		}
	}
}
