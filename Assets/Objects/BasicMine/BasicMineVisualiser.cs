using UnityEngine;
using System.Collections;

public class BasicMineVisualiser : CustomObjectVisualiser
{
  public BasicMine m_Parent;
  public void OnInstall(PlanerCore parent)
  {

    //Debug.Log(parent);
    Vector3 targetVector = transform.position - parent.Visualiser.transform.position;
    float dist = targetVector.magnitude / transform.parent.localScale.x;
    if (dist == 0) return;
    transform.parent.rotation = Quaternion.identity;
    transform.parent.localScale = transform.parent.localScale * dist;
    transform.localScale = transform.localScale / dist;
    float angle = Vector3.Angle(targetVector, Vector3.forward);
    //Debug.Log(angle);
    if (targetVector.x < 0)
      angle = -angle;
    transform.parent.Rotate(new Vector3(0, angle, 0));

    //animation.GetClip("InstallMine").localBounds=new Bounds(Vector3.zero, new Vector3(dist,0,dist));

    GetComponent<Animation>().Play("InstallMine");
    //Debug.Log(angle);
    //Debug.Log(targetVector);
    //Debug.Log(parent.Visualiser.transform.position);
    //Debug.Break();
  }
  public void OnDestroyed()
  {
    GetComponent<Animation>().Play("DestroyMine");
  }
  public void OnDestroyFinished()
  {
    Destroy(gameObject);
    
  }

}
