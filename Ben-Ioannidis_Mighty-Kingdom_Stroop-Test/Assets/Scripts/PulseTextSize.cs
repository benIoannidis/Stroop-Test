using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Purpose:
/// The purpose of this class is to pulse the "Highscore!" text on the end game screen
/// 
/// Date created: 17/12/21
/// Developer: Ben Ioannidis
/// 
/// Changelist:
/// -> 17/12/21 - Script created
/// </summary>
public class PulseTextSize : MonoBehaviour
{
    //text to "pulse"
    private TMPro.TMP_Text m_text;

    //size limits
    [SerializeField]
    [Range(20, 250)]
    private float m_maxTextSize;

    [SerializeField]
    [Range(20, 250)]
    private float m_minTextSize;

    //"pulse" speed
    [SerializeField]
    [Range(0.001f, 1f)]
    private float pulseRate;

    //should be growing or shrinking
    private bool growing = true;

    private void Start()
    {
        m_text = GetComponent<TMP_Text>();
        if (m_minTextSize > m_maxTextSize)
        {
            float temp = m_minTextSize;
            m_minTextSize = m_maxTextSize;
            m_maxTextSize = temp;
        }
    }

    private void Update()
    {
        //grow the text by the specified iteration value
        if (growing)
        {
            if (m_text.fontSize < m_maxTextSize)
            {
                m_text.fontSize += pulseRate;
            }
            else
            {
                growing = false;
            }
        }
        //shrink the text by the specified iteration value
        else
        {
            if (m_text.fontSize > m_minTextSize)
            {
                m_text.fontSize -= pulseRate;
            }
            else
            {
                growing = true;
            }
        }
    }
}
