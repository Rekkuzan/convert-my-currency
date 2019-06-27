using System.Diagnostics;
using UnityEngine;

public static class Logger
{
    [Conditional("UNITY_LOG_ENABLE")]
    public static void Debug(string message)
    {
#if UNITY_LOG_ENABLE
        UnityEngine.Debug.Log(message);
#endif
    }

    [Conditional("UNITY_LOG_ENABLE")]
    public static void Warning(string message)
    {
#if UNITY_LOG_ENABLE
        UnityEngine.Debug.LogWarning(message);
#endif
    }

    [Conditional("UNITY_LOG_ENABLE")]
    public static void Error(string message)
    {
#if UNITY_LOG_ENABLE
        UnityEngine.Debug.LogError(message);
#endif
    }
}
