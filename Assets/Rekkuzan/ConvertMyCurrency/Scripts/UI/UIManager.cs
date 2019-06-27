using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rekkuzan.ConvertMyCurrency.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Flag's Sprite")]
        [SerializeField] List<Sprite> AllFlags = new List<Sprite>();
        [SerializeField] string FormatCurrency = "{0} - {1}";

        [Header("Currency Modal")]
        [SerializeField] GameObject ModalCurreny;
        [SerializeField] Transform ContainerScrollCurrency;
        [SerializeField] GameObject PrefabCurrency;
        [SerializeField] TMP_InputField CurrencyInputField;

        [Header("Inputs")]
        [SerializeField] TextMeshProUGUI CurrencyOne;
        [SerializeField] Image CurrencyOneFlag;

        [SerializeField] TextMeshProUGUI CurrencyTwo;
        [SerializeField] Image CurrencyTwoFlag;

        [SerializeField] TMP_InputField AmountInput;
        [SerializeField] TextMeshProUGUI DateInput;

        [Header("Results")]
        [SerializeField] TextMeshProUGUI Result;
        [SerializeField] GameObject Loading;

        private List<CurrenyItem> currencyItems = new List<CurrenyItem>();
        
        /// <summary>
        /// Class holding references from prefab
        /// </summary>
        private class CurrenyItem
        {
            public TMPro.TextMeshProUGUI Label;
            public Image Flag;
            public Button Button;
            public GameObject Item;
            public string Code;
        }

        private void Update()
        {
            if (Loading.activeSelf)
            {
                Loading.transform.Rotate(Vector3.forward * Time.deltaTime * 100.0f);
            }
        }

        /// <summary>
        /// Will initialize the UI
        /// </summary>
        /// <param name="OnCurrencySelected">Callback when currency is selected from modal</param>
        /// <param name="code01">Default currency one</param>
        /// <param name="code02">Default currency two</param>
        public void Initialize(System.Action<string> OnCurrencySelected, string code01, string code02)
        {
            SetResultText("...");
            SetCurrencyModalEnable(false);
            UpdateInputsCurrencyOne(code01);
            UpdateInputsCurrencyTwo(code02);
            SetDate(string.Format("{0:yyyy-MM-dd}", System.DateTime.Now.Date));
            SetLoading(false);
            StartCoroutine(InitializeCoroutine(OnCurrencySelected));
        }

        /// <summary>
        /// Coroutine of Initialize
        /// </summary>
        /// <param name="OnCurrencySelected">Callback when currency is selected from modal</param>
        /// <returns></returns>
        private IEnumerator InitializeCoroutine(System.Action<string> OnCurrencySelected)
        {
            yield return null;

            List<string> keys = CurrencyRequestData.CurrencyLabel.Keys.ToList();
            keys.Sort();

            foreach (string key in keys)
            {
                GameObject Item = Instantiate(PrefabCurrency, ContainerScrollCurrency);
                CurrenyItem currenyItem = new CurrenyItem()
                {
                    Item = Item,
                    Label = Item.GetComponentInChildren<TMPro.TextMeshProUGUI>(),
                    Button = Item.GetComponent<Button>(),
                    Flag = Item.transform.Find("Flag").GetComponentInChildren<Image>(),
                    Code = key
                };


                currenyItem.Button?.onClick.RemoveAllListeners();
                currenyItem.Button?.onClick.AddListener(delegate ()
                {
                    OnCurrencySelected?.Invoke(key);
                });

                if (currenyItem.Label)
                {
                    currenyItem.Label.text = string.Format(FormatCurrency, key, CurrencyRequestData.CurrencyLabel[key]);
                }

                if (currenyItem.Flag != null)
                {
                    Sprite sp = AllFlags.FirstOrDefault(e => e.name == key.ToLower());
                    currenyItem.Flag.overrideSprite = sp;
                }

                currencyItems.Add(currenyItem);

                yield return null;
            }
        }

        /// <summary>
        /// Will update the currency one infos
        /// </summary>
        /// <param name="code">code of currency</param>
        public void UpdateInputsCurrencyOne(string code)
        {
            string text = string.Format(FormatCurrency, code, CurrencyRequestData.CurrencyLabel[code]);
            Sprite sp = AllFlags.FirstOrDefault(e => e.name == code.ToLower());
            CurrencyOne.text = text;
            CurrencyOneFlag.overrideSprite = sp;
        }

        /// <summary>
        /// Will update the currency two infos
        /// </summary>
        /// <param name="code">code of currency</param>
        public void UpdateInputsCurrencyTwo(string code)
        {
            string text = string.Format(FormatCurrency, code, CurrencyRequestData.CurrencyLabel[code]);
            Sprite sp = AllFlags.FirstOrDefault(e => e.name == code.ToLower());
            CurrencyTwo.text = text;
            CurrencyTwoFlag.overrideSprite = sp;
        }

        /// <summary>
        /// Callback when input field of currency modal changed
        /// Will reduce the result of the scroll view
        /// </summary>
        /// <param name="code">code of currency</param>
        public void OnInputCurrencyChanged()
        {
            string text = CurrencyInputField.text == null ? string.Empty : CurrencyInputField.text.ToUpper();

            foreach (var currencyItem in currencyItems)
            {
                currencyItem.Item.SetActive(currencyItem.Code.StartsWith(text));
            }
        }
        
        /// <summary>
        /// Will display the currency modal
        /// </summary>
        /// <param name="enable"></param>
        public void SetCurrencyModalEnable(bool enable)
        {
            ModalCurreny.SetActive(enable);
            CurrencyInputField.text = string.Empty;
        }

        /// <summary>
        /// Get the amount specified in the input field of amount
        /// </summary>
        /// <returns></returns>
        public float GetAmountInput()
        {
            string amountText = AmountInput.text;
            if (float.TryParse(amountText, out float result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Will set the result text
        /// </summary>
        /// <param name="text"></param>
        public void SetResultText(string text)
        {
            Result.text = text;
        }

        /// <summary>
        /// Will set the date text
        /// </summary>
        /// <param name="date"></param>
        public void SetDate(string date)
        {
            DateInput.text = date;
        }

        /// <summary>
        /// Will enable/disable the loading feedback
        /// </summary>
        /// <param name="enable"></param>
        public void SetLoading(bool enable)
        {
            Loading.SetActive(enable);
        }
    }
}
