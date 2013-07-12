using UnityEngine;
using System.Collections;

public class DistantPortalExit : CustomObject
{
  public int Direction
  {
    get{return m_direction;}
    set{m_direction=value;}
  }
  [SerializeField]
  int m_direction=-1;
  public override void OnStart ()
  {
  }
  public DistantPortalSaveData GetData()
  {
    DistantPortalSaveData x;
    x.name = name;
    x.node = Node;
    x.portalID = this.GetInstanceID();
    x.direction=Direction;
    return x;
  }
  [System.Serializable]
  public struct DistantPortalSaveData
  {
    public int direction;
    public GraphNode node;
    public int portalID;
    public string name;
  }
  protected new void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    Gizmos.DrawIcon(transform.position, "distantPortalExit.png");
  }
}
