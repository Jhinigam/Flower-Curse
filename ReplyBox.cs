using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplyBox : MonoBehaviour,PlayerInteractionController.IInteractable
{
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.MouseClick;

    public string GetInteractMessage()
    {
        return "Press keys on the keyboard to reply";
    }

    public void Interact()
    {
        //
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }
}
