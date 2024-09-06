using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkyRidingMono_BestScore : MonoBehaviour
{

    public float m_bestRaceTime= 600;
    public float m_lastRaceTiming = 0;
    public string m_scoreFormat = "{0:000.000}";

    public UnityEvent<float> m_lastTimingChanged;
    public UnityEvent<float> m_bestTimingChanged;
    public UnityEvent<string> m_lastTimingChangedAsString;
    public UnityEvent<string> m_bestTimingChangedAsString;


    [ContextMenu("Reset current best score")]
    public void ResetCurrentBestScore() { 
    
        m_bestRaceTime = 600;
        m_bestTimingChanged.Invoke(m_bestRaceTime);
        m_bestTimingChangedAsString.Invoke(string.Format(m_scoreFormat, m_bestRaceTime));
    }

    [ContextMenu("Set current as best score")]
    public void SetCurrentBestScoreToLastRace() { 
    
        ResetCurrentBestScore();
        SetLastRaceTimingInSeconds(m_lastRaceTiming);
    }

    public void SetLastRaceTimingInSeconds(float timeInSeconds)
    {
        m_lastRaceTiming = timeInSeconds;
        bool bestRaceTimeChanged = m_lastRaceTiming < m_bestRaceTime;
        if(bestRaceTimeChanged)
        {
            m_bestRaceTime = m_lastRaceTiming;
        }

        m_lastTimingChanged.Invoke(m_lastRaceTiming);
        m_lastTimingChangedAsString.Invoke(string.Format(m_scoreFormat, m_lastRaceTiming));
        if(bestRaceTimeChanged)
        {
            m_bestTimingChanged.Invoke(m_bestRaceTime);
            m_bestTimingChangedAsString.Invoke(string.Format(m_scoreFormat, m_bestRaceTime));
        }
    }
}
