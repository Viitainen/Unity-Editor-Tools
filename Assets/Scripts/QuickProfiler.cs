using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class QuickProfiler: MonoBehaviour
{

    #region Inner Classes

    public class VariableHistoryItem {

        public int frame;
    }

    public class StringHistoryItem : VariableHistoryItem {
        public string value;

        public StringHistoryItem() {
            //Empty constructor
        }

        public StringHistoryItem(int frame, string value) {
            this.frame = frame;
            this.value = value;
        }

        public override string ToString() {
            return value;
        }
    }

    #endregion

    public static int frame = 0;

    private static Dictionary<string, Stopwatch> timers = new Dictionary<string, Stopwatch>();
    private static Dictionary<string, List<VariableHistoryItem>> variableHistory = new Dictionary<string, List<VariableHistoryItem>>();

    #region Properties

    public static Dictionary<string, Stopwatch> Timers { get { return timers; } }
    public static Dictionary<string, List<VariableHistoryItem>> VariableHistory { get { return variableHistory; } }

    #endregion

    #region Inherited Methods

    private void OnApplicationQuit() {
        StopAllTimers();
    }

    private void Update() {
        frame++;
    }
    
    #endregion

    #region Public Static Methods

    public static void Log(string key, string value) {

        List<VariableHistoryItem> history = null;

        if (!variableHistory.TryGetValue(key, out history)) {
            history = new List<VariableHistoryItem>();
            variableHistory.Add(key, history);
        }

        history.Add(new StringHistoryItem(frame, value));
    }   

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

    public static void StopAllTimers() {
        foreach(var timerKeyValue in timers) {
            timerKeyValue.Value.Stop();
        }
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
