using UnityEngine;
using System.Collections.Generic;

public class BuildingDatabase : MonoBehaviour
{
    [SerializeField] private List<BuildingData> buildings;  // Assign in the Inspector
    private Dictionary<int, BuildingData> buildingDict;

    private void Awake()
    {
        buildingDict = new Dictionary<int, BuildingData>();
        foreach (var building in buildings)
        {
            buildingDict[building.id] = building;
        }
    }

    public BuildingData GetBuildingById(int id)
    {
        return buildingDict.ContainsKey(id) ? buildingDict[id] : null;
    }

    public List<BuildingData> GetAllBuildings()
    {
        return new List<BuildingData>(buildings);
    }
}
