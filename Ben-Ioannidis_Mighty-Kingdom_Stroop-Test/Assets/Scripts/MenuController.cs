using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Purpose:
/// The purpose of this script, is to handle menu button clicks and game scene loading
/// 
/// Date created: 19/12/21
/// Developer: Ben Ioannidis
/// 
/// Changelist:
/// -> 19/12/21 - Script created
/// </summary>
public class MenuController : MonoBehaviour
{
    /// <summary>
    /// On "SpeedRound" button press event
    /// </summary>
    public void SpeedRoundStart()
    {
        GameModeData.CurrentGameMode = GameModeData.speed10Round;

        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// On "Survival" button press event
    /// </summary>
    public void SurvivalRoundStart()
    {
        GameModeData.CurrentGameMode = GameModeData.survive;
        
        SceneManager.LoadScene("MainScene");
    }
    
    /// <summary>
    /// On "Exit" button press event
    /// </summary>
    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}