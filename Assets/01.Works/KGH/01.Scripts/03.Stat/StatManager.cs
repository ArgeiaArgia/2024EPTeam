using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class StatManager : SerializedMonoBehaviour
{
    [SerializeField] private InGameUI inGameUI;
    [OdinSerialize] private Dictionary<StatType, float> delay;
    [HideInInspector] public Dictionary<StatType, int> statValues;

    private void Start()
    {
        statValues = new Dictionary<StatType, int>();
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            statValues.Add(type, 100);
        }
        foreach (var d in delay)
        {
            StartCoroutine(ReduceAsTime(d.Key, d.Value));
        }
    }

    IEnumerator ReduceAsTime(StatType type, float time)
    {
        yield return new WaitForSeconds(time);
        statValues[type] -= 1;
        inGameUI.ChangeStatValue(type, statValues[type]);
        StartCoroutine(ReduceAsTime(type, time));
    }
}
