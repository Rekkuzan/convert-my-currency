using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class APITests
    {
        [UnityTest]
        public IEnumerator APITest()
        {
            Rekkuzan.ConvertMyCurrency.CurrencyRequestData r = new Rekkuzan.ConvertMyCurrency.CurrencyRequestData()
            {
                CurrencyOneCode = "USD",
                CurrencyTwoCode = "JPY",
                Date = "2019-06-20",
            };

            bool finished = false;

            Rekkuzan.ConvertMyCurrency.API.APIExchanger.Request(r, e =>
            {
                finished = true;
            });

            yield return new WaitUntil(() => finished);
            Assert.AreEqual(r.isError, false);
            Assert.AreEqual(r.CurrencyOneResult, 1.1307f);
            Assert.AreEqual(r.CurrencyTwoResult, 121.71f);
        }

        [UnityTest]
        public IEnumerator APITestWithEuro()
        {
            Rekkuzan.ConvertMyCurrency.CurrencyRequestData r = new Rekkuzan.ConvertMyCurrency.CurrencyRequestData()
            {
                CurrencyOneCode = "EUR",
                CurrencyTwoCode = "JPY",
                Date = "2019-06-20",
            };

            bool finished = false;

            Rekkuzan.ConvertMyCurrency.API.APIExchanger.Request(r, e =>
            {
                finished = true;
            });                                   

            yield return new WaitUntil(() => finished);
            Assert.AreEqual(r.isError, false);
            Assert.AreEqual(r.CurrencyOneResult, 1.0f);
            Assert.AreEqual(r.CurrencyTwoResult, 121.71f);
        }

        [UnityTest]
        public IEnumerator APITestWithEmptyCurrency()
        {
            Rekkuzan.ConvertMyCurrency.CurrencyRequestData r = new Rekkuzan.ConvertMyCurrency.CurrencyRequestData()
            {
                CurrencyOneCode = string.Empty,
                CurrencyTwoCode = "JPY",
                Date = "2019-06-20",
            };

            bool finished = false;

            Rekkuzan.ConvertMyCurrency.API.APIExchanger.Request(r, e =>
            {
                finished = true;
            });

            yield return new WaitUntil(() => finished);

            Assert.AreEqual(r.isError, true);
            Assert.AreEqual(r.CurrencyTwoResult, 121.71f);
        }

        [UnityTest]
        public IEnumerator APITestWithEmptyDate()
        {
            Rekkuzan.ConvertMyCurrency.CurrencyRequestData r = new Rekkuzan.ConvertMyCurrency.CurrencyRequestData()
            {
                CurrencyOneCode = "USD",
                CurrencyTwoCode = "JPY",
                Date = string.Empty,
            };

            bool finished = false;
            Debug.Log(r);
            Rekkuzan.ConvertMyCurrency.API.APIExchanger.Request(r, e =>
            {
                finished = true;
                Debug.Log(r);
            });

            yield return new WaitUntil(() => finished);

            Assert.AreEqual(r.isError, false);
        }
    }
}
