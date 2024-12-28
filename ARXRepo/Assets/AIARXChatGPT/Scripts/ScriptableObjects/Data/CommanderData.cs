using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCommander", menuName = "Game/Commander")]
public class CommanderData : ScriptableObject
{
    public int id;                      // Unique ID for the commander
    public string name;                 // Name of the commander
    public string specialty;            // Specialty (e.g., Archer, Infantry)
    public Sprite portrait;             // Portrait of the commander
    public List<Bonus> bonuses;         // List of bonuses provided by the commander
}

[System.Serializable]
public class Bonus
{
    public string bonusName;  // Name of the bonus (e.g., "Attack Boost")
    public int bonusValue;    // Value of the bonus (e.g., 10)
}
