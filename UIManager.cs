using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public Slider sanityMeter;
    public Slider suspicionMeter;
    public TextMeshProUGUI slackPointsText;
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI workdayTimerText;
    public TextMeshProUGUI productivityLabel; // Fake "Productivity" label for sanity

    [Header("Interaction UI")]
    public GameObject interactionPrompt;
    public TextMeshProUGUI interactionText;

    [Header("Event Notifications")]
    public GameObject eventNotification;
    public TextMeshProUGUI eventText;

    [Header("Game Over / Victory")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Badges")]
    public GameObject badgeNotification;
    public TextMeshProUGUI badgeText;

    [Header("Floor Unlock")]
    public GameObject floorUnlockPanel;
    public TextMeshProUGUI floorUnlockText;

    [Header("Settings")]
    public GameObject settingsPanel;

    void Start()
    {
        UpdateSanity(GameManager.Instance.sanity);
        UpdateSuspicion(GameManager.Instance.suspicion);
        UpdateSlackPoints(GameManager.Instance.slackPoints);
        UpdateFloor(GameManager.Instance.currentFloor);

        // Style sanity meter as "Productivity" to fool viewers
        productivityLabel.text = "Productivity: " + Mathf.RoundToInt(GameManager.Instance.sanity) + "%";
    }

    void Update()
    {
        // Update productivity label to match sanity
        productivityLabel.text = "Productivity: " + Mathf.RoundToInt(GameManager.Instance.sanity) + "%";
    }

    public void UpdateSanity(float value)
    {
        sanityMeter.value = value;
    }

    public void UpdateSuspicion(float value)
    {
        suspicionMeter.value = value;
    }

    public void UpdateSlackPoints(int points)
    {
        slackPointsText.text = "Slack Points: " + points;
    }

    public void UpdateFloor(int floor)
    {
        string[] floorNames = {
            "Basement - Cafeteria",
            "Floor 1 - General Operations",
            "Floor 2 - IT & Tech Support",
            "Floor 3 - Marketing & Social Media",
            "Floor 4 - Finance & Accounts",
            "Floor 5 - C-Suite / Executive Level"
        };

        floorText.text = floorNames[floor];
    }

    public void UpdateWorkdayTimer(float timeRemaining)
    {
        int minutes = Mathf.CeilToInt(timeRemaining / 60f);
        workdayTimerText.text = "Time Remaining: " + minutes + "m";
    }

    public void ShowInteractionPrompt(string text)
    {
        interactionText.text = text;
        interactionPrompt.SetActive(true);
    }

    public void HideInteractionPrompt()
    {
        interactionPrompt.SetActive(false);
    }

    public void ShowEventNotification(string eventName)
    {
        eventText.text = GetEventDescription(eventName);
        eventNotification.SetActive(true);

        Invoke("HideEventNotification", 3f);
    }

    string GetEventDescription(string eventName)
    {
        switch (eventName)
        {
            case "ServerOutage": return "Server Outage! Manager frozen!";
            case "StationeryTheft": return "Stationery theft discovered!";
            case "BefriendedOverachiever": return "Overachiever won't report you!";
            case "AssignedInternWork": return "Intern did your work!";
            case "PermanentWFH": return "PERMANENT WFH STATUS UNLOCKED!";
            default: return eventName;
        }
    }

    void HideEventNotification()
    {
        eventNotification.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Slack Points: " + GameManager.Instance.slackPoints;
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
        finalScoreText.text = "You escaped! WFH forever!";
    }

    public void ShowBadge(string badgeName)
    {
        badgeText.text = badgeName;
        badgeNotification.SetActive(true);

        Invoke("HideBadge", 3f);
    }

    void HideBadge()
    {
        badgeNotification.SetActive(false);
    }

    public void ShowFloorUnlock(int floorNumber)
    {
        floorUnlockText.text = "Floor " + floorNumber + " Unlocked!";
        floorUnlockPanel.SetActive(true);

        Invoke("HideFloorUnlock", 3f);
    }

    void HideFloorUnlock()
    {
        floorUnlockPanel.SetActive(false);
    }

    public void ShowWarning(string warning)
    {
        eventText.text = warning;
        eventNotification.SetActive(true);

        Invoke("HideEventNotification", 2f);
    }
}
