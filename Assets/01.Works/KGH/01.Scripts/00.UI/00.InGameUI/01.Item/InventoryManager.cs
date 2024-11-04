using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int[,] _inventory;

    private void OnEnable()
    {
    }

    public void AddItem(ItemSO item, int count, Vector2Int position)
    {
    }

    public void RemoveItem(ItemSO item, int count)
    {
    }

    public void MoveItem(ItemSO item, Vector2Int position)
    {
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

    public InventoryItem(ItemSO item, int count, Vector2Int position)
    {
        this.item = item;
        this.count = count;
    }
}