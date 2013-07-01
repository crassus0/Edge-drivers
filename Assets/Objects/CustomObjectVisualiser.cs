using UnityEngine;
using System.Collections;

public abstract class CustomObjectVisualiser : MonoBehaviour 
{
  protected void Awake()
  {

  }
  public void SetSpeed()
  {
    float speed=1 / Creator.creator.turnDuration;
    if (Creator.OnPause)
      speed = 0;
    foreach (AnimationState x in animation)
    {
      x.speed = speed;
    }
  }
  
}
