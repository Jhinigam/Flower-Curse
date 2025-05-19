using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionController;

public class Chair : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private Transform sittingPosition;
    [SerializeField] private Transform standingPosition;
    [SerializeField] private string sittingMessage = "Press Space to stand up";
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.KeyPress;

    private bool isOccupied = false;
    private GameObject player;
    private CharacterController playerController;
    private PlayerMove playerMoveScript;
    private PlayerLook playerLookScript;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    private bool showSittingMessage = false;
    private GUIStyle guiStyle;

    private void Start()
    {
        // If sitting position is not assigned, create one as a child
        if (sittingPosition == null)
        {
            GameObject sitPos = new GameObject("SittingPosition");
            sitPos.transform.parent = transform;
            sitPos.transform.localPosition = new Vector3(0, 0.5f, 0);
            sittingPosition = sitPos.transform;
        }

        // If standing position is not assigned, create one as a child
        if (standingPosition == null)
        {
            GameObject standPos = new GameObject("StandingPosition");
            standPos.transform.parent = transform;
            standPos.transform.localPosition = new Vector3(0, 0, -1.0f);
            standingPosition = standPos.transform;
        }

        SetupGUI();
    }

    private void SetupGUI()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 16;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.normal.textColor = Color.white;
        guiStyle.alignment = TextAnchor.MiddleCenter;
    }

    public void Interact()
    {
        if (!isOccupied)
        {
            // Get the player only when needed
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                SitDown();
            }
        }
    }

    private void SitDown()
    {
        // Get fresh references to components each time
        playerController = player.GetComponent<CharacterController>();
        playerMoveScript = player.GetComponent<PlayerMove>();
        playerLookScript = player.GetComponentInChildren<PlayerLook>();

        originalPlayerPosition = player.transform.position;
        originalPlayerRotation = player.transform.rotation;

        // Disable player movement and controller
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = false;
        }

        // Force immediate teleport to sitting position
        player.transform.position = sittingPosition.position;
        player.transform.rotation = sittingPosition.rotation;

        isOccupied = true;
        showSittingMessage = true;

        StartCoroutine(WaitForStandUp());
    }

    private IEnumerator WaitForStandUp()
    {
        while (isOccupied)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StandUp();
                yield break;
            }
            yield return null;
        }
    }

    public void StandUp()
    {
        // First move the player to the standing position before re-enabling components
        player.transform.position = standingPosition.position;
        player.transform.rotation = originalPlayerRotation;

        // Small delay before re-enabling components to prevent collision issues
        StartCoroutine(DelayedEnableControls());

        isOccupied = false;
        showSittingMessage = false;
    }

    private IEnumerator DelayedEnableControls()
    {
        // Small delay to ensure the position is set before enabling physics
        yield return new WaitForSeconds(0.05f);

        // Re-enable components
        if (playerMoveScript != null)
            playerMoveScript.enabled = true;

        if (playerController != null)
            playerController.enabled = true;
    }

    public string GetInteractMessage()
    {
        return isOccupied ? "" : "Press E to sit down";
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }

    private void OnGUI()
    {
        if (showSittingMessage)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 80, 200, 50), sittingMessage, guiStyle);
        }
    }
    public bool getIsOccupied()
    {
        return isOccupied;
    }
}