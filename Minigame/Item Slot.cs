using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private int slotNumber;
    [SerializeField] private MinigameManager minigameManager;
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.MouseClick;

    public string GetInteractMessage()
    {
        return "";
    }

    public void Interact()
    {
        minigameManager.ChangeSelectedSlot(slotNumber);
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }
}