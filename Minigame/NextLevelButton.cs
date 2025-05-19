using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionController;

public class NextLevelButton : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.MouseClick;

    // Reference to the button's renderer to toggle visibility
    private Renderer buttonRenderer;
    private Collider buttonCollider;

    private void Start()
    {
        // Get references to the components
        buttonRenderer = GetComponent<Renderer>();
        buttonCollider = GetComponent<Collider>();

        // Initially hide the button
        HideButton();
    }

    public void ShowButton()
    {
        // Make button visible and interactable
        if (buttonRenderer != null)
            buttonRenderer.enabled = true;
        if (buttonCollider != null)
            buttonCollider.enabled = true;
    }

    public void HideButton()
    {

        if (LevelManager.Instance.currentLevel == 18) return;
        // Hide button and disable interaction
        if (buttonRenderer != null)
            buttonRenderer.enabled = false;
        if (buttonCollider != null)
            buttonCollider.enabled = false;
    }

    public void Interact()
    {
        // When button is clicked, load the next level
        LevelManager.Instance.LoadNextLevel();
    }

    public string GetInteractMessage()
    {
        // Return empty string as specified
        return "";
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }
}