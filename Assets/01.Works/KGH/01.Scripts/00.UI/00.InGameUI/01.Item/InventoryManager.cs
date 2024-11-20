using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : SerializedMonoBehaviour
{
    [OdinSerialize] public Dictionary<ItemSO, List<InteractEvent>> ItemEvents;
    [OdinSerialize] public Dictionary<ToolType, string> ToolNames;
    [field: SerializeField] public List<DefaultItemInventory> DefaultItemInventories { get; private set; }
    private List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    [field: SerializeField] public List<string> Inventories { get; private set; }
    [field: SerializeField] public ItemListSO ItemListSO { get; private set; }

    public event Action<string> OnInventoryChanged;
    public event Action<List<DefaultItemInventory>> OnInventoryInitialized;

    private void Start()
    {
        OnInventoryInitialized?.Invoke(DefaultItemInventories);
    }

    #region item

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
            if (isSeparated && InventoryItems.Find(x => x.name == item.name) != null && item
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
                Inventories.Add(inventoryItem.name);
            }

            parentItem?.itemsIn.Add(inventoryItem);
        }

        OnInventoryChanged?.Invoke(location);
    }

    private void AddItem(InventoryItem item, string location, bool isSeparated = false)
    {
        var inventoryItems = InventoryItems.FindAll(x => x.item == item.item);
        var parentItem = InventoryItems.Find(x => x.name == location) ??
                         (InventoryParents)DefaultItemInventories.Find(x => x.name == location);

        if (item.item.toolType == ToolType.Inventory) isSeparated = true;

        item.loction = location;

        var itemInInventory = inventoryItems.Find(x => x.loction == location);
        if (inventoryItems.Count > 0 && !isSeparated && itemInInventory != null)
        {
            inventoryItems.Find(x => x.loction == location).count += item.count;
        }
        else
        {
            InventoryItems.Add(item);
            parentItem?.itemsIn.Add(item);
            if (item.item.toolType == ToolType.Inventory && parentItem.GetType() == typeof(DefaultItemInventory))
            {
                Inventories.Add(item.name);
            }
        }

        OnInventoryChanged?.Invoke(location);
    }

    private void RemoveItem(InventoryItem inventoryItem, bool isMoving = false)
    {
        InventoryItems.Remove(inventoryItem);
        var parentItem = InventoryItems.Find(x => x.name == inventoryItem.loction) ??
                         (InventoryParents)DefaultItemInventories.Find(x => x.name == inventoryItem.loction);
        parentItem?.itemsIn.Remove(inventoryItem);
        if (inventoryItem.item.toolType == ToolType.Inventory && !isMoving)
        {
            Inventories.Remove(inventoryItem.name);
        }
    }

    public void MoveItem(InventoryItem inventoryItem, string location)
    {
        var preParentItem = InventoryItems.Find(x => x.name == inventoryItem.loction) ??
                            (InventoryParents)DefaultItemInventories.Find(x => x.name == inventoryItem.loction);

        RemoveItem(inventoryItem, true);
        AddItem(inventoryItem, location);

        if (preParentItem != null) OnInventoryChanged?.Invoke(preParentItem.name);
        OnInventoryChanged?.Invoke(location);
    }

    #endregion

    #region craft

    public bool CheckIfMakeable(ItemSO item, out List<ItemSO> lackItems)
    {
        var craftItems = item.materialList;
        lackItems = (from craftItem in craftItems
            let inventoryItem = InventoryItems.Find(x => x.item == craftItem.Key && x.count >= craftItem.Value)
            where inventoryItem == null
            select craftItem.Key).ToList();

        return lackItems.Count <= 0;
    }

    public void CraftItem(ItemSO item)
    {
        if (!CheckIfMakeable(item, out var lackItems)) return;
        foreach (var craftItem in item.materialList)
        {
            var inventoryItem = InventoryItems.Find(x => x.item == craftItem.Key && x.count >= craftItem.Value);
            inventoryItem.count -= craftItem.Value;
            if (inventoryItem.count <= 0)
            {
                RemoveItem(inventoryItem);
            }
        }
        AddItem(item, 1, "갑판");
    }
    #endregion

}

[Serializable]
public class InteractEvent
{
    public string EventName;
    public UnityEvent OnInteract;
}