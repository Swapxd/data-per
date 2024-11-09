using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TitleManager : MonoBehaviour
{
    public TMP_InputField nameText;
    public TMP_Text warning;
    public Button start;
    public Button exit;
    public Button clearScore;
    public List<TMP_Text> scoreList;

    // Start is called before the first frame update
    void Start()
    {
        warning.gameObject.SetActive(false);
        start.onClick.AddListener(ButtonStart);
        exit.onClick.AddListener(ButtonExit);
        clearScore.onClick.AddListener(ButtonClearScore);

        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        int saveLength = GameManager.instance.scoreData.Count;

        for (int loop = 0; loop < saveLength; loop++)
        {
            scoreList[loop].text = (loop + 1) + ". " + GameManager.instance.scoreData[loop].DataToString();
        }

        for (int loop = saveLength; loop < GameManager.SAVE_MAX; loop++)
        {
            scoreList[loop].text = (loop + 1) + ". <Empty>";
        }
    }

    public void ButtonStart()
    {
        if (string.IsNullOrWhiteSpace(nameText.text))
        {
            warning.text = "Name should not empty or only white spaces";
            warning.gameObject.SetActive(true);
        }
        else if (nameText.text.Length > 16)
        {
            warning.text = "Name length should not exceed 16";
            warning.gameObject.SetActive(true);
        }
        else
        {
            GameManager.instance.StartGame(nameText.text);
            SceneManager.LoadScene(1);
        }
    }

    public void ButtonExit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ButtonClearScore()
    {
        GameManager.instance.ClearScore();
    }
}
