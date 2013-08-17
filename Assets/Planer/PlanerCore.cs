using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
public class PlanerCore : CustomObject, IPlanerLike, IFireflyDestroyable
{
  public static float BasicScale = 1.2f;
  uint id;

  bool m_initislized = false;
  //public int energy=2;

  public int agility;

  GraphNode m_savedNode;
  int m_savedDirection;
  [SerializeField]
  int direction = 0;
  PlanerMoveControls m_moveControls;
  BasicPlanerAI m_basicAI;
  MineController m_mineController;
  Action<IPlanerLike> m_updateFunc;
  CancelAction m_cancelAction;
  HomePortal m_homePortal;
	StayButton m_stayButton;
  int m_hitPoints=maxHp;
  static readonly int maxHp = 3;
  int m_regenCooldown;
  public int State
  {
    get { return m_state;}
    set
    {
      m_state=value;
      if (m_state == 1)
        m_visualiser.GetComponent<PlanerVisualControls>().SetSpeed(2);
      if (m_state == 0)
        m_visualiser.GetComponent<PlanerVisualControls>().SetSpeed(4);
    }
  }
  int m_state=0;
  bool m_renewedUpdater = false;
  public bool EnteredPortal;// { get; set; }
  public bool HasTarget
  {
    get { return HasAction(); }
    set { m_hasTarget = value; }
  }
  bool m_hasTarget=false;
  public override void OnStart()
  {
  }
  public List<string> Upgrades
  {
    get { return m_upgrades; }
    set { m_upgrades = value; }
  }
  [SerializeField]
  List<string> m_upgrades=new List<string>();

