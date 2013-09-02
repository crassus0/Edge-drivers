using UnityEngine;
using System.Collections;

public class PlanerMoveControls : ScriptableObject
{
  PlanerCore m_planer;

  int m_agility;
  int m_direction;
  int m_maxRotationAngle;
  int m_rotationAngle = 0;
  public bool isHit=false;
	bool m_stay=false;
  public int Agility
  {
    get { return m_agility; }
    set 
    {
      m_agility = value;
      m_maxRotationAngle = (2 - m_agility) + 1;
    }
  }
  public int Direction
  {
    get { return m_direction; }
    set
    {
      m_direction = value%6;
			while(m_direction<0)
				m_direction+=6;
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
    isHit=false;
    m_planer.prevNode = m_planer.GetNode();
    m_direction %= 6;
    int rotatedAngle = m_rotationAngle;

    ApplyRotation();
    WayStatus[] directions = GraphTagMachine.GetDirections(m_planer.GetNode());
    if(m_agility==0&&m_stay)
		{
       m_planer.Visualiser.Stay();
		}
		else if(directions[m_direction]==WayStatus.Blocked)
    {
      isHit=true;
      int newDirection = m_planer.GetNode().GetHitDirection(m_direction);
      int changeDirection = (newDirection + 3) % 6 - m_direction;
      if (changeDirection < -1) changeDirection += 6;
      if (changeDirection > 1) changeDirection %= 6;
      m_direction = newDirection;
      m_planer.transform.rotation = Quaternion.identity;
      m_planer.transform.Rotate(new Vector3(0, -60 * m_direction, 0));
      m_planer.Visualiser.Hit(rotatedAngle);
    }
		else if(directions[m_direction]==WayStatus.Unavailable)
		{
			Direction=(3+Direction)%6;
			isHit=true;
		}
    else
    {
      m_planer.Move(m_direction);
      m_planer.Visualiser.Move(rotatedAngle);

    }
		Direction=GraphTagMachine.GetDirection(m_planer.Node, Direction);
		m_stay=false;
		
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
  public void SetNewDirection(int newDirection, bool forced)
  {
    if(!forced)
    {
      int angle = newDirection - m_direction;
      if (angle < 0)
        angle += 6;
      Rotate(angle);
    }
    else
    {
      Direction=newDirection;
    }
  }
  public void Rotate(int angle)
  {
    if (m_planer.CanRotateWithTag(m_planer.Node, m_direction).Count==1) return;
    m_rotationAngle += angle;
    if (Mathf.Abs(m_rotationAngle) > 3)
      m_rotationAngle -= 6 * (int)Mathf.Sign(m_rotationAngle);
    if (Mathf.Abs(m_rotationAngle) > m_maxRotationAngle)
      m_rotationAngle = (int)(m_maxRotationAngle * Mathf.Sign(m_rotationAngle));
  }
	public void Stay()
	{
		int ang=3;
//		Debug.Log("sasd");
		if((m_planer.GetNode().Index + m_planer.Direction % 2) % 2==1)
			ang=-3;
    Rotate(ang);
    m_stay=true;
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
