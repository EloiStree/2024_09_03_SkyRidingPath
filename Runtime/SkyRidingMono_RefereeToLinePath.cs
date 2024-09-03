using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRidingMono_RefereeToLinePath : MonoBehaviour
{

    public SkyRidingMono_RaceReferee m_refereeSource;
    public SkyRidingMono_ListOfCheckPoints m_listOfCheckPoints;

    
    public void OnEnable()
    {
        RefreshListOfNextCheckPointsForPlayer();
        m_refereeSource.AddListenerToCheckPointChanged(RefreshListOfNextCheckPointsForPlayer);
    }
    public void OnDisable()
    {
        m_refereeSource.RemoveListenerToCheckPointChanged(RefreshListOfNextCheckPointsForPlayer);
    }

    [ContextMenu("Refresh")]
    public void RefreshListOfNextCheckPointsForPlayer() { 
    
        m_refereeSource.GetNextRaceCheckPointLeft(out List<SkyRidingCheckPointTag> tags);
        m_listOfCheckPoints.SetCheckPoints(tags);
    }
}
