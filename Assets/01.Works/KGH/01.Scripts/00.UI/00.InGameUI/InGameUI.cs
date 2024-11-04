using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class InGameUI : ToolkitParents
{
    [OdinSerialize] private Dictionary<StatType, StatIcon> _statIcons;
    [OdinSerialize] private Dictionary<AbilityType, Sprite> _abilityIcons;
    private Dictionary<StatType, StatUI> _statUIs;
    private Dictionary<AbilityType, AbilityUI> _abilityUIs;
    private Inventory _inventory;

    public UnityEvent<AbilityType, int> OnChangeAbilityValue;
    protected override void Awake()
    {
        base.Awake();
        _statUIs = new Dictionary<StatType, StatUI>();
        _abilityUIs = new Dictionary<AbilityType, AbilityUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _statUIs.Clear();
        var statElements = root.Q<VisualElement>("StatList").Query<StatElement>().ToList();
        for (var i = 0; i < statElements.Count; i++)
        {
            var statElement = statElements[i];
            var statType = (StatType)i;
            var statUI = new StatUI(statElement, _statIcons[statType]);
            _statUIs.Add(statType, statUI);
        }
        var abilityElements = root.Q<VisualElement>("AbilityList").Query<StatElement>().ToList();
        for (int i = 0; i < abilityElements.Count; i++)
        {
            var abilityElement = abilityElements[i];
            var abilityType = (AbilityType)i;
            var abilityUI = new AbilityUI(abilityElement);
            _abilityUIs.Add(abilityType, abilityUI);
            
            abilityUI.OnChangeStatValue += value => OnChangeAbilityValue?.Invoke(abilityType, value);
        }
    }

    public void ChangeStatValue(StatType statType, int value) => _statUIs[statType].ChangeStatUI(value);
    public void AddAbilityValue(AbilityType abilityType, int value) => _abilityUIs[abilityType].AddAbility(value);
}

public enum StatType
{
    Hunger,
    Thirst,
    Tired,
    Bored,
    Health,
}

public enum AbilityType
{
    Fishing,
    Cooking,
    Repairing
}