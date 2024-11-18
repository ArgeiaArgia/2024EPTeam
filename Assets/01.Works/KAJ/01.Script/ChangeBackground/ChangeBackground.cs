using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    [System.Serializable]
    public class Background
    {
        public SpriteRenderer[] spriteRenderers; // 하나의 배경을 구성하는 SpriteRenderer들
        public float fadeDuration = 1f; // 투명도 전환 시간
    }

    public Background[] backgrounds; // 배경 배열
    public float changeInterval = 10f; // 배경 변경 간격 (초 단위)

    private int currentBackgroundIndex = 0; // 현재 활성화된 배경 인덱스
    private Coroutine transitionCoroutine;

    void Start()
    {
        InitializeBackgrounds();
        StartCoroutine(ChangeBackgroundRoutine());
    }

    private void InitializeBackgrounds()
    {
        // 모든 배경을 투명하게 초기화
        for (int i = 0; i < backgrounds.Length; i++)
        {
            foreach (var spriteRenderer in backgrounds[i].spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = i == 0 ? 1f : 0f; // 첫 번째 배경은 불투명, 나머지는 투명
                spriteRenderer.color = color;
            }
        }
    }

    private IEnumerator ChangeBackgroundRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);
            int nextIndex = (currentBackgroundIndex + 1) % backgrounds.Length;

            // 현재 배경 페이드 아웃, 다음 배경 페이드 인
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
            }
            transitionCoroutine = StartCoroutine(TransitionBackground(currentBackgroundIndex, nextIndex));

            currentBackgroundIndex = nextIndex;
        }
    }

    private IEnumerator TransitionBackground(int currentIndex, int nextIndex)
    {
        float timer = 0f;
        Background currentBackground = backgrounds[currentIndex];
        Background nextBackground = backgrounds[nextIndex];

        while (timer < currentBackground.fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / currentBackground.fadeDuration;

            // 현재 배경 페이드 아웃
            foreach (var spriteRenderer in currentBackground.spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = color;
            }

            // 다음 배경 페이드 인
            foreach (var spriteRenderer in nextBackground.spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                spriteRenderer.color = color;
            }

            yield return null;
        }

        // 마지막 값 보정
        foreach (var spriteRenderer in currentBackground.spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }

        foreach (var spriteRenderer in nextBackground.spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }
}
