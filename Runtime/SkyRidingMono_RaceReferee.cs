using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkyRidingMono_RaceReferee : MonoBehaviour
{
    public SkyRidingMono_ListOfCheckPoints m_raceFromCheckPoints;

    public CheckPointStatus[] m_checkPointStatus;
    public PlayerWhenInCheckPoint[] m_playerWhenInCheckPoints;

    public Transform m_playerCenterPoint;
    public SkyRidingMono_AbstractRadius m_playerCollisionRadius;


    public int m_playerCheckPointIndex = -1;

    public SkyRidingCheckPointTag m_previous;
    public SkyRidingCheckPointTag m_next;



    void Update()
    {

        

        int nextIndex = m_playerCheckPointIndex + 1;
        if (nextIndex < m_checkPointStatus.Length)
        {
            Vector3 nextPosition = m_checkPointStatus[nextIndex].m_worldPosition;
            float nextRadius = m_checkPointStatus[nextIndex].m_colliderRadius;
            Vector3 playerPosition = m_playerCenterPoint.position;
            float playerRadius = m_playerCollisionRadius.GetRadius();

            if (Vector3.Distance(nextPosition, playerPosition) < nextRadius + playerRadius)
            {
                m_playerCheckPointIndex += 1;
                m_playerWhenInCheckPoints[nextIndex].SetAsCollided();

            }
        }
        else { 
        
            m_playerCheckPointIndex = m_checkPointStatus.Length-1;
        }
       

        SkyRidingCheckPointTag currentNextPoint = m_next;
        if (m_playerCheckPointIndex == -1)
        {
            m_previous = null;
            m_next = m_raceFromCheckPoints.GetFirstTag();
        }
        else if (m_playerCheckPointIndex >= m_raceFromCheckPoints.GetCount()-1)
        {
            m_previous = m_raceFromCheckPoints.GetLastTag();
            m_next = null;
        }

        else
        {
            m_previous = m_raceFromCheckPoints.m_checkPoints[m_playerCheckPointIndex];
            m_next = m_raceFromCheckPoints.m_checkPoints[(m_playerCheckPointIndex + 1) % m_raceFromCheckPoints.m_checkPoints.Count];
        }



        bool hasCheckPointChanged = currentNextPoint != m_next;
        if (hasCheckPointChanged)
        {

            m_percentComplete = (float)m_playerCheckPointIndex / (float)(m_raceFromCheckPoints.GetCount() - 1);
            m_isComplete = m_percentComplete >= 1f;
            CheckForEventOfStartAndEndRace();
            NotifyNextCheckPoint(m_next);
        }

    }


    public void DebugLogText(string text) { 
    
        Debug.Log("> Text: " + text, this.gameObject);
    }

    private void CheckForEventOfStartAndEndRace()
    {
        if (m_previous == null && m_next == m_raceFromCheckPoints.GetTag(0))
        {

            m_onRaceReadyStart.Invoke();
        }
        if (m_previous == m_raceFromCheckPoints.GetTag(0) && m_next == m_raceFromCheckPoints.GetTag(1))
        {
            m_onPlayerStartedRace.Invoke();
        }
        if (m_previous == m_raceFromCheckPoints.GetTag(m_raceFromCheckPoints.GetCount() - 1) && m_next == null)
        {
            DateTime start = m_playerWhenInCheckPoints[0].m_time;
            DateTime end = m_playerWhenInCheckPoints[m_playerWhenInCheckPoints.Length - 1].m_time;
            TimeSpan duration = end.Subtract(start);
            
            m_onNewRaceTimingInSeconds.Invoke((float)duration.TotalSeconds);
            m_onRaceComplete.Invoke();
        }
    }

    public float m_percentComplete = 0f;
    public bool m_isComplete;



    public UnityEvent m_onRaceReadyStart;
    public UnityEvent m_onPlayerStartedRace;
    public UnityEvent m_onRaceComplete;

    public UnityEvent<float> m_onNewRaceTimingInSeconds;

    public void GetNextRaceCheckPoint(out SkyRidingCheckPointTag previous, out SkyRidingCheckPointTag next)
    {
        previous = m_previous;
        next = m_next;
    }

    public void GetNextRaceCheckPointLeft(out List<SkyRidingCheckPointTag> leftCheckPoints)
    {

       leftCheckPoints = new List<SkyRidingCheckPointTag>();
        for (int i = m_playerCheckPointIndex + 1; i < m_raceFromCheckPoints.GetCount(); i++)
        {
            leftCheckPoints.Add(m_raceFromCheckPoints.m_checkPoints[i]);
        }
    }



    [System.Serializable]
    public class CheckPointStatus {

        public Vector3 m_worldPosition;
        public float m_colliderRadius;

    }

    [Serializable]
    public class PlayerWhenInCheckPoint {

        public string m_debugDateTime;
        public DateTime m_time;
        public bool m_hadCollisionWith;

        public void SetAsCollided() {
            m_hadCollisionWith = true;
            m_time = DateTime.UtcNow;
            m_debugDateTime = m_time.ToString();
        }

        internal void ResetToUnused()
        {
            m_hadCollisionWith = false;
            m_time = DateTime.UtcNow;
            m_debugDateTime = "";
        }
    }



    public void Awake()
    {
        ResetToInitialState();
    }

    [ContextMenu("Reset to initial state")]
    public void ResetToInitialState()
    {
        m_playerCheckPointIndex = -1;
        m_percentComplete = 0f;
        m_isComplete = false;

        ImportTagInfoToRawPoints();
        ResetRaceState();
    }

    private void ResetRaceState()
    {
        m_playerWhenInCheckPoints = new PlayerWhenInCheckPoint[m_raceFromCheckPoints.GetCount()];
        for (int i = 0; i < m_playerWhenInCheckPoints.Length; i++) {
            m_playerWhenInCheckPoints[i] = new PlayerWhenInCheckPoint();
        }
        foreach (var playerWhenInCheckPoint in m_playerWhenInCheckPoints) {
            playerWhenInCheckPoint.ResetToUnused();
        }
    }

    private void ImportTagInfoToRawPoints()
    {
        m_checkPointStatus = new CheckPointStatus[m_raceFromCheckPoints.GetCount()];
        for (int i = 0; i < m_checkPointStatus.Length; i++)
        {
            m_checkPointStatus[i] = new CheckPointStatus();
        }
        List<Vector3> points=     m_raceFromCheckPoints.GetPositions();
        List<SkyRidingCheckPointTag> tags = m_raceFromCheckPoints.GetTags();
        List<SkyRidingMono_AbstractRadius> radius = m_raceFromCheckPoints.GetRadius();
        for (int i = 0; i < points.Count; i++)
        {
            m_checkPointStatus[i] = new CheckPointStatus();
            m_checkPointStatus[i].m_worldPosition = points[i];
            m_checkPointStatus[i].m_colliderRadius = radius[i].GetRadius();
        }
    }

    public void NotifyNextCheckPoint( SkyRidingCheckPointTag skyRidingCheckPointTag)
    {
        m_onNextCheckPointChanged.Invoke(skyRidingCheckPointTag);
        if (m_onNextCheckPointChangedListener != null) { 
            foreach (var listener in m_onNextCheckPointChangedListener)
            {
                if(listener != null)
                    listener.Invoke();
            }
        }
    }


    public UnityEvent<SkyRidingCheckPointTag> m_onNextCheckPointChanged;

    public List<Action> m_onNextCheckPointChangedListener = new List<Action>();


    public void AddListenerToCheckPointChanged(Action onCheckPointChanged)
    {
        if (m_onNextCheckPointChangedListener == null)
            m_onNextCheckPointChangedListener = new List<Action>();

        m_onNextCheckPointChangedListener.Add(onCheckPointChanged);
    }
    public void RemoveListenerToCheckPointChanged(Action onCheckPointChanged)
    {
        if (m_onNextCheckPointChangedListener == null)
            m_onNextCheckPointChangedListener = new List<Action>();

        m_onNextCheckPointChangedListener.Remove(onCheckPointChanged);
    }
}
