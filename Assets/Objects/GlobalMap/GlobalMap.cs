using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GlobalMap : DistantPortal
{
  public override void OnStart()
  {
    base.OnStart();
    m_targetScene = Creator.PreviousLevel;
  }
}
