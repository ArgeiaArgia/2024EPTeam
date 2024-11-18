using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<CraftElement, UxmlTraits> { }

    private readonly VisualElement _itemIcon;
    private readonly Label _itemName;

    private readonly VisualElement _ingredientList;

    private ItemSO _currentItem;
    public ItemSO CurrentItem
    {
        get => _currentItem;
        set
        {
            _currentItem = value;
            InitializeCraftItem();
        }
    }

    private Dictionary<ItemSO, Label> _ingredientCountLabels = new Dictionary<ItemSO, Label>();
    public event Action<ItemSO> OnCreateItem;
    public CraftElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/CraftElement");
        visualTree.CloneTree(this);
        
        var styleSheet = Resources.Load<StyleSheet>("CustomControls/CraftElement");
        styleSheets.Add(styleSheet);

        var craftItem = this.Q<Button>("CraftItem");
        _itemIcon = this.Q<VisualElement>("ItemIcon");
        _itemName = this.Q<Label>("ItemName");
        var ingredientContainer = this.Q<VisualElement>("Ingredients");
        _ingredientList = this.Q<VisualElement>("IngredientList");
        var craftButton = this.Q<Button>("CraftButton");
        
        craftItem.clickable.clicked += () =>
        {
            ingredientContainer.ToggleInClassList("hide");
        };
        craftButton.clickable.clicked += () =>
        {
            OnCreateItem?.Invoke(_currentItem);
        };
    }
    private void InitializeCraftItem()
    {
        _itemIcon.style.backgroundImage = new StyleBackground(_currentItem.itemIcon);
        _itemName.text = _currentItem.itemName;
        _ingredientList.Clear();
        _ingredientCountLabels.Clear();
        foreach (var material in _currentItem.materialList)
        {
            var ingredientElement = new VisualElement();
            ingredientElement.AddToClassList("required-item");
            var ingredientIcon = new VisualElement();
            ingredientIcon.style.backgroundImage = new StyleBackground(material.Key.itemIcon);
            ingredientIcon.AddToClassList("required-icon");
            var ingredientCount = new Label(material.Value.ToString());
            ingredientCount.AddToClassList("ingredient-text");
            _ingredientCountLabels.Add(material.Key, ingredientCount);
            ingredientElement.Add(ingredientIcon);
            ingredientElement.Add(ingredientCount);
            _ingredientList.Add(ingredientElement);
        }
    }
}