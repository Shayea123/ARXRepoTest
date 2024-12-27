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

        rankText.text = $"{playerData.playerState.rank}";
        // make check for influence
        if (!playerData.resources.ContainsKey("Influence"))
        {
            Debug.LogWarning("[HUDController] Missing 'Influence' in resources. Initializing to 0.");
            playerData.resources["Influence"] = 0;
        }

        coinsText.text = $"{playerData.resources["Coins"]}";
        foodText.text = $"{playerData.resources["Food"]}";
        infantryText.text = $"{playerData.troops["Infantry"]}";
        knightsText.text = $"{playerData.troops["Horsemen"]}";
        ram.text = $"{playerData.troops["Machinery"]}";

        Debug.Log("[HUDController] HUD updated with current PlayerData.");

       // var playerData = WalletGameManager.Instance.GetPlayerData();

        dateText.text = $"{playerData.time.day}/{playerData.time.month}/{playerData.time.year}";
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
