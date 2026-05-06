using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int slackPoints = 0;
    public float sanity = 100f;
    public float suspicion = 0f;
    public int currentFloor = 1;

    [Header("References")]
    public Transform[] floorSpawns;
    public Transform managerCabinPosition;
    public UIManager uiManager;

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isGameWon = false;
    public float workdayTimer = 480f; // 8 hours in seconds

    [Header("Badges")]
    public List<string> earnedBadges = new List<string>();

    [Header("NPCs")]
    public ManagerAI managerAI;
    public ITGuy itGuy;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(WorkdayTimer());
    }

    void Update()
    {
        CheckGameOver();
        CheckWinCondition();
    }

    System.Collections.IEnumerator WorkdayTimer()
    {
        while (workdayTimer > 0 && !isGameOver)
        {
            workdayTimer -= Time.deltaTime;
            uiManager.UpdateWorkdayTimer(workdayTimer);
            yield return null;
        }
    }

    void CheckGameOver()
    {
        if (sanity <= 0 || suspicion >= 100f || workdayTimer <= 0)
        {
            isGameOver = true;
            uiManager.ShowGameOver();
        }
    }

    void CheckWinCondition()
    {
        if (currentFloor >= 5 && earnedBadges.Contains("PermanentWFH"))
        {
            isGameWon = true;
            uiManager.ShowVictory();
        }
    }

    public void ChangeFloor(int floorNumber)
    {
        currentFloor = floorNumber;
        uiManager.UpdateFloor(currentFloor);
    }

    public void UpdateSlackPoints(int points)
    {
        slackPoints = points;
        uiManager.UpdateSlackPoints(slackPoints);

        // Check for floor unlocks
        CheckFloorUnlocks();
    }

    public void UpdateSanity(float value)
    {
        sanity = Mathf.Clamp(value, 0f, 100f);
        uiManager.UpdateSanity(sanity);
    }

    public void UpdateSuspicion(float value)
    {
        suspicion = Mathf.Clamp(value, 0f, 100f);
        uiManager.UpdateSuspicion(suspicion);
    }

    void CheckFloorUnlocks()
    {
        int[] thresholds = { 50, 100, 150, 200 };

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (slackPoints >= thresholds[i] && currentFloor < i + 2)
            {
                UnlockFloor(i + 2);
            }
        }
    }

    void UnlockFloor(int floorNumber)
    {
        uiManager.ShowFloorUnlock(floorNumber);
    }

    public void TriggerEvent(string eventName)
    {
        switch (eventName)
        {
            case "ServerOutage":
                managerAI.FreezeForDuration(60f);
                break;

            case "StationeryTheft":
                suspicion += 10f;
                break;

            case "BefriendedOverachiever":
                suspicion -= 10f;
                break;

            case "AssignedInternWork":
                slackPoints += 20;
                break;

            case "PermanentWFH":
                earnedBadges.Add("PermanentWFH");
                break;
        }

        uiManager.ShowEventNotification(eventName);
    }

    public void OnManagerDetection(PlayerController player)
    {
        uiManager.ShowWarning("Manager Detected!");
    }

    public Transform GetFloorSpawnPoint(int floorNumber)
    {
        if (floorSpawns != null && floorNumber - 1 < floorSpawns.Length)
            return floorSpawns[floorNumber - 1];
        return floorSpawns[0];
    }
}
