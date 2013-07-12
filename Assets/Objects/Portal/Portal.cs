using UnityEngine;
using System.Collections;

using System;
[ExecuteInEditMode]
[Serializable]
public class Portal : CustomObject
{
  public Portal PairPortal;
  bool m_awaken;
  public int LeavingDirection
  {
    get{return m_direction;}
    set{m_direction=value;}
  }
  [SerializeField]
  int m_direction=-1;
  public override void OnStart()
  {
    if (m_awaken) return;
    m_awaken = true;
    Interact = OnInteract;
    
  }
  
  void OnInteract(CustomObject obj, InteractType type)
  {
    if (PairPortal == null) return;
    if (type == InteractType.Stay)
    {
      PlanerCore x = obj as PlanerCore;
      if (x != null&&!x.EnteredPortal)
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
    x.OnEnterPortal(PairPortal.GetNode(), PairPortal.LeavingDirection);
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
}
