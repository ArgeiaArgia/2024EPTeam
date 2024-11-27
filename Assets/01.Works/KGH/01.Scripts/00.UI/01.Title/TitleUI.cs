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
    private VisualElement _container;
    private VisualElement _buttonContainer;

    protected override void OnEnable()
    {
        base.OnEnable();
        _startButton = root.Query<Button>("StartButton");
        _quitButton = root.Query<Button>("QuitButton");
        _container = root.Query<VisualElement>("Container");
        _buttonContainer = root.Query<VisualElement>("Buttons");

        _startButton.clicked += OnStartButtonClicked;
        _quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnDisable()
    {
        _startButton.clicked -= OnStartButtonClicked;
        _quitButton.clicked -= OnQuitButtonClicked;
    }

    private void Start()
    {
        StartCoroutine(ShowBoxesOnStart());
    }

    private IEnumerator ShowBoxesOnStart()
    {
        yield return new WaitForSeconds(2f);
        _container.RemoveFromClassList("hide");
        yield return new WaitForSeconds(2f);
        _buttonContainer.RemoveFromClassList("hide");
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