using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose:
/// The purpose of this script, is to hold the data of the different gamemodes
/// 
/// Date created: 13/12/21
/// Developer: Ben Ioannidis
/// 
/// Changelist:
/// -> 13/12/21 - Script created
/// -> 17/12/21 - Changed GameMode instances to have description too
/// -> 19/12/21 - Added AudioIsMuted bool which is set/checked in ToggleAudio.cs
/// </summary>
public static class GameModeData
{
    //Game mode struct and required parameters
    public struct GameMode
    {
        public GameMode(string name, int goal, string desc)
        {
            modeName = name;
            scoreGoal = goal;
            description = desc;
        }
        public string modeName;
        public int scoreGoal;
        public string description;
    }

    //Creation of the 2 game modes
    public static GameMode speed10Round = new GameMode("Speed Round", 10, "Get to 10 as fast as you can!");
    public static GameMode survive = new GameMode("Survive", 0, "Answer correctly to keep time on the clock!");

    /// <summary>
    /// Set the current game mode by referencing the game mode array (m_GameModes)
    /// </summary>
    public static GameMode CurrentGameMode { get; set; }

    public static GameMode[] m_GameModes = { speed10Round, survive };

    /// <summary>
    /// Get list of available game modes
    /// </summary>
    /// <returns>GameModeData.GameMode[] - the list of available game modes</returns>
    public static GameMode[] GetGameModes()
    {
        return m_GameModes;
    }

    //toggled via ToggleAudio.cs
    public static bool AudioIsMuted = false;
}
