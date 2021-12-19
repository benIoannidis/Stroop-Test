using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerFlash : MonoBehaviour
{
    private float minSize = 80;
    private TMP_Text m_text;
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
