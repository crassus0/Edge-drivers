using UnityEngine;
using System.Collections;

using System;
[Serializable]
public class Portal : CustomObject
{
  public Portal PairPortal;
  bool m_awaken;
  public override void OnStart()
  {
    if (m_awaken) return;
    m_awaken = true;
    Interact = OnInteract;
    
  }
  
  void OnInteract(CustomObject obj, InteractType type)
  {
    //Debug.Log("Interact");
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
    x.OnEnterPortal(PairPortal.GetNode());
    x.RemoveUpdateFunc(OnPlanerEnter);
    x.EnteredPortal = true;
  }

}
