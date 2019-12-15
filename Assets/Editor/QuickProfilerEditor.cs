using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class QuickProfilerEditor : EditorWindow
{

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/QuickProfilerEditor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        QuickProfilerEditor window = (QuickProfilerEditor) EditorWindow.GetWindow(typeof(QuickProfilerEditor));
        window.Show();
    }

    #region Inherited Methods

    void OnGUI()
    {
        EditorGUILayout.LabelField("Task Timers", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Timers")) {
            ClearTimers();
        }

        EditorGUILayout.EndHorizontal();
        foreach(KeyValuePair<string, Stopwatch> keyValue in QuickProfiler.Timers)
        {
            string taskKey = keyValue.Key;
            Stopwatch timer = keyValue.Value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(taskKey);
            EditorGUILayout.LabelField(timer.Elapsed.ToString(@"mm\:ss\:fff"));
            string iconToShow = timer.IsRunning ? "greenLight" : "redLight";
            EditorGUILayout.LabelField(EditorGUIUtility.IconContent(iconToShow));
            EditorGUILayout.EndHorizontal();
        }
    }

    void OnInspectorUpdate() {
        Repaint();
    }

    #endregion

    #region Private Methods

    private void ClearTimers() {
        QuickProfiler.ClearTimers();
    }

    #endregion
}
