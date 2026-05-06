using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int slackPoints;
    public float sanity;
    public float suspicion;
    public int currentFloor;
    public List<string> earnedBadges;
    public int unlockedFloors;
    public string fakeJobTitle;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    string savePath = "SlackAndThrive_SaveData.json";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            slackPoints = GameManager.Instance.slackPoints,
            sanity = GameManager.Instance.sanity,
            suspicion = GameManager.Instance.suspicion,
            currentFloor = GameManager.Instance.currentFloor,
            earnedBadges = GameManager.Instance.earnedBadges,
            unlockedFloors = GameManager.Instance.currentFloor,
            fakeJobTitle = "Senior Synergy Consultant"
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/" + savePath, json);
    }

    public SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/" + savePath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        return null;
    }

    public void DeleteSave()
    {
        string path = Application.persistentDataPath + "/" + savePath;
        if (File.Exists(path))
            File.Delete(path);
    }
}
