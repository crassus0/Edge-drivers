using UnityEngine;
using System.Collections;

public class GlobalMapPortal : DistantPortal 
{
  public GameObject[] m_visualisers;
  public TerraformingMine[] m_path;
  public override void OnStart()
  {
    base.OnStart();
    int isActive = PlayerSaveData.GetSceneStatus(m_targetScene);
    if (isActive == 0)
    {
      Destroy(gameObject);
    }
    else if (isActive == 1)
    {
      Interact = null;
      Destroy(m_visualisers[0].gameObject);
    }
    else if (isActive == 3)
    {
      foreach (TerraformingMine x in m_path)
        Destroy(x.gameObject);
    }
  }

}
