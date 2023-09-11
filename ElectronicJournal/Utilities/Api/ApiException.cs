using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.Utilities.Api
{
	[Serializable]
	public class ApiException : Exception
	{
		private static HttpStatusCode[] _handlers = new[] 
		{
			HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized 
		};

		public ApiException() { }
		
		public ApiException(string message) 
			: base(message) 
		{ }
		
		public ApiException(string message, Exception inner) 
			: base(message, inner) 
		{ }

		protected ApiException(SerializationInfo info, StreamingContext context) 
			: base(info, context) 
		{ }

		public static async Task ThrowIfBadResponseAsync(HttpResponseMessage response)
			=> await ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: new JsonSerializerOptions());

		public static async Task ThrowIfBadResponseAsync(HttpResponseMessage response, JsonSerializerOptions jsonSerializerOptions)
		{
			if (_handlers.Contains(value: response.StatusCode))
			{
				Error error = await JsonSerializer.DeserializeAsync<Error>(
					utf8Json: await response.Content.ReadAsStreamAsync(),
					options: jsonSerializerOptions
				);
				throw new ApiException(message: error.Message);
			}
		}

	}
}
