using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Game/Building")]
public class BuildingData : ScriptableObject
{
    public int id;                      // Unique ID for the building
    public string name;                 // Name of the building
    public int maxLevel;                // Maximum upgrade level
    public Sprite icon;                 // Icon representing the building
    public Dictionary<int, Dictionary<string, int>> productionByLevel; // Production values at each level
}
