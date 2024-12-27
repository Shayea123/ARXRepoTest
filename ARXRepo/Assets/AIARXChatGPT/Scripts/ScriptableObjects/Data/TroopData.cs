using UnityEngine;

[CreateAssetMenu(fileName = "NewTroop", menuName = "Game/Troop")]
public class TroopData : ScriptableObject
{
    public int id;                      // Unique ID for the troop
    public string name;                 // Name of the troop (e.g., Archer)
    public int attackPower;             // Attack power
    public int defensePower;            // Defense power
    public int cost;                    // Recruitment cost
    public Sprite icon;                 // Icon representing the troop
}
