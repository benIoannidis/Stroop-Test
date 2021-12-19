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
/// -> 
/// </summary>
public static class GameModeData
{
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

    public static GameMode speed10Round = new GameMode("Speed Round", 10, "Get to 10 as fast as you can!");
    public static GameMode survive = new GameMode("Survive", 0, "Answer correctly to keep time on the clock!");

    public static GameMode CurrentGameMode { get; set; }

    public static GameMode[] m_GameModes = { speed10Round, survive };

    public static GameMode[] GetGameModes()
    {
        return m_GameModes;
    }

    public static bool AudioIsMuted = false;
}
