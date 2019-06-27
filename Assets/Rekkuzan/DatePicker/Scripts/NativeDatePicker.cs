using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace Rekkuzan.DatePicker
{
    /// <summary>
    /// Handling the native NativeDatePicker call (Android and iOS)
    /// </summary>
    public class NativeDatePicker : MonoBehaviour
    {
        public static NativeDatePicker Instance { get; private set; }

        private System.Action<System.DateTime> Callback; 

        /// <summary>
        /// Awake function from Unity's MonoBehavior
        /// </summary>
        /// <returns>void</returns>
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Several instance of NativeDatePicker");
                return;
            }

            Instance = this;
        }

        /// <summary>
        /// OnDestroy function from Unity's Monoheaviour
        /// </summary>
        private void OnDestroy()
        {
            Instance = null;
        }

        /// <summary>
        /// Will request the native DatePicker and return the DateTime selected
        /// </summary>
        /// <param name="callback"></param>
        public void RequestDatePicker(System.Action<System.DateTime> callback)
        {
            Callback = null;
            Callback += callback;
#if UNITY_ANDROID && !UNITY_EDITOR
            NativeDatePickerAndroid.Show(this.gameObject.name, nameof(OnDatePickerRead));
#elif UNITY_IOS && !UNITY_EDITOR
            NativeDatePickeriOS.Show(this.gameObject.name, nameof(OnDatePickerRead));
#endif
        }

        private void OnDatePickerRead(string date)
        {
            DateTime dateTime = System.DateTime.Now;
            try
            {
                dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to parse the datetime " + e.Message);
            }

            Callback?.Invoke(dateTime);
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    public static class NativeDatePickerAndroid
    {
        public static void Show(string ObjectName, string ObjectMethod)
        {
            using (var mDatePickerClass = new AndroidJavaClass("com.rekkuzan.datepicker.DatePickerUnity"))
            {
                mDatePickerClass.CallStatic("StartDatePicker", ObjectName, ObjectMethod);
            }
        }
    }
#endif
#if UNITY_IOS && !UNITY_EDITOR
    public static class NativeDatePickeriOS
    {
        [DllImport("__Internal")]
        private static extern void StartDatePicker(string nameObject, string methodName);

        public static void Show(string ObjectName, string ObjectMethod)
        {
            StartDatePicker(ObjectName, ObjectMethod);
        }

    }
#endif

}
