using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipMoving : MonoBehaviour
{
    [SerializeField] private float shipSpeed;
    [SerializeField] private float center;

    private void Update()
    {
        var sinValue = center * (1 - Mathf.Sin(Time.time * shipSpeed));
        transform.localPosition = new Vector3(transform.localPosition.x, sinValue, transform.localPosition.z);
    }
}