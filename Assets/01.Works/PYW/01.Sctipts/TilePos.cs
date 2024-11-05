using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePos : MonoBehaviour
{
    private Transform _parentTrans;
    private void OnEnable()
    {
        _parentTrans = GetComponentInParent<Transform>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            
        }
    }
}
