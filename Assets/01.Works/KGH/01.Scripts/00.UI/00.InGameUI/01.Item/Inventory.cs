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
    private VisualTreeAsset _craftListTemplate;
    private TabElement _itemTab;
    private TabElement _craftTab;

    private ItemInventory _itemInventory;
    private CraftInventory _craftInventory;

    public Inventory(VisualElement root, InventoryManager inventoryManager, VisualTreeAsset itemListTemplate,
        VisualTreeAsset craftListTemplate, InGameUI inGameUI)
    {
        _root = root;
        _inventoryManager = inventoryManager;
        _itemListTemplate = itemListTemplate;
        _craftListTemplate = craftListTemplate;
        _itemTab = _root.Q<TabElement>("ItemTab");
        _craftTab = _root.Q<TabElement>("CraftTab");
        _inGameUI = inGameUI;

        _itemInventory = new ItemInventory(_itemTab, itemListTemplate, inventoryManager, inGameUI, root);
        _craftInventory = new CraftInventory(_craftTab, craftListTemplate, inventoryManager);
    }
}