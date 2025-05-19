using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PlayerInteractionController;

public class FillableArea : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private int correctSlot;
    [SerializeField] private GameObject realArea;
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.MouseClick;
    private Playaudio audioPlayer;
    private AudioClip placementClip;

    // Reference to the flower dialogue box prefab
    [SerializeField] private GameObject dialogBoxPrefab;

    // Flag to track if this specific area has already had a flower placed
    private bool hasPlacedFlower = false;

    private bool isCorrect = false;
    [SerializeField] private MinigameManager minigameManager;

    // Reference to the flower interaction manager
    private FlowerInteractionManager flowerManager;

    private void Start()
    {
        audioPlayer = FindObjectOfType<Playaudio>();
        placementClip = Resources.Load<AudioClip>("piazzamento");

        // Find the manager
        flowerManager = FlowerInteractionManager.Instance;
        if (flowerManager == null)
        {
            Debug.LogWarning("FlowerInteractionManager not found in scene. Add it to continue using flower interactions.");
        }

        // Initialize dialog box prefab if not set in inspector
        if (dialogBoxPrefab == null)
        {
            dialogBoxPrefab = Resources.Load<GameObject>("FlowerDialogueBox");
        }
    }

    public string GetInteractMessage()
    {
        return "";
    }

    public void Interact()
    {
        if (minigameManager.HasSelectedSlot())
        {
            // Get the selected slot number and sprite
            int selectedSlot = minigameManager.GetCurrentSelectedSlot();
            Sprite selectedSprite = minigameManager.GetCurrentSlotSprite();

            // Apply the sprite to the real area
            realArea.GetComponent<SpriteRenderer>().sprite = selectedSprite;

            // Play the placement sound
            if (audioPlayer != null && placementClip != null)
            {
                audioPlayer.PlayOneShotAtPosition(gameObject, placementClip, 1f);
            }

            // Special handling for flower slots (slot 5)
            if (selectedSlot == 5 && !hasPlacedFlower)
            {
                hasPlacedFlower = true;
                HandleFlowerInteraction();
            }

            // Check if correct slot was placed
            if (selectedSlot == correctSlot)
            {
                isCorrect = true;
                minigameManager.CheckIfDone();
            }
            else
            {
                isCorrect = false;
            }
        }
    }

    // Handle flower interactions with sequential messages and sounds
    private void HandleFlowerInteraction()
    {
        // Only proceed if we have the flower manager
        if (flowerManager != null && correctSlot == 5)
        {
            // Get the current message and sound
            string message = flowerManager.GetNextDialogText();
            AudioClip soundClip = flowerManager.GetCurrentSoundClip();

            // Show the dialog message
            ShowFlowerDialog(message);

            // Play the sound if available
            if (audioPlayer != null && soundClip != null)
            {
                audioPlayer.PlayOneShotAtPosition(gameObject, soundClip, 0.6f);
            }
             
            // Advance to the next interaction
            flowerManager.AdvanceInteraction();
        }
    }

    // Show the flower dialog with a specific message
    private void ShowFlowerDialog(string message)
    {
        // Alternatively, if you want to instantiate a visual dialog box:
        if (dialogBoxPrefab != null)
        {
            GameObject dialogBox = Instantiate(dialogBoxPrefab, this.gameObject.transform);
            dialogBox.transform.localPosition = new Vector3(-21.52f, 0.03f, -15.87f);
            TextMeshProUGUI textComponent = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = message;
            }

            // Destroy the dialog box after a few seconds
            Destroy(dialogBox, 3f);
        }
    }

    public bool IsCorrect()
    {
        return isCorrect;
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }

    // Reset method for this area
    public void ResetLocalFlowerStatus()
    {
        hasPlacedFlower = false;
    }
}