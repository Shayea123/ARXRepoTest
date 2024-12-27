using UnityEngine;
using System.Collections.Generic;

public class QuestDatabase : MonoBehaviour
{
    [SerializeField] private List<QuestData> quests;  // Assign in the Inspector
    private Dictionary<int, QuestData> questDict;

    private void Awake()
    {
        questDict = new Dictionary<int, QuestData>();
        foreach (var quest in quests)
        {
            questDict[quest.id] = quest;
        }
    }

    public QuestData GetQuestById(int id)
    {
        return questDict.ContainsKey(id) ? questDict[id] : null;
    }

    public List<QuestData> GetAllQuests()
    {
        return new List<QuestData>(quests);
    }
}
