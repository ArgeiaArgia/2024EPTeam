using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 1. �������̽�: ��� �������� �����ؾ� �ϴ� �⺻ ����
public interface IItem
{
    string Name { get; }
    void Use();
}

// ������ ����: ü�� ����
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
        Debug.Log($"{Name}�� ����Ͽ� ü���� {HealAmount} ȸ���߽��ϴ�!");
    }
}

// ������ ����: ���ݷ� ���� ����
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
        Debug.Log($"{Name}�� ����Ͽ� ���ݷ��� {AttackBoost} �����߽��ϴ�!");
    }

}


// 2. ���ʸ�: �پ��� Ÿ���� �������� �����ϴ� �κ��丮
public class Inventory<T> where T : IItem
{
    private List<T> items = new List<T>();

    public void AddItem(T item)
    {
        items.Add(item);
        Debug.Log($"{item.Name}��(��) �κ��丮�� �߰��Ǿ����ϴ�.");
    }

    public void UseItem(string itemName)
    {
    }

}
