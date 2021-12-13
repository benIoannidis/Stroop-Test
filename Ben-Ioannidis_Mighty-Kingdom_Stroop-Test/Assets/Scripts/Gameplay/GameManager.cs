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
    private bool scoreReached;

    private colour m_currentColour;

    public colour[] m_colours;

    [System.Serializable]
    public enum colour
    {
        blue,
        cyan,
        green,
        magenta,
        red,
        yellow
    }

    private void Start()
    {
        //Remove duplicate colours
        HashSet<colour> distinctColours = new HashSet<colour>(m_colours);

        //copy distinct list of colours to the colour list for use
        m_colours = new colour[distinctColours.Count];
        distinctColours.CopyTo(m_colours);

        //check current gamemode that has been set via the main menu
        m_gameMode = GameModeData.CurrentGameMode.modeName;
    }

    private void CorrectAnswer()
    {
        GameModeData.GameMode[] gameModes = GameModeData.GetGameModes();

        if (m_gameMode == gameModes[0].modeName) //regular round (fast as you can to goal score (10))
        {
            if (m_currentScore < m_goalScore - 1)
            {
                m_currentScore++;
                scoreText.text = m_currentScore + "/" + m_goalScore;
            }
            else//game completed
            {
                //Show game ended screen with "You did it!"
                
            }
        }
        else //survival game mode
        {

        }
    }

    private void WrongAnswer()
    {
        GameModeData.GameMode[] gameModes = GameModeData.GetGameModes();

        if (m_gameMode == gameModes[0].modeName) //regular round (fast as you can to goal score(10))
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
        else //survival game mode
        {

        }
    }

    private bool CheckAnswer(colour col)
    {
        if (col == m_currentColour)
        {
            return true;
        }
        return false;
    }

    #region ButtonInteraction
    public void RedPressed()
    {
        if (CheckAnswer(colour.red))
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    public void BluePressed()
    {
        if (CheckAnswer(colour.blue))
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    public void YellowPressed()
    {
        if (CheckAnswer(colour.yellow))
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    public void MagentaPressed()
    {
        if (CheckAnswer(colour.magenta))
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
