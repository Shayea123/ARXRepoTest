using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //public static QuestManager Instance; // Singleton instance for easy access

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;
    //}

    //public void CompleteQuest(int coinReward, int foodReward, int influenceReward, int infantry, int knights, int ram)
    //{
    //    Debug.Log($"[QuestManager] Completing quest: Coins +{coinReward}, Food +{foodReward}, Influence +{influenceReward}");

    //    var playerData = WalletGameManager.Instance.GetPlayerData();

    //    // Apply rewards
    //    playerData.coins += coinReward;
    //    playerData.food += foodReward;
    //    playerData.influence += influenceReward;
    //    playerData.infantry -= infantry; // Some troops lost
    //    playerData.knights -= knights; // Some troops lost
    //    playerData.ram -= ram; // Some siege weapons lost   


    //    WalletGameManager.Instance.ManualSave(); // Save updated PlayerData
    //    HUDController.Instance.UpdateHUD();      // Refresh HUD

    //    Debug.Log("[QuestManager] Quest rewards applied and saved.");
    //}
}
