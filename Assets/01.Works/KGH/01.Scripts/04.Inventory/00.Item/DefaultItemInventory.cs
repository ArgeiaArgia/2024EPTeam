using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefaultItemInventory : InventoryParents
{
    [SerializeField] private string _name;
    [SerializeField] float _holdableWeight;

    public override string name
    {
        get=>_name;
        set=>_name = value;
    }
    public override List<InventoryItem> itemsIn { get; set; } = new List<InventoryItem>();
    public override float holdableWeight => _holdableWeight;   
}