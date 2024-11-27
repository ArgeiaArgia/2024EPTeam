using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ScriptSO : SerializedScriptableObject
{
    public Dictionary<ItemSO, string> _itemExplore = new Dictionary<ItemSO, string>();
    public List<string> _radioListen = new List<string>();

    public string[] GetRandomRadio()
    {
        var randomIndex = Random.Range(0, _radioListen.Count);
        var radioScript = _radioListen[randomIndex];
        return radioScript.Split('\n');
    }
}
