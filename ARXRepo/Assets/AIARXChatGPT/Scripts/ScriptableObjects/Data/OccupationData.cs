using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewOccupation", menuName = "Game/Occupation")]
public class OccupationData : ScriptableObject
{
    public string occupationName;        // Name of the occupation (e.g., Farmer)
    public Sprite icon;                  // Icon for the occupation
    public Dictionary<string, int> resourceBonuses;  // Resource bonuses (e.g., { "Food", 30 })
}
