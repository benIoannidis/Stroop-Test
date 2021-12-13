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
        public GameMode(string name, int goal)
        {
            modeName = name;
            scoreGoal = goal;
        }
        public string modeName;
        public int scoreGoal;
    }

    public static GameMode speed10Round = new GameMode("SpeedRound", 10);
    public static GameMode survive = new GameMode("Survive", 0);

    public static GameMode CurrentGameMode { get; set; }

    public static GameMode[] m_GameModes = { speed10Round, survive };

    public static GameMode[] GetGameModes()
    {
        return m_GameModes;
    }


}
