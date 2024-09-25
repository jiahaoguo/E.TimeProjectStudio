using UnityEngine;
using TMPro;

public class AnnouncementText : MonoBehaviour
{
    public float movementSpeed = 10f; // Speed of the upward movement
    public float fadeSpeed = 1f; // Speed of the fade-out

    private TextMeshProUGUI textMeshPro;
    private CanvasGroup canvasGroup; // To handle transparency more smoothly

    private bool isFading = false;
    private Vector3 initialPosition; // Store the initial position

    private void Awake()
    {
        // Get the TextMeshPro component from the child object
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = textMeshPro.gameObject.AddComponent<CanvasGroup>(); // To control transparency easily

        // Record the initial position of the GameObject
        initialPosition = textMeshPro.transform.position;

        // Find the main camera in the scene
        Camera mainCamera = Camera.main;

        // Check if both cameras are found and assign the main camera to the eventCamera
        if (mainCamera != null)
        {
            // Assuming the Camera is part of a Canvas, set the eventCamera to the mainCamera
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvas.worldCamera = mainCamera;
            }
        }
        else
        {
            Debug.LogWarning("Child camera or main camera not found.");
        }
    }

    private void OnEnable()
    {
        // Reset the text position to the initial position and transparency
        textMeshPro.transform.position = initialPosition;
        canvasGroup.alpha = 1f;
        isFading = false;
    }

    private void Update()
    {
        // Move the child text upwards
        textMeshPro.transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);

        // Begin fading if not already fading
        if (!isFading)
        {
            StartCoroutine(FadeOut());
            isFading = true;
        }
    }

    private System.Collections.IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Once fully faded, set the object inactive
        gameObject.SetActive(false);
    }
}
