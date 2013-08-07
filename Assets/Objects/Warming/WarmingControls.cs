using UnityEngine;
using System;
using System.Collections;

public class WarmingControls : CustomObject, IPlanerLike
{
  public BasicPlanerAI m_ai { get; set; }
  System.Action<IPlanerLike> m_updateFunc;
  bool m_renewedUpdater = false;
  public bool isEaten { get; set; }
  public int Direction
  {
    get { return m_direction; }
    set { SetNewDirection(value); }
  }
  [SerializeField]
  int m_direction = 0;
  public override int GetStepCount()
  {
    return 2;
  }

  public float m_warmingConcentration;
  public int MaxRotateAngle
  {
    get { return m_maxRotation; }
    set { m_maxRotation = value; }
  }
  [SerializeField]
  int m_maxRotation;
  public int Stopped
  {
    get { return m_stopped; }
    set { m_stopped = value; }
  }
  [SerializeField]
  int m_stopped;
  public void SetNewDirection(int dir)
  {
    //Debug.Log("adafdsfasd");
    int oldDirection=Direction;

    m_direction = dir;
    float rotateAng = (oldDirection - dir)*60;
    transform.Rotate(new Vector3(0, rotateAng, 0));
    m_visualiser.transform.Rotate(new Vector3(0, -rotateAng, 0));
  }
  public override void OnStart()
  {
    OnUpdate = OnUpdated;
    m_updateFunc = OnUpdated;
    Interact = OnInteract;
  }
  public void Init()
  {
    OnStart();
    m_ai = ScriptableObject.CreateInstance<BasicPlanerAI>();
    m_ai.Init(this);
  }

  void OnUpdated()
  {
    m_updateFunc(this);
  }
  void OnUpdated(IPlanerLike warming)
  {
    
    (m_visualiser.GetComponent<WarmingVisualiser>()).Rewind();
    OnMove();
  }
  void Update()
  {

  }
  public void OnMove()
  {
    //Debug.Log("move");
    if (isEaten)
      Creator.DestroyObject(this);
    m_ai.OnUpdate();
    int index = (Node.Index + Direction % 2) % 2;
    while (GraphTagMachine.GetDirections(Node)[m_direction]==WayStatus.Blocked)
    {
      if (index == 0)
      {
        m_direction = (m_direction + 2) % 6;
        transform.Rotate(new Vector3(0, -120, 0));
        m_visualiser.transform.Rotate(new Vector3(0, 120, 0));
      }
      else
      {
        m_direction = (m_direction + 4) % 6;
        transform.Rotate(new Vector3(0, 120, 0));
        m_visualiser.transform.Rotate(new Vector3(0, -120, 0));
      }

    }
    Move(m_direction);

    index = (Node.Index + Direction % 2) % 2;


    (m_visualiser.GetComponent<WarmingVisualiser>()).PassiveAnimations();
    (m_visualiser.GetComponent<WarmingVisualiser>()).Move(index);
    m_ai.CheckTarget();
  }
  public void AddUpdateFunc(Action<IPlanerLike> newUpdateFunc)
  {
    //Debug.Log("add");
    if (!m_renewedUpdater)
    {

      m_updateFunc = newUpdateFunc;
      m_renewedUpdater = true;
    }
    else
    {
      m_updateFunc += newUpdateFunc;
    }

  }
  public void RemoveUpdateFunc(Action<IPlanerLike> newUpdateFunc)
  {
    m_updateFunc -= newUpdateFunc;

    if (m_updateFunc == null)
    {
      //Debug.Log("back");
      m_updateFunc = null;
      m_updateFunc = OnUpdated;
      m_renewedUpdater = false;
      //Debug.Log(m_updateFunc.Method.Name);
    }
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if (type == InteractType.Enter)
    {
      WarmingHole x = obj as WarmingHole;
      if (x != null && !m_ai.HasTarget)
      {
        Creator.DestroyObject(this);
      }
    }
  }
  public float EntityValue(CustomObject x)
  {
    //if(x.Node!=m_ai.Target.node)
      //return BasicPlanerAI.MaxWeight;
    return 0;
  }
  public bool CanRotateWithTag(NodeTag tag)
  {
    return true;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		throw new NotImplementedException ();
	}
}
