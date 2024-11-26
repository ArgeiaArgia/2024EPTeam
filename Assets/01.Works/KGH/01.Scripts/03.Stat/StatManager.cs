using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class StatManager : SerializedMonoBehaviour
{
    [OdinSerialize] private Dictionary<StatType, float> _delay;

    [HideInInspector] public Dictionary<StatType, int> StatValues;

    [HideInInspector] public Action<StatType, int> OnStatChanged;
    
    public UnityEvent<StatType> OnDeath;
    private void Start()
    {
        StatValues = new Dictionary<StatType, int>();
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            StatValues.Add(type, 100);
            OnStatChanged?.Invoke(type, StatValues[type]);
        }
        foreach (var d in _delay)
        {
            StartCoroutine(ReduceAsTime(d.Key, d.Value));
        }
    }

    private IEnumerator ReduceAsTime(StatType type, float time)
    {
        yield return new WaitForSeconds(time);
        StatValues[type] -= 1;
        OnStatChanged?.Invoke(type, StatValues[type]);
        StartCoroutine(ReduceAsTime(type, time));
        
        CheckDeath(type);
    }
    private void CheckDeath(StatType statType)
    {
        if (StatValues[statType] <= 0)
        {
            OnDeath?.Invoke(statType);
        }
    }
}
