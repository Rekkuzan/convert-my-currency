
using UnityEngine;
using SimpleJSON;

namespace Rekkuzan.ConvertMyCurrency.API
{
    /// <summary>
    /// Will handle the extraction and parsing of data received by request
    /// </summary>
    public static class JsonDataConverter
    {
        private static JSONNode GetDesiredDateData(JSONNode datas, string dateDesired)
        {
            if (string.IsNullOrEmpty(dateDesired))
                return datas["rates"];

            foreach (var data in datas["rates"])
            {
                if (data.Key == dateDesired)
                    return data.Value;
            }

            return null;
        }

        /// <summary>
        /// Will parse the json response of API request and return a ResultData
        /// </summary>
        /// <param name="json">json response of API request</param>
        /// <param name="request">request used to perform the request</param>
        /// <returns>ResultData is returned containing the returned informations</returns>
        public static ResultData ParseFromJson(string json, CurrencyRequestData request)
        {
            ResultData result = null;
            try
            {
                var datas = SimpleJSON.JSON.Parse(json);
                if (datas == null)
                {
                    Debug.LogWarning("Parse json data is null");
                    return null;
                }
                var currencies = GetDesiredDateData(datas, request.Date);
                if (currencies == null)
                {
                    Debug.LogWarning("Parse json currencies at the date specified is null");
                    return null;
                }

                result = new ResultData()
                {
                    CurrencyOne = currencies[request.CurrencyOneCode] ?? -1,
                    CurrencyTwo = currencies[request.CurrencyTwoCode] ?? -1
                };

                if (request.CurrencyOneCode == "EUR")
                    result.CurrencyOne = 1;
                else if (request.CurrencyTwoCode == "EUR")
                    result.CurrencyTwo = 1;
            }
            catch (System.Exception e)
            {
                Debug.LogError("An exception occured " + e.Message);
            }

            return result;
        }

        public class ResultData
        {
            public float CurrencyOne;
            public float CurrencyTwo;
        }
    }
}
