using UnityEngine;
using System.Collections;

public class FireflyVisualiser: CustomObjectVisualiser
{


  public void Move(int index)
  {
    animation.Play("MoveI" + index);
  }
  public void Hit(int index)
  {
    animation.Play("HitI" + index);
  }

}
