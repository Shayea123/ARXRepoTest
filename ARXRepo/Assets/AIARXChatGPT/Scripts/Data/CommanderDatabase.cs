using UnityEngine;
using System.Collections.Generic;

public class CommanderDatabase : MonoBehaviour
{
    [SerializeField] private List<CommanderData> commanders;  // Assign in the Inspector
    private Dictionary<int, CommanderData> commanderDict;

    private void Awake()
    {
        commanderDict = new Dictionary<int, CommanderData>();
        foreach (var commander in commanders)
        {
            commanderDict[commander.id] = commander;
        }
    }

    public CommanderData GetCommanderById(int id)
    {
        return commanderDict.ContainsKey(id) ? commanderDict[id] : null;
    }

    public List<CommanderData> GetAllCommanders()
    {
        return new List<CommanderData>(commanders);
    }
}
