using UnityEngine;
using System.Collections;

public abstract class CustomObjectVisualiser : MonoBehaviour 
{

  protected void Awake()
  {
    foreach (AnimationState x in animation)
    {
      x.speed = 1 / Creator.turnDuration;
    }
  }
  
}
