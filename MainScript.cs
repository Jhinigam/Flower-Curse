using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public MessageSender messageSender;
    public LevelManager levelManager;
    public MessageInput messageInput;
    public ChatController chatController;
    public Chair chair;
    public BedScript bed;
    public Computer computer;

    public GameObject minigameIcon;
    public ChatIcon chatIcon;

    public PlayerMove playerMove;
    public GameObject playerGO;
    public Transform couchSpotSitting;
    public Transform couchSpotStanding;

    public Playaudio audioPlayer;
    public AudioClip flowerScream;

    public GameObject KnightDialoguePrefab;
    public Image KnightDialogueImage;
    public TextMeshProUGUI KnightDialogueText;
    public string[] KnightQuotes;
    public AudioClip[] KnightAudioClips;

    public GameObject lastFlowerArea;

    public GameObject boxGO;
    public AudioClip doorBellsound;
    public GameObject doorBellSource;

    private void Start()
    {
        chatIcon.onInteract.AddListener(() => StartCoroutine(OnChatIconInteract()));
        chatIcon.onFirstClickPerDay.AddListener(() => StartCoroutine(OnFirstChatIconClickedPerDay()));
        messageInput.onMessageSent.AddListener((messageIndex) => StartCoroutine(OnReply(messageIndex)));
        minigameIcon.GetComponent<MinigameIcon>().onOpenedFirstTime.AddListener(() => StartCoroutine(OnMinigameIconInteract()));
        levelManager.OnLevelLoaded.AddListener((levelIndex) => StartCoroutine(OnLevelLoaded(levelIndex)));
        bed.onDay4Start.AddListener(() => StartCoroutine(OnDay4Start()));
        audioPlayer = FindObjectOfType<Playaudio>();

        /*if (computer != null && computer.onFirstTurnOnPerDay != null)
        {
            computer.onFirstTurnOnPerDay.AddListener(() => StartCoroutine(OnFirstPCUsePerDay()));
        }*/
    }
    private IEnumerator OnChatIconInteract()
    {
        if (chatIcon.isFirstTimeOpening())
        {
            yield return new WaitForSeconds(1f);
            messageSender.EnableChat(0);
            messageSender.SetText("Lance:\nHello hello, I'm Lance and this is your first day at Horn Games, the job is simple.");

            yield return new WaitForSeconds(4f);
            messageSender.EnableChat(1);
            messageSender.SetText("Lance:\nYou have to rebuild the played levels so that the next players can enjoy our game.");

            yield return new WaitForSeconds(4f);
            messageSender.EnableChat(2);
            messageSender.SetText("Lance:\nBasically, you just have to replace blocks, gems and stuff like that.");

            yield return new WaitForSeconds(4f);

            messageSender.SetText("Lance:\nJust remember that we can see everything you do on the PC during working hours.");

            yield return new WaitForSeconds(3.5f);

            messageSender.SetText("Lance:\nDon't worry though, the first mistakes will not be punished.");

            yield return new WaitForSeconds(3f);

            messageSender.SetText("Lance:\nAnd if you have any questions just ask!");

            yield return new WaitForSeconds(1f);
            
            messageInput.StartTyping(0);
        }

        yield return null;
    }
    private IEnumerator OnFirstChatIconClickedPerDay()
    {
        // Get the current day from the BedScript
        int day = bed.GetCurrentDay();

        switch (day)
        {
            case 2:
                yield return new WaitForSeconds(2f);
                messageSender.SetText("Lance:\nReady for day two?");
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(1);
                break;

            case 3:
                yield return new WaitForSeconds(2f);
                messageSender.SetText("Lance:\nGood evening again, try to give your best these days.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nThey are watching us.");
                yield return new WaitForSeconds(2f);
                messageSender.SetText("Lance:\nThey are demanding.");

                minigameIcon.SetActive(true);
                break;

            case 5:
                messageInput.StartTyping(6);
                break;
        }


        yield return null;
    }


    public IEnumerator OnReply(int messageIndex)
    {
        switch (messageIndex)
        {
            case 0:
                yield return new WaitForSeconds(1f);
                messageSender.SetText("Lance:\nLet's get started then. Begin by launching the game app");
                yield return new WaitForSeconds(1f);
                minigameIcon.SetActive(true);

                break;

            case 1:
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(2);
                break;

            case 2:
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nMr Lance! I could get used to this...");
                yield return new WaitForSeconds(3.5f);
                messageSender.SetText("Lance:\nAnd yes, I'll also unlock a couple more features for you.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nThere's not much to say about them");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nKeep up the work, I'll keep an eye on you");
                yield return new WaitForSeconds(1f);

                minigameIcon.SetActive(true);
                break;

            case 3:
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(4);
                break;


            case 4:
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nIt's not just for kids, and it's not usually like that.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nThe bodies tend to rot after a while.");
                yield return new WaitForSeconds(4f);
                messageSender.SetText("Lance:\nNow get to work, you can use the new tool to clean the mess.");
                levelManager.GetCurrentLevel().GetComponent<MinigameManager>().nextLevelButton.GetComponent<Collider>().enabled = true;

                break;

            case 5:
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nI don't know, I supervise like fifteen games in the Horn series.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nI don't remember the features of each one.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nBut don't worry, it's just a game.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nYou did a good job today, you can leave early if you want.");
                yield return new WaitForSeconds(1f);
                ThinkingSystem.Instance.ShowThought("I need to take some air..", 3f);
                
                break;

            case 6:
                yield return new WaitForSeconds(0.2f);
                messageInput.StartTyping(7);
                break;

            case 7:
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nRelax pal, I was just playing with you.");
                yield return new WaitForSeconds(2f);
                messageSender.SetText("Lance:\nI do that all the times with the new ones.");
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(8);
                break;

            case 8:
                yield return new WaitForSeconds(0.2f);
                messageInput.StartTyping(9);
                break;

            case 9:
                yield return new WaitForSeconds(0.2f);
                messageInput.StartTyping(10);
                break;

            case 10:
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nI know that the creatures in the game are alive");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nThere are no big bosses in the shadows threathening us.");
                yield return new WaitForSeconds(2f);
                messageSender.SetText("Lance:\nJust a little harmless prank.");
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(11);

                break;

            case 11:
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(12);
                break;

            case 12:
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nThey really are alive and everything they told you is true");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nBut they're just an harmless handful of bits and pixels");
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(13);

                break;

            case 13:
                yield return new WaitForSeconds(1f);
                messageInput.StartTyping(14);
                break;

            case 14:
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nIf it can help, think about the zeros on your salary.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nAnd to make up for my little prank, I'll do something for you"); //Era "unlock something for you", l'ho cambiato per la jam
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nI muted them, they can scream all they want..");
                yield return new WaitForSeconds(1f);
                Application.Quit();

                break;


        }
    }

    private IEnumerator OnMinigameIconInteract()
    {
        yield return new WaitForSeconds(3f);
        messageSender.SetText("Lance:\nI'll roll out a few tutorial levels to get you started.");
        yield return new WaitForSeconds(3f);
        messageSender.SetText("Lance:\nOkkkkk, now click the block icon on the top left.");
        yield return new WaitForSeconds(2f);
        messageSender.SetText("Lance:\nThen on the exclamation mark to replace the missing block.");
    }

    private IEnumerator OnLevelLoaded(int levelIndex)
    {
        switch (levelIndex) 
        {
            case 1:
                yield return new WaitForSeconds(0.5f);
                messageSender.SetText("Lance:\nNice work, are you sure it's your first time working as a rebuilder?");
                break;

            case 2:
                yield return new WaitForSeconds(0.5f);
                messageSender.SetText("Lance:\nNow I'll leave you be, but remember that I'm always watching.");
                break;

            case 5:

                levelManager.GetCurrentLevel().SetActive(false);
                minigameIcon.SetActive(false);
                levelManager.StopBackgroundMusic();
                yield return new WaitForSeconds(1f);
                messageSender.SetText("Lance:\nNice work today.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nAnyway, we will send you a gift in the next few days to celebrate your hiring.");
                yield return new WaitForSeconds(3f);
                messageSender.SetText("Lance:\nGet some rest now, see you tomorrow.");
                yield return new WaitForSeconds(2f);
                ThinkingSystem.Instance.ShowThought("I should brush my teeth before going to sleep.", 4f);
                break;

            case 10:
                levelManager.GetCurrentLevel().SetActive(false);
                minigameIcon.SetActive(false);
                levelManager.StopBackgroundMusic();
                yield return new WaitForSeconds(0.5f);
                messageSender.SetText("Lance:\nNice second day, see you tomorrow.");
                yield return new WaitForSeconds(2.5f);
                messageSender.SetText("Lance:\nStay tuned, your gift will arrive soon");
                ThinkingSystem.Instance.ShowThought("I'll watch a movie before going to sleep", 4f);

                yield return new WaitForSeconds(2f);

                //Scena DIVANO
                ScreenFader.Instance.FadeIn();
                chair.StandUp();
                yield return new WaitForSeconds(3f);
                playerMove.SetMovementEnabled(false);

                
                CharacterController playerController = playerGO.GetComponent<CharacterController>();

                playerController.enabled = false;
                // Move player
                playerGO.transform.position = couchSpotSitting.position;
                // Re-enable controller
                playerController.enabled = true;

                ScreenFader.Instance.FadeOut();

                yield return new WaitForSeconds(2f);

                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, flowerScream, 0.5f);

                yield return new WaitForSeconds(3f);

                ScreenFader.Instance.FadeIn();

                yield return new WaitForSeconds(ScreenFader.Instance.fadeDuration + 0.5f);

                ThinkingSystem.Instance.ShowThought("What the hell was that?", 3f);
                playerMove.SetMovementEnabled(true);
                
                playerController.enabled = false;
                playerGO.transform.position = couchSpotStanding.position;
                playerController.enabled = true;

               
                ScreenFader.Instance.FadeOut();


                break;

            case 12: //LIVELLO con SANGUE
                levelManager.GetCurrentLevel().GetComponent<MinigameManager>().nextLevelButton.GetComponent<Collider>().enabled = false;
                ScreenFader.Instance.FadeIn(0);
                yield return new WaitForSeconds(1f);
                ScreenFader.Instance.FadeOut(0);
                
                yield return new WaitForSeconds(1f);
                messageSender.SetText("Lance:\nIs something wrong?");
                yield return new WaitForSeconds(1f);
                
                messageInput.StartTyping(3);

                break;

            case 13:
                levelManager.GetCurrentLevel().GetComponent<MinigameManager>().onLevelCompleted.AddListener(() => StartCoroutine(On13LevelCompleted()));
                yield return new WaitForSeconds(1f);
                messageSender.SetText("Lance:\nUse the new tool to replace them");
                yield return new WaitForSeconds(1f);

                break;

            case 18: //ULTIMO LIVELLO / NERO
                yield return new WaitForSeconds(0.01f);
                levelManager.GetCurrentLevel().GetComponent<MinigameManager>().onLevelCompleted.RemoveListener(() => StartCoroutine(On13LevelCompleted()));
                levelManager.GetCurrentLevel().GetComponent<MinigameManager>().onLevelCompleted.AddListener(() => StartCoroutine(OnLastFlowerPlaced()));
                lastFlowerArea.GetComponent<Collider>().enabled = false;
                yield return new WaitForSeconds(1f);

                KnightDialoguePrefab.SetActive(true);
                KnightDialogueText.text = KnightQuotes[0];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[0], 0.5f);
                yield return new WaitForSeconds(3f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true);
                KnightDialogueText.text = KnightQuotes[1];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[1], 0.5f);
                yield return new WaitForSeconds(4f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true); //Thank you truly
                KnightDialogueText.text = KnightQuotes[2];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[2], 0.5f);
                yield return new WaitForSeconds(2f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true);
                KnightDialogueText.text = KnightQuotes[3];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[3], 0.5f);
                yield return new WaitForSeconds(6f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true);
                KnightDialogueText.text = KnightQuotes[4];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[4], 0.5f);
                yield return new WaitForSeconds(5f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true); //6
                KnightDialogueText.text = KnightQuotes[5];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[5], 0.5f);
                yield return new WaitForSeconds(9f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true); //7
                KnightDialogueText.text = KnightQuotes[6];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[6], 0.5f);
                yield return new WaitForSeconds(12f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true); //8
                KnightDialogueText.text = KnightQuotes[7];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[7], 0.5f);
                yield return new WaitForSeconds(10f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true); //9
                KnightDialogueText.text = KnightQuotes[8];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[8], 0.5f);
                yield return new WaitForSeconds(4f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true); //10
                KnightDialogueText.text = KnightQuotes[9];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[9], 0.5f);
                yield return new WaitForSeconds(8f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);

                KnightDialoguePrefab.SetActive(true); //11
                KnightDialogueText.text = KnightQuotes[10];
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[10], 0.5f);
                yield return new WaitForSeconds(4f);
                KnightDialoguePrefab.SetActive(false);
                yield return new WaitForSeconds(1.5f);
                
                lastFlowerArea.GetComponent<Collider>().enabled = true;
                levelManager.GetCurrentLevel().GetComponent<MinigameManager>().nextLevelButton.SetActive(true);
                break;

            case 19:
                
                minigameIcon.SetActive(false);
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[12], 0.5f);
                yield return new WaitForSeconds(10f);
                audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[13], 0.5f);
                yield return new WaitForSeconds(4f);
                messageSender.SetText("Lance:\nEven without me you did a good job handling HIM, you can now disconnect.");
                yield return new WaitForSeconds(2f);
                boxGO.SetActive(true);
                audioPlayer.PlayOneShotAtPosition(doorBellSource, doorBellsound, 1f);
                break;
        }
    }

    private IEnumerator On13LevelCompleted()
    {
        yield return new WaitForSeconds(0.0001f);
        levelManager.GetCurrentLevel().GetComponent<MinigameManager>().nextLevelButton.GetComponent<Collider>().enabled = false;
        levelManager.GetCurrentLevel().GetComponent<MinigameManager>().nextLevelButton.GetComponent<SpriteRenderer>().enabled = false;
        messageInput.StartTyping(5);
    }

    private IEnumerator OnLastFlowerPlaced()
    {
        yield return new WaitForSeconds(0.5f);
        ScreenFader.Instance.FadeIn(0);
        yield return new WaitForSeconds(2f);
        ScreenFader.Instance.FadeOut(0);
        yield return new WaitForSeconds(1f);

        KnightDialoguePrefab.SetActive(true); //12 FOR NOW I'm SATISFIED
        KnightDialogueText.text = KnightQuotes[11];
        audioPlayer.PlayOneShotAtPosition(levelManager.gameObject, KnightAudioClips[11], 0.5f);
        yield return new WaitForSeconds(4f);
        KnightDialoguePrefab.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        levelManager.GetCurrentLevel().SetActive(false);
        minigameIcon.SetActive(false);
        yield return new WaitForSeconds(1f);

        messageSender.SetText("Lance:\nI can't leave you alone for five minutes...");
        yield return new WaitForSeconds(4f);
        messageSender.SetText("Lance:\nI was ready to forgive small mistakes, but look at this mess you've made.");
        yield return new WaitForSeconds(4f);
        messageSender.SetText("Lance:\nI'll clean up fast, you go off, there will be consequences. We can't let them see this");
        yield return new WaitForSeconds(2f);

        boxGO.SetActive(true);
        audioPlayer.PlayOneShotAtPosition(doorBellSource,doorBellsound, 1f);
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator OnDay4Start()
    {
        yield return new WaitForSeconds(2f);
        messageSender.SetText("Lance:\nI'll be a little late today, but you already know what to do.");
        yield return new WaitForSeconds(2f);
        messageSender.SetText("Lance:\nDon't disappoint me.");
        levelManager.currentLevel++;
    }

    /*private IEnumerator OnFirstPCUsePerDay()
    {
        // Get the current day from the BedScript (assuming it exists in the scene)
        BedScript bedScript = FindObjectOfType<BedScript>();
        if (bedScript != null)
        {
            int day = bedScript.GetCurrentDay();

            switch (day)
            {
                case 2:
                    yield return new WaitForSeconds(2f);
                    messageSender.SetText("Lance:\nReady for day two?");
                    yield return new WaitForSeconds(1f);
                    messageInput.StartTyping(1);
                    break;
            }
        }

        yield return null;
    }*/
}
