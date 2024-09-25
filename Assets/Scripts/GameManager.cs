using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TimeManager timeManager;

    private int gold = 0;
    private Animator animator; // Add Animator variable

    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            GameEvents.OnVariableUpdated.Invoke("Gold", gold); // Broadcast the event
        }
    }

    private int coffee = 0;
    public int Coffee
    {
        get { return coffee; }
        set
        {
            coffee = value;
            GameEvents.OnVariableUpdated.Invoke("Coffee", coffee); // Broadcast the event
        }
    }

    public int goldRewardNum;
    public float goldRewardChance;

    private int timerLevel = 1;
    public int TimerLevel
    {
        get { return timerLevel; }
        set
        {
            timerLevel = value;
            GameEvents.OnVariableUpdated.Invoke("TimerLevel", timerLevel); // Broadcast the event
            GameEvents.OnVariableUpdated.Invoke("TimerNextLevel", TimerNextLevel); // Update the next level as well
        }
    }

    public int TimerNextLevel
    {
        get { return timerLevel + 1; } // Always return timerLevel + 1
    }

    private int upgradeCostGold = 1; // The initial cost for upgrading the timer
    // Property for upgradeCostGold that triggers event
    public int UpgradeCostGold
    {
        get { return upgradeCostGold; }
        set
        {
            upgradeCostGold = value;
            GameEvents.OnVariableUpdated.Invoke("UpgradeCostGold", upgradeCostGold); // Broadcast the event
        }
    }

    void Awake()
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

    void Start()
    {
        // Get the Animator component from the current GameObject
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on GameManager's GameObject.");
        }

        BroadCastVariables();
    }

    public void BroadCastVariables()
    {
        // Broadcast initial values for all variables
        GameEvents.OnVariableUpdated.Invoke("Gold", Gold);
        GameEvents.OnVariableUpdated.Invoke("Coffee", Coffee);
        GameEvents.OnVariableUpdated.Invoke("TimerLevel", TimerLevel);
        GameEvents.OnVariableUpdated.Invoke("TimerNextLevel", TimerNextLevel);
        GameEvents.OnVariableUpdated.Invoke("UpgradeCostGold", UpgradeCostGold); // Broadcast the initial upgrade cost
    }

    public void OpenFocusTimeScreen()
    {
        timeManager.StartCountdown(timeManager.countdownTime);
    }

    public void CloseFocusTimer()
    {
        timeManager.ResetTimer();
    }

    public void tryUpgradeTimer()
    {
        if (gold >= upgradeCostGold)
        {
            gold -= upgradeCostGold; // Deduct the gold cost
            
           
            animator.SetTrigger("UpgradeTimer");
        }
    }

    public void UpgradeTimer()
    {
        TimerLevel += 1; // Increase the timer level
        UpgradeCostGold += 5; // Example increment to make the upgrade cost higher for the next upgrade
    }
}
