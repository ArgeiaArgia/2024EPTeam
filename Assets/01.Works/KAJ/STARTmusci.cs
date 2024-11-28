using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STARTmusci : MonoBehaviour
{
    [SerializeField] private AudioClip backGroundSOund;

    private void Start()
    {
        EazySoundManager.PlayMusic(backGroundSOund, 0.5f, true, false);
    }
}
