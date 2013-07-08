using UnityEngine;
using System.Collections;

public class TerraformingMine : CustomObject
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
      Node.ChangeState(m_prevState, Creator.creator.levels);
    }
    foreach(GraphNode x in Node.GetAdjacent())
      x.Reactivate();
  }
}
