using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartNewDay()
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();

        // Refresh Action Points based on rank
        playerData.actionPoints = WalletGameManager.Instance.GetMaxActionPoints(playerData.rank);

        // Increment day
        playerData.day++;
        if (playerData.day > 30) // New month
        {
            playerData.day = 1;
            playerData.month++;
            if (playerData.month > 12) // New year
            {
                playerData.month = 1;
                playerData.year++;
            }
        }

        WalletGameManager.Instance.ManualSave(); // Save updated time and AP
        HUDController.Instance.UpdateHUD();      // Update the UI

        // Check for rank-up notification
        if (WalletGameManager.Instance.CanRankUp())
        {
            HUDController.Instance.ShowRankUpNotification();
        }

        Debug.Log($"[TimeSystem] New Day: {playerData.day}/{playerData.month}/{playerData.year}");
    }

    public bool UseActionPoint(int cost)
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();

        if (playerData.actionPoints >= cost)
        {
            playerData.actionPoints -= cost;
            WalletGameManager.Instance.ManualSave(); // Save after using AP
            HUDController.Instance.UpdateHUD();      // Update the UI
            Debug.Log($"[TimeSystem] Used {cost} AP. Remaining: {playerData.actionPoints}");
            return true;
        }
        else
        {
            Debug.LogWarning("[TimeSystem] Not enough Action Points!");
            return false;
        }
    }
}
