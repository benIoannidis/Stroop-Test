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
/// -> 17/12/21 - Added UI element references 
/// -> 17/12/21 - Created "landing screen" which shows gamemode and it's description
/// -> 17/12/21 - Created end-game state (game completed) - this included the highscore checking and setting
/// -> 17/12/21 - Created event functions for in-game menu actions
/// </summary>
public class GameManager : MonoBehaviour
{
    //Name of the current game mode
    private static string m_gameMode;

    //score text element
    public TMPro.TMP_Text scoreText;

    //player's current score
    private int m_currentScore;

    //used for the "Speed Round", not for survival
    private int m_goalScore;
    private bool gameOver;

    //text for timers
    public TMPro.TMP_Text timerText;
    public TMPro.TMP_Text answerTimerText;

    //timer variables
    private float time;
    private float milliseconds, seconds, minutes;

    //Colour text and enum for picking colours via inspector
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

    //UI elements
    public GameObject startMenu, pauseMenu;
    public TMPro.TMP_Text gameModeText, gameModeDescription;

    public GameObject endGamePanel, newHighscoreText;
    public TMPro.TMP_Text playerScoreText, highscoreText, winText;

    //Audio sources/clips
    public AudioClip correctSound, incorrectSound;
    private AudioSource correctSource, incorrectSource;

    private void Start()
    {
        //Remove duplicate colours
        HashSet<colour> distinctColours = new HashSet<colour>(m_colours);

        //copy distinct list of colours to the colour list for use
        m_colours = new colour[distinctColours.Count];
        distinctColours.CopyTo(m_colours);

        //check current gamemode that has been set via the main menu
        m_gameMode = GameModeData.CurrentGameMode.modeName;

        //if in the survival game mode, set the timer to 20 seconds and set the score text
        if (m_gameMode == GameModeData.survive.modeName)
        {
            time = 20f;
            scoreText.text = "Score: 0";
        }
        //if in the speed round, get the goal score, and set the score text to be " 0 / goalScore"
        else
        {
            m_goalScore = GameModeData.CurrentGameMode.scoreGoal;
            scoreText.text = m_currentScore + "/" + m_goalScore;
        }

        //Set the game mode name and description on the "landing screen"
        gameModeText.text = m_gameMode;
        gameModeDescription.text = GameModeData.CurrentGameMode.description;

        //hide currently unrequired UI elements
        pauseMenu.SetActive(false);
        endGamePanel.SetActive(false);

        //add the audio clips to the audio sources
        correctSource = this.gameObject.AddComponent<AudioSource>();
        correctSource.playOnAwake = false;
        correctSource.loop = false;
        correctSource.clip = correctSound;

        incorrectSource = this.gameObject.AddComponent<AudioSource>();
        incorrectSource.playOnAwake = false;
        incorrectSource.loop = false;
        incorrectSource.clip = incorrectSound;
    }

    private void Update()
    {
        //once the timer reaches 0, stop the timers and run game complete function
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

    /// <summary>
    /// Called on an answer being provided, will select a new colour, and colour name to display
    /// </summary>
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

        //give a higher chance of the text not matching the colour (but still leave the chance for it to match occassionally)
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

    /// <summary>
    /// Starts the stopwatch (counting up for speed round highscore times)
    /// </summary>
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

    /// <summary>
    /// Starts the timer (counting down for the survival lose condition (ie. timer reaches 0))
    /// </summary>
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
        //if audio is allowed, play the "correct" sound
        if (!GameModeData.AudioIsMuted)
        {
            correctSource.Play();
        }
        //speed round, add 1 to the current player score
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
                GameCompleted();
            }
        }

        //survival game mode, add 0.5 seconds to the timer and add 1 to the current score
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
        //generate a new colour and name
        NewRound();
    }

    /// <summary>
    /// Called when the user has answered incorrectly
    /// </summary>
    private void WrongAnswer()
    {
        //if audio is allowed, play the "incorrect" sound
        if (!GameModeData.AudioIsMuted)
        {
            incorrectSource.Play();
        }
        //speed round, subtract 2 from the current player score (if possible)
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

        //survival game mode, subtract 2 seconds from the timer
        else
        {
            answerTimerText.text = "- 00:02:00";
            answerTimerText.color = Color.red;
            answerTimerText.gameObject.SetActive(true);
            answerTimerText.GetComponent<TimerFlash>().FlashNewTime();

            time -= 2f;
        }
        //generate a new colour and name
        NewRound();
    }

    /// <summary>
    /// Compare answer given to the correct answer
    /// </summary>
    /// <param name="col">pass in the relevant colour based on button pressed</param>
    /// <returns>true if the button press matches the current colour</returns>
    private bool CheckAnswer(colour col)
    {
        if (col == m_currentColour)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Called once a end condition has been met (score reached, or timer reaches 0)
    /// </summary>
    private void GameCompleted()
    {
        //speed round, set the text on the end-game screen and check if a highscore has been obtained
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
        //survival round, set the text on the end-game screen, and check if a highscore has been obtained
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
    /// <summary>
    /// Pause button press event, freeze time and show the pause menu UI
    /// </summary>
    public void PausePressed()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Resume button press event, resume time and hide the pause menu UI
    /// </summary>
    public void ResumePressed()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Restart button press event, resume time and reload the scene
    /// </summary>
    public void RestartPressed()
    {
        time = 20;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Exit button press event, load the menu scene
    /// </summary>
    public void ExitPressed()
    {
        //load menu scene
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// Play button press event, start a new round, and start the stopwatch, or the timer for speed round, and survival round respectively
    /// </summary>
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
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Red button press event, check whether the answer was Red or not
    /// </summary>
    public void RedPressed()
    {
        if (CheckAnswer(colour.Red))
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    /// <summary>
    /// Blue button press event, check whether the answer was Blue or not
    /// </summary>
    public void BluePressed()
    {
        if (CheckAnswer(colour.Blue))
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    /// <summary>
    /// Yellow button press event, check whether the answer was Yellow or not
    /// </summary>
    public void YellowPressed()
    {
        if (CheckAnswer(colour.Yellow))
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    /// <summary>
    /// Magenta button press event, check whether the answer was Magenta or not
    /// </summary>
    public void MagentaPressed()
    {
        if (CheckAnswer(colour.Magenta))
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }
    #endregion
}
