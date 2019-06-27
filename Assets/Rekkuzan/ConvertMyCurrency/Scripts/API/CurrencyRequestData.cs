using System.Collections.Generic;

namespace Rekkuzan.ConvertMyCurrency
{
    public class CurrencyRequestData
    {
        public string CurrencyOneCode;
        public string CurrencyTwoCode;
        public string Date;

        public float CurrencyOneResult;
        public float CurrencyTwoResult;

        public bool isError;
        public string errorMessage;


        public override string ToString()
        {
            return string.Format("CurrencyRequest {0} ({1}) / {2} ({3}) at {4} (isError {5} - {6})", CurrencyOneCode, CurrencyOneResult, CurrencyTwoCode, CurrencyTwoResult, Date, isError, errorMessage);
        }

        public static readonly Dictionary<string, string> CurrencyLabel = new Dictionary<string, string>()
    {
            { "EUR", "Euros" },
            { "THB", "" },
            { "PHP", "" },
            { "CZK", "" },
            { "BRL", "" },
            { "CHF", "" },
            { "INR", "" },
            { "ISK", "" },
            { "HRK", "" },
            { "BGN", "" },
            { "NOK", "" },
            { "USD", "Dollars (American)" },
            { "CNY", "" },
            { "RUB", "" },
            { "SEK", "" },
            { "MYR", "" },
            { "SGD", "" },
            { "ILS", "" },
            { "TRY", "" },
            { "PLN", "" },
            { "NZD", "" },
            { "HKD", "" },
            { "RON", "" },
            { "MXN", "" },
            { "CAD", "" },
            { "AUD", "" },
            { "GBP", "" },
            { "KRW", "" },
            { "ZAR", "" },
            { "JPY", "Yens" },
            { "DKK", "" },
            { "IDR", "" },
            { "HUF", "" },
        };
    }
}
