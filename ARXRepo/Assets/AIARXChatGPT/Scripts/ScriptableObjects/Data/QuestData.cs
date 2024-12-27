using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Game/Quest")]
public class QuestData : ScriptableObject
{
    public int id;                      // Unique ID for the quest
    public string name;                 // Name of the quest
    public string description;          // Description of the quest
    public Dictionary<string, int> objectives; // Objectives (e.g., { "CollectFood", 100 })
    public Dictionary<string, int> rewards;    // Rewards upon completion (e.g., { "Coins", 50 })
}
