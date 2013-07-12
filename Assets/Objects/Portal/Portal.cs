using UnityEngine;
using System.Collections;

using System;
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

}
