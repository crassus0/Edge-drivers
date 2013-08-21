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
      Creator.DestroyObject(this);
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
		x.m_targetNode=new NodeInformation(m_targetNode);
		x.m_targetPortalID=m_targetPortalID;
		x.m_targetScene=m_targetScene;
		x.m_sceneStatus=defaultStatus;
		x.targetPortalName=m_targetPortalName;
		x.direction=direction;
		x.BasicSerialization(this);
		x.m_path=m_path.ConvertAll<int>(y=>y.ObjectID);
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(GlobalMapPortalInfo);
	}
}
[System.Serializable]
public class GlobalMapPortalInfo:DistantPortalEnterInfo
{
	public List<int> m_path;
	[System.NonSerialized]
	GlobalMapPortal portal;
	public override CustomObject Deserialize ()
	{
		portal  = base.Deserialize() as GlobalMapPortal;
		return portal;
	}
	public override void EstablishConnections ()
	{
		
		portal.m_path=m_path.ConvertAll<TerraformingMine>((x)=>GetObjectByID(x) as TerraformingMine);
	}
	public override string GetName ()
	{
		return "GlobalMapPortal";
	}
}
