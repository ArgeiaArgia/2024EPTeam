using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItemSO : ItemSO
{
    public List<ItemSO> MaterialList = new List<ItemSO>();
    public ToolType toolType;
}

public enum ToolType
{
    Material,
    FishingRod,
}