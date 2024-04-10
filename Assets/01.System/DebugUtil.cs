using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtil : MonoBehaviour
{
    public static void DebugLogColor(string text, Color textColor)
    {
#if UNITY_EDITOR
        Debug.Log($"<color=textColor>{text}</color>");
#endif
    }

    public static void DebugLog()
    {

    }
}
