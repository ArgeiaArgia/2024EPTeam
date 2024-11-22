using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class CreateCook : MonoBehaviour
{
    [SerializeField]private GameObject cookTile;
    [SerializeField] private int repeatCnt = 5;

    private void Start()
    {
        StartCoroutine(Cook());
    }

    IEnumerator Cook()
    {
        for (int i = 0; i < repeatCnt; i++)
        {
            Instantiate(cookTile, transform.position, Quaternion.identity);
            float wait = Random.Range(1.6f, 2.3f);
            yield return new WaitForSeconds(wait);
        }
    }
}
