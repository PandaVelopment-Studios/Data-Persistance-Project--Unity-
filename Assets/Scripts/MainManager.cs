using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text playerText;
    public Text HighScoreText;

    private bool m_Started = false;
    private int m_Points;
    private int m_HighScore;
    private string m_HighScorePlayer;

    private bool m_GameOver = false;

    private string playerName;

    // Start is called before the first frame update
    void Start()
    {

        if (NameManager.Instance != null)
        {
            SetName(NameManager.Instance.Name);
            playerText.text = "Player: " + playerName;

        }
        LoadHighScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
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

    public void GameOver()
    {
        if (isHighScore())
        {
            saveHighScore();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    private void SetName(string name)
    {
        playerName = name;
    }

    public bool isHighScore()
    {
        if (m_Points > m_HighScore)
        {
            HighScoreText.text = $"Best Score : {m_Points} : {playerName}";
            return true; 
        }
        else return false;
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScore data = JsonUtility.FromJson<HighScore>(json);

            m_HighScore = data.highScore;
            m_HighScorePlayer = data.player;
        }
        else
        {
            m_HighScore = 0;
            m_HighScorePlayer = "";
        }

        HighScoreText.text = $"Best Score : {m_HighScorePlayer} : {m_HighScore}";
    }
    public void saveHighScore()
    {
        HighScore score = new HighScore();
        score.highScore = m_Points;
        score.player = playerName;

        string json = JsonUtility.ToJson(score);
        File.WriteAllText(Application.persistentDataPath+"/highscore.json", json);
    }
    [System.Serializable]
    class HighScore
    {
        public int highScore;
        public string player;
    }
}
