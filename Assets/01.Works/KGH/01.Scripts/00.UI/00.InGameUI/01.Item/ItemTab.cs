using System.Collections.Generic;
using UnityEngine.UIElements;

public class ItemTab : Inventory
{
    
    public ItemTab(VisualElement root, InventoryManager inventoryManager) : base(root, inventoryManager)
    {
    }

    protected override void UpdateInventory(List<InventoryItem> obj)
    {
    }
}