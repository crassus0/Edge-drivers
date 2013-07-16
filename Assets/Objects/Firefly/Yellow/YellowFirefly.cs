using UnityEngine;
using System.Collections;
public class YellowFirefly : CustomObject, IAutoMove
{
  bool init = false;
  public int Direction
  {
    get { return m_direction; }
    set
    {
      m_direction = (value+6)%6;
      
      transform.rotation = Quaternion.identity;
      transform.Rotate(0, -60*m_direction, 0);

    }
  }
  public int m_direction;
  public override int GetStepCount()
  {
    return 4;
  }
  public override void OnStart()
  {
    Direction = m_direction;
    OnUpdate = OnUpdated;
    Interact = OnInteract;
    init = true; 
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if (type == InteractType.Stay||!init) return;
    
    DistantPortalEnter x = obj as DistantPortalEnter;
    if (x != null)
    {
      if(x.Status==2)
        x.Status = 3;
      Destroy(gameObject);
    }
    IFireflyDestroyable dest = obj as IFireflyDestroyable;
    if (dest != null&&!obj.Destroyed)
    {
      dest.FireflyDestroy(this);
    }
  }
  void OnUpdated()
  {
    Move();
  }
  public void Move()
  {
    Move(Direction);
    (m_visualiser.GetComponent<FireflyVisualiser>()).Move((Node.Index+Direction % 2)%2);
    if (Camera.main.WorldToViewportPoint(transform.position).magnitude > 4 && !Node.IsOnField())
      Destroy(gameObject);
  }
  public bool CanRotateWithTag(NodeTag tag)
  {
    return true;
  }
}


