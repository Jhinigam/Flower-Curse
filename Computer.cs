using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Computer : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private GameObject[] desktopObjects;
    [SerializeField] private GameObject[] minigameObjects;
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.KeyPress;

    [Header("First Use Tracking")]
    [SerializeField] private BedScript bedScript; // Reference to the BedScript to check current day

    // Event that triggers when PC is turned on for the first time in a day
    public UnityEvent onFirstTurnOnPerDay;

    public bool isOn = false;
    private int lastUsedDay = 0; // Track the day when the PC was last used

    private void Start()
    {
        // Find the BedScript reference if not set
        if (bedScript == null)
        {
            bedScript = FindObjectOfType<BedScript>();
            if (bedScript == null)
            {
                Debug.LogWarning("BedScript not found - PC first use per day tracking won't work properly");
            }
        }
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }

    public string GetInteractMessage()
    {
        return isOn ? "Press E to turn off the computer" : "Press E to turn on the computer";
    }

    public void Interact()
    {
        // Check if we're turning on the PC
        if (!isOn)
        {
            // Check if this is the first time turning on the PC today
            if (bedScript != null && bedScript.GetCurrentDay() != lastUsedDay)
            {
                // Update the last used day
                lastUsedDay = bedScript.GetCurrentDay();

                // Invoke the first use event
                onFirstTurnOnPerDay?.Invoke();
                Debug.Log("First PC use of day " + lastUsedDay);
            }
        }

        // Toggle the PC state
        isOn = !isOn;
        if (isOn)
        {
            ShowDesktop();
        }
        else
        {
            HideAll();
        }
    }

    private void ShowDesktop()
    {
        foreach (GameObject obj in desktopObjects)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in minigameObjects)
        {
            obj.SetActive(false);
        }
    }

    private void HideAll()
    {
        foreach (GameObject obj in desktopObjects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in minigameObjects)
        {
            obj.SetActive(false);
        }
    }
}