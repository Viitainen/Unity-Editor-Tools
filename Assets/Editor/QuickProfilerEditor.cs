using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using static QuickProfiler;

public class QuickProfilerEditor : EditorWindow
{

    #region Inner Classes and Enums

    public enum Tab {
        Timers = 0,
        Variables = 1,
        Other = 2
    }

    #endregion

    private Tab currentTab = Tab.Timers;
    private int currentFrame = 0;
    private bool moveAutomaticallyToNewestFrame = true;

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
        currentTab = (Tab) GUILayout.Toolbar ((int) currentTab, new string[] {"Timers", "Variables", "Other"});

        switch(currentTab) {
            case Tab.Timers: 
                DrawTimersGUI();
                break;
            case Tab.Variables:
                DrawVariablesGUI();
                break;
        }
    }

    /// <summary>
    /// Draws the editor GUI for the variables tab
    /// </summary>
    private void DrawVariablesGUI()
    {
        int newestFrame = QuickProfiler.frame;
        int selectedFrame = currentFrame;

        if (moveAutomaticallyToNewestFrame) {
            currentFrame = newestFrame;
        }

        EditorGUILayout.LabelField("Variable History", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        //Frame backward
        if (GUILayout.RepeatButton("<-")) {
            MoveFrameBack();
        }
        //Current frame indicator
        EditorGUILayout.LabelField($"{currentFrame}/{newestFrame}");
        //Frame forward

        if (GUILayout.RepeatButton("->")) {
            MoveFrameForward();
        }

        EditorGUILayout.EndHorizontal();
        foreach(KeyValuePair<string, List<VariableHistoryItem>> keyValuePair in VariableHistory) {
            VariableHistoryItem item = keyValuePair.Value.Find(x => x.frame == selectedFrame);
            string valueInFrame = item != null ? item.ToString() : "";
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(keyValuePair.Key);
            EditorGUILayout.LabelField(valueInFrame);

            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// Draws the editor GUI for the timers tab
    /// </summary>
    private void DrawTimersGUI()
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

    private void OnInspectorUpdate() {
        Repaint();
    }

    #endregion

    #region Private Methods

    private void ClearTimers() {
        QuickProfiler.ClearTimers();
    }

    /// <summary>
    /// Move one frame backwards
    /// </summary>
    private void MoveFrameBack() {
        if (currentFrame > 0) {
            currentFrame--;
            moveAutomaticallyToNewestFrame = false;
        }
    }

    /// <summary>
    /// Move one frame forward
    /// </summary>
    private void MoveFrameForward() {
        if (currentFrame < QuickProfiler.frame) {
            currentFrame++;
        }

        if (currentFrame == QuickProfiler.frame) {
            moveAutomaticallyToNewestFrame = true;
        } else {
            moveAutomaticallyToNewestFrame = false;
        }
    }

    #endregion
}
