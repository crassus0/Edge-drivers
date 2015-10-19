using UnityEngine;
using System.Collections;

public class FireflyVisualiser: CustomObjectVisualiser
{

  public void Move(int index)
  {
    
    GetComponent<Animation>().Play("MoveI" + index,PlayMode.StopSameLayer);
    //Debug.Log(transform.parent.name);
    //Debug.Log(animation["MoveI" + index].normalizedSpeed);
  }
  public void Hit(int index)
  {
    GetComponent<Animation>().Play("HitI" + index, PlayMode.StopSameLayer);
  }

}
