using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour
{
    // Define a delegate for slot selection change events
    public delegate void SlotSelectionChangeHandler(int newSelectedSlot, int previousSelectedSlot);

    // Event that will be fired when the selected slot changes
    public event SlotSelectionChangeHandler OnSlotSelectionChanged;

    private int currentSelectedSlot = 1;
    private int previousSelectedSlot = -1; // Track the previous selected slot
    [SerializeField] private GameObject[] realSlots;
    [SerializeField] private GameObject[] fillableAreas;
    private bool hasSelectedSlot = false;
    [SerializeField] public GameObject nextLevelButton;

    public UnityEvent onLevelCompleted;


    // Reference to the highlight object
    private GameObject highlightObject;
    // The highlight prefab that will be loaded from Resources
    private GameObject highlightPrefab;

    public void ChangeSelectedSlot(int slot)
    {
        // Store previous slot before changing
        previousSelectedSlot = currentSelectedSlot;
        currentSelectedSlot = slot;
        hasSelectedSlot = true;

        // Update the highlight to the newly selected real slot
        UpdateHighlight();

        // Notify all listeners that the selected slot has changed
        OnSlotSelectionChanged?.Invoke(currentSelectedSlot, previousSelectedSlot);
    }

    public int GetCurrentSelectedSlot()
    {
        return currentSelectedSlot;
    }

    public Sprite GetCurrentSlotSprite()
    {
        return realSlots[currentSelectedSlot - 1].GetComponent<SpriteRenderer>().sprite;
    }

    public bool HasSelectedSlot()
    {
        return hasSelectedSlot;
    }

    public void CheckIfDone()
    {
        foreach (GameObject area in fillableAreas)
        {
            if (area.GetComponent<FillableArea>().IsCorrect())
            {
                //Area is correct
            }
            else
            {
                return;
            }
        }

        // If all areas are filled correctly
        onLevelCompleted?.Invoke();
        Debug.Log("All areas filled correctly!");
        nextLevelButton.GetComponent<NextLevelButton>().ShowButton();
    }

    // You might want to initialize the first slot as selected when the game starts
    private void Start()
    {
        // Load the highlight prefab
        highlightPrefab = Resources.Load<GameObject>("SlotHighlight");
        if (highlightPrefab == null)
        {
            Debug.LogError("SlotHighlight prefab not found in Resources folder! Please create one.");
        }

    }

    // Method to update the highlight position
    private void UpdateHighlight()
    {
        // Make sure we have realSlots and a valid currentSelectedSlot
        if (realSlots == null || currentSelectedSlot <= 0 || currentSelectedSlot > realSlots.Length)
        {
            return;
        }

        // Make sure we have a highlight prefab
        if (highlightPrefab == null)
        {
            highlightPrefab = Resources.Load<GameObject>("SlotHighlight");
            if (highlightPrefab == null) return;
        }

        // Get the target real slot
        GameObject targetSlot = realSlots[currentSelectedSlot - 1];

        // If highlight doesn't exist, create it
        if (highlightObject == null)
        {
            highlightObject = Instantiate(highlightPrefab);
        }

        // Position the highlight behind the real slot
        highlightObject.transform.position = targetSlot.transform.position;
        highlightObject.transform.SetParent(targetSlot.transform);

        // Move it slightly behind the slot for proper rendering
        Vector3 localPos = highlightObject.transform.localPosition;
        localPos.z = -0.01f; // Adjust this value as needed
        highlightObject.transform.localPosition = localPos;

        // Make sure it's sized appropriately (adjust as needed)
        highlightObject.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
    }
}