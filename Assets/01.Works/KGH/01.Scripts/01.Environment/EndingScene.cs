using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class EndingScene : SerializedMonoBehaviour
{
    [SerializeField] private TransitionUI transitionUI;
    [OdinSerialize] private Dictionary<StatType, GameObject> _animator;
    [SerializeField] private GameObject _endingUI;
    
    private void Start()
    {
        var diedType = PlayerPrefs.GetInt("DiedType");
        if (diedType == -1)
        {
            _endingUI.SetActive(true);
            return;
        }
        _animator[(StatType)diedType].SetActive(true);
    }
}
