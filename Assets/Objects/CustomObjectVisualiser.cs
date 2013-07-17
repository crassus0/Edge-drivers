using UnityEngine;
using System.Collections;

public class CustomObjectVisualiser : MonoBehaviour
{


  public void SetSpeed(int counts)
  {
    if (animation != null)
      foreach (AnimationState x in animation)
      {
        x.layer = counts;
        x.normalizedSpeed = (4 / (counts * x.length));
      }
  }
}
