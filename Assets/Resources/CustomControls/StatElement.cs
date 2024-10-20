using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StatElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<StatElement> { }

    private VisualElement _icon;
    private Label _name;
    private ProgressBar _progressBar;
    
    public StatElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/StatElement");
        visualTree.CloneTree(this);

        var styleSheet = Resources.Load<StyleSheet>("CustomControls/StatElement");
        styleSheets.Add(styleSheet);
    }
}
