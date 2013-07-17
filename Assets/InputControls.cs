using UnityEngine;
using System.Collections;

public class InputControls : MonoBehaviour
{
  // Use this for initialization
  GameArrowsControls m_gameArrows;
  Vector3 m_targetPosition;
  Vector3 m_screenPosition;
  Vector2 prevTouch;
  Vector2 prevTouch2;
  bool m_hasTarget = false;
  float screenSensivity = 0.05f;
  public void Start()
  {
    m_gameArrows = GameObject.Find("GameArrows").GetComponent<GameArrowsControls>();
  }
  void Update()
  {

    if (Creator.OnPause)
    {

      return;
    }
    GetMouseInput();
    GetScroll();
#if UNITY_EDITOR||UNITY_STANDALONE
    GetKey();
#endif
    //	if(Mathf.Abs(x)>0.0001)
    //		  controledPlaner.

  }
#if UNITY_EDITOR||UNITY_STANDALONE
  void GetKey()
  {

    if (Input.GetKeyDown(KeyCode.RightArrow))
      ArcadeControlButton.KeyPressed(1);
    if (Input.GetKeyDown(KeyCode.LeftArrow))
      ArcadeControlButton.KeyPressed(-1);
  }
#endif
  void GetMouseInput()
  {

    if (Input.touchCount >= 2)
    {
      if (m_gameArrows.IsActive)
        m_gameArrows.Disappear();
      return;
    }
    if (Input.GetMouseButtonDown(0) && CheckClick(false))
    {
      if (Creator.Player.State == 1) return;
      m_targetPosition = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
      m_targetPosition.y = 0;
      GraphNode targetNode = GraphNode.GetNodeByCoords(m_targetPosition, Creator.Level);
      m_gameArrows.transform.position = targetNode.NodeCoords();
      m_hasTarget = true;

      m_gameArrows.transform.localScale = Vector3.one * Camera.main.GetComponent<CameraControls>().CameraSize / 10;
      m_gameArrows.Appear();
      //Debug.Log("press");
    }

    if (Input.GetMouseButtonUp(0) && CheckClick(true) && m_hasTarget)
    {
      if (Creator.Player.State == 1) return;
      GraphNode x = GraphNode.GetNodeByCoords(m_targetPosition, (int)Creator.Level);
      m_hasTarget = false;
      Creator.Player.SetTarget(x, GetTargetAngle());
      m_gameArrows.Disappear();
      //Debug.Log("unPress");
    }
    else if (m_hasTarget)
    {
      //m_gameArrows.transform.localScale=Vector3.one;
      m_gameArrows.SetActive(GetTargetAngle());

    }
    if (Input.GetMouseButtonUp(1))
      Debug.Break();
  }
  int GetTargetAngle()
  {
    Vector3 coords = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(m_gameArrows.transform.position);
    coords.z = 0;
    if (coords.magnitude > screenSensivity)
    {
      float ang360 = Vector3.Angle(coords, Vector3.right);
      if (coords.y < 0)
        ang360 = 360 - ang360;

      int ang = ((int)(ang360 / 60 + 0.5f)) % 6;
      return (ang);

    }
    else
    {
      return (-1);
    }
  }

  void GetScroll()
  {
    float scaler = 0;
#if UNITY_EDITOR||UNITY_STANDALONE
    scaler = Input.GetAxis("MouseScrollWheel");
#endif

#if UNITY_ANDROID

      if (Input.touchCount != 2)
      {
        prevTouch=Vector2.zero;
        prevTouch2 = Vector2.zero;
        return;
      }
    Touch touch= Input.GetTouch(0);
    Touch touch2 = Input.GetTouch(1);
    // Find out how the touches have moved relative to eachother:
    Vector2 curDist = touch.position - touch2.position;
    Vector2 prevDist = (prevTouch - prevTouch2) ;
    scaler = 0;
    prevTouch = touch.position;
    prevTouch2 = touch2.position;
    if(prevDist.magnitude>0)
      scaler = curDist.magnitude - prevDist.magnitude;

#endif
    //Debug.Log(scaler);

    Camera.main.GetComponent<CameraControls>().OnMouseScrollWheel(scaler);
  }
  bool CheckClick(bool isUp)
  {
    
    GUIElement button = Camera.main.GetComponent<GUILayer>().HitTest(Input.mousePosition);
    if (button != null)
    {
      IButton controls = button.GetComponent<MonoBehaviour>() as IButton;
      if (controls != null)
        controls.OnPressed(isUp);
      if (isUp)
        Armory.HideUpgrades();
      return false;
    }
    if (isUp)
      Armory.HideUpgrades();
    return true;
  }
  //TODO
}
