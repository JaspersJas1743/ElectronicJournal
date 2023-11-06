using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI
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

        private static string _token;
        #endregion Fields

        #region Constructors
        static ApiClient()
        {
            _client.Timeout = TimeSpan.FromSeconds(value: 2);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        #endregion Constructors

        #region Properties
        public static string Token
        {
            get => _token;
            set
            {
                _token = value;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    scheme: "Bearer", parameter: value
                );
            }
        }
        #endregion Properties

        #region Methods
        #region Get
        public static async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _client.GetAsync(requestUri: uri, cancellationToken: cancellationToken);
            await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
            return response;
        }

        public static async Task<TOut> GetAsync<TOut>(Uri uri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await GetAsync(uri: uri, cancellationToken: cancellationToken);
            return await response.Content.ReadFromJsonAsync<TOut>(options: _options, cancellationToken: cancellationToken);
        }

        public static async Task<TOut> GetAsync<TOut>(string apiMethod, Dictionary<string, string> argQuery = default, CancellationToken cancellationToken = default)
            => await GetAsync<TOut>(uri: CreateUri(apiMethod: apiMethod, arg: argQuery), cancellationToken: cancellationToken);

        public static async Task<byte[]> GetBytesAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await GetAsync(uri: uri, cancellationToken: cancellationToken);
            return await response.Content.ReadAsByteArrayAsync();
        }

        public static async Task<byte[]> GetBytesAsync(string apiMethod, Dictionary<string, string> argQuery = default, CancellationToken cancellationToken = default)
            => await GetBytesAsync(uri: CreateUri(apiMethod: apiMethod, arg: argQuery), cancellationToken: cancellationToken);
        #endregion Get

        #region Post
        public static async Task<TOut> PostAsync<TOut, TIn>(string apiMethod, TIn arg, CancellationToken cancellationToken = default)
            => await PostAsync<TOut, TIn>(uri: CreateUri(apiMethod: apiMethod), arg: arg, cancellationToken: cancellationToken);

        public static async Task<TOut> PostAsync<TOut, TIn>(Uri uri, TIn arg, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await HelperForPostAsync<TIn>(uri: uri, arg: arg, cancellationToken: cancellationToken);
            return await response.Content.ReadFromJsonAsync<TOut>(options: _options);
        }

        public static async Task PostAsync<TIn>(string apiMethod, TIn arg, CancellationToken cancellationToken = default)
            => await PostAsync<TIn>(uri: CreateUri(apiMethod: apiMethod), arg: arg, cancellationToken: cancellationToken);

        public static async Task PostAsync<TIn>(Uri uri, TIn arg, CancellationToken cancellationToken = default)
            => await HelperForPostAsync<TIn>(uri: uri, arg: arg, cancellationToken: cancellationToken);

        public static async Task PostAsync(string apiMethod, Dictionary<string, string> arg, CancellationToken cancellationToken = default)
            => await PostAsync(uri: CreateUri(apiMethod, arg: arg), cancellationToken: cancellationToken);

        public static async Task PostAsync(Uri uri, CancellationToken cancellationToken = default)
            => await HelperForPostAsync(uri: uri, arg: String.Empty, cancellationToken: cancellationToken);

        private static async Task<HttpResponseMessage> HelperForPostAsync<TIn>(Uri uri, TIn arg, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _client.PostAsync(requestUri: uri, content: JsonContent.Create<TIn>(inputValue: arg), cancellationToken: cancellationToken);
            await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
            return response;
        }
        #endregion Post

        public static Uri CreateUri(string apiMethod, Dictionary<string, string> arg = default)
        {
            StringBuilder uri = new StringBuilder(value: _serverAddress + apiMethod);
            if (arg != default)
            {
                uri.Append("?");
                foreach (var pair in arg)
                    uri.Append(value: $"{pair.Key}={pair.Value}&");
                uri.Remove(startIndex: uri.Length - 1, length: 1);
            }
            return new Uri(uriString: uri.ToString());
        }
        #endregion Methods
    }
}
