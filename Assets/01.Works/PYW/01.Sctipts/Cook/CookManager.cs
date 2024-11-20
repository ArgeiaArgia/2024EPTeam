using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    public static CookManager instance;
    public int Success;
    public string key;

    private void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
}
