using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkyRidingMono_ListOfCheckPoints : MonoBehaviour { 

    public List<SkyRidingCheckPointTag> m_checkPoints = new List<SkyRidingCheckPointTag>();

    public int GetCount()
    {
        return m_checkPoints.Count;
    }

    public List<Vector3> GetPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (SkyRidingCheckPointTag checkPoint in m_checkPoints)
        {
            positions.Add(checkPoint.transform.position);
        }
        return positions;
    }

    public List<SkyRidingMono_AbstractRadius> GetRadius()
    {
        List<SkyRidingMono_AbstractRadius> radius = new List<SkyRidingMono_AbstractRadius>();
        foreach (SkyRidingCheckPointTag checkPoint in m_checkPoints)
        {
            SkyRidingMono_AbstractRadius r = checkPoint.GetComponent<SkyRidingMono_AbstractRadius>();
            if(r == null)
                r = checkPoint.GetComponentInChildren<SkyRidingMono_AbstractRadius>();
            radius.Add(r);
        }
        return radius;
    }

    public List<SkyRidingCheckPointTag> GetTags()
    {
        return m_checkPoints.ToList();
    }

    public void SetCheckPoints(List<SkyRidingCheckPointTag> tags)
    {
        m_checkPoints = tags;
    }

    public SkyRidingCheckPointTag GetTag(int index)
    {
        if(index < 0 || index >= m_checkPoints.Count)
        {
            return null;
        }
        return m_checkPoints[index];
    }

    public SkyRidingCheckPointTag GetLastTag()
    {
        return m_checkPoints[m_checkPoints.Count - 1];
    }
    public SkyRidingCheckPointTag GetFirstTag()
    {
        return m_checkPoints[0];
    }
}
