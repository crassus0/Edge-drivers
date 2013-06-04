using UnityEngine;
using System.Collections;

public class InputControls : MonoBehaviour {
	// Use this for initialization
	GameArrowsControls m_gameArrows;
	Vector3 m_targetPosition;
    Vector3 m_screenPosition;
	bool m_hasTarget=false;
	float screenSensivity=0.02f;
	void Start()
	{
	  m_gameArrows=GameObject.Find("GameArrows").GetComponent<GameArrowsControls>();
	}
    void Update()
    {   
	    
		if(Creator.OnPause)
		{
		  
		  return;
		}
        GetMouseInput ();
		GetNewDirection();
		GetScroll();
		
	//	if(Mathf.Abs(x)>0.0001)
//		  controledPlaner.
        
    }
	void GetMouseInput()
	{

      if(Input.GetMouseButtonDown(0)&&CheckClick())
      { 
		m_targetPosition=Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		m_targetPosition.y=0;
		GraphNode targetNode=GraphNode.GetNodeByCoords(m_targetPosition, Creator.Level);
		m_gameArrows.transform.position=targetNode.NodeCoords();
		//m_screenPosition=Camera.main.WorldToViewportPoint(m_gameArrows.transform.position);
		//m_screenPosition.z=0;
		//Debug.Log(m_screenPosition);
        m_hasTarget=true;
		
		//m_gameArrows.transform.localScale=Vector3.one;
		m_gameArrows.Appear();
		//Debug.Log("press");
	  }

	  if(Input.GetMouseButtonUp(0)&&m_hasTarget)
	  {

		GraphNode x=GraphNode.GetNodeByCoords(m_targetPosition, (int)Creator.Level);  	
		m_hasTarget=false;
		Creator.Player.SetTarget(x, GetTargetAngle());
	    m_gameArrows.Disappear();
		//Debug.Log("unPress");
	  }
	  else if(m_hasTarget)
	  {
		//m_gameArrows.transform.localScale=Vector3.one;
		m_gameArrows.SetActive(GetTargetAngle());

	  }
	  if(Input.GetMouseButtonUp(1))
	    Debug.Break();
	}
	int GetTargetAngle()
	{
	  	Vector3 coords=Camera.main.ScreenToViewportPoint(Input.mousePosition)-Camera.main.WorldToViewportPoint(m_gameArrows.transform.position);
		coords.z=0;
		if(coords.magnitude>screenSensivity)
		{  
		  //Debug.Log(Camera.main.ScreenToViewportPoint(m_gameArrows.transform.position));
		  //Debug.Log(coords);
		  //Debug.Log(coords.magnitude);
		  float ang360=Vector3.Angle(coords, Vector3.right);
		  if(coords.y<0)
			ang360=360-ang360;

		  int ang=((int)(ang360/60+0.5f))%6;
		  return(ang);
		  
		}
		else
		{
		  return(-1);
		}
	}
	void GetScroll()
	{
	  float scaler=Input.GetAxis("MouseScrollWheel");
		//Debug.Log(scaler);

	  Camera.main.GetComponent<CameraControls>().OnMouseScrollWheel(scaler);
	}
    void GetNewDirection()
	{
	  
	  
	  

	}
	bool CheckClick()
	{
	  
	  GUIElement button= Camera.main.GetComponent<GUILayer>().HitTest(Input.mousePosition);
	  if(button!=null)
	  {
		GUIButtonControls controls=button.GetComponent<GUIButtonControls>();
		if(controls!=null)
		  controls.OnPressed();
		return false;
	  }
	  return true;
	}
	//TODO
}
