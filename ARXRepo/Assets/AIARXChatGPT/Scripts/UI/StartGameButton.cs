using Lean.Gui;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private LeanButton StartGameBtn;


    private void Start()
    {
        if (WalletGameManager.Instance == null)
        {
            Debug.LogError("WalletGameManager instance is null");
            return;
        }

        if (WalletGameManager.Instance.GetIndeveloping())
        {
            // In dev mode, just enable the button
            SetStartButtonInteractable(true);
        }
        else
        {
            // If not in dev mode, check if data is already loaded
            if (WalletGameManager.Instance.DataLoaded)
            {
                // Data already loaded before this UI appeared
                SetStartButtonInteractable(true);
            }
            else
            {
                // Data not loaded yet, subscribe to the event
                WalletGameManager.OnDataLoaded += OnDataLoadedHandler;
            }
        }
    }


    private void OnDisable()
    {
        WalletGameManager.OnDataLoaded -= OnDataLoadedHandler;
    }

    private void OnDataLoadedHandler()
    {
        SetStartButtonInteractable(true);
    }

    private void SetStartButtonInteractable(bool interactable)
    {
        if (StartGameBtn == null)
        {
            Debug.LogWarning("StartGameButton not assigned");
            return;
        }

        StartGameBtn.interactable = interactable;
        Debug.Log("Start game button is now " + (interactable ? "interactable" : "not interactable"));
    }

}
