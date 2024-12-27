using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCard", menuName = "Game/Card")]
public class CardData : ScriptableObject
{
    public int id;                      // Unique ID for the card
    public string name;                 // Name of the card
    public string description;          // Description of its effects
    public Sprite icon;                 // Icon representing the card
    public Dictionary<string, int> effects;  // Effects of the card (e.g., { "Coins", 50 })
}
