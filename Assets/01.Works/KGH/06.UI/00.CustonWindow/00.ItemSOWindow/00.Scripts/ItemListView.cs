using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemListView
{
    private ItemSOWindow _itemSOWindow;
    private ItemSO _currentItem;

    private ItemSO CurrentItem
    {
        get => _currentItem;
        set
        {
            _currentItem = value;
            OnItemSelect?.Invoke(value);
        }
    }

    private readonly Foldout _trashFoldout;
    private readonly Foldout _toolsFoldout;
    private readonly Foldout _ingredientFoldout;
    private readonly Foldout _foodFoldout;

    private readonly Button _createButton;

    private VisualTreeAsset m_itemElement = default;

    private Dictionary<ItemSO, TemplateContainer> _itemElements = new Dictionary<ItemSO, TemplateContainer>();

    public event Action<ItemSO> OnItemSelect;

    public ItemListView(VisualElement content, ItemSOWindow itemSOWindow, VisualTreeAsset itemElement)
    {
        _itemSOWindow = itemSOWindow;
        _currentItem = null;
        m_itemElement = itemElement;

        _trashFoldout = content.Q<Foldout>("TrashFoldout");
        _toolsFoldout = content.Q<Foldout>("ToolFoldout");
        _ingredientFoldout = content.Q<Foldout>("IngredientFoldout");
        _foodFoldout = content.Q<Foldout>("FoodFoldout");

        _createButton = content.Q<Button>("CreateNewButton");
        _createButton.clicked += CreateItem;

        
    }

    private void CreateItem()
    {
        var item = ScriptableObject.CreateInstance<ItemSO>();
        var guid = GUID.Generate().ToString();
        item.itemName = guid;
        item.itemType = ItemType.Trash;
        item.itemIcon = null;
        AssetDatabase.CreateAsset(item, "Assets/" + Path.Combine(_itemSOWindow.SOPath, guid + ".asset"));
        AssetDatabase.SaveAssets();
        SetUpItem(item);
        CurrentItem = item;
    }

    public void SetUpItem(ItemSO item)
    {
        var itemElement = m_itemElement.CloneTree();
        switch (item.itemType)
        {
            case ItemType.Trash:
                _trashFoldout.contentContainer.Add(itemElement);
                break;
            case ItemType.Tool:
                _toolsFoldout.contentContainer.Add(itemElement);
                break;
            case ItemType.Ingredient:
                _ingredientFoldout.contentContainer.Add(itemElement);
                break;
            case ItemType.Food:
                _foodFoldout.contentContainer.Add(itemElement);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        itemElement.Q<VisualElement>("ItemIcon").style.backgroundImage = new StyleBackground(item.itemIcon);
        itemElement.Q<Label>("ItemName").text = item.itemName;
        itemElement.Q<Button>("DeleteButton").clicked += () => HandleItemDeleted(item);

        _itemElements.Add(item, itemElement);
    }

    private void HandleItemDeleted(ItemSO item)
    {
        if(_currentItem == item)
            CurrentItem = null;
        var itemElement = _itemElements[item];
        _itemElements.Remove(item);
        itemElement.RemoveFromHierarchy();

        var path = Path.Combine(Application.dataPath, _itemSOWindow.SOPath, item.itemName + ".asset");
        try
        {
            File.Delete(path);
            File.Delete(path+".meta");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void MoveType(ItemSO item)
    {
        var itemElement = _itemElements[item];
        _itemElements.Remove(item);
        itemElement.RemoveFromHierarchy();
        SetUpItem(item);
    }
}