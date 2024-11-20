using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == FishManager.instance.fishPrefab)
            FishManager.instance.fishCnt++;
        if (collision.gameObject == FishManager.instance.trashPrefab)
            FishManager.instance.trashCnt++;
    }
}
