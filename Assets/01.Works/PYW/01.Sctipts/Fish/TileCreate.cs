using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileCreate : MonoBehaviour
{
    [Header("TileCnt")]
    [SerializeField] private int fishTileCnt = 2;
    [SerializeField] private int trashTileCnt = 3;
    [SerializeField] private int normalTileCnt = 8;

    private void Start()
    {
        CreateTile();
    }

    private void CreateTile()
    {
        int normalCnt = normalTileCnt;
        int trashCnt = trashTileCnt;
        int fishCnt = fishTileCnt;
        while(true)
        {
            int tileType = Random.Range(0, 3);
            switch (tileType)
            {
                case 0:
                    if (trashCnt > 0)
                    {
                        Instantiate(FishManager.instance.trashPrefab, transform);
                        trashCnt--;
                    }
                    break;
                case 1:
                    if (fishCnt > 0)
                    {
                        Instantiate(FishManager.instance.fishPrefab, transform);
                        fishCnt--;
                    }
                    break;
                case 2:
                    if (normalTileCnt > 0)
                    {
                        Instantiate(FishManager.instance.normalPrefab, transform);
                        normalCnt--;
                    }
                    break;
            }
            if (trashCnt <= 0 && normalCnt <= 0 && fishCnt <= 0)
            {
                break;
            }
        }
    }
}
