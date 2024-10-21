using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : ToolkitParents
{
    [OdinSerialize] private Dictionary<StatType, StatIcon> _statIcons;
    private Dictionary<StatType, StatUI> _statUIs;

    protected override void Awake()
    {
        base.Awake();
        _statUIs = new Dictionary<StatType, StatUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _statUIs.Clear();
        var statElements = root.Query<StatElement>().ToList();
        for (var i = 0; i < statElements.Count; i++)
        {
            var statElement = statElements[i];
            var statType = (StatType)i;
            var statUI = new StatUI(statElement, _statIcons[statType]);
            _statUIs.Add(statType, statUI);
        }
    }

    public void ChangeValue(StatType statType, int value) => _statUIs[statType].ChangeStatUI(value);
}

public enum StatType
{
    Hunger,
    Thirst,
    Tired,
    Bored,
    Health,
}