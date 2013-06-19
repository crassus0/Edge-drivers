using UnityEngine;
using System.Collections;

public class Portal : CustomObject 
{
  public Portal PairPortal;
  bool m_awaken;
  void Start()
  {
    if (m_awaken) return;
    m_awaken = true;

    
  }
  public override void OnUpdate()
  {
    
  }
  public override void Interact(CustomObject obj, InteractType type)
  {
    //Debug.Log("Interact");
    if (PairPortal == null) return;
    if (type == InteractType.Stay)
    {

      PlanerCore x = obj as PlanerCore;
      if (x != null&&!x.EnteredPortal)
      {
        x.RemoveUpdateFunc(OnPlanerEnter);
        x.AddUpdateFunc(OnPlanerEnter);
      }
    }
  }
  void OnPlanerEnter(PlanerCore x)
  {
    //Debug.Log(PairPortal);

    x.OnEnterPortal(PairPortal.GetNode());
    x.RemoveUpdateFunc(OnPlanerEnter);
    x.EnteredPortal = true;
  }

}
