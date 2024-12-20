using Lean.Gui;
using Newtonsoft.Json;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class TelegramIAWCustomAuth : MonoBehaviour
{
    [field: SerializeField] private string EncryptionKey;
    [field: SerializeField] private TMP_Text LogText;
    [field: SerializeField] private TMP_Text WalletAdresBtnLog;
    [SerializeField] private LeanButton StartGameButton; // Assign in Inspector

    private string LeanButtonText;
    private string walletAddress;

    private bool walletConnected = false;
    private bool dataReady = false;


    private void OnDisable()
    {
        WalletGameManager.OnDataLoaded -= OnDataLoadedHandler;
    }

    private void Start()
    {



        /*
        if (WalletAdresBtnLog != null)
        {
            LeanButtonText = WalletAdresBtnLog.text;
        }

         * Initially disable the start button until conditions are met
        if (StartGameButton != null)
        {
            StartGameButton.interactable = false;
            Debug.Log("[TelegramIAWCustomAuth] StartGameButton initially disabled.");
        }
        else
        {
            Debug.LogError("[TelegramIAWCustomAuth] StartGameButton not assigned in the Inspector.");
        }*/

        Log("Waiting for payload...");

        var url = Application.absoluteURL;
        var uri = new System.Uri(url);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var payload = query.Get("payload");
        var payloadDecoded = System.Web.HttpUtility.UrlDecode(payload);
        var payloadObject = JsonConvert.DeserializeObject<Payload>(payloadDecoded);

        Log($"Payload: {JsonConvert.SerializeObject(payloadObject)}");
        ProcessPayload(payloadObject);

        // Add a fallback check every few seconds
        InvokeRepeating(nameof(FallbackCheck), 3f, 2f);
    }

    private async void ProcessPayload(Payload payload)
    {
        if (WalletAdresBtnLog != null)
        {
            WalletAdresBtnLog.text = "CONNECTING...";
        }

        var connection = new WalletOptions(
            provider: WalletProvider.InAppWallet,
            chainId: 421614,
            inAppWalletOptions: new InAppWalletOptions(
                authprovider: AuthProvider.AuthEndpoint,
                jwtOrPayload: JsonConvert.SerializeObject(payload),
                encryptionKey: EncryptionKey
            ),
            smartWalletOptions: new SmartWalletOptions(sponsorGas: true)
        );

        Log("Attempting wallet connection...");

        try
        {
            var smartWallet = await ThirdwebManager.Instance.ConnectWallet(connection);
            walletAddress = await smartWallet.GetAddress();
            Log($"Connected to Wallet: {walletAddress}");

            if (WalletAdresBtnLog != null)
            {
                WalletAdresBtnLog.text = $"{walletAddress[..4]}...{walletAddress[^4..]}";
            }

            walletConnected = true;
            CheckConditions();
        }
        catch (System.Exception e)
        {
            Log($"Error: {e.Message}");
        }
    }

    private void OnDataLoadedHandler()
    {
        dataReady = true;
        Debug.Log("[TelegramIAWCustomAuth] Data loaded event received.");
        CheckConditions();
    }

    public void CheckConditions()
    {
        // Subscribe to OnDataLoaded event if WalletGameManager is available
        if (WalletGameManager.Instance != null)
        {
            WalletGameManager.OnDataLoaded += OnDataLoadedHandler;
        }
        else
        {
            WalletGameManager.OnDataLoaded -= OnDataLoadedHandler;
            Debug.LogError("[TelegramIAWCustomAuth] WalletGameManager instance is null, cannot check indeveloping.");
            return;
        }
        // If Indeveloping is true, enable the button immediately
        if (WalletGameManager.Instance.GetIndeveloping())
        {
            Debug.Log("[TelegramIAWCustomAuth] Indeveloping is true, enabling StartGameButton immediately.");
            SetStartButtonInteractable(true);
            StartGameButton.GetComponent<MMF_Player>().PlayFeedbacks();
            WalletAdresBtnLog.text = "0x000" ;
            return;
        }

        // Check if the wallet address is not null or empty. If true.
        if (!string.IsNullOrEmpty(walletAddress))
        {
            Debug.Log("[TelegramIAWCustomAuth] Both conditions met (wallet + data), enabling StartGameButton.");
            SetStartButtonInteractable(true);
            StartGameButton.GetComponent<MMF_Player>().PlayFeedbacks();
            return;
        }
        else
        {
            Debug.Log("[TelegramIAWCustomAuth] Conditions not met yet. walletConnected: " + walletConnected + ", dataReady: " + dataReady);
        }
    }

    private void SetStartButtonInteractable(bool interactable)
    {
        if (StartGameButton == null)
        {
            Debug.LogWarning("[TelegramIAWCustomAuth] StartGameButton is not assigned.");
            return;
        }

        StartGameButton.interactable = interactable;
        Debug.Log("[TelegramIAWCustomAuth] Start game button is now " + (interactable ? "interactable" : "not interactable"));
    }

    public void OpenWalletExplorer()
    {
        if (!string.IsNullOrEmpty(walletAddress))
        {
            string explorerUrl = "https://sepolia.arbiscan.io/address/" + walletAddress;
            Application.OpenURL(explorerUrl);
        }
        else
        {
            Log("Wallet not connected.");
        }
    }

    private void Log(string message)
    {
        if (LogText != null)
        {
            LogText.text = message;
        }
        Debug.Log(message);
    }


    private void FallbackCheck()
    {
        CheckConditions();
    }

    [System.Serializable]
    public class Payload
    {
        public string signature;
        public string message;
    }
}
