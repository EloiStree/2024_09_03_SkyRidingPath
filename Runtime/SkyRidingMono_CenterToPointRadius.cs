using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRidingMono_CenterToPointRadius : SkyRidingMono_AbstractRadius
{

    public Transform m_center;
    public Transform m_point;

    public override float GetRadius()
    {
        return Vector3.Distance(m_center.position, m_point.position);
    }

    private void Reset()
    {
        m_center = transform;

    }
}
