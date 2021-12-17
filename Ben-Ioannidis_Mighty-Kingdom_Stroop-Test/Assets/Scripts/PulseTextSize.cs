using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulseTextSize : MonoBehaviour
{
    private TMPro.TMP_Text m_text;

    [SerializeField]
    [Range(20, 250)]
    private float m_maxTextSize;

    [SerializeField]
    [Range(20, 250)]
    private float m_minTextSize;

    [SerializeField]
    [Range(0.001f, 1f)]
    private float pulseRate;

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
