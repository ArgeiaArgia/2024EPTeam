using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FoldableElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<FoldableElement, UxmlTraits> { }

    private Label foldableName;
    private Button foldableButton;
    
    private VisualElement foldableContents;
    public override VisualElement contentContainer => this.foldableContents;

    public FoldableElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/FoldableElement");
        visualTree.CloneTree(this);

        foldableName = this.Q<Label>("Name");
        foldableButton = this.Q<Button>("FoldButton");
        foldableContents = this.Q<VisualElement>("Contents");
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        
    }
}
