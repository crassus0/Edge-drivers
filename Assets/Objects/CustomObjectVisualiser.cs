using UnityEngine;
using System.Collections;

public class CustomObjectVisualiser : MonoBehaviour
{


  public void SetSpeed(int counts)
  {
    if (GetComponent<Animation>() != null)
      foreach (AnimationState x in GetComponent<Animation>())
      {
        x.layer = counts;
        x.normalizedSpeed = (4 / (counts * x.length));
      }
  }
}
