using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkyRidingMono_BestScore : MonoBehaviour
{

    public float m_bestRaceTime= 600;
    public float m_lastRaceTiming = 0;


    public UnityEvent<float> m_lastTimingChanged;
    public UnityEvent<float> m_bestTimingChanged;
    public UnityEvent<string> m_lastTimingChangedAsString;
    public UnityEvent<string> m_bestTimingChangedAsString;
 
    public void SetLastRaceTiminginSeconds(float timeInSeconds)
    {
        m_lastRaceTiming = timeInSeconds;
        bool bestRaceTimeChanged = m_lastRaceTiming < m_bestRaceTime;
        if(bestRaceTimeChanged)
        {
            m_bestRaceTime = m_lastRaceTiming;
        }

        m_lastTimingChanged.Invoke(m_lastRaceTiming);
        m_lastTimingChangedAsString.Invoke("" + (int)m_lastRaceTiming);
        if(bestRaceTimeChanged)
        {
            m_bestTimingChanged.Invoke(m_bestRaceTime);
            m_bestTimingChangedAsString.Invoke(""+(int) m_bestRaceTime);
        }
    }
}
