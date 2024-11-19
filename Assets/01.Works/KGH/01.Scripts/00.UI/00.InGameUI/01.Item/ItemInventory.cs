using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ItemInventory
{
    private readonly TabElement _itemTab;
    private readonly VisualElement _tabContent;
    private readonly VisualTreeAsset _itemListTemplate;
    private readonly VisualElement _root;
    private readonly InventoryManager _inventoryManager;
    private readonly Dictionary<VisualElement, ItemTab> _itemTabs;
    private readonly Dictionary<string, VisualElement> _itemTabElements;

    private readonly InGameUI _inGameUI;

    public ItemInventory(TabElement itemTab, VisualTreeAsset itemListTemplate, InventoryManager inventoryManager,
        InGameUI inGameUI, VisualElement root)
    {
        _itemTab = itemTab;
        _inventoryManager = inventoryManager;
        _itemListTemplate = itemListTemplate;

        _itemTabs = new Dictionary<VisualElement, ItemTab>();
        _itemTabElements = new Dictionary<string, VisualElement>();

        inventoryManager.OnInventoryChanged += HandleInventoryChanged;
        inventoryManager.OnInventoryInitialized += HandleInventoryInitialized;

        _tabContent = _itemTab.Q<VisualElement>("TabContents");
        _inGameUI = inGameUI;

        _root = root;
        _inGameUI.OnInteracting += HandleInteracting;
    }

    private void HandleInteracting(bool obj)
    {
        foreach (var itemTab in _itemTabs)
        {
            itemTab.Value.SetPickingMode(obj);
        }
    }

    private void HandleInventoryInitialized(List<DefaultItemInventory> defaultItemInventories)
    {
        _itemTab.TabNames = "";
        foreach (var defaultItemInventory in defaultItemInventories)
        {
            AddItemTab(defaultItemInventory);
        }

        _itemTab.TabCount = defaultItemInventories.Count;
    }

    private void HandleInventoryChanged(string location)
    {
        var updatedTab = _itemTabElements[location];
        _itemTabs[updatedTab].UpdateItemTab();
    }

    public void AddItemTab(InventoryParents parentItem)
    {
        var currentTab = _itemTab.CurrentTabButton;
        if (_itemTabElements.ContainsKey(parentItem.name)) return;

        _itemTab.TabNames += $"{parentItem.name},";
        _itemTab.TabCount++;

        var tab = _itemListTemplate.CloneTree();
        tab.name = parentItem.name;

        _tabContent.Add(tab);
        _itemTabElements.Add(parentItem.name, tab);
        _itemTabs.Add(tab,
            new ItemTab(tab, parentItem, this, _inventoryManager.ToolNames, _inGameUI, _root, _inventoryManager));

        tab.style.display = DisplayStyle.None;

        _itemTabs[tab].UpdateItemTab();
        _itemTab.TabButtonClick(currentTab);
    }

    public void RemoveItemTab(string tabName)
    {
        var currentTab = _itemTab.CurrentTabButton;
        if (!_itemTabElements.ContainsKey(tabName)) return;
        Debug.Log("remove tab");

        _itemTab.TabNames = _itemTab.TabNames.Replace($"{tabName},", "");
        _itemTab.TabCount--;

        var tab = _itemTabElements[tabName];
        _itemTabs.Remove(tab);
        _itemTabElements.Remove(tabName);

        tab.RemoveFromHierarchy();

        if (string.Equals(_itemTab.CurrentTabButton.text, tabName))
        {
            Debug.Log($"{_itemTab.CurrentTabButton.text} and {tabName} are same");
            _itemTab.TabButtonClick(_itemTab.TabButtons[0]);
        }
        else
        {
            _itemTab.TabButtonClick(currentTab);
        }
    }

    public bool IsInventory(string item)
    {
        return _inventoryManager.Inventories.Contains(item);
    }

    public string OverlappedTabButton(InventoryItem item)
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var localMousePosition = _root.WorldToLocal(new Vector2(mousePosition.x, Screen.height - mousePosition.y));

        foreach (var itemTab in _itemTab.TabButtons)
        {
            if (itemTab.text == _itemTab.CurrentTabButton.text || itemTab.text == item.name || item.itemsIn.Find(x =>
                    x.name == itemTab.text) != null) continue;

            if (itemTab.worldBound.Contains(localMousePosition))
            {
                return itemTab.text;
            }
        }

        return null;
    }
}