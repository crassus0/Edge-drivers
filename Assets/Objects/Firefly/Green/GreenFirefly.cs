using UnityEngine;
using System.Collections;

public class GreenFirefly : CustomObject, IAutoMove
{
  public int Direction
  {
    get;
    set;
  }
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
      (m_visualiser as FireflyVisualiser).Move((Node.Index + Direction % 2) % 2);
    }
    else
    {
      int newDirection = GetNode().GetHitDirection(Direction);
      int changeDirection = (newDirection + 3) % 6 - Direction;
      if (changeDirection < -1) changeDirection += 6;
      if (changeDirection > 1) changeDirection %= 6;
      Direction = newDirection;
      transform.rotation = Quaternion.identity;
      transform.Rotate(new Vector3(0, -60 * Direction, 0));
      (m_visualiser as FireflyVisualiser).Hit((Node.Index + Direction % 2) % 2);
    }
  }
}
