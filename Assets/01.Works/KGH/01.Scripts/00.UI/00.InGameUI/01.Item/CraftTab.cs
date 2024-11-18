using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftTab
{
    private List<ItemSO> _craftItems;

    private ScrollView _craftScrollView;
    private InventoryManager _inventoryManager;

    public CraftTab(List<ItemSO> craftItems, ScrollView craftScrollView, InventoryManager inventoryManager)
    {
        _craftItems = craftItems;
        _craftScrollView = craftScrollView;
        _inventoryManager = inventoryManager;

        SetUpCraftTab();
        inventoryManager.OnInventoryChanged += (location) => SetUpCraftTab();
    }

    private void SetUpCraftTab()
    {
        if (_craftItems == null) return;
        _craftScrollView.Clear();
        foreach (var item in _craftItems)
        {
            if (item == null) continue;

            var element = new CraftElement()
            {
                CurrentItem = item
            };

            if (!_inventoryManager.CheckIfMakeable(item, out var lackItems))
            {
                element.Q<Button>("CraftItem").AddToClassList("lack");
                foreach (var itemSo in lackItems)
                {
                    element.Q<VisualElement>(className:"required-icon").AddToClassList("lack");
                    element.Q<Label>(className:"required-text").AddToClassList("lack");
                }

                element.Q<Button>("CraftButton").pickingMode = PickingMode.Ignore;
            }

            element.OnCreateItem += _inventoryManager.CraftItem;
            _craftScrollView.Add(element);
        }
    }
}