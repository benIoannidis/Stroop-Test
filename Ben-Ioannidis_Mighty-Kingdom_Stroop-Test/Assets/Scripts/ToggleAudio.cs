using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Purpose:
/// The purpose of this script, is to handle muting/unmuting in menu and in-game
/// 
/// Date created: 19/12/21
/// Developer: Ben Ioannidis
/// 
/// Changelist:
/// -> 19/12/21 - Script created
/// </summary>
public class ToggleAudio : MonoBehaviour
{
    public Sprite unmuted, muted;

    private void Start()
    {
        //On scene load, set the mute button sprite according to saved information in GameModeData
        if (GameModeData.AudioIsMuted)
        {
            this.GetComponent<Image>().sprite = muted;
        }
        else
        {
            this.GetComponent<Image>().sprite = unmuted;
        }
    }

    /// <summary>
    /// Toggle audio and swap sprite for the mute button
    /// </summary>
    public void ToggleAudioSprite()
    {
        if (GameModeData.AudioIsMuted)
        {
            this.GetComponent<Image>().sprite = unmuted;
        }
        else
        {
            this.GetComponent<Image>().sprite = muted;
        }

        GameModeData.AudioIsMuted = !GameModeData.AudioIsMuted;
    }
}
