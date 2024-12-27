using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCommander", menuName = "Game/Commander")]
public class CommanderData : ScriptableObject
{
    public int id;                      // Unique ID for the commander
    public string name;                 // Name of the commander
    public string specialty;            // Specialty (e.g., Archer, Infantry)
    public Sprite portrait;             // Portrait of the commander
    public Dictionary<string, int> bonuses;  // Bonuses provided by the commander
}
