using UnityEngine;
using System.Collections;

public class YellowFireflySpawner : ObjectSpawner
{
  public int spin=1;
  public override void OnStart()
  {
    base.OnStart();
    transform.rotation=Quaternion.identity;
    transform.Rotate(new Vector3(0, Direction * -60, 0));
  }
  protected override void OnUpdated()
  {
    base.OnUpdated();
    if (step == 1)
    {
      transform.Rotate(new Vector3(0, spin * -60, 0));
      Direction = Direction + spin;
    }
  }
  protected new void OnDrawGizmos()
  { 
    Gizmos.color = new Color(0, 0, 0, 0);
    Gizmos.DrawSphere(transform.position, 20);
  }
}
