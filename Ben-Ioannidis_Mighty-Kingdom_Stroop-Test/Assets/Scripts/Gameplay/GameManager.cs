using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Purpose:
/// The purpose of this class is to handle gameplay operations such as: button events, score tracking, and randomisation
/// 
/// Date created: 13/12/21
/// Developer: Ben Ioannidis
/// 
/// Changelist:
/// -> 13/12/21 - Script created
/// -> 13/12/21 - Added button interaction functions
/// -> 13/12/21 - Added "Correct Answer" function
/// -> 
/// </summary>
public class GameManager : MonoBehaviour
{
    private static string m_gameMode;

    public TMPro.TMP_Text scoreText;

    private int m_currentScore;
    private int m_goalScore;
    private bool gameOver;

    public TMPro.TMP_Text timerText;
    public TMPro.TMP_Text answerTimerText;
    private float time;
    private float milliseconds, seconds, minutes;

    public TMPro.TMP_Text colourText;
    private colour m_currentColour;

    public colour[] m_colours;

    [System.Serializable]
    public enum colour
    {
        Blue,
        Cyan,
        Green,
        Magenta,
        Red,
        Yellow
    }

    public GameObject startMenu, pauseMenu;
    public TMPro.TMP_Text gameModeText, gameModeDescription;

    public GameObject endGamePanel, newHighscoreText;
    public TMPro.TMP_Text playerScoreText, highscoreText, winText;

    private void Start()
    {
        //Remove duplicate colours
        HashSet<colour> distinctColours = new HashSet<colour>(m_colours);

        //copy distinct list of colours to the colour list for use
        m_colours = new colour[distinctColours.Count];
        distinctColours.CopyTo(m_colours);

        //check current gamemode that has been set via the main menu
        //m_gameMode = GameModeData.CurrentGameMode.modeName;
        GameModeData.CurrentGameMode = GameModeData.survive;
        m_gameMode = GameModeData.CurrentGameMode.modeName;
        if (m_gameMode == GameModeData.survive.modeName)
        {
            time = 20f;
        }
        m_goalScore = GameModeData.CurrentGameMode.scoreGoal;

        gameModeText.text = m_gameMode;
        gameModeDescription.text = GameModeData.CurrentGameMode.description;


        scoreText.text = m_currentScore + "/" + m_goalScore;
        pauseMenu.SetActive(false);
        endGamePanel.SetActive(false);
    }

    private void Update()
    {
        if (m_gameMode == GameModeData.survive.modeName)
        { 
            if (time <= 0 && Time.timeScale > 0)
            {
                StopAllCoroutines();
                time = 0f;
                GameCompleted();
            }
        }
    }

    private void NewRound()
    {
        m_currentColour = m_colours[Random.Range(0, m_colours.Length)];
        Color textColour = Color.white;

        switch (m_currentColour)
        {
            case colour.Blue:
                textColour = Color.blue;
                break;
            case colour.Cyan:
                textColour = Color.cyan;
                break;
            case colour.Green:
                textColour = Color.green;
                break;
            case colour.Magenta:
                textColour = Color.magenta;
                break;
            case colour.Red:
                textColour = Color.red;
                break;
            case colour.Yellow:
                textColour = Color.yellow;
                break;
            default:
                break;
        }

        List<colour> otherColours = new List<colour>();

        //make a higher chance of the text not matching the colour (but still leave the chance for it to match occassionally)
        foreach (colour c in m_colours)
        {
            if (c != m_currentColour)
            {
                for (int i = 0; i < 6; i++)
                {
                    otherColours.Add(c);
                }
            }
            else
            {
                otherColours.Add(c);
            }
        }

        string text = otherColours[Random.Range(0, otherColours.Count)].ToString();

        colourText.text = text;
        colourText.color = textColour;
    }

