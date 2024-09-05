using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRidingMono_DrawRacePath : MonoBehaviour
{

    public SkyRidingMono_ListOfCheckPoints m_checkPoints;
    public bool m_isLoopRace = false;
    public Color m_debugColor=Color.red;

    public LineRenderer m_lineRenderer;
    public float m_designerDilatation = 1;

    public float m_lineSpeed = 0.1f;


    public bool m_usePlayerPosition;
    public Transform m_playerPosition;


    //public void OnValidate()
    //{
    //    RefreshLineDrawing();
    //}

    public void Update()
    {
        RefreshLineDrawing();

        Material mat = m_lineRenderer.material;

        mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + -Time.deltaTime * m_lineSpeed, mat.mainTextureOffset.y);

    }

    public List<Vector3> m_whatToDraw = new List<Vector3>();

    [ContextMenu("Refresh")]
    private void RefreshLineDrawing()
    {
        if (m_checkPoints == null)
        {
            return;
        }

        if (m_checkPoints.GetCount() <= 0) {

            m_lineRenderer.enabled = false;
            return;
        }
        else
        {
            m_lineRenderer.enabled = true;
        }

        m_whatToDraw.Clear();
        if(m_usePlayerPosition && m_playerPosition!=null)
        {
            
            m_whatToDraw.Add(m_playerPosition.position);
        }
        m_whatToDraw.AddRange(m_checkPoints.GetPositions());


        for (int i = 0; i < m_whatToDraw.Count; i++)
        {
            if (i == m_whatToDraw.Count - 1 && m_isLoopRace)
            {
                Debug.DrawLine(m_whatToDraw[i], m_whatToDraw[0], m_debugColor);
            }
            else if (i < m_whatToDraw.Count - 1)
            {
                Debug.DrawLine(m_whatToDraw[i], m_whatToDraw[i + 1], m_debugColor);
            }
        }

        if (m_lineRenderer != null)
        {
            m_lineRenderer.positionCount = m_whatToDraw.Count;
            for (int i = 0; i < m_whatToDraw.Count; i++)
            {
                m_lineRenderer.SetPosition(i, m_whatToDraw[i]);
            }
            if (m_isLoopRace)
            {
                m_lineRenderer.positionCount = m_whatToDraw.Count + 1;
                m_lineRenderer.SetPosition(m_whatToDraw.Count, m_whatToDraw[0]);
            }
        }
        m_lineRenderer.textureScale= new Vector2(GetDistanceOfPath()*m_designerDilatation, 1f);
    }

    public float GetDistanceOfPath() { 
    
        float distance = 0f;
        for (int i = 0; i < m_whatToDraw.Count; i++)
        {
            if (i == m_whatToDraw.Count - 1 && m_isLoopRace)
            {
                distance += Vector3.Distance(m_whatToDraw[i], m_whatToDraw[0]);
            }
            else if (i < m_whatToDraw.Count - 1)
            {
                distance += Vector3.Distance(m_whatToDraw[i], m_whatToDraw[i + 1]);
            }
        }
        return distance; 

    }

}
