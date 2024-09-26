using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class cloudSaveScript : MonoBehaviour
{
    public Text status;

    public async void Start()
    {
        // Initialize Unity services and wait for authentication
        await UnityServices.InitializeAsync();

        // Ensure that Cloud Save operations only happen after successful authentication
        if (AuthenticationService.Instance.IsSignedIn)
        {
            LoadData(); // Automatically load data after successful sign-in
            StartCoroutine(AutoSave()); // Start the auto-save coroutine
        }
        else
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                LoadData(); // Load data once authentication is complete
                StartCoroutine(AutoSave()); // Start the auto-save coroutine
            };
        }
    }

    // Coroutine to automatically save data every 3 seconds
    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(3); // Wait for 3 seconds
            SaveData(); // Save data to the cloud
        }
    }

    // Save GameManager data as JSON to the cloud
    public async void SaveData()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            GameManager gameManager = GameManager.Instance;
            SaveData saveData = gameManager.GetSaveData();

            string jsonData = JsonUtility.ToJson(saveData);

            var data = new Dictionary<string, object> {
                { "GameManagerSave", jsonData }
            };

            try
            {
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
                status.text = "Data saved!";
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error saving data: " + ex.Message);
                status.text = "Save failed: " + ex.Message;
            }
        }
        else
        {
            status.text = "Save failed. User is not authenticated.";
        }
    }

    // Load GameManager data from the cloud
    public async void LoadData()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "GameManagerSave" });

                if (serverData.ContainsKey("GameManagerSave"))
                {
                    string jsonData = serverData["GameManagerSave"];
                    SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

                    GameManager gameManager = GameManager.Instance;
                    gameManager.LoadSaveData(saveData);

                    status.text = "Data loaded!";
                }
                else
                {
                    status.text = "No data found!";
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error loading data: " + ex.Message);
                status.text = "Load failed: " + ex.Message;
            }
        }
        else
        {
            status.text = "Load failed. User is not authenticated.";
        }
    }
}
