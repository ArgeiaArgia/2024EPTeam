using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ItemSO : ScriptableObject
{
    public string itemName; // 아이템 이름
    public Sprite itemIcon; // 아이템 아이콘
    public Sprite itemSprite; // 아이템 스프라이트
    public ItemType itemType; // 아이템 타입
    public int itemCount = 0; // 아이템 개수
    public float percentageOfCatch; // 잡을 확률
    public List<ItemSO> materialList = new List<ItemSO>(); //재료 리스트(도구, 요리)
    public ToolType toolType; // 도구 타입(도구)
    public int slotCount; // 슬롯 개수(도구)
    public Dictionary<StatType, int> StatEffect
    {
        get
        {
            var returnDic = new Dictionary<StatType, int>();
            for (var i = 0; i < StatEffectKey.Count; i++)
            {
                returnDic.Add(StatEffectKey[i], (int)StatEffectValue[i]);
            }

            return returnDic;
        }
        set
        {
            StatEffectKey.Clear();
            StatEffectValue.Clear();
            
            foreach (var statEffect in value)
            {
                StatEffectKey.Add(statEffect.Key);
                StatEffectValue.Add(statEffect.Value);
            }
        }
    } //물고기, 요리
    public List<StatType> StatEffectKey = new List<StatType>(); //물고기, 요리
    public List<int> StatEffectValue = new List<int>(); //물고기, 요리
    public FoodType foodType; //요리

    public string description; // 설명
}

public enum ItemType
{
    Ingredient,
    Food,
    Trash,
    Tool
}
public enum ToolType
{
    Material,
    FishingRod,
    Radio,
    Inventory
}
public enum FoodType
{
    FirstLevelFood,
    SecondLevelFood,
    ThirdLevelFood
}