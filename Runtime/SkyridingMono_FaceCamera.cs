using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyridingMono_FaceCamera : MonoBehaviour
{
    public Transform m_whatToRotate;
    public Camera m_cameraToFace;

    private void Reset()
    {
        m_whatToRotate = transform;
        m_cameraToFace = Camera.main;
    }
    private void OnValidate()
    {
        Refresh();
    }
    void Update()
    {
        Refresh();

    }

    private void Refresh()
    {
        if (m_cameraToFace == null)
        {
            m_cameraToFace = Camera.main;
        }
        if (m_cameraToFace == null)
        {
            return;
        }

        if (m_cameraToFace != null)
        {
            transform.LookAt(m_cameraToFace.transform);
        }
    }
}
