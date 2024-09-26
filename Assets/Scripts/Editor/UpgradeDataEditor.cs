using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpgradeData))]
public class UpgradeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UpgradeData upgradeData = (UpgradeData)target;

        // Ensure both lists are the same size
        if (upgradeData.requiredGold.Count != upgradeData.requiredCoffee.Count)
        {
            int newSize = Mathf.Max(upgradeData.requiredGold.Count, upgradeData.requiredCoffee.Count);
            while (upgradeData.requiredGold.Count < newSize)
                upgradeData.requiredGold.Add(0);
            while (upgradeData.requiredCoffee.Count < newSize)
                upgradeData.requiredCoffee.Add(0);
        }

        // Display header with labels
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Level", GUILayout.MaxWidth(100));
        GUILayout.Label("Required Gold", GUILayout.MaxWidth(100));
        GUILayout.Label("Required Coffee", GUILayout.MaxWidth(100));
        EditorGUILayout.EndHorizontal();

        // Loop through and display each entry
        for (int i = 0; i < upgradeData.requiredGold.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Display level index (e.g., "1 to 2", "2 to 3")
            GUILayout.Label($"{i + 1} to {i + 2}", GUILayout.MaxWidth(100));

            // Display input fields for required gold and coffee
            upgradeData.requiredGold[i] = EditorGUILayout.IntField(upgradeData.requiredGold[i], GUILayout.MaxWidth(100));
            upgradeData.requiredCoffee[i] = EditorGUILayout.IntField(upgradeData.requiredCoffee[i], GUILayout.MaxWidth(100));

            EditorGUILayout.EndHorizontal();
        }

        // Buttons for adding/removing upgrade levels
        if (GUILayout.Button("Add Upgrade Level"))
        {
            upgradeData.requiredGold.Add(0);
            upgradeData.requiredCoffee.Add(0);
        }

        if (GUILayout.Button("Remove Upgrade Level"))
        {
            if (upgradeData.requiredGold.Count > 0)
            {
                upgradeData.requiredGold.RemoveAt(upgradeData.requiredGold.Count - 1);
                upgradeData.requiredCoffee.RemoveAt(upgradeData.requiredCoffee.Count - 1);
            }
        }

        EditorUtility.SetDirty(upgradeData); // Mark the object as dirty to save changes
    }
}
