using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;
using UnityEngine;

public class DetectLine : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            if (Input.GetKeyDown(CookManager.instance.key))
            {
                CookManager.instance.Success++;
            }
        }
    }
    
}
