using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Thirdweb.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Utilities;

public class WalletGameManager : MonoBehaviour
{
    public static WalletGameManager Instance { get; private set; } // Singleton instance
    public static event System.Action OnDataLoaded; // Event fired when data is loaded
    [SerializeField] private bool Indeveloping = true;
    public bool DataLoaded { get; private set; } = false;

    private ThirdwebManager sdk;
    private string walletAddress = string.Empty;   // Store the wallet address
    private PlayerData playerData;

    [SerializeField] private TMPro.TMP_Text walletAddressText; // optional display

    private async void Awake()
    {
        // Implement Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize Unity Services
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        // Initialize Thirdweb SDK and load progress
        var activeWallet = ThirdwebManager.Instance.GetActiveWallet();

        if (Indeveloping == false)
        {
            walletAddress = await activeWallet.GetAddress();
            walletAddressText.text = walletAddress;

        }
        else
        {
            // In developing mode use a dummy wallet address
            walletAddress = "0x00000000";
            walletAddressText.text = walletAddress;
        }

        if (walletAddressText != null)
        {
            walletAddressText.text = $"{walletAddress}";
        }

        await LoadProgress(walletAddress); // Load PlayerData on initialization

        // Once loading is done, invoke the event
        OnDataLoaded?.Invoke();
        DataLoaded = true;
    }

