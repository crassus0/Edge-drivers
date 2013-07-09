using UnityEngine;
using System.Collections;

public class GreenFirefly : CustomObject, IAutoMove
{
  public int Direction
  {
    get { return m_direction;}
    set 
    {
      m_direction=value;
      transform.rotation = Quaternion.identity;
      transform.Rotate(new Vector3(0, -60 * m_direction, 0));
    }
  }
  [SerializeField]
  int m_direction;
  public override int GetStepCount()
  {
    return 1;
  }
  public override void OnStart()
  {
    OnUpdate = OnUpdated;
    Interact = OnInteract;
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    DistantPortal x = obj as DistantPortal;
    if (x != null)
    {
      x.Status = 1;
    }
  }
  void OnUpdated()
  {
    Move();
  }
  public void Move()
  {
    bool[] directions = GetNode().GetDirections();
    if (directions[Direction])
    {
      //		Debug.Log(m_hiroscope+","+rotated);
      Move(Direction);
      //Visualiser.Move(rotatedAngle);
      (m_visualiser.GetComponent<FireflyVisualiser>()).Move((Node.Index + Direction % 2) % 2);
    }
    else
    {
      int newDirection = GetNode().GetHitDirection(Direction);
      int changeDirection = (newDirection + 3) % 6 - Direction;
      if (changeDirection < -1) changeDirection += 6;
      if (changeDirection > 1) changeDirection %= 6;
      Direction = newDirection;

      (m_visualiser.GetComponent<FireflyVisualiser>()).Hit((Node.Index + Direction % 2) % 2);
    }
  }
}
