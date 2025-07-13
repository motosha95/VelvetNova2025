using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadManager
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    [System.Serializable]
    public class SaveData
    {
        public int score;
        public List<int> matchedIDs;
    }

    public static void SaveGame(int score, List<Card> cards)
    {
        SaveData data = new SaveData { score = score, matchedIDs = new() };
        foreach (var card in cards)
        {
            if (card.IsMatched)
                data.matchedIDs.Add(card.cardID);
        }
        File.WriteAllText(SavePath, JsonUtility.ToJson(data));
    }

    public static void LoadGame()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        GameManager.Instance.UpdateScore(0); // Set current score
        foreach (var card in GameObject.FindObjectsOfType<Card>())
        {
            if (data.matchedIDs.Contains(card.cardID))
            {
                card.SetMatched();
            }
        }
    }
}