  public uint ID
  {
    get { return id; }
  }
  public List<ButtonObject> Mines { get { return m_mineController.Mines; } }
  public GraphNode prevNode { get; set; }
  public PlanerMoveControls MoveControls
  {
    get { return m_moveControls; }
  }
  public PlanerVisualControls Visualiser
  {
    get
    {

      return m_visualiser.GetComponent<PlanerVisualControls>();
    }
  }
  public MineController MineController
  {
    get { return m_mineController; }
    set 
    {

      if (m_mineController != null)
        Destroy(m_mineController);
      m_mineController = value; 
    }
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
  public float Concentration
  {
    get { return m_concentration; }
    set { m_concentration = value; }
  }
  [SerializeField]
  float m_concentration=0.5f;
  public float MaxConcentration
  {
    get { return m_MaxConcentration; }
    set { m_MaxConcentration = value; }
  }
  [SerializeField]
  float m_MaxConcentration=1;
  static readonly int MaxConcentrationIncrement = 2;
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
      {
        SetNewDirection(value, true);
      }
      else
      {
        direction = value;
      }
    }
  }
  public int MaxRotateAngle { get { return m_moveControls.MaxRotationAngle; } }
  public void CancelTarget()
  {
    m_basicAI.SetTarget(null);
  }
  public void AddConcentration(float t)
  {
    float t1 = Mathf.Sqrt(1 / (MaxConcentration - Concentration)) + t;
    Concentration =MaxConcentration*(1- 1 / (t1 * t1));
  }
  public void RemoveConcentration(float t)
  {
    Concentration -= t;
  }
  public void AddMaxConcentration(float t)
  {
    float t1=Mathf.Pow(MaxConcentrationIncrement, MaxConcentration)+t;
    MaxConcentration = Mathf.Log(t1) / Mathf.Log(MaxConcentrationIncrement);
  }
  public override int GetStepCount ()
  {
    if(State==1)
      return base.GetStepCount ()/2;
    return  base.GetStepCount ();
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
      m_updateFunc = UpdateFuncBasic;
      m_renewedUpdater = false;
      //Debug.Log(m_updateFunc.Method.Name);
    }
  }
  public void OnLevelChange(int i) { m_basicAI.ChangeLevel(i); }
  public void OnDamageDealt(int damage)
  {
    if (!m_initislized) return;
  }


  public void Init()
  {
		GetInstanceID();
    for (int i = 0; i < transform.childCount; i++)
      DontDestroyOnLoad(transform.GetChild(i).gameObject);
    PlayerSaveData.Clear();
    OnUpdate = OnUpdated;
    Interact = OnInteract;
    if (!Creator.creator.testBuild) 
      EnteredPortal = false;
    if (m_initislized) return;
    prevNode = GetNode();
    //	  Debug.Log(m_init);
    m_updateFunc = UpdateFuncBasic;
    Type = ObjectType.Planer;
    id = Creator.GetID();
    m_moveControls = ScriptableObject.CreateInstance<PlanerMoveControls>();
    PlanerMoveControls.MoveParameters moveParameters;
    moveParameters.agility = agility;
    moveParameters.direction = direction;
    
    m_moveControls.Initialize(this, moveParameters);
    m_basicAI = ScriptableObject.CreateInstance<BasicPlanerAI>();
    m_basicAI.Init(this);
    Agility=3-m_hitPoints;
    //Debug.Log(m_initislized);
    (m_visualiser.GetComponent<PlanerVisualControls>()).Init(this);
    //m_visualiser.Init(this);
    //	  GetComponent<Catcher>().Init(this);
    m_initislized = true;
		Vector3 newPosition=new Vector3(0,0,-0.5773503f);
		if((Node.Index +Direction % 2) % 2==1)
			newPosition.z=-newPosition.z;
    m_visualiser.transform.localPosition=newPosition;
    //Debug.Log(m_initislized);

  }
  public void InitInterface()
  {
		CutInterface();
		m_cancelAction = ScriptableObject.CreateInstance<CancelAction>();
    m_cancelAction.Init(this, 0);
    m_homePortal=ScriptableObject.CreateInstance<HomePortal>();
    m_homePortal.Init(this,1);
		m_stayButton=ScriptableObject.CreateInstance<StayButton>();
		m_stayButton.Init(this,2);
    MineController = ScriptableObject.CreateInstance<MineController>();
    MineController.Init(this);
  }
  public void CutInterface()
  {
    Destroy( m_cancelAction);
    Destroy(m_homePortal);
		Destroy(m_stayButton);
    Destroy(m_mineController);
  }
	public void Stay()
	{
		m_moveControls.Stay();
		HasTarget=true;
		
	}
  public void SetTarget(GraphNode x, int angle)
  {
    if (!Node.Equals(x))
      m_basicAI.SetTarget(x, angle);
    else
    {
      if(angle>=0)
        SetNewDirection(angle);
      m_hasTarget = true;
      m_basicAI.SetTarget(null);
    }
  }
  bool HasAction()
  {
    if (State == 1) return true;
    return m_basicAI.HasTarget || m_hasTarget;
  }
  void OnUpdated()
  {
    if (m_regenCooldown == 0 && m_hitPoints != maxHp)
    {
      m_hitPoints++;
      Agility=3-m_hitPoints;
      m_regenCooldown = 10;
//      Creator.creator.SetSpeed(1-0.3f*Agility);
    }
    else
      m_regenCooldown--;
    EnteredPortal = false;
    if (m_updateFunc != null)
      m_updateFunc(this);
   
  }
  void UpdateFuncBasic(IPlanerLike planer)
  {

    m_basicAI.OnUpdate();
    m_moveControls.OnUpdate();
    m_mineController.OnUpdate();
    m_basicAI.CheckTarget();
    m_hasTarget = false;
  }
  public void Rotate(int angle)
  {
    m_moveControls.Rotate(angle);
  }
  public void SetNewDirection(int newDirection, bool forced)
  {
    m_moveControls.SetNewDirection(newDirection, forced);
  }
  public void SetNewDirection(int newDirection)
  {
    SetNewDirection(newDirection, false);
  }
  protected void OnDrawGizmos()
  {
    if (!Application.isPlaying) return;
    Vector3 dir;
    //	  Debug.Log(Energy);

    //Node=GraphNode.GetNodeByCoords(transform.position, (int)Energy);
    //Vector3	newPosition=Node.NodeCoords();
    //newPosition.y=transform.position.y;
    WayStatus[] directions = GraphTagMachine.GetDirections(Node);
    bool[] planerDirections = m_moveControls.Directions();
    //	  transform.position=newPosition;
    for (int i = 0; i < 6; i++)
    {
      dir = new Vector3(Mathf.Cos(i * Mathf.PI / 3f), 0, Mathf.Sin(i * Mathf.PI / 3f))*16;
      //		Debug.Log(direction+","+i);

      if ((directions[i]==WayStatus.Free) && planerDirections[i])
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
  public void DefaultInteract()
  {
    Interact = OnInteract;
  }
  void OnInteract(CustomObject obj, InteractType type)
  {
      Warming warm = obj as Warming;
      if (warm != null&&!warm.isEaten)
      {
        AddConcentration(warm.m_warmingConcentration);
        warm.isEaten = true;
      }
  }
  public void OnEnterPortal(GraphNode node, int direction)
  {
    Node = node;
    m_savedNode=node;
    m_savedDirection=direction;
    if (direction >= 0)
      SetNewDirection(direction, true);
    m_basicAI.ChangeLevel(node.Level);
  }
  public void OnWeaponsChanged()
  {
    if (m_mineController != null)
      Destroy(m_mineController);
    m_mineController = ScriptableObject.CreateInstance<MineController>() as MineController;
    m_mineController.Init(this);
  }
  public float EntityValue(CustomObject entity)
  {
    //const int m_maxWeight = 500;
    if (entity.GetType() == typeof(BasicMine)) return 5f;
    if (entity.GetType() == typeof(Portal)) return BasicPlanerAI.MaxWeight;
    if (entity.GetType() == typeof(DistantPortalEnter)&&(entity as DistantPortalEnter).Status>0) return BasicPlanerAI.MaxWeight;
    if (entity.GetType() == typeof(WeaponPrototype)) return BasicPlanerAI.MaxWeight;
    if (entity.GetType() == typeof(RedFireflySpawner)) return BasicPlanerAI.MaxWeight;
		if (entity.GetType() == typeof(Anchor)) return BasicPlanerAI.MaxWeight;
    return 0;
  }
  public List<int> CanRotateWithTag(GraphNode node, int direction)
  {
		
		List<int>x = new List<int>();
		x.Add(direction);
		if(GraphTagMachine.GetTagStatus(node.Tag))
		  for(int i=1; i<MaxRotateAngle; i++)
		  {
			  x.Add((i+direction+6)%6);
			  x.Add((-i+direction+6)%6);
		  }
		return x;
  }
  public void FireflyDestroy(YellowFirefly firefly)
  {
    firefly.Direction++;
    //SetNewDirection(Direction - 1, true);
    if (m_hitPoints == 1)
    {
      EnteredPortal = true;
      OnEnterPortal(m_savedNode, m_savedDirection);
    }
    else
    {
      m_hitPoints--;
      Agility=3-m_hitPoints;
      m_regenCooldown = 10;
//      Creator.creator.SetSpeed(1-0.3f*Agility);
    }
  }
  public override CustomObjectInfo SerializeObject ()
	{
		throw new NotImplementedException ();
	}
	public override Type SerializedType ()
	{
		throw new NotImplementedException ();
	}
  //TODO
}
