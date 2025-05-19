using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ThinkingSystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject thinkingPanel;
    [SerializeField] private TextMeshProUGUI thinkingText;

    [Header("Configuration")]
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float defaultTextSpeed = 0.05f; // Time between characters appearing

    private Coroutine currentThinkingCoroutine;
    private bool isThinking = false;

    // Singleton pattern
    public static ThinkingSystem Instance { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize UI elements
        if (thinkingPanel != null)
        {
            thinkingPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("ThinkingSystem: Thinking panel reference not set");
        }
    }

    /// <summary>
    /// Display a thinking text with default settings
    /// </summary>
    /// <param name="text">The text to display as thought</param>
    public void ShowThought(string text)
    {
        ShowThought(text, displayDuration, defaultTextSpeed);
    }

    /// <summary>
    /// Display a thinking text with custom duration
    /// </summary>
    /// <param name="text">The text to display as thought</param>
    /// <param name="duration">How long to display the thought (in seconds)</param>
    public void ShowThought(string text, float duration)
    {
        ShowThought(text, duration, defaultTextSpeed);
    }

    /// <summary>
    /// Display a thinking text with full customization
    /// </summary>
    /// <param name="text">The text to display as thought</param>
    /// <param name="duration">How long to display the thought (in seconds)</param>
    /// <param name="textSpeed">Speed of the text typing animation (in seconds per character)</param>
    public void ShowThought(string text, float duration, float textSpeed)
    {
        // Stop any existing thinking coroutine
        if (currentThinkingCoroutine != null)
        {
            StopCoroutine(currentThinkingCoroutine);
        }

        // Start new thinking sequence
        currentThinkingCoroutine = StartCoroutine(ThinkingSequence(text, duration, textSpeed));
    }

    /// <summary>
    /// Immediately hide any active thinking text
    /// </summary>
    public void HideThought()
    {
        if (currentThinkingCoroutine != null)
        {
            StopCoroutine(currentThinkingCoroutine);
            currentThinkingCoroutine = null;
        }

        thinkingPanel.SetActive(false);
        isThinking = false;
    }

    /// <summary>
    /// Coroutine that handles the entire thinking sequence
    /// </summary>
    private IEnumerator ThinkingSequence(string text, float duration, float textSpeed)
    {
        isThinking = true;

        // Set up the thinking panel
        thinkingPanel.SetActive(true);
        thinkingText.text = "";

        // Fade in
        yield return FadePanel(0f, 1f, fadeInDuration);

        // Type the text
        yield return TypeText(text, textSpeed);

        // Wait for the display duration
        yield return new WaitForSeconds(duration);

        // Fade out
        yield return FadePanel(1f, 0f, fadeOutDuration);

        // Hide the panel
        thinkingPanel.SetActive(false);
        isThinking = false;
        currentThinkingCoroutine = null;
    }

    /// <summary>
    /// Coroutine that handles typing animation
    /// </summary>
    private IEnumerator TypeText(string text, float textSpeed)
    {
        thinkingText.text = "";

        // Type each character one by one
        for (int i = 0; i < text.Length; i++)
        {
            thinkingText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }
    }

    /// <summary>
    /// Coroutine that handles fading the panel
    /// </summary>
    private IEnumerator FadePanel(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color textColor = thinkingText.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            // Update alpha values
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);

            textColor.a = currentAlpha;

            thinkingText.color = textColor;

            yield return null;
        }

        // Ensure we end at the exact target value
        textColor.a = endAlpha;
        thinkingText.color = textColor;
    }

    /// <summary>
    /// Check if the thinking system is currently displaying a thought
    /// </summary>
    public bool IsThinking()
    {
        return isThinking;
    }
}