using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void SpeedRoundStart()
    {
        GameModeData.CurrentGameMode = GameModeData.speed10Round;

        SceneManager.LoadScene("MainScene");
    }

    public void SurvivalRoundStart()
    {
        GameModeData.CurrentGameMode = GameModeData.survive;
        
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}