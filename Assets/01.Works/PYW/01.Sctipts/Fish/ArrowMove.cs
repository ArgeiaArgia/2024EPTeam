using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowMove : MonoBehaviour
{
    private int fishCnt = 5;
    [SerializeField]private GameObject collider;
    private bool clicked = false;
    private Coroutine moveCoroutine;
    private float timer = 0f;
    private void Start()
    {
        moveCoroutine = StartCoroutine(Move());
    }

    private void Update()
    {
        if (FishManager.instance.click)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        FishManager.instance.click = false;
        StopCoroutine(moveCoroutine); 
        collider.SetActive(true);
        yield return new WaitForSeconds(1);
        fishCnt--;
        collider.SetActive(false);
        moveCoroutine = StartCoroutine(Move()); 
    }

    IEnumerator Move()
    {
        while (true)
        {
            timer += Time.deltaTime; 
            float x = Mathf.PingPong(timer * 2, 13) - 6.5f;
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            yield return null;
        }
    }
}
