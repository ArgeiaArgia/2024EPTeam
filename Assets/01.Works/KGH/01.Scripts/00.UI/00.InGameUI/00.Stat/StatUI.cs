using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StatUI
{
    private StatElement _element;
    private StatIcon _statIcon;
    public StatUI(StatElement element, StatIcon statIcon)
    {
        _element = element;
        _statIcon = statIcon;
    }
    public void ChangeStatUI(int value)
    {
        _element.Value = value;
        _element.IconName = _statIcon.GetIcon(value);
    }
}
