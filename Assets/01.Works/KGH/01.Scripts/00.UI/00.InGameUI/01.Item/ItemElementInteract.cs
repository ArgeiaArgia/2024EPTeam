using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ItemElementInteract
{
    private ItemElement _itemElement;
    private VisualElement _root;
    private VisualElement _originalParent;
    
    private bool _isHolding;
    
    public ItemElementInteract(ItemElement itemElement, VisualElement root,InGameUI inGameUI)
    {
        _itemElement = itemElement;
        _itemElement.RegisterCallback<MouseDownEvent>(e=> inGameUI.CoroutineHelper(ItemHolding()));
        _itemElement.RegisterCallback<MouseUpEvent>(e=>_isHolding = false);
        _originalParent = _itemElement.parent;
        _root = root;
    }
    private IEnumerator ItemHolding()
    {
        _isHolding = true;
        
        _root.Add(_itemElement);
        _itemElement.style.position = Position.Absolute;
        
        while (_isHolding)
        {
            var mousePos = Mouse.current.position.ReadValue();
            var localMousePos = _root.WorldToLocal(new Vector2(mousePos.x, Screen.height - mousePos.y));
            _itemElement.style.left = localMousePos.x - _itemElement.layout.width / 2;
            _itemElement.style.top = localMousePos.y - _itemElement.layout.height / 2;
            yield return null;
        }
        var itemUpper = _originalParent.Children().ToList()[0];
        foreach(var item in _originalParent.Children())
        {
            if (item.layout.position.y <= _originalParent.WorldToLocal(_itemElement.layout.position).y)
            {
                itemUpper = item;
            }
        }
        
        _originalParent.Add(_itemElement);
        
        _itemElement.style.position = Position.Relative;
        _itemElement.style.left = 0;
        _itemElement.style.top = 0;
        
        _itemElement.PlaceInFront(itemUpper);
    }
}
