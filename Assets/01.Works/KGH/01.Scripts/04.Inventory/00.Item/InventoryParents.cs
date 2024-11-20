using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryParents
{
    public abstract string name { get; set; }
    public abstract List<InventoryItem> itemsIn { get; set; }
    public abstract float holdableWeight { get; } 
}
