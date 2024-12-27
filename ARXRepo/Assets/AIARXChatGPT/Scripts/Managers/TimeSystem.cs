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
        playerData.actionPoints = RankManager.Instance.GetMaxActionPoints(playerData.playerState.rank);

        // Increment day
        playerData.time.day++;
        if (playerData.time.day > 30) // New month
        {
            playerData.time.day = 1;
            playerData.time.month++;
            if (playerData.time.month > 12) // New year
            {
                playerData.time.month = 1;
                playerData.time.year++;
            }
        }

        WalletGameManager.Instance.SavePlayerData(); // Save updated time and AP
        HUDController.Instance.UpdateHUD();      // Update the UI

        // Check for rank-up notification
        if (RankManager.Instance.CanRankUp(playerData))
        {
            HUDController.Instance.ShowRankUpNotification();
        }

        Debug.Log($"[TimeSystem] New Day: {playerData.time.day}/{playerData.time.month}/{playerData.time.year}");
    }

    public bool UseActionPoint(int cost)
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();

        if (playerData.actionPoints >= cost)
        {
            playerData.actionPoints -= cost;
            WalletGameManager.Instance.SavePlayerData(); // Save after using AP
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
