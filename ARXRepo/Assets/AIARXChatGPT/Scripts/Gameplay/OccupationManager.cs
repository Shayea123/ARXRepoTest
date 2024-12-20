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

    public void ChooseOccupation(string occupation)
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();

        // Deduct 1 Action Point for choosing an occupation
        if (!TimeSystem.Instance.UseActionPoint(1))
        {
            Debug.LogWarning("Not enough Action Points to choose an occupation!");
            return;
        }

        playerData.occupation = occupation;

        WalletGameManager.Instance.ManualSave(); // Save updated data
        HUDController.Instance.UpdateHUD();      // Refresh HUD

        Debug.Log($"[OccupationManager] Occupation set to: {occupation}");
    }

    public void GenerateMonthlyResources()
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();

        switch (playerData.occupation)
        {
            case "Farmer":
                playerData.food += 30; // Example food production
                break;
            case "Miner":
                playerData.coins += 50; // Example coin production
                break;
            case "Blacksmith":
                playerData.technology += 10; // Example technology production
                break;
            default:
                Debug.LogWarning("[OccupationManager] No occupation selected.");
                break;
        }

        WalletGameManager.Instance.ManualSave(); // Save updated data
        HUDController.Instance.UpdateHUD();      // Refresh HUD

        Debug.Log($"[OccupationManager] Resources generated for occupation: {playerData.occupation}");
    }
}
