using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Purpose:
/// The purpose of this class is to visualise time additions/subtractions to the player
/// 
/// Date created: 19/12/21
/// Developer: Ben Ioannidis
/// 
/// Changelist:
/// -> 19/12/21 - Script created
/// </summary>
public class TimerFlash : MonoBehaviour
{
    private float minSize = 80;
    private TMP_Text m_text;
    /// <summary>
    /// Call following the text component of this object being updated
    /// </summary>
    public void FlashNewTime()
    {
        m_text = GetComponent<TMPro.TMP_Text>();
        m_text.fontSize = 160;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Shrink the font size by 5px every frame until is reaches it's minimum specified size
        if (m_text.fontSize > minSize)
        {
            m_text.fontSize -= 5f;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
