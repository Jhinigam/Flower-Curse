using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerInteractionController;

public class BedScript : MonoBehaviour, PlayerInteractionController.IInteractable
{
    [SerializeField] private PlayerInteractionController.InteractionType interactionType = PlayerInteractionController.InteractionType.KeyPress;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Computer computer;
    [SerializeField] private Toothbrush toothbrush;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject playerGO;

    [Header("Day Tracking")]
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int maxDays = 5;

    [Header("Day 1 Tasks")]
    [SerializeField] public int requiredLevelForDay1 = 5;
    [SerializeField] private bool requirePCOffForDay1 = true;
    [SerializeField] private bool requireTeethBrushedForDay1 = true;

    [Header("Day 2 Tasks")]
    [SerializeField] public int requiredLevelForDay2 = 10;
    [SerializeField] private bool requirePCOffForDay2 = true;
    [SerializeField] private bool requireTeethBrushedForDay2 = true;

    [Header("Day 3 Tasks")]
    [SerializeField] public int requiredLevelForDay3 = 15;
    [SerializeField] private bool requirePCOffForDay3 = true;
    [SerializeField] private bool requireTeethBrushedForDay3 = true;
    [SerializeField] private bool requireGardenVisitForDay3 = true;

    [Header("Day 4 Tasks")]
    [SerializeField] public int requiredLevelForDay4 = 20;
    [SerializeField] private bool requirePCOffForDay4 = true;
    [SerializeField] private bool requireTeethBrushedForDay4 = false;


    [Header("Day 5 Tasks")]
    [SerializeField] public int requiredLevelForDay5 = 25;
    [SerializeField] private bool requirePCOffForDay5 = true;
    [SerializeField] private bool requireTeethBrushedForDay5 = true;

    // Sleep transition parameters
    [SerializeField] private float sleepFadeDuration = 2.0f;
    [SerializeField] private float sleepHoldDuration = 4.0f;

    [SerializeField] public UnityEvent onDay4Start;

    // Cached task completion status
    [SerializeField] private bool boxTaskCompletedForDay4 = false;
    private bool hasVisitedGarden = false;
    private bool areDailyTasksCompleted = false;