    private IEnumerator StopWatch()
    {
        while (true)
        {
            time += Time.deltaTime;

            milliseconds = (int)((time - (int)time) * 100);
            seconds = (time % 60);
            minutes = (time / 60 % 60);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            yield return null;
        }
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            time -= Time.deltaTime;

            milliseconds = (int)((time - (int)time) * 100);
            seconds = (time % 60);
            minutes = (time / 60 % 60);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            yield return null;
        }
    }

    /// <summary>
    /// Called when the user has answered correctly
    /// </summary>
    private void CorrectAnswer()
    {
        //regular round (fast as you can to goal score (10))
        if (m_gameMode == GameModeData.GetGameModes()[0].modeName) 
        {
            if (m_currentScore < m_goalScore - 1)
            {
                m_currentScore++;
                scoreText.text = m_currentScore + "/" + m_goalScore;
            }

            //game completed
            else
            {
                //Show game ended screen with "You did it!"
                GameCompleted();
            }
        }

        //survival game mode
        else
        {
            m_currentScore++;
            scoreText.text = "Score: " + m_currentScore;

            answerTimerText.text = "+ 00:00:50";
            answerTimerText.color = Color.green;
            answerTimerText.gameObject.SetActive(true);
            answerTimerText.GetComponent<TimerFlash>().FlashNewTime();

            time += 0.5f;
        }
        NewRound();
    }

    /// <summary>
    /// Called when the user has answered incorrectly
    /// </summary>
    private void WrongAnswer()
    {
        //regular round (fast as you can to goal score(10))
        if (m_gameMode == GameModeData.GetGameModes()[0].modeName)
        {
            if (m_currentScore >= 2)
            {
                m_currentScore -= 2;
            }

            else
            {
                m_currentScore = 0;
            }
            scoreText.text = m_currentScore + "/" + m_goalScore;
        }

        //survival game mode
        else
        {
            answerTimerText.text = "- 00:02:00";
            answerTimerText.color = Color.red;
            answerTimerText.gameObject.SetActive(true);
            answerTimerText.GetComponent<TimerFlash>().FlashNewTime();

            time -= 2f;
        }
        NewRound();
    }

    /// <summary>
    /// Compare answer given to the correct answer
    /// </summary>
    /// <param name="col">pass in the relevant colour based on button pressed</param>
    /// <returns></returns>
    private bool CheckAnswer(colour col)
    {
        if (col == m_currentColour)
        {
            return true;
        }
        return false;
    }

    private void GameCompleted()
    {
        //regular round (fast as you can to goal score(10))
        if (m_gameMode == GameModeData.GetGameModes()[0].modeName)
        {
            Time.timeScale = 0f;

            winText.text = "Score Reached!";

            endGamePanel.SetActive(true);
            playerScoreText.text = "Your score: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

            if (time < PlayerPrefs.GetFloat("SPHighscore") || !PlayerPrefs.HasKey("SPHighscore"))
            {
                newHighscoreText.SetActive(true);
                PlayerPrefs.SetFloat("SPHighscore", time);
            }
            else
            {
                newHighscoreText.SetActive(false);
            }

            float mil, sec, min;
            mil = (int)((PlayerPrefs.GetFloat("SPHighscore") - (int)PlayerPrefs.GetFloat("SPHighscore")) * 100);
            sec = (PlayerPrefs.GetFloat("SPHighscore") % 60);
            min = (PlayerPrefs.GetFloat("SPHighscore") / 60 % 60);

            highscoreText.text = "Highscore: " + string.Format("{0:00}:{1:00}:{2:00}", min, sec, mil);
        }
        else
        {
            Time.timeScale = 0f;

            winText.text = "You lasted a while!";

            endGamePanel.SetActive(true);
            playerScoreText.text = "Your score: " + m_currentScore;

            if (m_currentScore > PlayerPrefs.GetInt("SurvivalHighscore") || !PlayerPrefs.HasKey("SurvivalHighscore"))
            {
                newHighscoreText.SetActive(true);
                PlayerPrefs.SetInt("SurvivalHighscore", m_currentScore);
            }
            else
            {
                newHighscoreText.SetActive(false);
            }

            highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("SurvivalHighscore");
        }
    }

    #region ButtonInteraction
    public void PausePressed()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumePressed()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    public void RestartPressed()
    {
        time = 20;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitPressed()
    {
        //load menu scene
        Application.Quit();
    }

    public void PlayPressed()
    {
        //start game
        startMenu.SetActive(false);
        NewRound();
        if (m_gameMode == GameModeData.speed10Round.modeName)
        {
            StartCoroutine("StopWatch");
        }
        else
        {
            StartCoroutine("Timer");
        }
    }


    public void RedPressed()
    {
        if (CheckAnswer(colour.Red))
        {
            Debug.Log("Correct answer!");
            CorrectAnswer();
        }
        else
        {
            Debug.Log("wrong answer :(");
            WrongAnswer();
        }
    }

    public void BluePressed()
    {
        if (CheckAnswer(colour.Blue))
        {
            Debug.Log("Correct answer!");
            CorrectAnswer();
        }
        else
        {
            Debug.Log("wrong answer :(");
            WrongAnswer();
        }
    }

    public void YellowPressed()
    {
        if (CheckAnswer(colour.Yellow))
        {
            Debug.Log("Correct answer!");
            CorrectAnswer();
        }
        else
        {
            Debug.Log("wrong answer :(");
            WrongAnswer();
        }
    }

    public void MagentaPressed()
    {
        if (CheckAnswer(colour.Magenta))
        {
            Debug.Log("Correct answer!");
            CorrectAnswer();
        }
        else
        {
            Debug.Log("wrong answer :(");
            WrongAnswer();
        }
    }
    #endregion
}
