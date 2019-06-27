using Rekkuzan.ConvertMyCurrency.UI;
using UnityEngine;

namespace Rekkuzan.ConvertMyCurrency
{
    public class CurrencyApp : MonoBehaviour
    {
        public enum State
        {
            None,
            CurrencyOneSelection,
            CurrencyTwoSelection,
            WaitingForResult
        }

        [Header("Default Data")]
        [SerializeField] string CurrencyOneInfos = "EUR";
        [SerializeField] string CurrencyTwoInfos = "USD";

        [Space]
        [SerializeField] UIManager uIManager;
        public State CurrentState { get; private set; }

        private CurrencyRequestData CurrencyRequestData;
        private string CurrentDateRequested = null;

        private void Start()
        {
            uIManager.Initialize(OnCurrencySelected, CurrencyOneInfos, CurrencyTwoInfos);
        }

        /// <summary>
        /// Will apply the currency selected to the currency previously clicked
        /// </summary>
        /// <param name="currency"></param>
        private void OnCurrencySelected(string currency)
        {
            Debug.Log("Currency selected " + currency);
            if (CurrencyRequestData.CurrencyLabel.ContainsKey(currency))
            {
                if (CurrentState == State.CurrencyOneSelection)
                {
                    CurrencyOneInfos = currency;
                    uIManager.UpdateInputsCurrencyOne(CurrencyOneInfos);
                }
                else if (CurrentState == State.CurrencyTwoSelection)
                {
                    CurrencyTwoInfos = currency;
                    uIManager.UpdateInputsCurrencyTwo(CurrencyTwoInfos);
                }
            }
            CurrentState = State.None;
            uIManager.SetCurrencyModalEnable(false);
        }

        /// <summary>
        /// Display date picker and update the date text
        /// </summary>
        public void OnDateButtonClicked()
        {
            Rekkuzan.DatePicker.NativeDatePicker.Instance.RequestDatePicker(e =>
            {
                string dateFormat = string.Format("{0:yyyy-mm-dd}", e);
                if (e.Date != System.DateTime.Now.Date)
                    CurrentDateRequested = dateFormat;
                uIManager.SetDate(dateFormat);
            });
        }

        /// <summary>
        /// Display the currency modal for the currency one
        /// </summary>
        public void CurrencyOneSelected()
        {
            CurrentState = State.CurrencyOneSelection;
            uIManager.SetCurrencyModalEnable(true);
        }

        /// <summary>
        /// Display the currency modal for the currency two
        /// </summary>
        public void CurrencyTwoSelected()
        {
            CurrentState = State.CurrencyTwoSelection;
            uIManager.SetCurrencyModalEnable(true);
        }

        /// <summary>
        /// Will swap the 2 currencies input
        /// </summary>
        public void InverseCurrency()
        {
            string tmp = CurrencyTwoInfos;
            CurrencyTwoInfos = CurrencyOneInfos;
            CurrencyOneInfos = tmp;
            uIManager.UpdateInputsCurrencyOne(CurrencyOneInfos);
            uIManager.UpdateInputsCurrencyTwo(CurrencyTwoInfos);
        }

        /// <summary>
        /// Will convert the currency with infos specified in form
        /// </summary>
        public void Convert()
        {
            if (CurrentState == State.WaitingForResult)
                return;
            CurrencyRequestData = new CurrencyRequestData()
            {
                CurrencyOneCode = CurrencyOneInfos,
                CurrencyTwoCode = CurrencyTwoInfos,
                Date = CurrentDateRequested,
            };
            Debug.Log(CurrencyRequestData);
            uIManager.SetLoading(true);
            uIManager.SetResultText(string.Empty);
            CurrentState = State.WaitingForResult;
            API.APIExchanger.Request(CurrencyRequestData, r =>
            {
                CurrentState = State.None;
                if (r.isError)
                {
                    // error to handle
                    Debug.LogError("An error occured");
                    return;
                }

                float result = uIManager.GetAmountInput() / r.CurrencyOneResult * r.CurrencyTwoResult;
                uIManager.SetResultText(result.ToString());
                uIManager.SetLoading(false);
            });
        }

    }
}
