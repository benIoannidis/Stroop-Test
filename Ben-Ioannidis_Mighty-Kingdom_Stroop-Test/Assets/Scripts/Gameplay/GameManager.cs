using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        //Remove duplicate colours
        HashSet<colour> distinctColours = new HashSet<colour>(m_colours);

        //copy distinct list of colours to the colour list for use
        m_colours = new colour[distinctColours.Count];
        distinctColours.CopyTo(m_colours);

        //check current gamemode that has been set via the main menu
        //m_gameMode = GameModeData.CurrentGameMode.modeName;
        GameModeData.CurrentGameMode = GameModeData.speed10Round;
        m_gameMode = GameModeData.CurrentGameMode.modeName;
        m_goalScore = GameModeData.CurrentGameMode.scoreGoal;

        scoreText.text = m_currentScore + "/" + m_goalScore;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewRound();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine("StopWatch");
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
                otherColours.Add(c);
                otherColours.Add(c);
                otherColours.Add(c);
                otherColours.Add(c);
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

            milliseconds = ((time - (time) * 100));
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
                
            }
        }

        //survival game mode
        else
        {

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

        }
    }

    #region ButtonInteraction
    public void ExitPressed()
    {

    }

    public void PlayPressed()
    {

    }

    public void ResumePressed()
    {

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