    private void Start()
    {
        // Find references if not assigned in inspector
        if (levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();

        if (computer == null)
            computer = FindObjectOfType<Computer>();

        if (toothbrush == null)
            toothbrush = FindObjectOfType<Toothbrush>();
    }

    public void Interact()
    {
        CheckDailyTasks();

        // Only allow sleeping if all tasks for the current day are completed
        if (areDailyTasksCompleted)
        {
            // Handle the sleeping logic here
            Debug.Log("Going to sleep for Day " + currentDay);

            // Start the sleep transition
            SleepSequence();

        }
        else
        {
            // Optional: Show a message saying you can't sleep yet
            Debug.Log("You can't sleep yet, you still have tasks to complete for Day " + currentDay);
        }
    }

    public string GetInteractMessage()
    {
        CheckDailyTasks();

        if (areDailyTasksCompleted)
        {
            return "Press E to go to sleep";
        }
        else
        {
            // Create a list of remaining tasks based on the current day
            List<string> remainingTasks = GetRemainingTasksForDay(currentDay);

            // Join the remaining tasks into a single message
            return "You need to: " + string.Join(", ", remainingTasks);
        }
    }

    public PlayerInteractionController.InteractionType GetInteractionType()
    {
        return interactionType;
    }

    // Check if all required tasks for the current day are completed
    private void CheckDailyTasks()
    {
        // Check the tasks for the current day
        List<string> tasks = GetRemainingTasksForDay(currentDay);
        areDailyTasksCompleted = (tasks.Count == 0);
    }

    // Get the list of remaining tasks for the specified day
    private List<string> GetRemainingTasksForDay(int day)
    {
        List<string> remainingTasks = new List<string>();

        switch (day)
        {
            case 1:
                if (requirePCOffForDay1 && !IsPCTurnedOff())
                    remainingTasks.Add("Turn off the PC");

                if (requiredLevelForDay1 > 0 && !IsLevelCompleted(requiredLevelForDay1))
                    remainingTasks.Add("Finish level " + requiredLevelForDay1);

                if (requireTeethBrushedForDay1 && !AreTeethBrushed())
                    remainingTasks.Add("Brush your teeth");
                break;

            case 2:
                if (requirePCOffForDay2 && !IsPCTurnedOff())
                    remainingTasks.Add("Turn off the PC");

                if (requiredLevelForDay2 > 0 && !IsLevelCompleted(requiredLevelForDay2))
                    remainingTasks.Add("Finish level " + requiredLevelForDay2);

                if (requireTeethBrushedForDay2 && !AreTeethBrushed())
                    remainingTasks.Add("Brush your teeth");
                break;

            case 3:
                if (requirePCOffForDay3 && !IsPCTurnedOff())
                    remainingTasks.Add("Turn off the PC");

                if (requiredLevelForDay3 > 0 && !IsLevelCompleted(requiredLevelForDay3))
                    remainingTasks.Add("Finish level " + requiredLevelForDay3);

                if (requireTeethBrushedForDay3 && !AreTeethBrushed())
                    remainingTasks.Add("Brush your teeth");

                if (requireGardenVisitForDay3 && !hasVisitedGarden)
                    remainingTasks.Add("Clear your mind in the garden");
                break;

            case 4:
                if (requirePCOffForDay4 && !IsPCTurnedOff())
                    remainingTasks.Add("Turn off the PC");

                if (requiredLevelForDay4 > 0 && !IsLevelCompleted(requiredLevelForDay4))
                    remainingTasks.Add("Finish level " + requiredLevelForDay4);

                if (requireTeethBrushedForDay4 && !AreTeethBrushed())
                    remainingTasks.Add("Brush your teeth");

                if (!boxTaskCompletedForDay4)
                {
                    remainingTasks.Add("???");
                }
                    break;

            case 5:
                if (requirePCOffForDay5 && !IsPCTurnedOff())
                    remainingTasks.Add("Turn off the PC");

                if (requiredLevelForDay5 > 0 && !IsLevelCompleted(requiredLevelForDay5))
                    remainingTasks.Add("Finish level " + requiredLevelForDay5);

                if (requireTeethBrushedForDay5 && !AreTeethBrushed())
                    remainingTasks.Add("Brush your teeth");
                break;

            default:
                // For days beyond those specified, you can add default tasks
                remainingTasks.Add("No tasks for this day");
                break;
        }

        return remainingTasks;
    }

    // Check if the required level is completed
    private bool IsLevelCompleted(int requiredLevel)
    {
        return levelManager != null && levelManager.currentLevel >= requiredLevel;
    }

    // Check if the PC is turned off
    private bool IsPCTurnedOff()
    {
        return computer != null && !computer.isOn;
    }

    // Check if teeth are brushed
    private bool AreTeethBrushed()
    {
        return toothbrush != null && toothbrush.HasBeenUsed();
    }

    public void MarkGardenAsVisited()
    {
        hasVisitedGarden = true;
    }
    // Sleep sequence - handles the transition between days
    private void SleepSequence()
    {
        StartCoroutine(SleepAndReposition());
    }

    private IEnumerator SleepAndReposition()
    {
        // Start the fade effect
        ScreenFader.Instance.FadeIn();

        // Wait for fade in to complete
        yield return new WaitForSeconds(sleepFadeDuration);

        // During black screen, reposition player
        CharacterController playerController = playerGO.GetComponent<CharacterController>();
        if (playerController != null)
        {
            // Disable controller temporarily
            playerController.enabled = false;
            // Move player
            playerGO.transform.position = playerSpawn.position;
            // Re-enable controller
            playerController.enabled = true;
        }
        else
        {
            // No controller, just move directly
            playerGO.transform.position = playerSpawn.position;
        }

        TurnOffAllLights();
        // Wait during "sleep"
        yield return new WaitForSeconds(sleepHoldDuration);

        // Increment the day
        currentDay++;

        toothbrush.ResetForNewDay();
        if (currentDay > maxDays)
        {
            Debug.Log("All days completed!");
        }

        // Reset daily tasks if needed

        // Fade back out
        ScreenFader.Instance.FadeOut();

        ThinkingSystem.Instance.ShowThought("DAY " + currentDay, 3f);
        Debug.Log("Starting Day " + currentDay);

        if(currentDay == 4)
        {
            onDay4Start.Invoke();
        }
    }

    public void TurnOffAllLights()
    {
        // Find all Lightswitch components in the scene
        Lightswitch[] allLightswitches = FindObjectsOfType<Lightswitch>();

        Debug.Log($"Found {allLightswitches.Length} lightswitches in the scene");

        // Loop through each lightswitch and turn off its light
        foreach (Lightswitch lightswitch in allLightswitches)
        {
            // Check if lights are on before turning them off
            if (lightswitch.AreLightsOn())
            {
                lightswitch.Interact(); // Call the Interact method to toggle the lights off
                Debug.Log($"Turned off lights for lightswitch: {lightswitch.gameObject.name}");
            }
        }
    }

    public void MarkBoxTaskCompleted()
    {
        boxTaskCompletedForDay4 = true;
    }
    // Public method to get the current day
    public int GetCurrentDay()
    {
        return currentDay;
    }
}