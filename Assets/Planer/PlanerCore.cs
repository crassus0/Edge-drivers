using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlanerCore : CustomObject
{
  public static float BasicScale = 1.2f;
  uint id;

  bool m_initislized = false;
  //public int energy=2;

  public int agility;

  //	float time=1;
  public PlanerVisualControls m_visualiser;
  bool m_isPlayer = false;
  [SerializeField]
  public int direction = 0;
  PlanerMoveControls m_moveControls;
  BasicPlanerAI m_basicAI;
  MineController m_mineController;
  Action<PlanerCore> m_updateFunc;
  bool m_renewedUpdater = false;
  public bool EnteredPortal;// { get; set; }


  public uint ID
  {
    get { return id; }
  }
  public int Energy { get { return m_moveControls.Energy; } set { m_moveControls.Energy = value; } }
  public ButtonObject[] Mines { get { return m_mineController.Mines; } }
  public GraphNode prevNode { get; set; }
  public PlanerMoveControls MoveControls
  {
    get { return m_moveControls; }
  }
  public PlanerVisualControls Visualiser
  {
    get { return m_visualiser; }
  }
  public int Agility
  {
    get
    {
      if (Application.isPlaying)
        return m_moveControls.Agility;
      else
        return agility;
    }
    set
    {
      if (Application.isPlaying)
        m_moveControls.Agility = value;
      else
        agility = value;
    }
  }
  public int Direction
  {
    get
    {
      if (Application.isPlaying)
        return m_moveControls.Direction;
      else
        return direction;
    }
    set
    {
      if (Application.isPlaying)
        m_moveControls.Direction = value;
      else
      {
        direction = value;
      }
    }
  }
  public int MaxRotateAngle { get { return m_moveControls.MaxRotationAngle; } }
  public int Stopped { get { return m_moveControls.Stopped; } }

  public void AddUpdateFunc(Action<PlanerCore> newUpdateFunc)
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
  public void RemoveUpdateFunc(Action<PlanerCore> newUpdateFunc)
  {

    m_updateFunc -= newUpdateFunc;

    if (m_updateFunc == null)
    {
      //Debug.Log("back");
      m_updateFunc = null;
      m_updateFunc = UpdateFuncBasic;
      m_renewedUpdater = false;
      //Debug.Log(m_updateFunc.Method.Name);
    }
  }
  public void OnLevelChange(int i) { m_basicAI.ChangeLevel(i); }
  public int AdditionalDirection() { return m_moveControls.AdditionalRotateDirection(); }
  public void OnDamageDealt(int damage)
  {
    if (!m_initislized) return;
    m_moveControls.HitEnergy(damage);
  }
  public void DestroyPlaner()
  {

    Destroy(this.gameObject);
  }


  public void Init()
  {
    //Debug.Log(direction);
    
    //bug.Log(Level);
    EnteredPortal = false;
    if (m_initislized) return;
    prevNode = GetNode();
    //	  Debug.Log(m_init);
    m_updateFunc = UpdateFuncBasic;
    m_isPlayer = (name == "Player");
    //	  Debug.Log("node set");
    //Awake();

    Type = ObjectType.Planer;
    id = Creator.GetID();
    m_moveControls = ScriptableObject.CreateInstance<PlanerMoveControls>();
    PlanerMoveControls.MoveParameters moveParameters;
    moveParameters.agility = agility;
    moveParameters.direction = direction;

    m_moveControls.Initialize(this, moveParameters);
    if (m_isPlayer)
      m_basicAI = ScriptableObject.CreateInstance<BasicPlanerAI>();
    else
      m_basicAI = ScriptableObject.CreateInstance<AdvancedPlanerAI>();
    m_basicAI.Init(this);
    m_mineController = ScriptableObject.CreateInstance<MineController>();
    m_mineController.Init(this);
    //Debug.Log(m_initislized);
    m_visualiser.Init(this);
    //m_visualiser.Init(this);
    //	  GetComponent<Catcher>().Init(this);
    m_initislized = true;
    //Debug.Log(m_initislized);

  }
  public void SetTarget(GraphNode x, int angle)
  {
    m_basicAI.SetTarget(x, angle);
  }
  public override void OnUpdate()
  {
    EnteredPortal = false;
    //Debug.Log(m_updateFunc.Method.Name);
    //Debug.Log("update");
    if (m_updateFunc != null)
      m_updateFunc(this);
  }
  public void UpdateFuncBasic(PlanerCore planer)
  {
    //Debug.Log(Node.GetDirections()[0]);

    m_basicAI.OnUpdate();
    m_moveControls.OnUpdate();
    m_mineController.OnUpdate();

  }
  public void Rotate(int angle)
  {
    m_moveControls.Rotate(angle);
  }
  public void SetNewDirection(int newDirection)
  {
    m_moveControls.SetNewDirection(newDirection);
  }
  void MUpdate()
  {
    Vector3 dir;
    //	  Debug.Log(Energy);

    //Node=GraphNode.GetNodeByCoords(transform.position, (int)Energy);
    //Vector3	newPosition=Node.NodeCoords();
    //newPosition.y=transform.position.y;
    bool[] directions = Node.GetDirections();
    bool[] planerDirections = m_moveControls.Directions();
    //	  transform.position=newPosition;
    for (int i = 0; i < 6; i++)
    {
      dir = new Vector3(Mathf.Cos(i * Mathf.PI / 3f), 0, Mathf.Sin(i * Mathf.PI / 3f));
      //		Debug.Log(direction+","+i);

      if (directions[i] && planerDirections[i])
        Debug.DrawRay(transform.position, dir, Color.green);
      else
        Debug.DrawRay(transform.position, dir, Color.red);
    }
  }
  void Start()
  {
    //	  SetFlags();
    Init();
  }
  public override void Interact(CustomObject obj, InteractType type)
  {
    PlanerCore planer = obj as PlanerCore;
    if (planer == null) return;

  }
  public void OnEnterPortal(GraphNode node)
  {
    //m_freese = 1;
    
    Node = node;
    
    //Debug.Log(node);
    m_basicAI.ChangeLevel(node.Level);
  }

  //TODO
}

