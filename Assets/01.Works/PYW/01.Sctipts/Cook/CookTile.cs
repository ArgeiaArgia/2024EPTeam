using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookTile : MonoBehaviour
{
    private void Update()
    {
        transform.position += new Vector3(1, 0) * (5 * Time.deltaTime);
    }
}
