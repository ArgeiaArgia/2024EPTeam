using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class NewBehaviourScript : MonoBehaviour
{
    
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ItemSO item;
    
    [Button ("AddItem")]
    private void AddItem()
    {
        inventoryManager.AddItem(item, 1, "갑판");
    }
}
