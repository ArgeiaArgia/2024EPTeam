using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    public float targetY = -2f; // 목표 Y 좌표
    public float moveSpeed = 2f; // 이동 속도
    public float smoothTime = 0.3f; // 스무스 이동 시간 (0.1~0.5 정도 추천)

    private float velocity = 0f;
    public void OnCredit()
    {
        StartCoroutine(MoveUpToTargetY());
        ;
    }
    private IEnumerator MoveUpToTargetY()
    {
        Vector3 position = transform.position; // 현재 위치
        while (position.y < targetY)
        {
            // Y값을 부드럽게 목표 지점으로 변경
            position.y = Mathf.SmoothDamp(position.y, targetY, ref velocity, smoothTime);
            transform.position = position;

            yield return null; // 프레임마다 실행
        }

        // 목표 지점에서 정확히 멈춤
        position.y = targetY;
        transform.position = position;

        Debug.Log("도착 완료!");
    }
}
