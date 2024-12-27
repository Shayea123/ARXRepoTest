using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddResource(PlayerData playerData, string resourceName, int amount)
    {
        if (playerData.resources.ContainsKey(resourceName))
        {
            playerData.resources[resourceName] += amount;
            Debug.Log($"[ResourceManager] Added {amount} to {resourceName}. Total: {playerData.resources[resourceName]}");
        }
    }

    public void UseActionPoint(PlayerData playerData, int cost)
    {
        if (playerData.actionPoints >= cost)
        {
            playerData.actionPoints -= cost;
            Debug.Log($"[ResourceManager] Used {cost} Action Points. Remaining: {playerData.actionPoints}");
        }
        else
        {
            Debug.LogWarning("[ResourceManager] Not enough Action Points!");
        }
    }
}
