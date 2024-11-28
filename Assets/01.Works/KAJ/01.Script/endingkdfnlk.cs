using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingkdfnlk : MonoBehaviour
{
    public float fadeDuration = 1.0f; // 서서히 나타나는 시간
    private SpriteRenderer spriteRenderer;

    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer 컴포넌트가 필요합니다!");
            return;
        }

        // 오브젝트를 처음엔 투명하게 설정
        Color initialColor = spriteRenderer.color;
        initialColor.a = 0; // 알파값 0으로 설정
        spriteRenderer.color = initialColor;
    }

    // 애니메이션 이벤트로 호출될 메서드
    public void OnAnimationEnd()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            Color color = spriteRenderer.color;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            spriteRenderer.color = color;
            yield return null;
        }

        // 완전히 나타남
        Color finalColor = spriteRenderer.color;
        finalColor.a = 1;
        spriteRenderer.color = finalColor;
    }
}
