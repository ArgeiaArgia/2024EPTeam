using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemListView
{
    private ItemSOWindow _itemSOWindow;
    private ItemSO _currentItem;

    public ItemSO CurrentItem
    {
        get=>_currentItem;
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

    private Button _createButton;

    [SerializeField] private string m_soPath = "Assets/00.ForEveryone/02.SOs/03.ItemSOs";
    [SerializeField] private VisualTreeAsset m_itemElement = default;
    
    public event Action<ItemSO> OnItemSelect;
    public event Action<ItemSO> OnItemDelete;
    
    public ItemListView(VisualElement content, ItemSOWindow itemSOWindow)
    {
        _itemSOWindow = itemSOWindow;
        _currentItem = null;
        
        _trashFoldout = content.Q<Foldout>("MaterialToolFoldout");
        _toolsFoldout = content.Q<Foldout>("ToolFoldout");
        _ingredientFoldout = content.Q<Foldout>("IngredientFoldout");
        _foodFoldout = content.Q<Foldout>("FoodFoldout");
        
        _createButton = content.Q<Button>("CreateButton");

        foreach (var item in GetItemList())
        {
            SetUpItem(item);
        }
    }

    private void SetUpItem(ItemSO item)
    {
        var itemElement = m_itemElement.CloneTree();
        switch (item.itemType)
        {
            case ItemType.Trash :
                _trashFoldout.contentContainer.Add(itemElement);
                break;
            case ItemType.Tool :
                _toolsFoldout.contentContainer.Add(itemElement);
                break;
            case ItemType.Ingredient :
                _ingredientFoldout.contentContainer.Add(itemElement);
                break;
            case ItemType.Food :
                _foodFoldout.contentContainer.Add(itemElement);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        itemElement.Q<VisualElement>("ItemIcon").style.backgroundImage = new StyleBackground(item.itemIcon);
        itemElement.Q<Label>("ItemName").text = item.itemName;
        itemElement.Q<Button>("DeleteButton").clicked += ()=> ItemDeleted(itemElement);
    }

    private void ItemDeleted(TemplateContainer item)
    {
        
        CurrentItem = null;
        item.RemoveFromHierarchy();
    }

    private List<ItemSO> GetItemList()
    {
        var dir = new DirectoryInfo(m_soPath);
        var info = dir.GetFiles("*.asset");
        return info.Select(file => UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSO>(m_soPath + "/" + file.Name)).Where(item => item != null).ToList();
    }
}
