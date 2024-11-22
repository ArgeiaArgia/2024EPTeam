using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TilePassCheck : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public UnityEvent keyShow; 
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        keyShow?.Invoke();
    }
}
