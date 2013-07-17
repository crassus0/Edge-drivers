using UnityEngine;
using System.Collections;

public class FireflyVisualiser: CustomObjectVisualiser
{

  public void Move(int index)
  {
    
    animation.Play("MoveI" + index,PlayMode.StopSameLayer);
    //Debug.Log(transform.parent.name);
    //Debug.Log(animation["MoveI" + index].normalizedSpeed);
  }
  public void Hit(int index)
  {
    animation.Play("HitI" + index, PlayMode.StopSameLayer);
  }

}
