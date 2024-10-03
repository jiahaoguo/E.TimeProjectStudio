using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton instance
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelData> levelList = new List<LevelData>();
    [SerializeField] private Transform LevelParent;
    [SerializeField] private Transform PlayerParent;

    public LevelData currentLevel;
    public LevelController levelController;
    private int currentLevelIndex;

    // Ensure only one instance of LevelManager exists
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the first level
        currentLevelIndex = 0;
        currentLevel = levelList[currentLevelIndex];
        LoadLevel(currentLevel);
    }

    // Public function to damage an enemy
    public void DamageEnemy(int damageAmount)
    {
        if (levelController != null && levelController.levelEnemyList.Count > levelController.currentEnemyId)
        {
            Enemy currentEnemy = levelController.levelEnemyList[levelController.currentEnemyId];

            if (currentEnemy != null)
            {
                // Subtract the damage from the enemy's health
                currentEnemy.Health -= damageAmount;

                // Check if the enemy's health is zero or below, for further logic like destroying the enemy
                if (currentEnemy.Health <= 0)
                {
                    Debug.Log("Enemy defeated!");

                    // Move to the next enemy
                    levelController.currentEnemyId++;

                    // Check if all enemies are defeated in the current level
                    if (levelController.currentEnemyId >= levelController.levelEnemyList.Count)
                    {
                        // All enemies defeated, move to the next level
                        NextLevel();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Invalid enemy ID or levelController is not set.");
        }
    }

    // Function to load the next level
    public void NextLevel()
    {
        // Destroy current level
        if (LevelParent.childCount > 0)
        {
            foreach (Transform child in LevelParent)
            {
                Destroy(child.gameObject); // Destroy all children of the level parent
            }
        }

        // Increment the level index
        currentLevelIndex++;

        // Check if there are more levels
        if (currentLevelIndex < levelList.Count)
        {
            // Load the next level
            currentLevel = levelList[currentLevelIndex];
            LoadLevel(currentLevel);
        }
        else
        {
            Debug.Log("All levels completed!");
            // Optionally, restart or load the first level again (depending on your game logic)
            currentLevelIndex = 0; // For example, restart from the first level
            currentLevel = levelList[currentLevelIndex];
            LoadLevel(currentLevel);
        }
    }

    // Function to initialize and load a level
    public void LoadLevel(LevelData levelData)
    {
        // Instantiate the level prefab and parent it to the LevelParent
        GameObject levelInstance = Instantiate(levelData.levelPrefab, LevelParent);

        // Get the LevelController from the instantiated level's prefab
        levelController = levelInstance.GetComponent<LevelController>();

        if (levelController == null)
        {
            Debug.LogError("LevelController component not found in the instantiated level prefab.");
        }
        else
        {
            // Reset the current enemy ID to 0 for the new level
            levelController.currentEnemyId = 0;
        }
    }
}
