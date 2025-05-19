using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionController;

public class Lightswitch : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private GameObject[] lightSources;
    [SerializeField] private GameObject[] blackLights;
    private bool lightsOn = false;
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.KeyPress;

    public void Interact()
    {
        ToggleLight();
    }

    public string GetInteractMessage()
    {
        return lightsOn ? "Press E to turn the lights off" : "Press E to the lights on";
    }

    private void ToggleLight()
    {
        lightsOn = !lightsOn;

        foreach (GameObject go in lightSources)
        {
            go.GetComponent<Light>().enabled = lightsOn;
        }
        foreach (GameObject go in blackLights)
        {
            go.SetActive(!lightsOn);
        }
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }

    public bool AreLightsOn()
    {
        return lightsOn;
    }

    public void SetLights(bool turnOn)
    {
        // Only change state if it's different
        if (lightsOn != turnOn)
        {
            lightsOn = turnOn;

            foreach (GameObject go in lightSources)
            {
                go.GetComponent<Light>().enabled = lightsOn;
            }
            foreach (GameObject go in blackLights)
            {
                go.SetActive(!lightsOn);
            }
        }
    }
}