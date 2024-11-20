using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager instance;
    public int fishCnt = 0;
    public int trashCnt = 0;
    public bool click = false;
    public bool fishEnd = false;
    [Header("Prefabs")]
    public GameObject fishPrefab;
    public GameObject trashPrefab;
    public GameObject normalPrefab;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        fishCnt = 0;
        trashCnt = 0;
    }

    public void EndFish()
    {
        // 끝났을때 실행
        //여기에 결과 식입력
    }
}
