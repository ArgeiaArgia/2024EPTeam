using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 1. 인터페이스: 모든 아이템이 구현해야 하는 기본 동작
public interface IItem
{
    string Name { get; }
    void Use();
}

// 아이템 예시: 체력 포션
public class HealthPotion : IItem
{
    public string Name { get; private set; }
    public int HealAmount { get; private set; }

    public HealthPotion(string name, int healAmount)
    {
        Name = name;
        HealAmount = healAmount;
    }

    public void Use()
    {
        Debug.Log($"{Name}을 사용하여 체력을 {HealAmount} 회복했습니다!");
    }
}

// 아이템 예시: 공격력 증가 포션
public class AttackPotion : IItem
{
    public string Name { get; private set; }
    public int AttackBoost { get; private set; }

    public AttackPotion(string name, int attackBoost)
    {
        Name = name;
        AttackBoost = attackBoost;
    }

    public void Use()
    {
        Debug.Log($"{Name}을 사용하여 공격력이 {AttackBoost} 증가했습니다!");
    }

}


// 2. 제너릭: 다양한 타입의 아이템을 저장하는 인벤토리
public class Inventory<T> where T : IItem
{
    private List<T> items = new List<T>();

    public void AddItem(T item)
    {
        items.Add(item);
        Debug.Log($"{item.Name}이(가) 인벤토리에 추가되었습니다.");
    }

    public void UseItem(string itemName)
    {
    }

}
