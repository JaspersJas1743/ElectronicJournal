using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
        public static async Task<TOut> GetAsync<TOut>(Uri uri, Dictionary<string, string> argQuery = null)
        {
            HttpResponseMessage response = await _client.GetAsync(requestUri: uri);
            await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
            return await response.Content.ReadFromJsonAsync<TOut>();
        }

        public static async Task<TOut> GetAsync<TOut>(string apiMethod, Dictionary<string, string> argQuery = null)
            => await GetAsync<TOut>(uri: CreateUri(apiMethod: apiMethod, arg: argQuery));
        #endregion Get

        #region Post
        public static async Task<TOut> PostAsync<TOut, TIn>(string apiMethod, TIn arg)
            => await PostAsync<TOut, TIn>(uri: CreateUri(apiMethod: apiMethod), arg: arg);

        public static async Task<TOut> PostAsync<TOut, TIn>(Uri uri, TIn arg)
            => await (await HelperForPostAsync<TIn>(uri: uri, arg: arg)).Content.ReadFromJsonAsync<TOut>(options: _options);

        public static async Task PostAsync<TIn>(string apiMethod, TIn arg)
            => await PostAsync<TIn>(uri: CreateUri(apiMethod: apiMethod), arg: arg);

        public static async Task PostAsync<TIn>(Uri uri, TIn arg)
            => await HelperForPostAsync<TIn>(uri: uri, arg: arg);

        public static async Task PostAsync(string apiMethod, Dictionary<string, string> arg)
            => await PostAsync(uri: CreateUri(apiMethod, arg: arg));

        public static async Task PostAsync(Uri uri)
            => await HelperForPostAsync(uri: uri, arg: String.Empty);

        private static async Task<HttpResponseMessage> HelperForPostAsync<TIn>(Uri uri, TIn arg)
        {
            HttpResponseMessage response = await _client.PostAsync(requestUri: uri, content: JsonContent.Create<TIn>(inputValue: arg));
            await ApiException.ThrowIfBadResponseAsync(response: response, jsonSerializerOptions: _options);
            return response;
        }
        #endregion Post

        public static Uri CreateUri(string apiMethod, Dictionary<string, string> arg = null)
        {
            StringBuilder uri = new StringBuilder(value: _serverAddress + apiMethod);
            if (arg != null)
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
