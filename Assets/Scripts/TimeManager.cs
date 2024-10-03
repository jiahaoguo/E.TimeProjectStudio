using System.Collections;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public float countdownTime = 10f; // Default value is 10 seconds
    public float currentTime;
    private bool isCountingDown = false;

    [SerializeField] private TextMeshProUGUI timerText;

    private float goldRewardInterval = 1f; // Interval for checking reward chance (1 second)
    private float rewardTimer = 0f;

    private Coroutine countdownCoroutine; // Store reference to the countdown coroutine

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Make this persist between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Update()
    {
        // Countdown is handled in the coroutine now
    }

    // Function to start the countdown with a specified time
    public void StartCountdown(float time)
    {
        currentTime = time;
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine); // Stop any running coroutine
        }
        countdownCoroutine = StartCoroutine(CountdownCoroutine()); // Start a new coroutine
    }

    // Coroutine that handles the countdown process
    private IEnumerator CountdownCoroutine()
    {
        isCountingDown = true;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            rewardTimer += Time.deltaTime;

            if (rewardTimer >= goldRewardInterval)
            {
                rewardTimer = 0f;
                if (Random.Range(0f, 1f) <= GameManager.Instance.goldRewardChance)
                {
                    GameManager.Instance.Gold += GameManager.Instance.goldRewardNum * (GameManager.Instance.TimerLevel/10+1);
                    int damage = Random.Range(3, 8) * (GameManager.Instance.TimerLevel+1);
                    LevelManager.Instance.DamageEnemy(damage);
                    Debug.Log("Gold Added! Total Gold: " + GameManager.Instance.Gold);
                    AnnouncementManager.Instance.ShowAnnouncement(GameManager.Instance.goldRewardNum + " Gold Added \n " + damage + "damage applied");
                }
                else
                {
                    Debug.Log("No gold this time.");
                }
            }

            UpdateTimerDisplay();

            yield return null; // Wait for the next frame
        }

        currentTime = 0;
        isCountingDown = false;
        timerText.text = "00:00"; // Display when time is up
        Debug.Log("Countdown Complete!");
    }

    // Function to reset the timer and stop the countdown
    public void ResetTimer()
    {
        // Stop the coroutine if it's running
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        currentTime = countdownTime; // Reset the current time to the initial countdown time
        rewardTimer = 0f; // Reset the reward timer
        isCountingDown = false; // Set countdown to false to avoid auto-starting
        UpdateTimerDisplay(); // Update the timer text display immediately
    }

    // Helper function to update the timer text display
    private void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(currentTime / 3600);
        int minutes = Mathf.FloorToInt((currentTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string timeString = string.Format("{1:D2}:{2:D2}", hours, minutes, seconds);
        timerText.text = timeString;
    }


}
