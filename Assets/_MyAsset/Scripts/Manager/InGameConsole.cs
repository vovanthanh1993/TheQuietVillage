using UnityEngine;
using System.Collections.Generic;

public class InGameConsole : MonoBehaviour
{
    private string log = "";
    private Queue<string> logQueue = new Queue<string>();
    private bool showConsole = true;
    public int maxLines = 30;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string message = $"[{type}] {logString}\n{stackTrace}";
        logQueue.Enqueue(message);
        if (logQueue.Count > maxLines)
            logQueue.Dequeue();

        log = string.Join("\n", logQueue.ToArray());
    }

    void OnGUI()
    {
        if (!showConsole) return;

        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
        GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height / 3), log, style);
    }
}
