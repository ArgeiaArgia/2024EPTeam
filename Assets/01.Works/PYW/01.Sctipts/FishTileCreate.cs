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
        if(cal == 1)//생성 됬을때 1/3확률로 fish 타일 생성 
            Instantiate(_fishTilePrefab,transform);
        else
            Instantiate(_trashTilePrefab,transform);
    }
}
