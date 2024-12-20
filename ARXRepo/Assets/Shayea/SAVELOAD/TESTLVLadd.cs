using System;
using UnityEngine;
using UnityEngine.UI;

public class TESTLVLadd : MonoBehaviour
{
    /*
    [SerializeField] private TMPro.TMP_Text lvlbefore; // Assign this in the Inspector
    [SerializeField] private TMPro.TMP_Text lvlafter;  // Assign this in the Inspector
    [SerializeField] private Button lvlup;             // Assign this in the Inspector

    private string walletAddress = string.Empty;       // Store the wallet address


    private void OnEnable()
    {
        // Subscribe to the OnDataLoaded event when the script is enabled
        WalletGameManager.OnDataLoaded += OnDataLoadedHandler;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        WalletGameManager.OnDataLoaded -= OnDataLoadedHandler;
    }

    private void Start()
    {
        Debug.Log("[TESTLVLadd] Start called. Waiting for data load event.");

        // Add a listener to the button to call AddLevel when clicked
        if (lvlup != null)
        {
            lvlup.onClick.AddListener(AddLevel);
            Debug.Log("[TESTLVLadd] lvlup button listener added.");
        }
        else
        {
            Debug.LogWarning("[TESTLVLadd] lvlup button is not assigned in the inspector.");
        }
    }

    private void OnDataLoadedHandler()
    {
        Debug.Log("[TESTLVLadd] OnDataLoaded event received.");

        if (WalletGameManager.Instance == null)
        {
            Debug.LogError("[TESTLVLadd] WalletGameManager instance is null. Cannot retrieve data.");
            return;
        }

        //int currentLevel = WalletGameManager.Instance.GetLevel();
        //lvlbefore.text = currentLevel.ToString();
        //lvlafter.text = currentLevel.ToString();
        walletAddress = WalletGameManager.Instance.GetWalletAddress();

        Debug.Log($"[TESTLVLadd] Data loaded event handled. Current level: {currentLevel}, Wallet: {walletAddress}");
    }

    public void AddLevel()
    {
        if (WalletGameManager.Instance == null)
        {
            Debug.LogError("WalletGameManager instance is null. Cannot add level.");
            return;
        }

        // Get the current loaded level from the WalletGameManager
       // int currentLevel = WalletGameManager.Instance.GetLevel();

        // Increment it
        currentLevel++;

        // Update the display text to the new level
        lvlafter.text = currentLevel.ToString();

        Debug.Log($"[TESTLVLadd] Level increased to {currentLevel} (not saved yet).");
    }

    public void Save()
    {
        Debug.Log("[TESTLVLadd] Save button pressed.");
        if (WalletGameManager.Instance == null)
        {
            Debug.LogError("[TESTLVLadd] WalletGameManager instance is null. Cannot save data.");
            return;
        }

        if (lvlafter != null && int.TryParse(lvlafter.text, out int newLevel))
        {
           // WalletGameManager.Instance.SaveProgress(newLevel, WalletGameManager.Instance.Getscore());
            Debug.Log("[TESTLVLadd] Level saved successfully.");
        }
        else
        {
            Debug.LogError("[TESTLVLadd] lvlafter is null or not a valid number. Cannot save level.");
        }
    }

    public async void LoadLevel()
    {
        Debug.Log("[TESTLVLadd] Load button pressed.");
        if (WalletGameManager.Instance == null)
        {
            Debug.LogError("[TESTLVLadd] WalletGameManager instance is null. Cannot load data.");
            return;
        }

        await WalletGameManager.Instance.LoadProgress(WalletGameManager.Instance.GetWalletAddress());
        Debug.Log("[TESTLVLadd] Level loaded from Cloud Save.");

        //lvlbefore.text = WalletGameManager.Instance.GetLevel().ToString();
        Debug.Log($"[TESTLVLadd] Loaded level: {lvlbefore.text}");
    }
    */
}
