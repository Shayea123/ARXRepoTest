using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Thirdweb.Unity;
using Lean.Gui;

public class Connected2Web3 : MonoBehaviour
{
    [SerializeField] private LeanButton StartGameButton; // Assign this in the Inspector

    private void OnEnable()
    {
        WalletGameManager.OnDataLoaded += OnDataLoadedHandler;
    }

    private void OnDisable()
    {
        WalletGameManager.OnDataLoaded -= OnDataLoadedHandler;
    }

    private void Start()
    {
        // Ensure the button is initially not interactable
        if (StartGameButton != null)
        {
            StartGameButton.interactable = false;
        }
        else
        {
            Debug.LogError("StartGameButton is not assigned in the Inspector.");
        }
    }

    private void OnDataLoadedHandler()
    {
        Debug.Log("[Connected2Web3] OnDataLoaded event received.");

        if (WalletGameManager.Instance == null)
        {
            Debug.LogError("[Connected2Web3] WalletGameManager instance is null. Cannot retrieve data.");
            return;
        }

        // Enable the button now that the wallet and auth are ready
        if (StartGameButton != null)
        {
            StartGameButton.interactable = true;
            Debug.Log("[Connected2Web3] StartGameButton is now interactable.");
        }
    }
}
