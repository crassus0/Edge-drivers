using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GlobalMap : DistantPortalEnter
{
  public override void OnStart()
  {
    m_targetScene = Creator.PreviousLevel;
    PlayerSaveData.SaveDiscoveredScene(m_targetScene, 2);
    base.OnStart();
  }
}
