// Author: Dominik Philp
// Description: Script that enables display of Unity Debug Log in 3D via a TextMesh component.
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class VRDebugText : MonoBehaviour
{
    [SerializeField]
    private TextMesh targetMesh;

    [Tooltip("Maximimum amount of simultaneously displayed Log messages.")]
    [SerializeField]
    private int maxMessages = 5;

    private List<string> log = new List<string>();

    private int enabledLogTypes = (int)(LogType.Log | LogType.Warning | LogType.Error);

    private bool isUpdating = true;


    private void Start()
    {
        targetMesh = GetComponent<TextMesh>();
        Application.logMessageReceived += HandleLog;
    }

    /// <summary> Handles filtering Log Types and changing the Text on the TextMesh. </summary>
    private void HandleLog(string message, string stackTrace, LogType type)
    {
        // don't update the text if logging is paused.
        if (!isUpdating) return;

        if ((int)type == enabledLogTypes)
        {
            if (log.Count + 1 > maxMessages)
            {
                log.RemoveAt(0);
            }
            log.Add(message);

            targetMesh.text = log[0];

            if (log.Count > 1)
            {
                for (int i = 1; i < log.Count; i++)
                {
                    targetMesh.text += "\n" + log[i];
                }
            }
        }
    }

    /// <summary>
    /// Set Log Type(s) that are displayed in the Log.
    /// </summary>
    /// <param name="newType">Type to allow in the Filter. Use Bitwise OR (|) to allow multiple Types.</param>
    public void SetLogType(LogType newType)
    {
        enabledLogTypes = (int)newType;
    }

    /// <summary>
    /// Clear the text on the TextMesh.
    /// </summary>
    public void ClearLog()
    {
        targetMesh.text = "";
    }

    /// <summary>
    /// Pause or Resume Updating of the TextMesh (Text will still be displayed, but no logs will be Cached.)
    /// </summary>
    public void SetLogUpdateEnabled(bool enabled)
    {
        isUpdating = enabled;
    }
}
