using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerInteractionController;

public class MinigameIcon : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private string iconName = "Minigame";
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.MouseClick;

    // Event for first time opening
    public UnityEvent onOpenedFirstTime;

    private bool isFirstOpen = true;
    private LevelManager levelManager;
    private bool isMinigameActive = false;

    private void Start()
    {
        // Get reference to the LevelManager singleton
        levelManager = LevelManager.Instance;

        if (levelManager == null)
        {
            Debug.LogError("LevelManager instance not found!");
        }
    }

    public void Interact()
    {
        // Call first-time event if this is the first opening
        if (isFirstOpen)
        {
            onOpenedFirstTime?.Invoke();
            isFirstOpen = false;
        }

        // Toggle minigame active state
        isMinigameActive = !isMinigameActive;

        // Get the current level from LevelManager and toggle its active state
        if (levelManager != null && levelManager.GetCurrentLevel() != null)
        {
            levelManager.GetCurrentLevel().SetActive(isMinigameActive);
        }
    }

    public string GetInteractMessage()
    {
        return isMinigameActive ? "Click to close " + iconName : "Click to launch " + iconName;
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }
}