using UnityEngine;
using System.Collections;

public class TerraformingMine : CustomObject, IFireflyDestroyable,  IDeactivatable, IActivatable
{
  public int steps = -1;
  public byte[] states= new byte[3];
  public byte[] PrevState { get { return m_prevState; } }
  byte[] m_prevState;
  public bool visible = true;
  bool m_initialized = false;
  bool m_active=false;
  public bool ActivateOnStart
  { 
    get { return m_activateOnStart;}
    set { m_activateOnStart=value;}
  }
  [SerializeField]
  bool m_activateOnStart;

  public override void OnStart()
  {
    if (!m_initialized)
    {
      //Debug.Log(Application.loadedLevelName);
      m_initialized = true;
      if (!visible)
        Destroy(transform.GetChild(0).gameObject);
      OnUpdate = OnUpdated;
      if(ActivateOnStart)
        Activate();
			ActivateOnStart=false;
    }
	}
  public void Activate()
  {
    m_active=true;
    if (Destroyed) return;
    m_prevState = Node.GetNodeGraph();
    Node.ChangeState(states, Creator.creator.levels);
  }
  public void Deactivate()
  {
		m_active=false;
    if (ActivateOnStart && !Destroyed) return;
    if (m_prevState != null)
    {
      Node.ChangeState(m_prevState, Creator.creator.levels, true);

    }
    Node.Reactivate();
    foreach (GraphNode x in Node.GetAdjacent())
    {
      x.Reactivate();
    }
  }
  void OnUpdated()
  {
    if(!m_active)return;
    if (steps < 0) 
      OnUpdate = null;
    if (steps-- == 0)
    {
      Creator.DestroyObject(this);

    }
  }
  new void OnDestroy()
  {
    if (Creator.IsLoading) return;
    Destroyed = true;
    base.OnDestroy();
    //Activate = null;
    //Debug.Log(Application.loadedLevelName);
    Deactivate();

  }
  protected new void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    Vector3[] coords = new Vector3[3];
    int yMultiplier=Node.Index==0?1:-1;
    coords[0] = new Vector3(-2, 0, yMultiplier * 1.154700f);
    coords[1] = new Vector3(2, 0, yMultiplier * 1.154700f);
    coords[2] = new Vector3(0, 0, yMultiplier * -2.309401f);
    ColorByState(states[0]);
    Gizmos.DrawLine(transform.position+coords[0]*16,transform.position+coords[1]*16);
    ColorByState(states[1]);
    Gizmos.DrawLine(transform.position + coords[0] * 16, transform.position + coords[2] * 16);
    ColorByState(states[2]);
    Gizmos.DrawLine(transform.position + coords[1] * 16, transform.position + coords[2] * 16);
  }
  static void ColorByState(int state)
  {
     if (state == 0)
      Gizmos.color = Color.black;
    if (state == 1)
      Gizmos.color = Color.white;
    if (state == 2)
      Gizmos.color = Color.red;
    if (state == 3)
      Gizmos.color = Color.yellow;
    if (state == 6)
      Gizmos.color = new Color(0,0,0,0);
  }
  public virtual void FireflyDestroy(YellowFirefly firefly)
  {
    
    YellowFirefly newFirefly =(Instantiate(firefly.gameObject) as GameObject).GetComponent<YellowFirefly>();
    newFirefly.m_visualiser.transform.position = newFirefly.transform.position;
    newFirefly.Direction--;
    firefly.Direction++;
    Interact = null;
    Creator.DestroyObject(this);
  }
	public override CustomObjectInfo SerializeObject ()
	{
		TerraformingMineInfo x = new TerraformingMineInfo();
		x.BasicSerialization(this);
  	x.steps=steps;
		x.states=states;
		x.visible=visible;
		x.ActivateOnStart=ActivateOnStart;
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(TerraformingMineInfo);
	}
}
[System.Serializable]
public class TerraformingMineInfo:CustomObjectInfo
{
	public int steps;
  public byte[] states;
  public bool visible;
	public bool ActivateOnStart;
	public override CustomObject Deserialize ()
	{
		TerraformingMine x = CreateInstance() as TerraformingMine;
		x.steps=steps;
		x.states=states;
		x.visible=visible;
		x.ActivateOnStart=ActivateOnStart;
		return x;
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "TerraformingMine";
	}
}
