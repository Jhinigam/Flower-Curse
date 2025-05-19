using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionController;

public class Toothbrush : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.KeyPress;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BedScript bedScript;

    // This will store whether the toothbrush is available for the current day
    private bool isAvailableToday = false;
    private bool hasBeenUsed = false;

    private void Start()
    {
        // Find references if not assigned in inspector
        if (levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();

        if (bedScript == null)
            bedScript = FindObjectOfType<BedScript>();

        // Reset usage status at the start of each day
        // This would typically be called from elsewhere, like when changing days
        ResetForNewDay();
    }

    private void Update()
    {
        // Check if the day's levels are completed to determine availability
        CheckAvailability();
    }

    private void CheckAvailability()
    {
        if (bedScript == null || levelManager == null)
            return;

        int currentDay = bedScript.GetCurrentDay();
        int currentLevel = levelManager.currentLevel;

        // Using the required level fields from BedScript to determine when toothbrush becomes available
        switch (currentDay)
        {
            case 1:
                isAvailableToday = currentLevel >= bedScript.requiredLevelForDay1;
                break;
            case 2:
                isAvailableToday = currentLevel >= bedScript.requiredLevelForDay2;
                break;
            case 3:
                isAvailableToday = currentLevel >= bedScript.requiredLevelForDay3;
                break;
            case 4:
                isAvailableToday = currentLevel >= bedScript.requiredLevelForDay4;
                break;
            case 5:
                isAvailableToday = currentLevel >= bedScript.requiredLevelForDay5;
                break;
            default:
                isAvailableToday = true; // For days beyond those specified
                break;
        }
    }

    public void Interact()
    {
        // Only allow using the toothbrush if it's available and hasn't been used
        if (isAvailableToday && !hasBeenUsed)
        {
            // Mark as used
            hasBeenUsed = true;

            // Use the screen fader for the tooth brushing animation
            if (ScreenFader.Instance != null)
            {
                ScreenFader.Instance.FadeInThenOut(2f);
            }

            // Optionally, show a thought or message
            if (ThinkingSystem.Instance != null)
            {
                ThinkingSystem.Instance.ShowThought("My teeth feel clean now.", 3f);
            }
        }
    }

    public string GetInteractMessage()
    {
        if (!isAvailableToday)
            return "Complete today's levels first";
        else if (hasBeenUsed)
            return ""; // No message if already used
        else
            return "Press E to brush your teeth";
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }

    // Public method to check if the toothbrush has been used
    public bool HasBeenUsed()
    {
        return hasBeenUsed;
    }

    // Reset the toothbrush for a new day
    public void ResetForNewDay()
    {
        hasBeenUsed = false;
        CheckAvailability();
    }

    // Public method to set availability (could be used by other scripts)
    public void SetAvailability(bool available)
    {
        isAvailableToday = available;
    }
}