    public async Task LoadProgress(string walletAddress)
    {
        try
        {
            Debug.Log($"[LoadProgress] Starting load for Wallet: {walletAddress}");

            var keys = new HashSet<string> { "walletAddress", "PlayerData" };
            Dictionary<string, Item> data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            // Log keys and values for debugging
            Debug.Log("[LoadProgress] Keys and Values from Cloud Save:");
            foreach (var kvp in data)
            {
                Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value.Value.GetAsString()}");
            }

            // Verify if walletAddress exists in saved data
            if (data.TryGetValue("walletAddress", out Item walletItem))
            {
                string savedWallet = walletItem.Value.GetAsString();
                Debug.Log($"[LoadProgress] Saved Wallet: {savedWallet}, Current Wallet: {walletAddress}");

                if (savedWallet == walletAddress)
                {
                    // Load PlayerData if it exists
                    if (data.TryGetValue("PlayerData", out Item playerDataItem))
                    {
                        playerData = JsonUtility.FromJson<PlayerData>(playerDataItem.Value.GetAsString());
                        Debug.Log("[LoadProgress] PlayerData successfully loaded!");
                    }
                    else
                    {
                        Debug.Log("[LoadProgress] No 'PlayerData' key found; initializing new PlayerData.");
                        playerData = new PlayerData(walletAddress);
                        await SaveProgress();
                    }
                }
                else
                {
                    Debug.Log("[LoadProgress] Wallet mismatch; initializing new PlayerData.");
                    playerData = new PlayerData(walletAddress);
                    await SaveProgress();
                }
            }
            else
            {
                Debug.Log("[LoadProgress] No wallet data found; initializing new PlayerData.");
                playerData = new PlayerData(walletAddress);
                await SaveProgress();
            }

            Debug.Log($"[LoadProgress] Final PlayerData: Coins={playerData.coins}, Food={playerData.food}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LoadProgress] Failed to load PlayerData: {ex.Message}");
            playerData = new PlayerData(walletAddress); // Fallback to default data
        }
    }

    public async Task SaveProgress()
    {
        try
        {
            Debug.Log("[SaveProgress] Starting save for PlayerData...");

            var data = new Dictionary<string, object>
            {
                { "walletAddress", walletAddress },
                { "PlayerData", JsonUtility.ToJson(playerData) }
            };

            Debug.Log("[SaveProgress] Data to save:");
            foreach (var kvp in data)
            {
                Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
            }

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);

            Debug.Log("[SaveProgress] PlayerData saved successfully!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveProgress] Failed to save PlayerData: {ex.Message}");
        }
    }

    public async void ManualSave()
    {
        await SaveProgress(); // Now the warning is gone because we are awaiting the call
    }

    public async void ManualLoad()
    {
        await LoadProgress(walletAddress); // Trigger manual load via a button
    }

    // Getters for other scripts to access
    public string GetWalletAddress()
    {
        return walletAddress;
    }

    public bool GetIndeveloping()
    {
        return Indeveloping;
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    // Methods to modify PlayerData
    public void AddCoins(int amount)
    {
        playerData.coins += amount;
        Debug.Log($"[WalletGameManager] Coins updated to {playerData.coins}");
        SaveProgress().Forget();
    }

    public void RecruitInfantry(int count)
    {
        int cost = count * 10;
        if (playerData.coins >= cost)
        {
            playerData.infantry += count;
            playerData.coins -= cost;
            Debug.Log($"[WalletGameManager] Recruited {count} infantry. Remaining coins: {playerData.coins}");

            ManualSave();
            HUDController.Instance.UpdateHUD(); // Update HUD
        }
        else
        {
            Debug.LogWarning("[WalletGameManager] Not enough coins to recruit infantry!");
        }
    }

    public int GetMaxActionPoints(string rank)
    {
        switch (rank)
        {
            case "Freeman": return 3;
            case "Soldier": return 5;
            case "Sergeant": return 7;
            case "Captain": return 10;
            case "Major": return 12;
            case "Colonel": return 15;
            case "General": return 20;
            case "Count": return 25;
            case "Earl": return 30;
            case "Duke": return 35;
            case "King/Queen": return 40;
            case "Emperor": return 50;
            default: return 3;
        }
    }

    public void VerifyTimeOnLoad()
    {
        var playerData = GetPlayerData();

        // Simulate restoring time system (replace with real date logic if needed)
        if (playerData.actionPoints == 0)
        {
            TimeSystem.Instance.StartNewDay(); // Start a new day if needed

            if (CanRankUp())
            {
                HUDController.Instance.ShowRankUpNotification();
            }
        }
        Debug.Log($"[WalletGameManager] Verified Time: {playerData.day}/{playerData.month}/{playerData.year}");
    }

    public bool CanRankUp()
    {
        var playerData = GetPlayerData();

        switch (playerData.rank)
        {
            case "Freeman":
                return playerData.coins >= 500 && playerData.influence >= 25;
            case "Soldier":
                return playerData.coins >= 1500 && playerData.influence >= 50;
            case "Sergeant":
                return playerData.coins >= 5000 && playerData.influence >= 100;
            case "Captain":
                return playerData.coins >= 10000 && playerData.influence >= 150;
            case "Major":
                return playerData.coins >= 20000 && playerData.influence >= 200;
            case "Colonel":
                return playerData.coins >= 40000 && playerData.influence >= 300;
            case "General":
                return playerData.coins >= 70000 && playerData.influence >= 400;
            case "Count":
                return playerData.coins >= 100000 && playerData.influence >= 500;
            case "Earl":
                return playerData.coins >= 150000 && playerData.influence >= 600;
            case "Duke":
                return playerData.coins >= 200000 && playerData.influence >= 800;
            case "King/Queen":
                return playerData.coins >= 300000 && playerData.influence >= 1000;
            case "Emperor":
                return playerData.coins >= 500000 && playerData.influence >= 1500;
            default:
                return false;
        }
    }

    public void RankUp()
    {
        var playerData = GetPlayerData();

        if (CanRankUp())
        {
            switch (playerData.rank)
            {
                case "Freeman":
                    playerData.rank = "Soldier";
                    break;
                case "Soldier":
                    playerData.rank = "Sergeant";
                    break;

                    // Add other rank upgrades...
            }

            playerData.actionPoints = GetMaxActionPoints(playerData.rank); // Refresh AP
            ManualSave();
            HUDController.Instance.UpdateHUD();
            Debug.Log($"[WalletGameManager] Ranked up to {playerData.rank}");
        }
    }


}

// Optional TaskExtensions if needed, placed in a Utilities namespace or just once in your project
public static class TaskExtensions
{
    public static void Forget(this Task task) { /* Do nothing */ }
}
