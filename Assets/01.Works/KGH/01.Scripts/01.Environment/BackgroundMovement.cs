using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    private SpriteRenderer _spriteRenderer;

    private float _xOffset;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MoveBackground();
    }

    void MoveBackground()
    {
        _xOffset = (_xOffset + Time.deltaTime * speed) % 2;
        _spriteRenderer.material.SetTextureOffset("_MainTex", new Vector2(_xOffset, 0));
    }
}