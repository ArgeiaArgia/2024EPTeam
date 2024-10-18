using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public int itemNumber;
    public string itemName;
    public Sprite itemIcon;
    public Sprite itemSprite;
    public ItemType itemType;
    public int itemCount = 0;
    public float percentageOfCatch;
    
    public Dictionary<string, Action> ItemActions = new Dictionary<string, Action>();
}

public enum ItemType
{
    Fish,
    Food,
    Material,
    Tool
}