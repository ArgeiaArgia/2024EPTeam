using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputController;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, IPlayerActionActions
{
    private InputController _inputController;
    public Action<Vector2> OnMoveDownEvent;
    public Action<Vector2> OnMovePressEvent;
    public Action<Vector2> OnMoveUpEvent;
    private void OnEnable()
    {
        if (_inputController == null)
        {
            _inputController = new InputController();
            _inputController.PlayerAction.SetCallbacks(this);
        }
        _inputController.PlayerAction.Enable();
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if (context.performed)
        {
            OnMovePressEvent?.Invoke(value);
        }
        else if (context.canceled)
        {
            OnMoveUpEvent?.Invoke(value);
        }
        else
        {
            OnMoveDownEvent?.Invoke(value);
        }
    }
}
