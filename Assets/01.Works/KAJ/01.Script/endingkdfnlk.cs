using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingkdfnlk : MonoBehaviour
{
    public float fadeDuration = 1.0f; // ������ ��Ÿ���� �ð�
    private SpriteRenderer spriteRenderer;

    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer ������Ʈ�� �ʿ��մϴ�!");
            return;
        }

        // ������Ʈ�� ó���� �����ϰ� ����
        Color initialColor = spriteRenderer.color;
        initialColor.a = 0; // ���İ� 0���� ����
        spriteRenderer.color = initialColor;
    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
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

        // ������ ��Ÿ��
        Color finalColor = spriteRenderer.color;
        finalColor.a = 1;
        spriteRenderer.color = finalColor;
    }
}
