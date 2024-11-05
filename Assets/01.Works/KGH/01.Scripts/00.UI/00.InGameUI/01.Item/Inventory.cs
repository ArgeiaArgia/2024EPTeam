using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Inventory
{
    public Inventory(VisualElement root, InventoryManager inventoryManager)
    {
        inventoryManager.OnInventoryChanged += UpdateInventory;
    }

    protected abstract void UpdateInventory(List<InventoryItem> obj);
}
