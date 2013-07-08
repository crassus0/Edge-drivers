using UnityEngine;
using System.Collections;

public class CameraControls:MonoBehaviour
{
	public float maxCameraSize=300;

	float m_curentScale=1;
	float m_targetScale=1;
	float m_previousScale=1;
	Vector3 m_targetPosition;
	Vector3 m_basicPosition;
	Color m_targetLensColorA;
	Color m_curentLensColorA;
	float m_changeTimeColor;

	float m_timeToArtifact;
	float m_moveTime=1;
	float m_maxMoveTime=1;
//	static Material m_basicMaterial;

	public float CameraSize{get; set;}
	
	// Update is called once per frame
	public void OnMouseScrollWheel(float deltha)
	{
	  if(Mathf.Abs(deltha)>0.0001)
	  {
	    CameraSize-=0.3f*deltha;
	    if(Camera.main.GetComponent<CameraControls>().CameraSize>300)
	      Camera.main.GetComponent<CameraControls>().CameraSize=300;
		if(Camera.main.GetComponent<CameraControls>().CameraSize<100)
		  Camera.main.GetComponent<CameraControls>().CameraSize=100;
	  }
	}
	void Update () 
	{	
		
	  const float BasicScale=1.2f;
	  if(Mathf.Abs(m_curentScale-m_targetScale)<0.001)
		m_curentScale=m_targetScale;
	  else
		m_curentScale=(9*m_curentScale+m_targetScale)/10;
      
	 
	  Vector3 newScale = transform.localScale;
//	  newScale.z=
	  if(m_moveTime<m_maxMoveTime)
	  {
		m_moveTime+=Time.deltaTime/5;
		float alpha=1-(m_moveTime/m_maxMoveTime);
		transform.position=Vector3.Lerp(m_basicPosition, m_targetPosition, 1-alpha*alpha);
	    m_curentScale=(1-alpha*alpha)*m_targetScale+(alpha*alpha)*m_previousScale;
	  }
	  Camera cam=Camera.main;
	  cam.orthographicSize=CameraSize*BasicScale;
	  cam.transform.localScale=Vector3.one;
	  if(Camera.main.orthographicSize>maxCameraSize)
		Camera.main.orthographicSize=maxCameraSize;
	  newScale.z=Creator.ScreenSize.y/10;
	  newScale.x=Creator.ScreenSize.x/10;
	  //lens.transform.localScale=newScale;
	  m_timeToArtifact-=Time.deltaTime;

	  //if(changeTimeColor<1)
		//ChangeColor();

	}
	
	public void SetNewTargetPosition(Vector3 targetPosition, float scale, float speedIndex)
	{
    //Debug.Log(scale);
	  m_targetPosition=targetPosition;
	  m_targetPosition.y=5;
	  m_maxMoveTime=speedIndex;
	  m_previousScale=m_curentScale;
	  m_targetScale=scale;
	  m_basicPosition=transform.position;
	  m_moveTime=0;
	}
	public void ForceSetPosition(Vector3 position)
	{
	  position.y=5;
	  transform.position=position;
    m_basicPosition = position;
	}
	public void Init()
	{
	  
//	  m_basicMaterial = new Material(Shader.Find ("Transparent/Diffuse"));
	  CameraSize=160;
	}
}
