using UnityEngine;
using UnityEngine.UI; // Add for UI components
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AlphaController : MonoBehaviour
{
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<TextMeshProUGUI> textMeshes = new List<TextMeshProUGUI>();
    private List<Image> uiImages = new List<Image>(); // Add for UI Image components

    // Example input format: "TargetName,startAlpha,endAlpha,duration"
    // Public method to start scanning and tweening using a string input
    public void StartAlphaTweenWithString(string input)
    {
        // Example input: "TargetName,startAlpha,endAlpha,duration"
        // Split the input string by commas
        string[] parameters = input.Split(',');

        if (parameters.Length != 4)
        {
            Debug.LogError("Invalid input format. Expected format: 'TargetName,startAlpha,endAlpha,duration'");
            return;
        }

        // Parse the target object name
        string targetName = parameters[0];
        Transform target = transform.Find(targetName)?.transform;

        if (target == null)
        {
            Debug.LogError($"Target object '{targetName}' not found.");
            return;
        }

        // Parse the alpha and duration values
        if (float.TryParse(parameters[1], out float startAlpha) &&
            float.TryParse(parameters[2], out float endAlpha) &&
            float.TryParse(parameters[3], out float duration))
        {
            // Clear previous references
            spriteRenderers.Clear();
            textMeshes.Clear();
            uiImages.Clear(); // Clear UI Images

            // Scan for all children containing SpriteRenderer, TextMeshProUGUI, or UI Image components
            ScanAllChildren(target);

            // Start the tweening coroutine
            StartCoroutine(TweenAlpha(startAlpha, endAlpha, duration));
        }
        else
        {
            Debug.LogError("Failed to parse startAlpha, endAlpha, or duration.");
        }
    }

    // Scans all children recursively for SpriteRenderer, TextMeshProUGUI, and UI Image components
    void ScanAllChildren(Transform parent)
    {
        foreach (SpriteRenderer sr in parent.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderers.Add(sr);
        }

        foreach (TextMeshProUGUI tmp in parent.GetComponentsInChildren<TextMeshProUGUI>())
        {
            textMeshes.Add(tmp);
        }

        foreach (Image img in parent.GetComponentsInChildren<Image>()) // Add Image components
        {
            uiImages.Add(img);
        }
    }

    // Coroutine for tweening alpha values
    IEnumerator TweenAlpha(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        // Loop over the tween duration
        while (elapsedTime < duration)
        {
            // Calculate the current alpha value based on elapsed time
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            // Update alpha for all SpriteRenderers
            foreach (var sr in spriteRenderers)
            {
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }

            // Update alpha for all TextMeshes
            foreach (var tmp in textMeshes)
            {
                Color color = tmp.color;
                color.a = alpha;
                tmp.color = color;
            }

            // Update alpha for all UI Images
            foreach (var img in uiImages)
            {
                Color color = img.color;
                color.a = alpha;
                img.color = color;
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure final alpha value is set
        SetFinalAlpha(endAlpha);
    }

    // Sets the final alpha value after the tween ends
    void SetFinalAlpha(float alpha)
    {
        // Set final alpha for SpriteRenderers
        foreach (var sr in spriteRenderers)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }

        // Set final alpha for TextMeshes
        foreach (var tmp in textMeshes)
        {
            Color color = tmp.color;
            color.a = alpha;
            tmp.color = color;
        }

        // Set final alpha for UI Images
        foreach (var img in uiImages)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }
    }
}
