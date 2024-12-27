using UnityEngine;
using System.Collections.Generic;

public class OccupationDatabase : MonoBehaviour
{
    [SerializeField] private List<OccupationData> occupations;  // Assign in the Inspector
    private Dictionary<string, OccupationData> occupationDict;

    private void Awake()
    {
        occupationDict = new Dictionary<string, OccupationData>();
        foreach (var occupation in occupations)
        {
            occupationDict[occupation.occupationName] = occupation;
        }
    }

    public OccupationData GetOccupationByName(string name)
    {
        return occupationDict.ContainsKey(name) ? occupationDict[name] : null;
    }

    public List<OccupationData> GetAllOccupations()
    {
        return new List<OccupationData>(occupations);
    }
}
