using UnityEngine;
using System.Collections;

public class GlobalMapPortal : DistantPortalEnter
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
    if (isActive == 1)
    {
      Interact = null;
      Destroy(m_visualisers[0].gameObject);
    }
    if (isActive >= 1)
    {
      foreach (TerraformingMine x in m_path)
      {
        if(x!=null)
          Destroy(x.gameObject);
      }
      
    }

  }

}
