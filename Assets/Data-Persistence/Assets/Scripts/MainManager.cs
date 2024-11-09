using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    public TMPro.TMP_Text playerName;
    public Button exit;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_BricksRemaining; // New variable to track remaining bricks
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        m_BricksRemaining = LineCount * perLine; // Initialize brick count based on setup

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(OnBrickDestroyed); // Update to use OnBrickDestroyed
            }
        }

        playerName.text = "Name : " + GameManager.instance.currentPlayerName;
        exit.onClick.AddListener(Exit);
        UpdateBestScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    void OnBrickDestroyed(int point)
    {
        AddPoint(point);
        m_BricksRemaining--; // Decrease the brick count
        
        if (m_BricksRemaining <= 0) // Check if all bricks are destroyed
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        GameManager.instance.SaveScore(m_Points);
        UpdateBestScore();
    }

    void UpdateBestScore()
    {
        BestScoreText.text = "Best Score : " + GameManager.instance.GetBestScore();
    }

    private void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
