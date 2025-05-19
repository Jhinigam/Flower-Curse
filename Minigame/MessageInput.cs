using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MessageInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typingArea; // The TMP text area where typing animation shows
    [SerializeField] private MessageSender messageSender; // Reference to the MessageSender script
    [SerializeField] private GameObject chatInputObject; // The gameobject containing the typing area, to toggle on/off
    [SerializeField] private float typingSpeed = 1f; // Time between characters appearing
    [SerializeField] private Chair chairRef;

    public UnityEvent<int> onMessageSent;
    [Tooltip("Define scripted messages that will be 'typed' by the player")]
    [SerializeField] private string[] scriptedMessages;

    private string currentMessage = ""; // The full message to be typed
    private string displayedMessage = ""; // The message as it appears during typing
    private int currentCharIndex = 0; // Keep track of how many characters have been typed
    private bool isTypingActive = false; // Is the typing mechanic currently active
    private int currentMessageIndex = 0; // Index of the current scripted message
    private bool isWaitingForInput = false; // Are we waiting for any key press to continue typing
    private bool canSendMessage = false; // Can we send the message (i.e., is the message complete)

    private void Start()
    {
        // Ensure the typing area is hidden at the start
        if (chatInputObject != null)
        {
            chatInputObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!chairRef.getIsOccupied())
        {
            // If the player is not sitting, disable typing
            isTypingActive = false;
        }
        else
        {
            isTypingActive = true;    
        }
        if (!isTypingActive) return;

        if (isWaitingForInput && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Escape))
        {
            // Player pressed a key, continue the typing animation
            isWaitingForInput = false;
            StartCoroutine(TypeNextCharacter());
        }

        // Check if message is complete and player pressed Enter
        if (currentCharIndex >= currentMessage.Length && Input.GetKeyDown(KeyCode.Return) && canSendMessage)
        {
            SendMessage();
        }

    }

    // Call this method to start the typing mechanic with a specific message
    public void StartTyping(int messageIndex = -1)
    {
        canSendMessage = true;
        chatInputObject.SetActive(true); // Show the typing area
        // If messageIndex is provided, use that specific message
        if (messageIndex >= 0 && messageIndex < scriptedMessages.Length)
        {
            currentMessageIndex = messageIndex;
        }

        // Validate we have a valid message to type
        if (scriptedMessages.Length <= currentMessageIndex)
        {
            Debug.LogError("No scripted message available at index: " + currentMessageIndex);
            return;
        }

        // Set up the typing sequence
        currentMessage = scriptedMessages[currentMessageIndex];
        displayedMessage = "";
        currentCharIndex = 0;

        // Show the typing area
        if (chatInputObject != null)
        {
            chatInputObject.SetActive(true);
        }

        // Update the text display
        if (typingArea != null)
        {
            typingArea.text = displayedMessage + "<color=#AAAAAA>|</color>"; // Add cursor
        }

        isTypingActive = true;
        isWaitingForInput = true; // Wait for first key press

        // Move to next message index for next time
        
    }

    private IEnumerator TypeNextCharacter()
    {
        // If we've reached the end of the message, just wait for Enter
        if (currentCharIndex >= currentMessage.Length)
        {
            isWaitingForInput = false;
            yield break;
        }

        // Add the next character to the displayed message
        displayedMessage += currentMessage[currentCharIndex];
        currentCharIndex++;

        // Update the displayed text
        if (typingArea != null)
        {
            typingArea.text = displayedMessage + "<color=#AAAAAA>|</color>"; // Add cursor
        }

        // Wait for the typing speed delay
        yield return new WaitForSeconds(typingSpeed);

        // If we have more characters, wait for next key press
        if (currentCharIndex < currentMessage.Length)
        {
            isWaitingForInput = true;
        }
    }

    private void SendMessage()
    {
        // Send the completed message to the chat
        if (messageSender != null)
        {
            // Create the full message with sender name if needed
            string fullMessage = "You:\n" + currentMessage;

            // Send to the MessageSender
            messageSender.SetText(fullMessage);

            // If the chat isn't visible, make it visible
            messageSender.EnableChat(0);
            typingArea.text = ""; // Clear the typing area
        }
        onMessageSent?.Invoke(currentMessageIndex); // Invoke the event with the current message index
        currentMessageIndex = (currentMessageIndex + 1) % scriptedMessages.Length;
        // Reset and hide the typing area
        isTypingActive = false;
        canSendMessage = false;
        if (chatInputObject != null)
        {
            chatInputObject.SetActive(false);
        }
    }

    // Public method to check if typing is currently active
    public bool IsTypingActive()
    {
        return isTypingActive;
    }
}
