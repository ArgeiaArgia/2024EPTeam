using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI
{
    private StatElement _element;
    
    public AbilityUI(StatElement element, StatIcon statIcon)
    {
        _element = element;
    }
    public void AddAbility(int value)
    {
        _element.Value += value;
    }
}
