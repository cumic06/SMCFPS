using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LoadTextScriptable", menuName = "LoadTextScriptable", order = 0)]
public class LoadTextScriptable : ScriptableObject
{
    public LoadTextDatas loadTextDatas;
}

[Serializable]
public class LoadTextDatas
{
    public List<string> lodingTextList = new();

    public string GetRandomText()
    {
        int randomTextRange = UnityEngine.Random.Range(0, lodingTextList.Count);
        string randomText = lodingTextList[randomTextRange];
        return randomText;
    }
}