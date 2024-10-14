using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StateSO")]
public class StatsSO : ScriptableObject
{
    public string StateName;
    [SerializeField]private float Maxstat;
    public float Currentstat;
    public int StateLevel;
}
