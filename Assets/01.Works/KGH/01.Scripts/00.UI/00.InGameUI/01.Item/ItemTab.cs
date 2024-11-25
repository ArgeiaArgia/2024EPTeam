using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ItemTab
{
    private readonly ItemInventory _itemInventory;
    private readonly InventoryManager _inventoryManager;

    private VisualElement _itemList;
    private readonly ScrollView _itemScrollView;
    private readonly Dictionary<InventoryItem, ItemElement> _itemElements;
    private readonly Dictionary<ToolType, string> _toolNames;
    private readonly Dictionary<ItemElement, ItemElementInteract> _itemElementInteracts;
    private readonly InventoryParents _parentItem;
    private readonly Label _weightLabel;

    private readonly InGameUI _inGameUI;
    private readonly VisualElement _root;

    #region HoverDescription

    private InventoryItem _hoverItem;
    private readonly VisualElement _itemDesc;
    private readonly Label _hoverName;
    private readonly Label _hoverCategory;
    private readonly VisualElement _effectName;
    private readonly VisualElement _effectValue;
    private readonly Label _hoverDescription;

    #endregion

    private readonly Dictionary<ItemSO, List<InteractEvent>> _itemEvents;


    public ItemTab(VisualElement itemList, InventoryParents parentItem, ItemInventory itemInventory,
        Dictionary<ToolType, string> toolNames, InGameUI inGameUI, VisualElement root,
        InventoryManager inventoryManager)
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
        _inventoryManager = inventoryManager;

        _itemDesc = _itemList.Q("ItemDesc");
        _hoverName = _itemDesc.Q<Label>("TitleLabel");
        _hoverCategory = _itemDesc.Q<Label>("ItemCategory");
        _hoverDescription = _itemDesc.Q<Label>("Description");
        _effectName = _itemDesc.Q("EffectName");
        _effectValue = _itemDesc.Q("EffectValue");
    }


    public void UpdateItemTab()
    {
        for (var i = _itemElements.Count - 1; i >= 0; i--)
        {
            var item = _itemElements.ElementAt(i).Key;
            var element = _itemElements.ElementAt(i).Value;

            if (_parentItem.itemsIn.Contains(item)) continue;
            element.RemoveFromHierarchy();
            _itemElements.Remove(item);

            // if (item.item.toolType == ToolType.Inventory && _parentItem.GetType() != typeof(DefaultItemInventory))
            // {
            //     Debug.Log("Item is in the item");
            //     _itemInventory.RemoveItemTab(item.name);
            // }
        }

        foreach (var item in _parentItem.itemsIn)
        {
            if (_itemElements.TryGetValue(item, out var element))
            {
                element.Value = item.count;
                continue;
            }

            var itemElement = new ItemElement
            {
                Name = item.name,
                Value = item.count,
                CurrentIcon = item.item.itemIcon
            };

            itemElement.Category = item.item.itemType switch
            {
                ItemType.Tool => _toolNames[item.item.toolType],
                ItemType.Trash => "쓰레기",
                ItemType.Food => "음식",
                ItemType.Ingredient => "식재료",
                _ => itemElement.Category
            };

            itemElement.AddToClassList("item-element");

            _itemScrollView.Add(itemElement);
            _itemElements.Add(item, itemElement);

            _itemElementInteracts.Add(itemElement, new ItemElementInteract(itemElement, item, _root, _inGameUI,
                this, _inventoryManager));

            if (item.item.toolType == ToolType.Inventory && _parentItem.GetType() == typeof(DefaultItemInventory))
            {
                _itemInventory.AddItemTab(item);
            }
            else if (item.item.toolType == ToolType.Inventory && _parentItem.GetType() != typeof(DefaultItemInventory))
            {
                _itemInventory.RemoveItemTab(item.name);
            }
        }

        var currentWeight = _parentItem.itemsIn.Sum(x => x.item.weight * x.count);
        _weightLabel.text = $"{currentWeight} / {_parentItem.holdableWeight}";
    }

    public bool CheckItemIsInventory(VisualElement itemElement) => _itemElements.Any(x => x.Value == itemElement);


    public string GetItemName(VisualElement itemElement)
    {
        return _itemElements.FirstOrDefault(x => x.Value == itemElement).Key.name;
    }

    public InventoryItem GetItem(VisualElement itemElement)
    {
        return _itemElements.FirstOrDefault(x => x.Value == itemElement).Key;
    }

    public bool IsInventory(string loction)
    {
        return _inventoryManager.Inventories.Contains(loction);
    }

    public void ItemHover(InventoryItem item)
    {
        _hoverItem = item;
        if (item == null)
        {
            _itemDesc.style.display = DisplayStyle.None;
            return;
        }

        _itemDesc.style.display = DisplayStyle.Flex;

        _hoverName.text = item.name;
        _hoverCategory.text = item.item.itemType switch
        {
            ItemType.Tool => _toolNames[item.item.toolType],
            ItemType.Trash => "쓰레기",
            ItemType.Food => "음식",
            ItemType.Ingredient => "식재료",
            _ => _hoverCategory.text
        };

        _hoverDescription.text = item.item.description;

        _effectName.Clear();
        _effectValue.Clear();

        var weightName = new Label { text = "무게 : " };
        var weightValue = new Label { text = item.item.weight.ToString() };
        _effectName.Add(weightName);
        _effectValue.Add(weightValue);

        if (item.count > 1)
        {
            var countName = new Label { text = "총 무게" };
            var countValue = new Label { text = (item.count * item.item.weight).ToString() };
            _effectName.Add(countName);
            _effectValue.Add(countValue);
        }

        foreach (var effect in item.item.StatEffect)
        {
            var effectText = effect.Key switch
            {
                StatType.Hunger => "배고픔",
                StatType.Thirst => "목마름",
                StatType.Tired => "피로도",
                StatType.Bored => "지루함",
                StatType.Health => "건강함",
                _ => effect.Key.ToString()
            };

            var effectName = new Label { text = $"{effectText} : " };
            var effectValue = new Label { text = effect.Value.ToString() };
            effectValue.AddToClassList(effect.Value > 0 ? "positive-effect" : "negative-effect");

            _effectName.Add(effectName);
            _effectValue.Add(effectValue);
        }

        _inGameUI.CoroutineHelper(DescriptionFollowingMouse());
    }

    private IEnumerator DescriptionFollowingMouse()
    {
        _root.Add(_itemDesc);
        _itemDesc.style.position = Position.Absolute;
        while (_hoverItem != null)
        {
            var mousePos = Mouse.current.position.ReadValue();
            var localMousePos = _root.WorldToLocal(new Vector2(mousePos.x, Screen.height - mousePos.y));
            _itemDesc.style.left = localMousePos.x + 25;
            _itemDesc.style.top = localMousePos.y + 25;
            yield return null;
        }
    }

    public void ShowInteractions(ItemSO item)
    {
        _inGameUI.ShowInteractions(_inventoryManager.ItemEvents[item]);
        ItemHover(null);
    }

    public void SetPickingMode(bool isIgnore)
    {
        foreach (var element in _itemElementInteracts)
        {
            element.Value.ChangePickingMode(isIgnore ? PickingMode.Ignore : PickingMode.Position);
        }
    }

    public string OverlappedTabButton(InventoryItem item) => _itemInventory.OverlappedTabButton(item);
}