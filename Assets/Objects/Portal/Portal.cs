using UnityEngine;
using System.Collections;

using System;
[ExecuteInEditMode]
[Serializable]
public class Portal : CustomObject, IPortalExit
{
  public Portal PairPortal;
  public int Direction
  {
    get{return m_direction;}
    set{m_direction=value;}
  }
  [SerializeField]
  int m_direction=-1;
  public override void OnStart()
  {

    Interact = OnInteract;
    
  }
  
  void OnInteract(CustomObject obj, InteractType type)
  {
    if (PairPortal == null) return;
    if (type == InteractType.Stay)
    {
      PlanerCore x = obj as PlanerCore;

      if (x != null&&!x.EnteredPortal&&!(x.State==1))
      {
        x.AddUpdateFunc(OnPlanerEnter);
        x.HasTarget = true;
      }
    }
  }
  void OnPlanerEnter(IPlanerLike planer)
  {
    //Debug.Log(PairPortal);
    PlanerCore x = planer as PlanerCore;
    x.OnEnterPortal(PairPortal.GetNode(), PairPortal.Direction);
    x.RemoveUpdateFunc(OnPlanerEnter);
    x.EnteredPortal = true;
  }
  protected new void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    
    if(m_direction>=0)
    {
      
      Gizmos.color=Color.yellow;
      float ang = (1f/3f)*Mathf.PI* m_direction;  
      Vector3 dest=Vector3.right*Mathf.Cos (ang)+Vector3.forward*Mathf.Sin(ang);
      //Debug.Log(dest);
      Gizmos.DrawRay(transform.position, dest*16);
    }
  }
	public override CustomObjectInfo SerializeObject ()
	{
		PortalInfo x = new PortalInfo();
		x.PairPortal=PairPortal.ObjectID;
		x.Direction=Direction;
		x.BasicSerialization(this);
		return x;
	}
	public override Type SerializedType ()
	{
		return typeof(PortalInfo);
	}
}

[System.Serializable]
public class PortalInfo:CustomObjectInfo
{
	public int PairPortal;
  public int Direction;
	Portal portal;
	public override CustomObject Deserialize ()
	{
		portal= CreateInstance() as Portal;
	  portal.Direction=Direction;
		return portal;
	}
	public override void EstablishConnections ()
	{
		portal.PairPortal=GetObjectByID(PairPortal) as Portal;
	}
	public override string GetName ()
	{
		return "Portal";
	}
}