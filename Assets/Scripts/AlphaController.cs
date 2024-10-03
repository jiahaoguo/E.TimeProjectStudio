using UnityEngine;
using UnityEngine.UI; // Add for UI components
using TMPro;
using UnityEngine.Tilemaps; // Add for Tilemaps
using System.Collections;
using System.Collections.Generic;

public class AlphaController : MonoBehaviour
{
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
            // Start the tweening coroutine for the target object
            StartCoroutine(TweenAlpha(target, startAlpha, endAlpha, duration));
        }
        else
        {
            Debug.LogError("Failed to parse startAlpha, endAlpha, or duration.");
        }
    }

    // Coroutine for tweening alpha values on a specific target
    IEnumerator TweenAlpha(Transform target, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        // Gather SpriteRenderers, TextMeshes, UI Images, and Tilemaps for this specific target
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        List<TextMeshProUGUI> textMeshes = new List<TextMeshProUGUI>();
        List<Image> uiImages = new List<Image>();
        List<Tilemap> tilemaps = new List<Tilemap>(); // Add Tilemap list

        // Scan for all children containing SpriteRenderer, TextMeshProUGUI, UI Image, or Tilemap components
        ScanAllChildren(target, spriteRenderers, textMeshes, uiImages, tilemaps);

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

            // Update alpha for all Tilemaps
            foreach (var tm in tilemaps)
            {
                Color color = tm.color;
                color.a = alpha;
                tm.color = color;
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure final alpha value is set
        SetFinalAlpha(spriteRenderers, textMeshes, uiImages, tilemaps, endAlpha);
    }

    // Scans all children recursively for SpriteRenderer, TextMeshProUGUI, UI Image, and Tilemap components
    void ScanAllChildren(Transform parent, List<SpriteRenderer> spriteRenderers, List<TextMeshProUGUI> textMeshes, List<Image> uiImages, List<Tilemap> tilemaps)
    {
        foreach (SpriteRenderer sr in parent.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderers.Add(sr);
        }

        foreach (TextMeshProUGUI tmp in parent.GetComponentsInChildren<TextMeshProUGUI>())
        {
            textMeshes.Add(tmp);
        }

        foreach (Image img in parent.GetComponentsInChildren<Image>())
        {
            uiImages.Add(img);
        }

        foreach (Tilemap tm in parent.GetComponentsInChildren<Tilemap>())
        {
            tilemaps.Add(tm);
        }
    }

    // Sets the final alpha value after the tween ends
    void SetFinalAlpha(List<SpriteRenderer> spriteRenderers, List<TextMeshProUGUI> textMeshes, List<Image> uiImages, List<Tilemap> tilemaps, float alpha)
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

        // Set final alpha for Tilemaps
        foreach (var tm in tilemaps)
        {
            Color color = tm.color;
            color.a = alpha;
            tm.color = color;
        }
    }
}
