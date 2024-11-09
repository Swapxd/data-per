using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Color TeamColor;
    private string savePath;
    public static int SAVE_MAX = 5;

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public int score;

        public PlayerData(string name, int score)
        {
            this.name = name;
            this.score = score;
        }

        public string DataToString()
        {
            return name + " : " + score;
        }
    }

    [System.Serializable]
    private class PlayerDataListWrapper
    {
        public List<PlayerData> players;

        public PlayerDataListWrapper(List<PlayerData> playerList)
        {
            players = playerList;
        }
    }

    public List<PlayerData> scoreData = null;

    public string currentPlayerName;

    public void SaveData()
    {
        string json = JsonUtility.ToJson(new PlayerDataListWrapper(scoreData), true);
        File.WriteAllText(savePath, json);
    }
 
    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            //scoreData = JsonUtility.FromJson<List<PlayerData>>(json);
            PlayerDataListWrapper wrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);
            scoreData = wrapper.players;
        }
    }


    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";

        if (!File.Exists(savePath))
        {
            scoreData = new List<PlayerData>();
            SaveData();
        } else
        {
            LoadData();
        }

        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScore()
    {
        LoadData();
        TitleManager titleManager = GameObject.Find("TitleManager").GetComponent<TitleManager>();
        if (titleManager != null)
        {
            titleManager.UpdateScore();
        }
    }

    public void ClearScore()
    {
        scoreData.Clear();
        SaveData();
        UpdateScore();
    }

    public string GetBestScore()
    {
        if (scoreData.Count <= 0)
        {
            return "<Empty>";
        } else
        {
            return scoreData[0].DataToString();
        }
    }

    public void StartGame(string name)
    {
        currentPlayerName = name;
    }

    public void SaveScore(int score)
    {
        PlayerData newScore = new PlayerData(currentPlayerName, score);
        scoreData.Add(newScore);
        scoreData.Sort((p1, p2) => p2.score.CompareTo(p1.score));
        if (scoreData.Count > SAVE_MAX)
        {
            scoreData.RemoveAt(SAVE_MAX);
        }
        SaveData();
    }
}
