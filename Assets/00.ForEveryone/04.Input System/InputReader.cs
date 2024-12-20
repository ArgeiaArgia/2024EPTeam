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

    public Action<Vector2> OnMouseInteractEvent;

    public Action OnEscapeEvent;

    public Action OnTriggerEvent;

    public Action<Vector2Int> OnNotesEvent;

    private void OnEnable()
    {
        _inputController ??= new InputController();

        _inputController.PlayerAction.SetCallbacks(this);
        _inputController.PlayerAction.Enable();
    }

    private void OnDisable()
    {
        _inputController.PlayerAction.Disable();
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        var value = Mouse.current.position.ReadValue();
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

    public void OnMouseInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var value = Mouse.current.position.ReadValue();
            OnMouseInteractEvent?.Invoke(value);
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnEscapeEvent?.Invoke();
        }
    }

    public void OnFish(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnTriggerEvent?.Invoke();
        }
    }

    public void OnNotes(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        var value = context.ReadValue<Vector2>();
        if (value == Vector2.zero) return;
            
        if (Mathf.Approximately(value.x, value.y))
        {
            value = new Vector2(value.x, 0);
        }

        OnNotesEvent?.Invoke(new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y)));
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Application.Quit();
        }
    }
}