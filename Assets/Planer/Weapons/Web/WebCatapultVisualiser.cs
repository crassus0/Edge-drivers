using UnityEngine;
using System.Collections;

public class WebCatapultVisualiser : MonoBehaviour 
{
  void OnHit()
  {
	animation.localBounds = new Bounds(Vector3.zero, Vector3.zero);
	animation.Play("Hit");
	
  }
  public void Push(PlanerCore parent)
  {
	Vector3 targetVector=transform.position-parent.Visualiser.transform.position;
	  float dist=targetVector.magnitude/transform.parent.localScale.x;
	  transform.parent.rotation=Quaternion.identity;
	  transform.parent.localScale=transform.parent.localScale*dist;
	  transform.localScale=transform.localScale/dist;
	  float angle=Vector3.Angle(targetVector, Vector3.forward);
	  //Debug.Log(angle);
	  if(targetVector.x<0)
	    angle=-angle;
	  transform.parent.Rotate(new Vector3(0,angle,0));
	
	animation.Play("Push");
	//m_visualiser.animation.localBounds

  }
}
