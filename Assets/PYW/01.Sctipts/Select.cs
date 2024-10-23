using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [SerializeField] private int _tryMaxCount = 5;
    private int _tryCount;
    [SerializeField]private float _moveDistance = 7.5f;
    [SerializeField]private float _speed = 2f;
    private bool _click = false;
    private Vector3 _startPosition;
    private float _time;
    public int _fishDefault = 1;
    public int _trashDefault = 5;
    public GameObject fishTilePrefab;
    public GameObject trashTilePrefab;
    private void Start()
    {
        //시작 위치 저장
        _startPosition = transform.position;
        _tryCount = _tryMaxCount;
        StartCoroutine(InputClick());
        print(_fishDefault);
        print(_trashDefault);
    }

    private void Update()
    {
        if (!_click)
        {
            if (_tryCount <= 0) FishEnd(); //시도 횟수가 0이면 함수 실행
            else Move(); //이동
            _time += Time.deltaTime; //이동하는 시간
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //멈췄을때 감지해주기
        if(_click)
        {
            if (collision.gameObject == fishTilePrefab)
                _fishDefault++;
            if (collision.gameObject == trashTilePrefab)
                _trashDefault++;
        }
    }
    private void Move()
    {
        //시간에 따라 위치를 반복적으로 변경
        float pingPong = Mathf.PingPong(_time * _speed, _moveDistance);
        transform.position = new Vector3(_startPosition.x + pingPong, _startPosition.y, _startPosition.z);
    }

    IEnumerator InputClick()
    {
        while (_tryCount > 0) //남은 시도 횟수가 있을 때만 클릭 감지
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !_click) 
            {
                _click = true;
                _tryCount--; 
                yield return new WaitForSeconds(2); // 2초 대기
                _click = false;
            }
            yield return null; // 프레임마다 대기
        }
    }
    private void FishEnd()//낚시 끝날을때 실행 
    {
        
    }
}