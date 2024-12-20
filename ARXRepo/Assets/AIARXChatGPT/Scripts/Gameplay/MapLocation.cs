using UnityEngine;
using Lean.Touch;
using TMPro;


public class MapLocation : MonoBehaviour
{
    [SerializeField] TMP_Text locationName; // Change TMPro to TMP_Text

    // Called by LeanTouch event
    public void OnLocationTapped(LeanFinger finger)
    {
        Debug.Log($"Tapped on location: {locationName.text}"); // Access the text property
        UIManager.Instance.OpenLocationPanel(locationName.text); // Pass the text property
    }
}
