using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InventoryItem : InventoryParents
{
    public ItemSO item;
    public int count;
    public string loction;

    public override List<InventoryItem> itemsIn { get; set; } = new List<InventoryItem>();

    public override float holdableWeight => item.holdableWeight;

    public InventoryItem(ItemSO item, int count, string loction)
    {
        this.item = item;
        this.count = count;
        this.loction = loction;
    }

    private string _additionalName;

    public override string name
    {
        get => item.itemName + _additionalName;
        set => _additionalName = value.Remove(0, item.itemName.Length);
    }
}