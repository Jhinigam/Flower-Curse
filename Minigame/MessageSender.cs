using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageSender : MonoBehaviour
{
    public GameObject[] chats;

    public TextMeshProUGUI[] chatTexts;
    private Playaudio audioPlayer;
    public GameObject soundSpot;
    public AudioClip notificationClip;

    private void Start()
    {
        // Initialize the audio player
        audioPlayer = FindObjectOfType<Playaudio>();
    }
    public void EnableChat(int index)
    {
        // Enable the chat game object
        chats[index].SetActive(true);
    }

    public void SetText(string text)
    {
        // Set the text of the chat
        if (chats[2].activeSelf && chats[1].activeSelf)
        {
            chatTexts[2].text = chatTexts[1].text;
        }
        if (chats[1].activeSelf && chats[0].activeSelf)
        {
            chatTexts[1].text = chatTexts[0].text;
        }
        chatTexts[0].text = text;
        if (text.StartsWith("Lance:"))
        {
            audioPlayer.PlayOneShotAtPosition(soundSpot, notificationClip, 0.5f);
        }
    }
}
