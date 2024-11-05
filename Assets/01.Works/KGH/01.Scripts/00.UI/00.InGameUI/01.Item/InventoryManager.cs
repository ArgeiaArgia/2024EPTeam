using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> inventoryItems { get; private set; }

    public event Action<List<InventoryItem>> OnInventoryChanged;

    public void AddItem(ItemSO item, int count)
    {
        inventoryItems.Add(new InventoryItem(item, count));
        OnInventoryChanged?.Invoke(inventoryItems);
    }

    public void RemoveItem(ItemSO item)
    {
        var itemIndex = inventoryItems.FindIndex(x => x.item == item);
        inventoryItems.RemoveAt(itemIndex);
        OnInventoryChanged?.Invoke(inventoryItems);
    }

    public void MoveItem(ItemSO item, ItemSO parentItem)
    {
        var itemIndex = inventoryItems.FindIndex(x => x.item == item);
        var parentItemIndex = inventoryItems.FindIndex(x => x.item == parentItem);

        inventoryItems[parentItemIndex].itemsKey.Add(item);
        inventoryItems[parentItemIndex].itemsValue.Add(inventoryItems[itemIndex].count);
        inventoryItems.RemoveAt(itemIndex);
        OnInventoryChanged?.Invoke(inventoryItems);
    }
}

[Serializable]
public class InventoryItem
{
    public ItemSO item;
    public int count;

    [ShowIf("item.toolType", ToolType.Inventory)]
    public Dictionary<ItemSO, int> itemsIn
    {
        get
        {
            var returnDic = new Dictionary<ItemSO, int>();
            for (var i = 0; i < itemsKey.Count; i++)
            {
                returnDic.Add(itemsKey[i], itemsValue[i]);
            }

            return returnDic;
        }
        set
        {
            itemsKey.Clear();
            itemsValue.Clear();

            foreach (var item in value)
            {
                itemsKey.Add(item.Key);
                itemsValue.Add(item.Value);
            }
        }
    }

    [HideInInspector] public List<ItemSO> itemsKey;
    [HideInInspector] public List<int> itemsValue;

    public InventoryItem(ItemSO item, int count)
    {
        this.item = item;
        this.count = count;
    }
}