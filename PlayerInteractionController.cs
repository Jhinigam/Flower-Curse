using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Camera playerCamera;
    private bool showInteractMsg;
    private string interactMessage = "";
    private GUIStyle guiStyle;

    public interface IInteractable
    {
        void Interact();
        string GetInteractMessage();
        InteractionType GetInteractionType(); // New method to determine interaction type
    }

    // Define interaction types
    public enum InteractionType
    {
        KeyPress,
        MouseClick,
        Both
    }

    private void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("Camera tagged as 'MainCamera' is missing!");
        }

        SetupGUI();
    }

    private void SetupGUI()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 16;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.normal.textColor = Color.white;
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        // Cast ray from center of screen
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        showInteractMsg = false;

        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, interactionRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                InteractionType interactionType = interactable.GetInteractionType();
                showInteractMsg = true;
                interactMessage = interactable.GetInteractMessage();

                // Handle interaction based on the type
                if ((interactionType == InteractionType.KeyPress || interactionType == InteractionType.Both) &&
                    Input.GetKeyDown(interactKey))
                {
                    interactable.Interact();
                }
                else if ((interactionType == InteractionType.MouseClick || interactionType == InteractionType.Both) &&
                         Input.GetMouseButtonDown(0))
                {
                    interactable.Interact();
                }
            }
        }
    }

    private void OnGUI()
    {
        if (showInteractMsg)
        {
            GUI.Label(new Rect(50, Screen.height - 50, 200, 50), interactMessage, guiStyle);
        }
    }
}