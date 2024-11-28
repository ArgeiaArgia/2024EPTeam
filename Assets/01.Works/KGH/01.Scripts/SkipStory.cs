using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SkipStory : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private TransitionUI transitionUI;

    private void Awake()
    {
        inputReader.OnEscapeEvent += Skip;
    }
    
    private void OnDestroy()
    {
        inputReader.OnEscapeEvent -= Skip;
    }

    private void Skip()
    {
        transitionUI.EnableUI(()=>SceneManager.LoadScene(2));
    }
}
