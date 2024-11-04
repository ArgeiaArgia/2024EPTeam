using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemElement : VisualElement
{
    
    public ItemElement()
    {
        var itemIcon = new Image();
        itemIcon.AddToClassList("ItemIcon");
        Add(itemIcon);
    }
}
