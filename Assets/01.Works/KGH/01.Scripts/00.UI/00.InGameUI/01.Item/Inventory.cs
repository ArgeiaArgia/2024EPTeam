using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory
{
    private InGameUI _inGameUI;
    private VisualElement _root;
    private InventoryManager _inventoryManager;
    private VisualTreeAsset _itemListTemplate;
    private TabElement _itemTab;
    private TabElement _craftTab;

    private ItemInventory _itemInventory;
    
    public Inventory(VisualElement root, InventoryManager inventoryManager, VisualTreeAsset itemListTemplate, InGameUI inGameUI)
    {
        _root = root;
        _inventoryManager = inventoryManager;
        _itemListTemplate = itemListTemplate;
        _itemTab = _root.Q<TabElement>("ItemTab");
        _craftTab = _root.Q<TabElement>("CraftTab");
        _inGameUI = inGameUI;
        
        _itemInventory = new ItemInventory(_itemTab, itemListTemplate, inventoryManager, inGameUI, root);
    }
}
