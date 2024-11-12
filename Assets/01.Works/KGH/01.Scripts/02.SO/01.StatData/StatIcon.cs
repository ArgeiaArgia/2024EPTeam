using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/UI/StatIcon")]
public class StatIcon : ScriptableObject
{
    [SerializeField] private int _bestIcon;
    [SerializeField] private int _goodIcon;
    [SerializeField] private int _normalIcon;
    [SerializeField] private int _badIcon;
    [SerializeField] private int _worstIcon;
    
    public int GetIcon(int value)
    {
        int result = 0;
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

        return result;
    }
}
