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
    public float percentageOfCatch; // 잡을 확률
    public float weight; // 무게
    public ToolType toolType; // 도구 타입(도구)
    public float holdableWeight; // 슬롯 개수(도구)

    public Dictionary<ItemSO, int> materialList
    {
        get
        {
            var returnDic = new Dictionary<ItemSO, int>();
            for (var i = 0; i < materialKey.Count; i++)
            {
                returnDic.Add(materialKey[i], (int)materialValue[i]);
            }

            return returnDic;
        }
        set
        {
            if (value == null) return;
            materialKey.Clear();
            materialValue.Clear();

            foreach (var materialList in value)
            {
                Debug.Log("materialList.Key : " + materialList.Key);
                if (materialList.Key == null) return;
                materialKey.Add(materialList.Key);
                materialValue.Add(materialList.Value);
            }

        }
    }

    public List<ItemSO> materialKey = new List<ItemSO>();
    public List<int> materialValue = new List<int>();

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
            if (value == null) return;
            StatEffectKey.Clear();
            StatEffectValue.Clear();

            foreach (var statEffect in value)
            {
                Debug.Log("statEffect.Key : " + statEffect.Key);
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