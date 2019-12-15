using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class QuickProfiler : MonoBehaviour
{
    private static Dictionary<string, Stopwatch> timers = new Dictionary<string, Stopwatch>();

    #region Properties

    public static Dictionary<string, Stopwatch> Timers { get { return timers; } }

    #endregion

    #region Inherited Methods

    #endregion

    #region Public Static Methods

    public static void StartTimer(string taskKey)
    {
        if (ValidateTaskKey(taskKey))
        {
            Stopwatch timer = new Stopwatch();
            timers.Add(taskKey, timer);
            timer.Start();
        }
    }

    public static void StopTimer(string taskKey)
    {
        Stopwatch stopwatch = null;
        if (timers.TryGetValue(taskKey, out stopwatch))
        {
            stopwatch.Stop();
        }
    }

    public static void ClearTimers() {
        timers.Clear();
    }

    #endregion

    #region Private Static Methods

    private static bool ValidateTaskKey(string taskKey)
    {
        bool isValid = true;

        if (taskKey == null || taskKey.Trim() == string.Empty)
        {
            isValid = false;
            UnityEngine.Debug.LogError("Task key must be given");
        }
        else if (timers.ContainsKey(taskKey))
        {
            isValid = false;
            UnityEngine.Debug.LogErrorFormat("Task key {0} is already in use", taskKey);
        }

        return isValid;
    }


    #endregion
}
