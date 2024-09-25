using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncementManager : MonoBehaviour
{
    public static AnnouncementManager Instance { get; private set; }

    [SerializeField] private GameObject announcementTextPrefab;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private Transform announcementParent;
    [SerializeField] private float hideTime = 15;

    private Queue<GameObject> announcementPool;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        announcementPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            // Instantiate announcement under the assigned parent
            GameObject obj = Instantiate(announcementTextPrefab, announcementParent);
            obj.SetActive(false);
            announcementPool.Enqueue(obj);
        }
    }

    // Method to dynamically set the parent object
    public void SetAnnouncementParent(Transform newParent)
    {
        announcementParent = newParent;
    }

    public void ShowAnnouncement(string message)
    {
        if (announcementPool.Count > 0)
        {
            GameObject announcementObj = announcementPool.Dequeue();
            announcementObj.SetActive(true);

            // Set the announcement text
            announcementObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;

            // Return the object to the pool after the delay
            StartCoroutine(HideAnnouncement(announcementObj, hideTime));
        }
        else
        {
            Debug.LogWarning("No announcement objects available in the pool!");
        }
    }

    private IEnumerator HideAnnouncement(GameObject announcementObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        announcementObj.SetActive(false);
        announcementPool.Enqueue(announcementObj);
    }
}
