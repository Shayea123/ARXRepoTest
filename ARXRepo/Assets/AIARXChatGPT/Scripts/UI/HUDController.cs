using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [Header("Player Data Display")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text infantryText;
    [SerializeField] private TMP_Text knightsText;
    [SerializeField] private TMP_Text ram;


    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text actionPointsText;

    [SerializeField] private GameObject rankUpNotificationPanel;

    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text influenceText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Initialize HUD with player data
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        var playerData = WalletGameManager.Instance.GetPlayerData();

        rankText.text = $"{playerData.rank}";
        influenceText.text = $"{playerData.influence}";

        coinsText.text = $"{playerData.coins}";
        foodText.text = $"{playerData.food}";
        infantryText.text = $"{playerData.infantry}";
        knightsText.text = $"{playerData.knights}";
        ram.text = $"{playerData.ram}";

        Debug.Log("[HUDController] HUD updated with current PlayerData.");

       // var playerData = WalletGameManager.Instance.GetPlayerData();

        dateText.text = $"{playerData.day}/{playerData.month}/{playerData.year}";
        actionPointsText.text = $"{playerData.actionPoints}";
    }

    public void ShowRankUpNotification()
    {
        rankUpNotificationPanel.SetActive(true);
    }

    public void HideRankUpNotification()
    {
        rankUpNotificationPanel.SetActive(false);
    }
}
