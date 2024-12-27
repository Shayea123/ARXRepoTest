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

    public async Task SaveProgress(PlayerData playerData)
    {
        try
        {
            Debug.Log("[SaveProgress] Saving PlayerData...");
            Debug.Log($"[SaveProgress] PlayerData: {Newtonsoft.Json.JsonConvert.SerializeObject(playerData)}");

            var data = new Dictionary<string, object>
        {
            { "walletAddress", playerData.walletAddress },
            { "resources", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.resources) },
            { "troops", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.troops) },
            { "commanderIds", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.commanderIds) },
            { "cardIds", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.cardIds) },
            { "buildingIds", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.buildingIds) },
            { "questIds", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.questIds) },
            { "playerState", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.playerState) },
            { "time", Newtonsoft.Json.JsonConvert.SerializeObject(playerData.time) },
            { "actionPoints", playerData.actionPoints },
            { "occupation", playerData.occupation }
        };

            Debug.Log($"[SaveProgress] Data to save: {Newtonsoft.Json.JsonConvert.SerializeObject(data)}");

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("[SaveProgress] PlayerData saved successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveProgress] Failed to save PlayerData: {ex.Message}");
        }
    }


    public async Task<PlayerData> LoadProgress(string walletAddress)
    {
        try
        {
            Debug.Log($"[LoadProgress] Starting load for Wallet: {walletAddress}");

            var keys = new HashSet<string>
        {
            "walletAddress", "resources", "troops", "commanderIds", "cardIds",
            "buildingIds", "questIds", "playerState", "time", "actionPoints", "occupation"
        };

            var cloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
            PlayerData playerData = new PlayerData(walletAddress);

            // Load each key and handle defaults
            playerData.walletAddress = cloudData.TryGetValue("walletAddress", out var walletAddressData)
                ? walletAddressData.Value.ToString()
                : walletAddress;

            playerData.resources = ParseJsonOrDefault(cloudData, "resources", new Dictionary<string, int>());
            playerData.troops = ParseJsonOrDefault(cloudData, "troops", new Dictionary<string, int>());
            playerData.commanderIds = ParseJsonOrDefault(cloudData, "commanderIds", new List<int>());
            playerData.cardIds = ParseJsonOrDefault(cloudData, "cardIds", new List<int>());
            playerData.buildingIds = ParseJsonOrDefault(cloudData, "buildingIds", new List<int>());
            playerData.questIds = ParseJsonOrDefault(cloudData, "questIds", new List<int>());
            playerData.playerState = ParseJsonOrDefault(cloudData, "playerState", new PlayerState { rank = "Freeman", socialLevel = 1, militaryLevel = 1 });
            playerData.time = ParseJsonOrDefault(cloudData, "time", new TimeData { day = 1, month = 1, year = 1 });
            playerData.actionPoints = cloudData.TryGetValue("actionPoints", out var actionPointsData)
                ? int.Parse(actionPointsData.Value.ToString())
                : 3;

            playerData.occupation = cloudData.TryGetValue("occupation", out var occupationData)
                ? occupationData.Value.ToString()
                : "None"; // Default to "None" if not found

            Debug.Log("[LoadProgress] PlayerData loaded successfully.");
            return playerData;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LoadProgress] Failed to load PlayerData: {ex.Message}");
            return new PlayerData(walletAddress); // Fallback to default data
        }
    }

    private T ParseJsonOrDefault<T>(Dictionary<string, Item> cloudData, string key, T defaultValue)
    {
        if (cloudData.TryGetValue(key, out var jsonData))
        {
            try
            {
                Debug.Log($"[ParseJsonOrDefault] Found key '{key}': {jsonData.Value}");
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData.Value.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ParseJsonOrDefault] Failed to parse key '{key}': {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"[ParseJsonOrDefault] Key '{key}' not found; using default value.");
        }

        return defaultValue;
    }




    private Dictionary<string, int> LoadDictionary(Dictionary<string, Item> cloudData, string key, Dictionary<string, int> defaultValue)
    {
        if (cloudData.TryGetValue(key, out var jsonData))
        {
            try
            {
                var loadedData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData.Value.ToString());

                // Ensure all default keys are present
                foreach (var defaultKey in defaultValue.Keys)
                {
                    if (!loadedData.ContainsKey(defaultKey))
                    {
                        loadedData[defaultKey] = defaultValue[defaultKey];
                    }
                }

                return loadedData;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[LoadDictionary] Failed to parse key '{key}': {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"[LoadDictionary] Key '{key}' not found; using default value.");
        }

        return defaultValue;
    }


    private List<int> LoadList(Dictionary<string, Item> cloudData, string key, List<int> defaultValue)
    {
        if (cloudData.TryGetValue(key, out var jsonData))
        {
            try
            {
                return JsonUtility.FromJson<List<int>>(jsonData.Value.ToString());
            }
            catch
            {
                Debug.LogWarning($"[LoadList] Failed to parse key '{key}'; using default value.");
            }
        }
        else
        {
            Debug.LogWarning($"[LoadList] Missing key '{key}'; using default value.");
        }

        return defaultValue;
    }


}
