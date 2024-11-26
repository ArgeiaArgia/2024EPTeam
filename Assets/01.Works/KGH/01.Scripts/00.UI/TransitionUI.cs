using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TransitionUI : ToolkitParents
{
    private List<VisualElement> _boxes;

    private void Start()
    {
        StartCoroutine(ShowBoxes());
        StartCoroutine(ShowBoxesOnStart());
    }

    IEnumerator ShowBoxesOnStart()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(HideBoxes());
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _boxes = root.Query<VisualElement>("Box").ToList();
    }
    
    public void EnableUI(Action action = null)
    {
        StartCoroutine(ShowBoxes(action));
    }

    IEnumerator ShowBoxes(Action action = null)
    {
        foreach (var box in _boxes)
        {
            box.RemoveFromClassList("hide");
        }

        yield return new WaitForSeconds(1.5f);
        action?.Invoke();
    }

    public void DisableUI(Action action = null)
    {
        StartCoroutine(HideBoxes(action));
    }

    private IEnumerator HideBoxes(Action action = null)
    {
        foreach (var box in _boxes)
        {
            box.AddToClassList("hide");
        }

        yield return new WaitForSeconds(1.5f);
        action?.Invoke();
    }

    public void ChangeScene(int index)
    {
        EnableUI(() => UnityEngine.SceneManagement.SceneManager.LoadScene(index));
    }
}