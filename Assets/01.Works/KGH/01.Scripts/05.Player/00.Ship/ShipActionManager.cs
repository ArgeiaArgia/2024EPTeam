using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipActionManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    public UnityEvent<List<InteractEvent>, Vector2> onInteractEvent;
    
    [Header("Ship Rects")]
    [SerializeField] private Rect shipFront;
    [SerializeField] private Rect shipMiddle;
    [SerializeField] private Rect shipBack;
    [SerializeField] private Rect shipInside;

    [Header("Interact Events")]
    [SerializeField] private List<InteractEvent> frontInteracts = new List<InteractEvent>();
    [SerializeField] private List<InteractEvent> middleInteracts = new List<InteractEvent>();
    [SerializeField] private List<InteractEvent> backInteracts = new List<InteractEvent>();
    [SerializeField] private List<InteractEvent> insideInteracts = new List<InteractEvent>();

    private Camera _mainCamera;
    private void Awake()
    {
        inputReader.OnMouseInteractEvent += HandleInteract;
        _mainCamera = Camera.main;
    }

    private void HandleInteract(Vector2 pos)
    {
        var worldPos = _mainCamera.ScreenToWorldPoint(pos);
        if (shipFront.Contains(worldPos))
        {
            foreach (var interact in frontInteracts)
            {
                onInteractEvent?.Invoke(frontInteracts, pos);
            }
        }
        else if (shipMiddle.Contains(worldPos))
        {
            foreach (var interact in middleInteracts)
            {
                onInteractEvent?.Invoke(middleInteracts, pos);
            }
        }
        else if (shipBack.Contains(worldPos))
        {
            foreach (var interact in backInteracts)
            {
                onInteractEvent?.Invoke(backInteracts, pos);
            }
        }
        else if (shipInside.Contains(worldPos))
        {
            foreach (var interact in insideInteracts)
            {
                onInteractEvent?.Invoke(insideInteracts, pos);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(shipFront.center + (Vector2)transform.position, shipFront.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(shipMiddle.center + (Vector2)transform.position, shipMiddle.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(shipBack.center + (Vector2)transform.position, shipBack.size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(shipInside.center + (Vector2)transform.position, shipInside.size);
    }
#endif
}
