using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerInteractionController;

public class ChatIcon : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private string iconName = "Chat App";
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.MouseClick;
    [SerializeField] private GameObject chatWindowPrefab;
    [SerializeField] public UnityEvent onInteract;
    [SerializeField] public UnityEvent onFirstClickPerDay;

    private bool isFirstTime = true;
    private int lastClickedDay = 0;
    private BedScript bedReference;

    private void Start()
    {
        // Find bed script reference to track current day
        bedReference = FindObjectOfType<BedScript>();
        if (bedReference == null)
        {
            Debug.LogWarning("BedScript reference not found. Day tracking will not work.");
        }
    }

    public void Interact()
    {
        if (chatWindowPrefab != null)
        {
            chatWindowPrefab.SetActive(true);
        }

        onInteract?.Invoke();

        // Check if this is the first click for the current day
        if (bedReference != null)
        {
            int currentDay = bedReference.GetCurrentDay();
            if (currentDay != lastClickedDay)
            {
                lastClickedDay = currentDay;
                onFirstClickPerDay?.Invoke();
                Debug.Log("First chat icon click for day " + currentDay);
            }
        }

        isFirstTime = false;
    }

    public string GetInteractMessage()
    {
        return "Click to launch " + iconName;
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }

    public bool isFirstTimeOpening()
    {
        return isFirstTime;
    }

    // Reset the day tracking when the day changes (optional method)
    public void ResetDayTracking()
    {
        lastClickedDay = 0;
    }
}