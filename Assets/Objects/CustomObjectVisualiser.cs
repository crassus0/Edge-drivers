using UnityEngine;
using System.Collections;

public class CustomObjectVisualiser : MonoBehaviour 
{

  public virtual void SetSpeed(float speed)
  {
    foreach (AnimationState x in animation)
    {
      x.speed = speed;
    }
  }
  
}
