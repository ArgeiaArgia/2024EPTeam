using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public int itemNumber; // 아이템 번호
    public string itemName; // 아이템 이름
    public Sprite itemIcon; // 아이템 아이콘
    public Sprite itemSprite; // 아이템 스프라이트
    public ItemType itemType; // 아이템 타입
    public int itemCount = 0; // 아이템 개수
    public float percentageOfCatch; // 잡을 확률
    public List<ItemSO> materialList = new List<ItemSO>(); //재료 리스트(도구, 요리)
    public ToolType toolType; // 도구 타입(도구)
    public Dictionary<StatType, int> StatEffect = new Dictionary<StatType, int>(); //물고기, 요리
    public FoodType foodType; //요리
    public Dictionary<string, Action> ItemActions = new Dictionary<string, Action>();
}

public enum ItemType
{
    Fish,
    Food,
    Material,
    Tool
}
public enum ToolType
{
    Material,
    FishingRod,
    Radio
}
public enum FoodType
{
    FirstLevelFood,
    SecondLevelFood,
    ThirdLevelFood
}