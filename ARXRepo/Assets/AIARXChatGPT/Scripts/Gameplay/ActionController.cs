using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;
using System;

public class ActionController : MonoBehaviour
{
    [SerializeField] private LeanButton recruitButton;
    [SerializeField] private LeanButton buyFoodButton;
    [SerializeField] private LeanButton buyRamButton;
    [SerializeField] private LeanButton attackButton;


    private void Start()
    {
        // Hook up button events
        recruitButton.OnClick.AddListener(OnRecruitTroops);
        buyFoodButton.OnClick.AddListener(OnBuyFood);
        buyRamButton.OnClick.AddListener(OnBuyRam);
        attackButton.OnClick.AddListener(OnAttack);
    }

    private void OnRecruitTroops()
    {
        Debug.Log("[ActionController] Recruit Troops clicked!");
        WalletGameManager.Instance.GetPlayerData().infantry += 10; // Add 10 troops
        WalletGameManager.Instance.ManualSave();
        Debug.Log("Recruited 10 infantry. Updated PlayerData.");
        HUDController.Instance.UpdateHUD(); // Refresh HUD with new data

    }

    private void OnBuyFood()
    {
        if (TimeSystem.Instance.UseActionPoint(1)) // Deduct 1 AP
        {
            var playerData = WalletGameManager.Instance.GetPlayerData();
            playerData.food += 50;
            WalletGameManager.Instance.ManualSave();
            HUDController.Instance.UpdateHUD();
        }
    }

    private void OnBuyRam()
    {
        if (TimeSystem.Instance.UseActionPoint(1)) // Deduct 1 AP
        {
            var playerData = WalletGameManager.Instance.GetPlayerData();
            playerData.ram += 1;
            WalletGameManager.Instance.ManualSave();
            HUDController.Instance.UpdateHUD();
        }
    }

    private void OnAttack()
    {
        Debug.Log("[ActionController] Attack clicked!");
        UIManager.Instance.OpenAttackPopup(); // Show attack confirmation popup
    }
}
