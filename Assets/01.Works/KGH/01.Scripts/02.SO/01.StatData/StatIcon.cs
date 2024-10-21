using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/UI/StatIcon")]
public class StatIcon : ScriptableObject
{
    [SerializeField] private string _bestIcon;
    [SerializeField] private string _goodIcon;
    [SerializeField] private string _normalIcon;
    [SerializeField] private string _badIcon;
    [SerializeField] private string _worstIcon;
    
    public string GetIcon(int value)
    {
        string result = string.Empty;
        if (value >= 80)
        {
            result = _bestIcon;
        }
        else if (value >= 60)
        {
            result = _goodIcon;
        }
        else if (value >= 40)
        {
            result = _normalIcon;
        }
        else if (value >= 20)
        {
            result = _badIcon;
        }
        else
        {
            result = _worstIcon;
        }
        return $"StatIcon/{result}";
    }
}
