using UnityEngine;
using System.Collections;

public class CustomObjectVisualiser : MonoBehaviour 
{

  public void SetSpeed(float speed)
  {
    foreach (AnimationState x in animation)
    {
      x.speed = speed;
    }
  }
  
}
