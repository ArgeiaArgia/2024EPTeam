using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory
{
    
    public Inventory(VisualElement root)
    {
        var itemTab = root.Q<TabElement>("ItemTab");
        var craftTab = root.Q<TabElement>("CraftTab");
    }
}
