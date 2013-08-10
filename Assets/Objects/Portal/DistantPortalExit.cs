using UnityEngine;
using System.Collections;

public class DistantPortalExit : CustomObject, IPortalExit
{
  public int Direction
  {
    get{return m_direction;}
    set{m_direction=value;}
  }
  [SerializeField]
  int m_direction=0;
  public override void OnStart ()
  {
  }
  public DistantPortalSaveData GetData()
  {
    DistantPortalSaveData x;
    x.name = name;
    x.node = Node;
    x.portalID = ObjectID;
    x.direction=Direction;
    return x;
  }
  [System.Serializable]
  public struct DistantPortalSaveData
  {
    public int direction;
    public GraphNode node;
    public int portalID;
    public string name;
  }
  protected new void OnDrawGizmos()
  {
    base.OnDrawGizmos();
//    Debug.Log(transform.position-Vector3.up);
//    Debug.Log(transform.position-Vector3.up);
    
    Gizmos.DrawIcon(transform.position, "distantPortalExit.png");

//    Debug.Log(m_direction);
    if(m_direction>=0)
    {
      
      Gizmos.color=Color.yellow;
      float ang = (1f/3f)*Mathf.PI* m_direction;  
      Vector3 dest=Vector3.right*Mathf.Cos (ang)+Vector3.forward*Mathf.Sin(ang);
      //Debug.Log(dest);
      Gizmos.DrawRay(transform.position, dest*20);
    }
  }
	public override CustomObjectInfo SerializeObject ()
	{
		DistantPortalExitInfo x =  new DistantPortalExitInfo();
		x.Direction=Direction;
		x.BasicSerialization(this);
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(DistantPortalExitInfo);
	}
}

[System.Serializable]
public class DistantPortalExitInfo:CustomObjectInfo
{
	public int Direction;
	public override CustomObject Deserialize ()
	{
		DistantPortalExit x = CreateInstance() as DistantPortalExit;
		x.Direction=Direction;
		return x;
	}
	public override void EstablishConnections ()
	{
		
	}
	public override string GetName ()
	{
		return "DistantPortalExit";
	}
}