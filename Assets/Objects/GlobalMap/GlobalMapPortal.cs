using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GlobalMapPortal : DistantPortalEnter
{
  public List<TerraformingMine> m_path;
	
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
      
    }
    if (isActive >= 1)
    {
      foreach (TerraformingMine x in m_path)
      {
        if (x != null)
          Creator.DestroyObject(x);
      }
      
    }

  }
	public override CustomObjectInfo SerializeObject ()
	{
	  GlobalMapPortalInfo x = new GlobalMapPortalInfo();
		x.m_targetNode=m_targetNode;
		x.m_targetPortalID=m_targetPortalID;
		x.m_targetScene=m_targetScene;
		x.direction=direction;
		x.node=Node;
		x.instanceID=ObjectID;
		x.m_path=m_path.ConvertAll<int>(y=>y.ObjectID);
		return x;
	}
	
}
[System.Serializable]
public class GlobalMapPortalInfo:DistantPortalEnterInfo
{
	public List<int> m_path;
	GlobalMapPortal portal;
	public override CustomObject Deserialize ()
	{
		GlobalMapPortal portal  = base.Deserialize() as GlobalMapPortal;
		return portal;
	}
	public override void EstablishConnections ()
	{
		portal.m_path=m_path.ConvertAll<TerraformingMine>(x=>GetObjectByID(x) as TerraformingMine);
	}
	public override string GetName ()
	{
		return "GlobalMapPortal";
	}
}