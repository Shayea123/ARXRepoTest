using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Thirdweb.Unity;
using System;
using System.Threading.Tasks;

public class WalletGameManager : MonoBehaviour
{
    public static WalletGameManager Instance { get; private set; }
    public static event Action OnDataLoaded;  // Event fired when PlayerData is loaded

    [SerializeField] private bool inDevelopmentMode = true;  // Use demo wallet address in dev mode
    public bool DataLoaded { get; private set; } = false;

    private ThirdwebManager sdk;
    private string walletAddress = string.Empty;
    private PlayerData playerData;

    [SerializeField] private TMPro.TMP_Text walletAddressText;  // Optional display for wallet

    private async void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure other scripts wait for wallet initialization
        await InitializeWallet();
    }

    private async Task InitializeWallet()
    {
        try
        {
            // Initialize Unity Services
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                // No user signed in yet, so do an anonymous sign?in.
                Debug.Log("User not signed in. Attempting anonymous sign?in.");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            else
            {
                // The user is “signed in” but we need to check if the token is valid or not.
                if (!AuthenticationService.Instance.IsAuthorized)
                {
                    // If they have a session token but it’s invalid or expired,
                    // we can clear the old token and re-sign in.
                    Debug.Log("Session token invalid—re?signing in.");
                    AuthenticationService.Instance.ClearSessionToken();
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
                else
                {
                    Debug.Log("User is already signed in with a valid session token.");
                }
            }

            // Initialize Thirdweb SDK and fetch wallet address
            try
            {
                var activeWallet = ThirdwebManager.Instance.GetActiveWallet();
                walletAddress = !inDevelopmentMode
                    ? await activeWallet.GetAddress()
                    : "0x00000000"; // Demo wallet address for development

                Debug.Log($"[InitializeWallet] Wallet Address: {walletAddress}");

                if (walletAddressText != null)
                {
                    walletAddressText.text = walletAddress;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[InitializeWallet] Failed to initialize Thirdweb SDK or fetch wallet address: {ex.Message}");
                walletAddress = "0x00000000"; // Fallback to demo wallet address
            }

            // Load PlayerData
            try
            {
                Debug.Log("[InitializeWallet] Loading PlayerData...");
                playerData = await CloudSaveManager.Instance.LoadPlayerData(walletAddress);
                Debug.Log("[InitializeWallet] PlayerData loaded successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[InitializeWallet] Failed to load PlayerData: {ex.Message}");
                playerData = new PlayerData(walletAddress); // Create default PlayerData
            }

            DataLoaded = true;
            OnDataLoaded?.Invoke(); // Notify other scripts that PlayerData is ready
        }
        catch (Exception ex)
        {
            Debug.LogError($"[InitializeWallet] Critical error during initialization: {ex.Message}");
            throw; // Re-throw exception if needed for further handling
        }
    }


    public PlayerData GetPlayerData() => playerData;

    public async void SavePlayerData()
    {
        await CloudSaveManager.Instance.SavePlayerData(playerData);
    }

    public string GetWalletAddress() => walletAddress;

    public bool IsInDevelopmentMode() => inDevelopmentMode;
}
