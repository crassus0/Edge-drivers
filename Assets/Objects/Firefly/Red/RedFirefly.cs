using UnityEngine;
using System.Collections;

public class RedFirefly : ScriptableObject
{
  int action=0;
  public void Rotate(int ang)
  {
    action=(int)System.Math.Sign(action);
    
  }
  public void Activate(PlanerCore planer)
  {
    planer.AddUpdateFunc(UpdatePlaner);
    planer.State=1;
    planer.m_visualiser.GetComponent<PlanerVisualControls>().SetSpeed(2);
    planer.CutInterface();
  }
  void UpdatePlaner(IPlanerLike planer)
  {
    PlanerCore x = planer as PlanerCore;
    x.MoveControls.Rotate(action);
    x.MoveControls.OnUpdate();
    if(x.MoveControls.isHit)
      OnHit(x);
  }
  void OnHit(PlanerCore planer)
  {
    planer.RemoveUpdateFunc(UpdatePlaner);
    planer.State=0;
    planer.m_visualiser.GetComponent<PlanerVisualControls>().SetSpeed(1);
    planer.InitInterface();
  }
}
