using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionController;

public class Door : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float closeAngle = 0f;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.KeyPress;

    private Coroutine doorRoutine;
    private bool isOpen = false;

    public void Interact()
    {
        if (doorRoutine != null)
        {
            StopCoroutine(doorRoutine);
        }

        doorRoutine = StartCoroutine(OpenCloseDoor());
        isOpen = !isOpen;
    }

    public string GetInteractMessage()
    {
        return isOpen ? "Premi E per chiudere" : "Premi E per aprire";
    }

    private IEnumerator OpenCloseDoor()
    {
        float targetAngle = isOpen ? closeAngle : openAngle;
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        float elapsedTime = 0;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime * rotationSpeed);

            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
        doorRoutine = null;
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }
}