using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    public Image blackScreenImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        // Singleton pattern (optional but helpful)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Ensure it's fully transparent at start
        SetAlpha(0);
        blackScreenImage.gameObject.SetActive(true);
    }

    private void SetAlpha(float alpha)
    {
        Color c = blackScreenImage.color;
        c.a = alpha;
        blackScreenImage.color = c;
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0, 1));
    }
    public void FadeIn(float customDuration = -1f)
    {
        float duration = customDuration > 0 ? customDuration : fadeDuration;

        // If duration is less than 1, instantly set alpha to 1 without animation
        if (duration < 1)
        {
            SetAlpha(1);
        }
        else
        {
            StartCoroutine(Fade(0, 1, duration));
        }
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1, 0));
    }
    public void FadeOut(float customDuration = -1f)
    {
        float duration = customDuration > 0 ? customDuration : fadeDuration;

        // If duration is less than 1, instantly set alpha to 0 without animation
        if (duration < 1)
        {
            SetAlpha(0);
        }
        else
        {
            StartCoroutine(Fade(1, 0, duration));
        }
    }

    public void FadeInThenOut(float holdTime = 0f)
    {
        StartCoroutine(FadeSequence(holdTime));
    }
    public void FadeInThenOut(float holdTime = 0f, float customFadeInDuration = -1f, float customFadeOutDuration = -1f)
    {
        float inDuration = customFadeInDuration > 0 ? customFadeInDuration : fadeDuration;
        float outDuration = customFadeOutDuration > 0 ? customFadeOutDuration : fadeDuration;

        // Handle instant transitions specially
        if (inDuration == 0 && outDuration == 0)
        {
            SetAlpha(1); // Instantly go to black
            StartCoroutine(InstantFadeSequence(holdTime));
        }
        else
        {
            StartCoroutine(FadeSequence(holdTime, inDuration, outDuration));
        }
    }

    private IEnumerator InstantFadeSequence(float holdTime)
    {
        // For when both fade durations are 0
        yield return new WaitForSeconds(holdTime);
        SetAlpha(0);
    }
    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(to);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(to);
    }

    private IEnumerator FadeSequence(float holdTime)
    {
        yield return Fade(0, 1);
        yield return new WaitForSeconds(holdTime);
        yield return Fade(1, 0);
    }
    private IEnumerator FadeSequence(float holdTime, float fadeInDuration, float fadeOutDuration)
    {
        // Handle instant fade in
        if (fadeInDuration == 0)
        {
            SetAlpha(1);
        }
        else
        {
            yield return Fade(0, 1, fadeInDuration);
        }

        yield return new WaitForSeconds(holdTime);

        // Handle instant fade out
        if (fadeOutDuration == 0)
        {
            SetAlpha(0);
        }
        else
        {
            yield return Fade(1, 0, fadeOutDuration);
        }
    }
}
