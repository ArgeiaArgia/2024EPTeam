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
        //���� ��ġ ����
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
            if (_tryCount <= 0) FishEnd(); //�õ� Ƚ���� 0�̸� �Լ� ����
            else Move(); //�̵�
            _time += Time.deltaTime; //�̵��ϴ� �ð�
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������� �������ֱ�
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
        //�ð��� ���� ��ġ�� �ݺ������� ����
        float pingPong = Mathf.PingPong(_time * _speed, _moveDistance);
        transform.position = new Vector3(_startPosition.x + pingPong, _startPosition.y, _startPosition.z);
    }

    IEnumerator InputClick()
    {
        while (_tryCount > 0) //���� �õ� Ƚ���� ���� ���� Ŭ�� ����
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !_click) 
            {
                _click = true;
                _tryCount--; 
                yield return new WaitForSeconds(2); // 2�� ���
                _click = false;
            }
            yield return null; // �����Ӹ��� ���
        }
    }
    private void FishEnd()//���� �������� ���� 
    {
        
    }
}