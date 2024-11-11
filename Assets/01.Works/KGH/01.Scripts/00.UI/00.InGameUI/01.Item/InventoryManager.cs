using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class InventoryManager : SerializedMonoBehaviour
{
    [OdinSerialize]public Dictionary<ToolType, string> ToolNames;
    [field: SerializeField] public List<DefaultItemInventory> DefaultItemInventories { get; private set; }
    public List<InventoryItem> InventoryItems { get; private set; } = new List<InventoryItem>();
    [field: SerializeField] public List<string> Inventories { get; private set; }

    public event Action<string> OnInventoryChanged;
    public event Action<List<DefaultItemInventory>> OnInventoryInitialized;
    private void Start()
    {
        OnInventoryInitialized?.Invoke(DefaultItemInventories);
    }

    public void AddItem(ItemSO item, int count, string location, bool isSeparated = false)
    {
        var inventoryItem = InventoryItems.Find(x => x.item == item);
        InventoryParents parentItem = InventoryItems.Find(x => x.item.itemName == location);
        if (parentItem == null)
        {
            parentItem = DefaultItemInventories.Find(x => x.name == location);
        }

        if (inventoryItem != null && !isSeparated)
        {
            inventoryItem.count += count;
        }
        else
        {
            inventoryItem = new InventoryItem(item, count, location);
            InventoryItems.Add(inventoryItem);
            if (item.toolType == ToolType.Inventory)
            {
                Inventories.Add(item.name);
            }
            parentItem?.itemsIn.Add(inventoryItem);
        }

        OnInventoryChanged?.Invoke(location);
    }

    public void RemoveItem(ItemSO item)
    {
        var inventoryItem = InventoryItems.Find(x => x.item == item);
        if (inventoryItem == null) return;
        
        InventoryParents parentItem = InventoryItems.Find(x => x.item.itemName == inventoryItem.loction);
        if (parentItem == null)
        {
            parentItem = DefaultItemInventories.Find(x => x.name == inventoryItem.loction);
        }

        InventoryItems.Remove(inventoryItem);
        if (item.toolType == ToolType.Inventory)
        {
            Inventories.Remove(item.name);
        }
        parentItem?.itemsIn.Remove(inventoryItem);

        OnInventoryChanged?.Invoke(inventoryItem.loction);
    }

    public void MoveItem(ItemSO item, string location)
    {
        var inventoryItem = InventoryItems.Find(x => x.item == item);
        if (inventoryItem == null) return;

        InventoryParents parentItem = InventoryItems.Find(x => x.item.itemName == inventoryItem.loction);
        if (parentItem == null)
        {
            parentItem = DefaultItemInventories.Find(x => x.name == inventoryItem.loction);
        }
        
        inventoryItem.loction = location;
        parentItem?.itemsIn.Remove(inventoryItem);

        if (parentItem != null) OnInventoryChanged?.Invoke(parentItem.name);
        OnInventoryChanged?.Invoke(location);
    }
}

