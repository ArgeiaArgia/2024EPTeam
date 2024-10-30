using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI
{
    private StatElement _element;
    public int Level;
    public Action<int> OnChangeStatValue;
    
    public AbilityUI(StatElement element)
    {
        _element = element;
    }
    public void AddAbility(int value)
    {
        var prevGauge = _element.Value / (_element.LineCount+1);
        _element.Value += value;
        var currentGauge = _element.Value/ (_element.LineCount+1);
        if (prevGauge != currentGauge)
        {
            Level = currentGauge;
            OnChangeStatValue?.Invoke(currentGauge);
        }
    }
}
