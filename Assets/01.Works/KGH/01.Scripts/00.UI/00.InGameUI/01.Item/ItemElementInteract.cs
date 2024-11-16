using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ItemElementInteract
{
    private readonly ItemElement _itemElement;
    private readonly InventoryItem _item;

    private readonly VisualElement _root;
    private readonly VisualElement _originalParent;
    private readonly Label _itemNameLabel;

    private readonly ItemTab _itemTab;
    private readonly InventoryManager _inventoryManager;

    private bool _isHolding;

    public ItemElementInteract(ItemElement itemElement, InventoryItem item, VisualElement root, InGameUI inGameUI,
        ItemTab
            itemTab, InventoryManager inventoryManager)
    {
        _itemElement = itemElement;
        _item = item;

        _itemElement.RegisterCallback<MouseDownEvent>(e =>
        {
            if (e.button == 1)
            {
                itemTab.ShowInteractions(item.item);
            }
            else
            {
                inGameUI.CoroutineHelper(ItemHolding());
            }
        });
        _itemElement.RegisterCallback<MouseUpEvent>(e => _isHolding = false);


        _originalParent = _itemElement.parent;
        _root = root;
        _itemTab = itemTab;
        _inventoryManager = inventoryManager;

        _itemNameLabel = _itemElement.Q<Label>("NameLabel");
        _itemNameLabel.RegisterCallback<MouseOverEvent>(e => _itemTab.ItemHover(item));
        _itemNameLabel.RegisterCallback<MouseOutEvent>(e => _itemTab.ItemHover(null));
    }

    private IEnumerator ItemHolding()
    {
        _isHolding = true;
        _itemNameLabel.pickingMode = PickingMode.Ignore;

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

        VisualElement itemUpper = null;
        VisualElement itemOverlapped = null;
        foreach (var item in _originalParent.Children())
        {
            var itemLayout = item.layout.position;
            var itemElementLayout = _originalParent.WorldToLocal(_itemElement.layout.position);
            if (itemLayout.y <= itemElementLayout.y)
            {
                itemUpper = item;
            }

            if (Vector2.Distance(itemLayout, itemElementLayout) < item.layout.height && _itemTab
                    .CheckItemIsInventory(item))
            {
                itemOverlapped = item;
            }
        }

        _originalParent.Add(_itemElement);

        _itemElement.style.position = Position.Relative;
        _itemElement.style.left = 0;
        _itemElement.style.top = 0;

        if (itemOverlapped == null)
        {
            _itemNameLabel.pickingMode = PickingMode.Position;
            if (itemUpper == null) _itemElement.SendToBack();
            else _itemElement.PlaceInFront(itemUpper);
        }
        else
        {
            var itemOverlappedName = _itemTab.GetItemName(itemOverlapped);
            _inventoryManager.MoveItem(_item, itemOverlappedName);
        }
    }
    public void ChangePickingMode(PickingMode pickingMode)
    {
        _itemElement.pickingMode = pickingMode;
        _itemNameLabel.pickingMode = pickingMode;
    }
}