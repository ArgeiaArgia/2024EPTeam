using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : SerializedMonoBehaviour
{
    [OdinSerialize] public Dictionary<ItemSO, List<InteractEvent>> ItemEvents;
    [OdinSerialize] public Dictionary<ToolType, string> ToolNames;
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
        var inventoryItems = InventoryItems.FindAll(x => x.item == item);
        var parentItem = InventoryItems.Find(x => x.item.itemName == location) ??
                         (InventoryParents)DefaultItemInventories.Find(x => x.name == location);

        if (item.toolType == ToolType.Inventory) isSeparated = true;

        var itemInInventory = inventoryItems.Find(x => x.loction == location);
        if (inventoryItems.Count > 0 && !isSeparated && itemInInventory != null)
        {
            inventoryItems.Find(x => x.loction == location).count += count;
        }
        else
        {
            InventoryItem inventoryItem;
            if (isSeparated && InventoryItems.Find(x => x.name == item.name && x.loction == location) != null && item
                    .toolType == ToolType.Inventory)
            {
                var itemCount = InventoryItems.FindAll(x => x.item.itemName == item.itemName).Count;
                inventoryItem = new InventoryItem(item, count, location)
                {
                    name = $"{item.name} {itemCount}"
                };
            }
            else
            {
                inventoryItem = new InventoryItem(item, count, location);
            }

            InventoryItems.Add(inventoryItem);
            if (item.toolType == ToolType.Inventory)
            {
                Inventories.Add(item.name);
            }

            parentItem?.itemsIn.Add(inventoryItem);
        }

        OnInventoryChanged?.Invoke(location);
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        var parentItem = InventoryItems.Find(x => x.item.itemName == inventoryItem.loction) ??
                         (InventoryParents)DefaultItemInventories.Find(x => x.name == inventoryItem.loction);

        InventoryItems.Remove(inventoryItem);
        if (inventoryItem.item.toolType == ToolType.Inventory)
        {
            Inventories.Remove(inventoryItem.name);
        }

        parentItem?.itemsIn.Remove(inventoryItem);

        OnInventoryChanged?.Invoke(inventoryItem.loction);
    }

    public void MoveItem(InventoryItem inventoryItem, string location)
    {
        var preParentItem = InventoryItems.Find(x => x.item.itemName == inventoryItem.loction) ??
                            (InventoryParents)DefaultItemInventories.Find(x => x.name == inventoryItem.loction);

        RemoveItem(inventoryItem);
        AddItem(inventoryItem.item, inventoryItem.count, location);

        if (preParentItem != null) OnInventoryChanged?.Invoke(preParentItem.name);
        OnInventoryChanged?.Invoke(location);
    }
}

[Serializable]
public class InteractEvent
{
    public string EventName;
    public UnityEvent OnInteract;
}