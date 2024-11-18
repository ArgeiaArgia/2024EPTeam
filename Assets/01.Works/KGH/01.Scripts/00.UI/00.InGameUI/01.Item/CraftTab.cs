using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class CraftTab
{
    private ItemSO[] _craftItems;

    private ScrollView _craftScrollView;
    private InventoryManager _inventoryManager;

    public CraftTab(ItemSO[] craftItems, ScrollView craftScrollView, InventoryManager inventoryManager)
    {
        _craftItems = craftItems;
        _craftScrollView = craftScrollView;

        ResetCraftTab();
    }

    private void ResetCraftTab()
    {
        _craftScrollView.Clear();
        foreach (var item in _craftItems)
        {
            var element = new CraftElement()
            {
                CurrentItem = item
            };

            if (!_inventoryManager.CheckIfMakeable(item, out var unmakeableItems))
            {
                element.AddToClassList("lack");
                foreach (var unmakeable in unmakeableItems)
                {
                    element.IngredientItems[unmakeable].AddToClassList("lack");
                }
            }
            
            element.OnCreateItem += _inventoryManager.CraftItem;
            _craftScrollView.Add(element);
        }
    }
}