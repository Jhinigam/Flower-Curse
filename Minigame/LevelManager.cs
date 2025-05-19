using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject[] levels;
    [SerializeField] public int currentLevel = 0;

    // Event for when a level is loaded
    public UnityEvent<int> OnLevelLoaded;

    // Audio-related variables
    private Playaudio audioPlayer;
    private AudioSource musicSource;
    private bool isMusicPlaying = false;
    private AudioClip backgroundMusic;

    // Flag to permanently disable music after level 12
    private bool musicPermanentlyDisabled = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize audio components
        audioPlayer = FindObjectOfType<Playaudio>();
        if (audioPlayer == null)
        {
            Debug.LogError("Playaudio component not found in scene!");
        }

        // Load the background music from Resources folder
        backgroundMusic = Resources.Load<AudioClip>("OST");
        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background music not found in Resources folder!");
        }
    }

    // Check if any level is currently active
    private bool IsAnyLevelActive()
    {
        foreach (GameObject level in levels)
        {
            if (level.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void LoadNextLevel()
    {
        levels[currentLevel].SetActive(false);
        currentLevel++;
        
        if (currentLevel >= levels.Length)
        {
            OnLevelLoaded?.Invoke(currentLevel);
            return;
        }
        levels[currentLevel].SetActive(true);
        
        // Check if we've reached level 12
        if (currentLevel >= 12)
        {
            StopBackgroundMusic();
            musicPermanentlyDisabled = true;
            Debug.Log("Music permanently disabled after level 12");
        }
        // Start playing music only if it's not permanently disabled
        else if (!musicPermanentlyDisabled)
        {
            PlayBackgroundMusic();
        }

        OnLevelLoaded?.Invoke(currentLevel);
        // Trigger the level loaded event with the current level index

    }

    public GameObject GetCurrentLevel()
    {
        if (currentLevel < levels.Length)
        {
            return levels[currentLevel];
        }
        return null;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;

        // Check if the new level is 12 or higher to permanently disable music
        if (level >= 11)
        {
            StopBackgroundMusic();
            musicPermanentlyDisabled = true;
            Debug.Log("Music permanently disabled when setting to level 12 or higher");
        }
    }

    // Play background music if any level is active and music isn't permanently disabled
    private void PlayBackgroundMusic()
    {
        if (!musicPermanentlyDisabled && !isMusicPlaying && audioPlayer != null && backgroundMusic != null && IsAnyLevelActive())
        {
            musicSource = audioPlayer.PlayOnLoopAtPosition(gameObject, backgroundMusic, 1f);
            isMusicPlaying = true;
            Debug.Log("Started playing minigame music");
        }
    }

    // Stop background music
    public void StopBackgroundMusic()
    {
        if (isMusicPlaying && audioPlayer != null)
        {
            audioPlayer.StopSound(gameObject);
            isMusicPlaying = false;
            Debug.Log("Stopped playing minigame music");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if we should play or stop the music
        // Only play music if it's not permanently disabled
        if (IsAnyLevelActive() && !isMusicPlaying && !musicPermanentlyDisabled)
        {
            PlayBackgroundMusic();
        }
        else if (!IsAnyLevelActive() && isMusicPlaying)
        {
            StopBackgroundMusic();
        }
    }
}