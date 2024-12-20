using UnityEngine;
using Lean.Gui;

public class AttackController : MonoBehaviour
{
    [SerializeField] private LeanButton confirmAttackButton;
    [SerializeField] private LeanButton cancelAttackButton;

    private void Start()
    {
        confirmAttackButton.OnClick.AddListener(OnConfirmAttack);
        cancelAttackButton.OnClick.AddListener(OnCancelAttack);
    }

    private void OnConfirmAttack()
    {
        Debug.Log("[AttackController] Confirm Attack clicked!");

        // Check if player has enough troops
        var playerData = WalletGameManager.Instance.GetPlayerData();

        if (playerData.infantry < 10)
        {
            Debug.LogWarning("Not enough troops to attack!");
            UIManager.Instance.CloseAttackPopup();
            return;
        }

        // Simulate battle outcome
        bool victory = Random.value > 0.5f; // 50% chance to win
        if (victory)
        {
            Debug.Log("Victory! You gained resources.");
            QuestManager.Instance.CompleteQuest(200, 50, 15, 5, 1, 0); // Example rewards


            Debug.Log($"[BattleController] Battle won! Gained 10 influence.");

        }
        else
        {
            Debug.Log("Defeat! You lost troops and gold.");
            playerData.infantry = Mathf.Max(0, playerData.infantry - 10);
            playerData.coins -= 50;
            playerData.isEnslaved = true; // Player becomes enslaved
        }

        WalletGameManager.Instance.ManualSave(); // Save the updated PlayerData
        HUDController.Instance.UpdateHUD(); // Refresh HUD
        UIManager.Instance.CloseAttackPopup();
    }

    private void OnCancelAttack()
    {
        Debug.Log("[AttackController] Cancel Attack clicked!");
        UIManager.Instance.CloseAttackPopup();
    }
}
