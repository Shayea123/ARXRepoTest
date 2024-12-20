using UnityEngine;
using Lean.Gui;
using TMPro;


public class UIManager : MonoBehaviour
{
    [SerializeField] private LeanWindow locationPanel; // Reference to LeanWindow
    [SerializeField] private TMP_Text locationNameText; // Text element for location name


    [SerializeField] private LeanWindow attackPopup; // Reference to the popup panel
    [SerializeField] private TMP_Text popupTitleText;

    public static UIManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenLocationPanel(string locationName)
    {
        locationNameText.text = locationName;
        locationPanel.TurnOn(); // Show the LeanWindow panel
        Debug.Log($"[UIManager] Opened panel for: {locationName}");
    }

    public void CloseLocationPanel()
    {
        locationPanel.TurnOff(); // Hide the LeanWindow panel
        Debug.Log("[UIManager] Closed location panel");
    }


    public void OpenAttackPopup()
    {
        popupTitleText.text = "Do you want to attack this kingdom?";
        attackPopup.TurnOn(); // Show popup
    }

    public void CloseAttackPopup()
    {
        attackPopup.TurnOff(); // Hide popup
    }
}
