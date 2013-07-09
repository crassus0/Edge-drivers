using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GlobalMap : DistantPortal
{
  public override void OnStart()
  {
    m_targetScene = Creator.PreviousLevel;
    PlayerSaveData.SaveDiscoveredScene(m_targetScene, 2);
    base.OnStart();
  }
}
