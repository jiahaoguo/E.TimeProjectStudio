using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveData
{
    public int Gold;
    public int Coffee;
    public int TimerLevel;
    public int UpgradeCostGold;

    // You can easily add more variables here in the future
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TimeManager timeManager;
    [SerializeField] private UpgradeData timerUpgradeData;
    [SerializeField] private cloudSaveScript cloudSave; // Reference to your cloudSaveScript for saving data

    private int gold = 0;
    private Animator animator;

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
        get { return timerLevel + 1; }
    }

    private int upgradeCostGold = 1;
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
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on GameManager's GameObject.");
        }

        BroadCastVariables();
        InitiateUpgradeData();
    }

    void InitiateUpgradeData()
    {
        UpgradeCostGold = timerUpgradeData.requiredGold[TimerLevel];
    }

    public void BroadCastVariables()
    {
        GameEvents.OnVariableUpdated.Invoke("Gold", Gold);
        GameEvents.OnVariableUpdated.Invoke("Coffee", Coffee);
        GameEvents.OnVariableUpdated.Invoke("TimerLevel", TimerLevel);
        GameEvents.OnVariableUpdated.Invoke("TimerNextLevel", TimerNextLevel);
        GameEvents.OnVariableUpdated.Invoke("UpgradeCostGold", UpgradeCostGold);
    }

    public SaveData GetSaveData()
    {
        SaveData data = new SaveData
        {
            Gold = Gold,
            Coffee = Coffee,
            TimerLevel = TimerLevel,
            UpgradeCostGold = UpgradeCostGold
        };
        return data;
    }

    public void LoadSaveData(SaveData data)
    {
        Gold = data.Gold;
        Coffee = data.Coffee;
        TimerLevel = data.TimerLevel;
        UpgradeCostGold = data.UpgradeCostGold;
        BroadCastVariables();
    }

    public void OpenFocusTimeScreen()
    {
        timeManager.StartCountdown(timeManager.countdownTime);
    }

    public void CloseFocusTimer()
    {
        timeManager.ResetTimer();
    }

    public void tryUpgradeTimer(int i)
    {
        if (gold >= upgradeCostGold)
        {
            gold -= upgradeCostGold;
            animator.SetTrigger("UpgradeTimer");
        }
    }

    public void UpgradeTimer()
    {
        TimerLevel += 1;
        UpgradeCostGold = timerUpgradeData.requiredGold[TimerLevel];
    }

    // Called when the application is about to quit
    private void OnApplicationQuit()
    {
        cloudSave.SaveData(); // Auto-save before the game closes
    }
}
