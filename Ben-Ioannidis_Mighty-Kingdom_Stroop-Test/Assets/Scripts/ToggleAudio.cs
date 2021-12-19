using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    public Sprite unmuted, muted;

    private void Start()
    {
        if (GameModeData.AudioIsMuted)
        {
            this.GetComponent<Image>().sprite = muted;
        }
        else
        {
            this.GetComponent<Image>().sprite = unmuted;
        }
    }

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
