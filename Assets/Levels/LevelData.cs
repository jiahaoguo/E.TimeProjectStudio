using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/New Level")]
public class LevelData : ScriptableObject
{
    public GameObject levelPrefab;
    public string levelName;
}