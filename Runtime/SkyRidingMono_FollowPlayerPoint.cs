using UnityEngine;

public class SkyRidingMono_FollowPlayerPoint : MonoBehaviour
{

    public Transform m_whatToMove;
    public Transform m_whatToFollow;



    public bool m_useLateUpdate;


    private void Update()
    {
        if (m_useLateUpdate)
        {
            Move();
        }
    }

    void LateUpdate()
    {

        if (!m_useLateUpdate)
        {
            Move();
        }

    }

    private void Move()
    {

        if (m_whatToFollow == null)
        {

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                m_whatToFollow = player.transform;
        }
        if (m_whatToMove == null)
            return;
        if (m_whatToFollow == null)
            return;
        m_whatToMove.rotation = m_whatToFollow.rotation;
        m_whatToMove.position = m_whatToFollow.position;
    }
}
