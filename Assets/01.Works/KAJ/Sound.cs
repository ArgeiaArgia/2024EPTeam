using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sound : MonoBehaviour
{
    [SerializeField] InputReader InputReader;
    [SerializeField] private AudioClip backGroundSOund;
    [SerializeField] private AudioClip buttonSound;

    private void Awake()
    {
        InputReader.OnMoveDownEvent += ClickSound;
    }

    private void ClickSound(Vector2 vector)
    {
        EazySoundManager.PlaySound(buttonSound);
    }

    private void Start()
    {
        EazySoundManager.PlayMusic(backGroundSOund, 0.5f, true, false);
    }
}
