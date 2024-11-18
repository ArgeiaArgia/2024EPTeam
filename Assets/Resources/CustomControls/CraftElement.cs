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

    public Dictionary<ItemSO, VisualElement> IngredientItems { get; private set; } = new Dictionary<ItemSO, VisualElement>();

    public event Action<ItemSO> OnCreateItem;
    public CraftElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/CraftElement");
        visualTree.CloneTree(this);
        
        var styleSheet = Resources.Load<StyleSheet>("CustomControls/CraftElement");
        styleSheets.Add(styleSheet);

        var craftItem = this.Q<Button>("CraftItem");
        _itemIcon = this.Q<VisualElement>("Icon");
        _itemName = this.Q<Label>("ItemName");
        var ingredientContainer = this.Q<VisualElement>("Ingredients");
        _ingredientList = this.Q<VisualElement>("ItemList");
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
        IngredientItems.Clear();
        foreach (var material in _currentItem.materialList)
        {
            var ingredientElement = new VisualElement();
            ingredientElement.AddToClassList("required-item");
            var ingredientIcon = new VisualElement();
            ingredientIcon.style.backgroundImage = new StyleBackground(material.Key.itemIcon);
            ingredientIcon.AddToClassList("required-icon");
            var ingredientCount = new Label(material.Value.ToString());
            ingredientCount.AddToClassList("ingredient-text");
            ingredientElement.Add(ingredientIcon);
            ingredientElement.Add(ingredientCount);
            _ingredientList.Add(ingredientElement);
            IngredientItems.Add(material.Key, ingredientElement);
        }
    }
}