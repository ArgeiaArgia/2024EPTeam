using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTileCreate : MonoBehaviour
{
    public GameObject _fishTilePrefab;
    public GameObject _trashTilePrefab;
    public void Create()
    {
        int cal = Random.Range(1, 4);
        if(cal == 1)//积己 夌阑锭 1/3犬伏肺 fish 鸥老 积己 
            Instantiate(_fishTilePrefab,transform);
        else
            Instantiate(_trashTilePrefab,transform);
    }
}
