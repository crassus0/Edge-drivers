using UnityEngine;
using System.Collections;

public class TerraformingMine : CustomObject, IFireflyDestroyable
{
  public int steps = -1;
  public byte[] states= new byte[3];
  public byte[] PrevState { get { return m_prevState; } }
  byte[] m_prevState;
  public bool visible = true;
  bool m_initialized = false;
  
  public override void OnStart()
  {
    if (!m_initialized)
    {
      //Debug.Log(Application.loadedLevelName);
      m_initialized = true;
      if (!visible)
        Destroy(transform.GetChild(0).gameObject);
      OnUpdate = OnUpdated;
      Activate = OnActivate;
      Activate();
    }
	}
  void OnActivate()
  {

    m_prevState = Node.GetNodeGraph();

    Node.ChangeState(states, Creator.creator.levels);
  }
  void OnUpdated()
  {
    if (steps < 0) 
      OnUpdate = null;
    if (steps-- == 0)
    {
      Destroy(gameObject);
    }
  }
  new void OnDestroy()
  {
    if (Creator.IsLoading) return;
    base.OnDestroy();
    Activate = null;
    //Debug.Log(Application.loadedLevelName);
    
    if (m_prevState != null)
    {
      Node.ChangeState(m_prevState, Creator.creator.levels, true);

    }
    Node.Reactivate();
    foreach(GraphNode x in Node.GetAdjacent())
      x.Reactivate();
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
    Destroy(gameObject);
    Destroyed = true;
  }
}
