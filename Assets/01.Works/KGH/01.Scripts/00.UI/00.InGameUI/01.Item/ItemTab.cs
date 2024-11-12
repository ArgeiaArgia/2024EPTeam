using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemTab
{
    private ItemInventory _itemInventory;

    private VisualElement _itemList;
    private ScrollView _itemScrollView;
    private Dictionary<InventoryItem, ItemElement> _itemElements;
    private Dictionary<ToolType, string> _toolNames;
    private Dictionary<ItemElement, ItemElementInteract> _itemElementInteracts;
    private InventoryParents _parentItem;
    private Label _weightLabel;

    private InGameUI _inGameUI;
    private VisualElement _root;

    public ItemTab(VisualElement itemList, InventoryParents parentItem, ItemInventory itemInventory,
        Dictionary<ToolType, string> toolNames, InGameUI inGameUI, VisualElement root)
    {
        _itemInventory = itemInventory;

        _itemList = itemList;
        _itemScrollView = _itemList.Q<ScrollView>("ItemContainer");
        _itemElements = new Dictionary<InventoryItem, ItemElement>();
        _itemElementInteracts = new Dictionary<ItemElement, ItemElementInteract>();
        _parentItem = parentItem;
        _weightLabel = _itemList.Q<Label>("Weight");
        _weightLabel.text = $"{0} / {_parentItem.holdableWeight}";
        _toolNames = toolNames;
        _inGameUI = inGameUI;
        _root = root;
    }


    public void UpdateItemTab()
    {
        foreach (var (item, element) in _itemElements)
        {
            if (_parentItem.itemsIn.Contains(item)) continue;
            element.RemoveFromHierarchy();
            _itemElements.Remove(item);
        }

        foreach (var item in _parentItem.itemsIn)
        {
            if (_itemElements.ContainsKey(item))
            {
                _itemElements[item].Value = item.count;
                continue;
            }

            var itemElement = new ItemElement();
            itemElement.Name = item.name;
            itemElement.Value = item.count;
            itemElement.CurrentIcon = item.item.itemIcon;

            switch (item.item.itemType)
            {
                case ItemType.Tool:
                    itemElement.Category = _toolNames[item.item.toolType];
                    break;
                case ItemType.Trash:
                    itemElement.Category = "쓰레기";
                    break;
                case ItemType.Food:
                    itemElement.Category = "음식";
                    break;
                case ItemType.Ingredient:
                    itemElement.Category = "식재료";
                    break;
            }

            itemElement.AddToClassList("item-element");

            _itemScrollView.Add(itemElement);
            _itemElements.Add(item, itemElement);

            _itemElementInteracts.Add(itemElement, new ItemElementInteract(itemElement, _root, _inGameUI));

            if (item.item.toolType == ToolType.Inventory)
                itemElement.RegisterCallback<MouseDownEvent>(evt => { _itemInventory.AddItemTab(item); });
        }

        float currentWeight = _parentItem.itemsIn.Sum(x => x.item.weight * x.count);
        _weightLabel.text = $"{currentWeight} / {_parentItem.holdableWeight}";
    }
}