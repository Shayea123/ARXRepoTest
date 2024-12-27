using UnityEngine;

public class OccupationManager : MonoBehaviour
{
    public static OccupationManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ChooseOccupation(OccupationData newOccupation)
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();

        // Deduct 1 Action Point
        if (!TimeSystem.Instance.UseActionPoint(1))
        {
            Debug.LogWarning("Not enough Action Points to choose an occupation!");
            return;
        }

        playerData.occupation = newOccupation.occupationName;
        WalletGameManager.Instance.SavePlayerData();  // Save updated data
        HUDController.Instance.UpdateHUD();  // Refresh HUD

        Debug.Log($"[OccupationManager] Occupation set to: {newOccupation.occupationName}");
    }

    public void GenerateMonthlyResources()
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();
        OccupationData currentOccupation = FindOccupationData(playerData.occupation);

        if (currentOccupation != null)
        {
            foreach (var bonus in currentOccupation.resourceBonuses)
            {
                playerData.resources[bonus.Key] += bonus.Value;
            }
            WalletGameManager.Instance.SavePlayerData();  // Save updated data
            HUDController.Instance.UpdateHUD();  // Refresh HUD
            Debug.Log($"[OccupationManager] Resources generated for occupation: {playerData.occupation}");
        }
        else
        {
            Debug.LogWarning("[OccupationManager] No valid occupation found.");
        }
    }

    private OccupationData FindOccupationData(string occupationName)
    {
        return null;
        // Search through all OccupationData assets for a match
        // (Assumes you have a centralized OccupationDatabase or list)
    }

}