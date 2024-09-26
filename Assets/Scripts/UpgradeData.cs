using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Game Data/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public List<int> requiredGold = new List<int>();
    public List<int> requiredCoffee = new List<int>();
}
