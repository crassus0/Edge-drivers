using UnityEngine;
using System.Collections;

public class PlanerMoveControls : ScriptableObject
{
  PlanerCore m_planer;

  int m_agility;
  int m_direction;
  int m_maxRotationAngle;
  int m_rotationAngle = 0;


  public int Agility
  {
    get { return m_agility; }
    set 
    {
      m_agility = value;
      m_maxRotationAngle = (6 - m_agility) / 3 + 1;
    }
  }
  public int Direction
  {
    get { return m_direction; }
    set
    {
      m_direction = value;
      m_planer.transform.rotation = Quaternion.identity;
      m_planer.transform.Rotate(new Vector3(0, -60 * m_direction, 0));
    }
  }
  public int MaxRotationAngle { get { return m_maxRotationAngle; } }
  public void Initialize(PlanerCore planer, MoveParameters parameters)
  {

    Agility = parameters.agility;
    m_direction = parameters.direction;
    m_planer = planer;
    m_planer.transform.rotation = Quaternion.identity;
    m_planer.transform.Rotate(new Vector3(0, -60 * m_direction, 0));
    

  }
  public void OnUpdate()
  {
    
      Move();
    
  }


  void Move()
  {
    m_planer.prevNode = m_planer.GetNode();
    m_direction %= 6;
    int rotatedAngle = m_rotationAngle;
    ApplyRotation();
    bool[] directions = m_planer.GetNode().GetDirections();
    if (directions[m_direction])
    {
      //		Debug.Log(m_hiroscope+","+rotated);
      m_planer.Move(m_direction);
      m_planer.Visualiser.Move(rotatedAngle);

    }
    else
    {
      int newDirection = m_planer.GetNode().GetHitDirection(m_direction);
      int changeDirection = (newDirection + 3) % 6 - m_direction;
      if (changeDirection < -1) changeDirection += 6;
      if (changeDirection > 1) changeDirection %= 6;
      m_direction = newDirection;
      m_planer.transform.rotation = Quaternion.identity;
      m_planer.transform.Rotate(new Vector3(0, -60 * m_direction, 0));
      m_planer.Visualiser.Hit(rotatedAngle);
    }
  }
  public bool[] Directions()
  {
    bool[] dirs = new bool[6];
    for (int i = 0; i < 6; i++)
      dirs[i] = false;
    int minDirection = (m_direction + 6 - m_maxRotationAngle);
    int maxDirection = (m_direction + 6 + m_maxRotationAngle);
    for (int i = minDirection; i <= maxDirection; i++)
    {
      int index = i % 6;
      dirs[index] = true;
    }
    return dirs;
  }
  public void SetNewDirection(int newDirection)
  {
    int angle = newDirection - m_direction;
    if (angle < 0)
      angle += 6;
    Rotate(angle);
  }
  public void Rotate(int angle)
  {
    m_rotationAngle += angle;
    if (Mathf.Abs(m_rotationAngle) > 3)
      m_rotationAngle -= 6 * (int)Mathf.Sign(m_rotationAngle);
    if (Mathf.Abs(m_rotationAngle) > m_maxRotationAngle)
      m_rotationAngle = (int)(m_maxRotationAngle * Mathf.Sign(m_rotationAngle));
  }

  void ApplyRotation()
  {
    m_direction += m_rotationAngle;
    m_direction = m_direction % 6;
    if (m_direction < 0)
      m_direction += 6;
    //m_planer.Planer.transform.rotation=Quaternion.identity;
    m_planer.transform.Rotate(new Vector3(0, -60 * m_rotationAngle, 0));
    m_rotationAngle = 0;
  }
  public struct MoveParameters
  {
    public int agility;
    public int direction;


  }
  //TODO
}
