using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class TitleUI : ToolkitParents
{
    [SerializeField] private TransitionUI transitionUI;
    [SerializeField] private int sceneIndex;
    private Button _startButton;
    private Button _quitButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        _startButton = root.Query<Button>("StartButton");
        _quitButton = root.Query<Button>("QuitButton");

        _startButton.clicked += OnStartButtonClicked;
        _quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnQuitButtonClicked()
    {
        transitionUI.EnableUI(Application.Quit);
    }

    private void OnStartButtonClicked()
    {
        transitionUI.EnableUI(() => SceneManager.LoadScene(sceneIndex));
    }
}