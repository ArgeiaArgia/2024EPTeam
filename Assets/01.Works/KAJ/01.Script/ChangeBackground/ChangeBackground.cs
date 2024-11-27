using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField] private TransitionUI transitionUI;
    [System.Serializable]
    public class Background
    {
        public SpriteRenderer[] spriteRenderers; // �ϳ��� ����� �����ϴ� SpriteRenderer��
        public float fadeDuration = 1f; // ���� ��ȯ �ð�
    }

    public Background[] backgrounds; // ��� �迭
    public float changeInterval = 10f; // ��� ���� ���� (�� ����)

    private int currentBackgroundIndex = 0; // ���� Ȱ��ȭ�� ��� �ε���
    private Coroutine transitionCoroutine;

    void Start()
    {
        InitializeBackgrounds();
        StartCoroutine(ChangeBackgroundRoutine());
        StartCoroutine(WaitForTheEnd());
    }

    private IEnumerator WaitForTheEnd()
    {
        yield return new WaitForSeconds(changeInterval * backgrounds.Length * 30);
        
        PlayerPrefs.SetInt("DiedType", -1);
        transitionUI.EnableUI(() => UnityEngine.SceneManagement.SceneManager.LoadScene(3));
    }

    private void InitializeBackgrounds()
    {
        // ��� ����� �����ϰ� �ʱ�ȭ
        for (int i = 0; i < backgrounds.Length; i++)
        {
            foreach (var spriteRenderer in backgrounds[i].spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = i == 0 ? 1f : 0f; // ù ��° ����� ������, �������� ����
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

            // ���� ��� ���̵� �ƿ�, ���� ��� ���̵� ��
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

            // ���� ��� ���̵� �ƿ�
            foreach (var spriteRenderer in currentBackground.spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = color;
            }

            // ���� ��� ���̵� ��
            foreach (var spriteRenderer in nextBackground.spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                spriteRenderer.color = color;
            }

            yield return null;
        }

        // ������ �� ����
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
