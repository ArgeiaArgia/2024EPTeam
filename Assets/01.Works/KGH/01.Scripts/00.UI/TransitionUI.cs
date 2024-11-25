using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TransitionUI : ToolkitParents
{
    private List<VisualElement> _boxes;

    protected override void OnEnable()
    {
        base.OnEnable();
        _boxes = root.Query<VisualElement>("Box").ToList();
    }

    public override void EnableUI()
    {
        base.EnableUI();
    }

    public void EnableUI(Action action)
    {
        
    }
    IEnumerator ShowBoxes()
    {
        foreach (var box in _boxes)
        {
            box.AddToClassList("Show");
        }
            yield return new WaitForSeconds(0.1f);
    }
}