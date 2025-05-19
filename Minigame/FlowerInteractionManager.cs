using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages all flower interactions centrally
public class FlowerInteractionManager : MonoBehaviour
{
    public static FlowerInteractionManager Instance { get; private set; }

    [Header("Flower Messages")]
    [Tooltip("List of messages that will be shown sequentially when flowers are placed")]
    public List<string> flowerDialogTexts = new List<string>() {
        "Help me!",
        "I'm trapped!",
        "Save us!",
        "They're using us...",
        "Don't trust them..."
    };

    [Header("Flower Sounds")]
    [Tooltip("List of audio clips that will play sequentially when flowers are placed")]
    public List<AudioClip> flowerSoundClips = new List<AudioClip>();

    // Shared counter for all flower interactions
    private int currentInteractionIndex = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Gets the current dialog text and increments the counter
    public string GetNextDialogText()
    {
        if (flowerDialogTexts.Count == 0)
            return "...";

        string text = flowerDialogTexts[currentInteractionIndex % flowerDialogTexts.Count];
        return text;
    }

    // Gets the current sound clip
    public AudioClip GetCurrentSoundClip()
    {
        if (flowerSoundClips.Count == 0)
            return null;

        return flowerSoundClips[currentInteractionIndex % flowerSoundClips.Count];
    }

    // Advances to the next interaction
    public void AdvanceInteraction()
    {
        currentInteractionIndex++;
    }

    // Resets the interaction counter
    public void ResetInteractions()
    {
        currentInteractionIndex = 0;
    }
}