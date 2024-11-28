using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    public float targetY = -2f; // ��ǥ Y ��ǥ
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float smoothTime = 0.3f; // ������ �̵� �ð� (0.1~0.5 ���� ��õ)

    private float velocity = 0f;
    public void OnCredit()
    {
        StartCoroutine(MoveUpToTargetY());
        ;
    }
    private IEnumerator MoveUpToTargetY()
    {
        Vector3 position = transform.position; // ���� ��ġ
        while (position.y < targetY)
        {
            // Y���� �ε巴�� ��ǥ �������� ����
            position.y = Mathf.SmoothDamp(position.y, targetY, ref velocity, smoothTime);
            transform.position = position;

            yield return null; // �����Ӹ��� ����
        }

        // ��ǥ �������� ��Ȯ�� ����
        position.y = targetY;
        transform.position = position;

        Debug.Log("���� �Ϸ�!");
    }
}
