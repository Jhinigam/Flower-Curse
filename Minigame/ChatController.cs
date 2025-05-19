using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    [SerializeField] private MessageInput messageInput;
    [SerializeField] private MessageSender messageSender;

    // Define specific conversation triggers
    [SerializeField] private bool enablePlayerResponse = true;
    [SerializeField] private float responseDelay = 2.0f; // Delay before player can respond

    // Call this method when you want to allow the player to type a specific response
    // For example after receiving a specific message
    public void AllowPlayerResponse(int messageIndex)
    {
        if (enablePlayerResponse)
        {
            StartCoroutine(DelayedPlayerResponse(messageIndex));
        }
    }

    private IEnumerator DelayedPlayerResponse(int messageIndex)
    {
        yield return new WaitForSeconds(responseDelay);
        messageInput.StartTyping(messageIndex);
    }

    // Disable normal player input when typing is active
    public bool ShouldDisablePlayerControls()
    {
        // Return true if the player is currently in a typing interaction
        return messageInput.IsTypingActive();
    }
}
