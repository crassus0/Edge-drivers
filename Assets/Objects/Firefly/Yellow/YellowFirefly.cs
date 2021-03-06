using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class YellowFirefly : CustomObject, IAutoMove
{
  
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
    
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if (type == InteractType.Stay) return;
    
    DistantPortalEnter x = obj as DistantPortalEnter;
    if (x != null)
    {
      if (x.defaultStatus == 0) return;
      if(x.defaultStatus==2)
        x.Status = 3;
      Creator.DestroyObject(this);
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
      Creator.DestroyObject(this);
  }
  public List<int> CanRotateWithTag(GraphNode node, int direction)
  {
		List<int>x = new List<int>();
		x.Add(direction);
    return x;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		throw new System.NotImplementedException ();
	}
	public override System.Type SerializedType ()
	{
		throw new System.NotImplementedException ();
	}
}


