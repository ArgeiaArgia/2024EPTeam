using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ScriptSO : SerializedScriptableObject
{
    public Dictionary<ItemSO, string> _itemExplore = new Dictionary<ItemSO, string>();
    public Dictionary<int, string> _radioListen = new Dictionary<int, string>();

    public string[] GetRandomRadio()
    {
        var randomIndex = Random.Range(0, _radioListen.Count);
        var radioScript = _radioListen[randomIndex];
        return radioScript.Split('\n');
    }
}
