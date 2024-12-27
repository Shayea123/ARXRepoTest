using UnityEngine;
using System.Collections.Generic;

public class TroopDatabase : MonoBehaviour
{
    [SerializeField] private List<TroopData> troops;  // Assign in the Inspector
    private Dictionary<int, TroopData> troopDict;

    private void Awake()
    {
        troopDict = new Dictionary<int, TroopData>();
        foreach (var troop in troops)
        {
            troopDict[troop.id] = troop;
        }
    }

    public TroopData GetTroopById(int id)
    {
        return troopDict.ContainsKey(id) ? troopDict[id] : null;
    }

    public List<TroopData> GetAllTroops()
    {
        return new List<TroopData>(troops);
    }
}
