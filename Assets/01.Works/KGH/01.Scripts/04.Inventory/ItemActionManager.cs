using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemActionManager : MonoBehaviour
{
    [SerializeField] private ItemSO water;
    [SerializeField] private ScriptSO _scriptSO;
    [SerializeField] private Player _player;
    [SerializeField] private SystemMessage _systemMessage;
    private StatManager _statManager;
    private InventoryManager _inventoryManager;

    private int _radioChangePerSecond = 0;

    private void Awake()
    {
        _statManager = GetComponent<StatManager>();
        _inventoryManager = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (_radioChangePerSecond > 0)
        {
            
            _statManager.StatValues[StatType.Bored] = Mathf.Clamp(_statManager.StatValues[StatType.Bored] + _radioChangePerSecond, 0, 100);
            _statManager.OnStatChanged?.Invoke(StatType.Bored, _statManager.StatValues[StatType.Bored]);
        }
    }

    public void EatItem(ItemSO item)
    {
        foreach (var effect in item.StatEffect)
        {
            if (effect.Key == StatType.Health)
            {
                var randomValue = Random.Range(0, 10);
                if (randomValue < 5)
                {
                    _statManager.StatValues[effect.Key] = Mathf.Clamp(_statManager.StatValues[effect.Key]+  effect.Value, 0, 100);
                    _statManager.OnStatChanged?.Invoke(effect.Key, _statManager.StatValues[effect.Key]);
                    continue;
                }
            }
            _statManager.StatValues[effect.Key] = Mathf.Clamp(_statManager.StatValues[effect.Key] + effect.Value, 0, 100);
            _statManager.OnStatChanged?.Invoke(effect.Key, _statManager.StatValues[effect.Key]);
        }

        _inventoryManager.RemoveItem(item);
        _systemMessage.ShowMessage($"{item.itemName}을/를 섭취하였습니다.");
    }

    public void ListenToRadio(int changePerSecond)
    {
        if (_radioChangePerSecond > 0)
        {
            _radioChangePerSecond = 0;
            _systemMessage.ShowMessages((string[])null);
        }
        else
        {
            _radioChangePerSecond = changePerSecond;
            _systemMessage.ShowMessages(_scriptSO.GetRandomRadio());
        }
    }
    public void ChangeFishRod(int change)
    {
        _player.CatchPercentage = change;
    }
    public void ExploreItem(ItemSO item)
    {
        _systemMessage.ShowMessage(_scriptSO._itemExplore[item]);
    }

    public void CleanWater()
    {
        if (!_inventoryManager.TryCraftItemBool(water))
        {
            _systemMessage.ShowMessage("병이 필요합니다.");
        }
    }
}