using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkyRidingMono_CollisionWithTargetPoint : MonoBehaviour
{
    public Transform m_colliderPoint;
    public Transform m_targetPoint;
    public SkyRidingMono_AbstractRadius m_resetPoint;
    public SkyRidingMono_AbstractRadius m_target;

    public bool m_isInCollision;
    public UnityEvent m_onCollisionEnter;
    public UnityEvent<bool> m_onCollisionChanged;
    public UnityEvent m_onCollisionExit;

    public void Update()
    {
        if (m_resetPoint == null || m_target == null)
            return;

        float radiusOne=  m_resetPoint.GetRadius();
        float radiusTwo = m_target.GetRadius();
        float distance = Vector3.Distance(m_colliderPoint.position, m_targetPoint.position);
        bool inCollision = distance < radiusOne + radiusTwo;
        if (inCollision != m_isInCollision)
        {
            m_isInCollision = inCollision;
            OnCollisionChange(inCollision);
        }
     
        
    }

    private void OnCollisionChange(bool isInCollision)
    {
        if (isInCollision)
        {
            m_onCollisionEnter.Invoke();
        }
        m_onCollisionChanged.Invoke(isInCollision);
        if(!isInCollision)
        {
            m_onCollisionExit.Invoke();
        }
    }
}
