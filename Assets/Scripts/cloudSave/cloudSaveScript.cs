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

    // Start method to initialize Unity services and ensure authentication is complete
    public async void Start()
    {
        await UnityServices.InitializeAsync();

        // Ensure that Cloud Save operations only happen after successful authentication
        if (AuthenticationService.Instance.IsSignedIn)
        {
            LoadData(); // Automatically load data after successful sign-in
        }
        else
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                LoadData(); // Load data once authentication is complete
            };
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

            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            //status.text = "Data saved!";
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
        else
        {
            status.text = "Load failed. User is not authenticated.";
        }
    }

    // Delete saved keys from the cloud
    public async void DeleteKey()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            await CloudSaveService.Instance.Data.ForceDeleteAsync("GameManagerSave");
            status.text = "Data deleted!";
        }
        else
        {
            status.text = "Delete failed. User is not authenticated.";
        }
    }

    // Retrieve all keys from the cloud and print them to the console
    public async void RetrieveAllKeys()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            List<string> allKeys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

            for (int i = 0; i < allKeys.Count; i++)
            {
                Debug.Log(allKeys[i]);
            }
        }
        else
        {
            status.text = "Retrieve failed. User is not authenticated.";
        }
    }
}
