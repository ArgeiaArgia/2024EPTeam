using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FoldableElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<FoldableElement, UxmlTraits>
    {
    }
    
    bool isFolded = false;

    private VisualElement foldingTab;
    private Label foldableNameLabel;
    private Button foldableButton;
    private VisualElement foldableContents;

    public string FoldableName
    {
        get => foldableNameLabel.text;
        set => foldableNameLabel.text = value;
    }

    private Color headerColor;
    public Color HeaderColor
    {
        get => headerColor;
        set
        {
            headerColor = value;
            foldingTab.style.backgroundColor = headerColor;
        }
    }

    public event Action<bool> OnFoldableButtonClicked; 
    public override VisualElement contentContainer => this.foldableContents;

    private void Initialize()
    {
        foldingTab = this.Q<VisualElement>("FoldingTab");
        foldableNameLabel = this.Q<Label>("Name");
        foldableButton = this.Q<Button>("FoldButton");
        foldableContents = this.Q<VisualElement>("Contents");
        
        foldableButton.clickable.clicked += FoldButtonClicked;
    }

    public FoldableElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/FoldableElement");
        visualTree.CloneTree(this);
        
        var styleSheet = Resources.Load<StyleSheet>("CustomControls/FoldableElement");
        styleSheets.Add(styleSheet);
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_foldableName = new()
            { name = "foldable-name", defaultValue = "Foldable Contents" };
        UxmlColorAttributeDescription m_headerColor = new()
            { name = "header-color", defaultValue = Color.gray };
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            if (ve is FoldableElement foldableElement)
            {
                foldableElement.Clear();

                foldableElement.Initialize();
                foldableElement.FoldableName = this.m_foldableName.GetValueFromBag(bag, cc);
                foldableElement.foldableContents = foldableElement.contentContainer;
                
                foldableElement.HeaderColor = this.m_headerColor.GetValueFromBag(bag, cc);
            }
        }
    }
    void FoldButtonClicked()
    {
        isFolded = !isFolded;
        contentContainer.ToggleInClassList("hide");
        
        OnFoldableButtonClicked?.Invoke(isFolded);
    }
}