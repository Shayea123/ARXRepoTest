using UnityEngine;
using System.Collections.Generic;

public class CardDatabase : MonoBehaviour
{
    [SerializeField] private List<CardData> cards;  // Assign in the Inspector
    private Dictionary<int, CardData> cardDict;

    private void Awake()
    {
        cardDict = new Dictionary<int, CardData>();
        foreach (var card in cards)
        {
            cardDict[card.id] = card;
        }
    }

    public CardData GetCardById(int id)
    {
        return cardDict.ContainsKey(id) ? cardDict[id] : null;
    }

    public List<CardData> GetAllCards()
    {
        return new List<CardData>(cards);
    }
}
