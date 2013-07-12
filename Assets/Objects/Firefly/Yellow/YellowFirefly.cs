using UnityEngine;
using System.Collections;
public class YellowFirefly : CustomObject, IAutoMove
{
  public float energy=0.5f;
  public int Direction
  {
    get { return m_direction; }
    set
    {
      m_direction = value;
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
    m_visualiser.GetComponent<CustomObjectVisualiser>().SetSpeed(0.25f);
    OnUpdate = OnUpdated;
    Interact = OnInteract;
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {

    if (type == InteractType.Stay) return;
    DistantPortalEnter x = obj as DistantPortalEnter;
    if (x != null)
    {
      if(x.Status==2)
        x.Status = 3;
      Destroy(gameObject);
    }
    if (obj as PlanerCore != null)
    {
      (obj as PlanerCore).RemoveConcentration(energy);
      Direction = (Direction + 1) % 6;
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
}


