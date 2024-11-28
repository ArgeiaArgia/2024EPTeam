using Hellmade.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stroySound : MonoBehaviour
{
    [SerializeField] private AudioClip backGroundSOund;
    public void Music()
    {
        EazySoundManager.PlayMusic(backGroundSOund, 0.2f, true, false);
    }
}
