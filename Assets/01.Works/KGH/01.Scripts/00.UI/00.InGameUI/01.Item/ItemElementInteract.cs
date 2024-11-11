using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ItemElementInteract
{
    private ItemElement _itemElement;
    private VisualElement _root;
    
    private bool _isHolding;
    
    public ItemElementInteract(ItemElement itemElement, VisualElement root,InGameUI inGameUI)
    {
        _itemElement = itemElement;
        _itemElement.RegisterCallback<MouseDownEvent>(e=> inGameUI.CoroutineHelper(ItemHolding()));
        _itemElement.RegisterCallback<MouseUpEvent>(e=>_isHolding = false);
        _root = root;
    }
    private IEnumerator ItemHolding()
    {
        _isHolding = true;
        
        var originalParent = _itemElement.parent;
        _root.Add(_itemElement);
        
        while (_isHolding)
        {
            var mousePos = Mouse.current.position.ReadValue();
            _itemElement.style.left = mousePos.x - _itemElement.layout.width / 2;
            _itemElement.style.top = (Screen.height - mousePos.y) - _itemElement.layout.height / 2;
            yield return null;
        }
        
        originalParent.Add(_itemElement);
    }
}
