using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Unity.Services.CloudSave.Models;

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager Instance { get; private set; }

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

    public async Task SavePlayerData(PlayerData playerData)
    {
        try
        {
            var data = new Dictionary<string, object>
        {
            { "walletAddress", playerData.walletAddress },
            { "Resources", JsonConvert.SerializeObject(playerData.resources) },
            { "Troops", JsonConvert.SerializeObject(playerData.troops) },
            { "CommanderIds", JsonConvert.SerializeObject(playerData.commanderIds) },
            { "CardIds", JsonConvert.SerializeObject(playerData.cardIds) },
            { "BuildingIds", JsonConvert.SerializeObject(playerData.buildingIds) },
            { "QuestIds", JsonConvert.SerializeObject(playerData.questIds) },
            { "PlayerState", JsonConvert.SerializeObject(playerData.playerState) },
            { "Time", JsonConvert.SerializeObject(playerData.time) },
            { "ActionPoints", playerData.actionPoints },
            {"occupation", playerData.occupation}
        };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("[CloudSaveManager] PlayerData saved successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[CloudSaveManager] Save failed: {ex.Message}");
        }
    }



    public async Task<PlayerData> LoadPlayerData(string walletAddress)
    {
        try
        {
            var keys = new HashSet<string>
        {
            "walletAddress", "Resources", "Troops", "CommanderIds", "CardIds",
            "BuildingIds", "QuestIds", "PlayerState", "Time", "ActionPoints"
        };

            var cloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            Debug.Log("[CloudSaveManager] Dumping raw Cloud Save data:");
            foreach (var kvp in cloudData)
            {
                Debug.Log($"Key: {kvp.Key} -> Value: {kvp.Value.Value}");
            }


            PlayerData playerData = new PlayerData(walletAddress);

            if (cloudData.TryGetValue("walletAddress", out var walletAddressData))
            {
                // Convert directly to string instead of using .ToString()
                playerData.walletAddress = walletAddressData.Value.GetAsString();

                string resourcesJson = cloudData["Resources"].Value.GetAsString();
                playerData.resources =
                    JsonConvert.DeserializeObject<Dictionary<string, int>>(resourcesJson);

                string troopsJson = cloudData["Troops"].Value.GetAsString();
                playerData.troops =
                    JsonConvert.DeserializeObject<Dictionary<string, int>>(troopsJson);

                string commanderJson = cloudData["CommanderIds"].Value.GetAsString();
                playerData.commanderIds =
                    JsonConvert.DeserializeObject<List<int>>(commanderJson);

                // ...and so on for each key you saved as JSON:
                string cardIdsJson = cloudData["CardIds"].Value.GetAsString();
                playerData.cardIds = JsonConvert.DeserializeObject<List<int>>(cardIdsJson);

                string buildingIdsJson = cloudData["BuildingIds"].Value.GetAsString();
                playerData.buildingIds = JsonConvert.DeserializeObject<List<int>>(buildingIdsJson);

                string questIdsJson = cloudData["QuestIds"].Value.GetAsString();
                playerData.questIds = JsonConvert.DeserializeObject<List<int>>(questIdsJson);

                string playerStateJson = cloudData["PlayerState"].Value.GetAsString();
                playerData.playerState = JsonConvert.DeserializeObject<PlayerState>(playerStateJson);

                string timeJson = cloudData["Time"].Value.GetAsString();
                playerData.time = JsonConvert.DeserializeObject<TimeData>(timeJson);

                playerData.actionPoints = int.Parse(
                    cloudData["ActionPoints"].Value.GetAsString()
                );

                // occupation is a simple string, so .ToString() is fine:
                if (cloudData.TryGetValue("occupation", out var occupationData))
                {
                    playerData.occupation = occupationData.Value.GetAsString();
                }
            }
            else
            {
                Debug.LogWarning("[CloudSaveManager] No PlayerData found. Creating new PlayerData...");
                await SavePlayerData(playerData);  // Save the newly created PlayerData
            }

            Debug.Log("[CloudSaveManager] PlayerData loaded successfully.");
            return playerData;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[CloudSaveManager] Load failed: {ex.Message}");
            return null;  // Return null to signal failure
        }
    }
}
