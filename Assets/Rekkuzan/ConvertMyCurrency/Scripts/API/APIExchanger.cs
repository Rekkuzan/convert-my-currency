using System.Net.Http;
using UnityEngine;

namespace Rekkuzan.ConvertMyCurrency.API
{
    /// <summary>
    /// Will perform API request
    /// </summary>
    public class APIExchanger : MonoBehaviour
    {
        #region Singleton
        private APIExchanger() { }
        public static APIExchanger Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Several Instance of APIExchanger");
                Destroy(this);
                return;
            }

            Instance = this;
        }

        #endregion

        private const string _apiRoute = "https://api.exchangeratesapi.io/history?start_at={0}&end_at={1}&symbols={2}";
        private const string _apiRouteLastest = "https://api.exchangeratesapi.io/latest?symbols={0}";

        private static readonly HttpClient m_ApiClient = new HttpClient();

        private static async void Request(HttpClient client, string url, CurrencyRequestData request, System.Action<CurrencyRequestData> callback)
        {
            using (var response = await client.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    request.isError = true;
                    request.errorMessage = string.Format("Error ({0})", response.StatusCode);
                }
                else
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JsonDataConverter.ResultData data = JsonDataConverter.ParseFromJson(json, request);

                    if (data == null)
                    {
                        request.isError = true;
                        request.errorMessage = "Failed to parse data from API";
                    }
                    else
                    {
                        request.CurrencyOneResult = data.CurrencyOne;
                        request.CurrencyTwoResult = data.CurrencyTwo;

                        if (request.CurrencyOneResult <= 0 || request.CurrencyTwoResult <= 0)
                        {
                            request.isError = true;
                            request.errorMessage = "Invalid value (<= 0)";
                        }
                    }
                }

                callback?.Invoke(request);
            }
        }

        /// <summary>
        /// Will request the data and invoke the callback with the result
        /// </summary>
        /// <param name="request">Request data for the request</param>
        /// <param name="callback">Callback to be invoked after the request</param>
        public static void Request(CurrencyRequestData request, System.Action<CurrencyRequestData> callback)
        {
            string countryRequest;
            if (request.CurrencyOneCode == "EUR" || string.IsNullOrEmpty(request.CurrencyTwoCode))
                countryRequest = request.CurrencyTwoCode;
            else if (request.CurrencyTwoCode == "EUR" || string.IsNullOrEmpty(request.CurrencyOneCode))
                countryRequest = request.CurrencyOneCode;
            else
                countryRequest = string.Format("{0},{1}", request.CurrencyOneCode, request.CurrencyTwoCode);

            string url;
            if (string.IsNullOrEmpty(request.Date))
                url = string.Format(_apiRouteLastest, countryRequest);
            else
                url = string.Format(_apiRoute, request.Date, request.Date, countryRequest);

            Request(m_ApiClient, url, request, callback);
        }
    }
}
