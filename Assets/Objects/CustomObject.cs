using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CustomObjectEditorSupply))]
public abstract class CustomObject : MonoBehaviour
{

  public ObjectType Type { get; set; }
  public int Level
  {
    get
    {
      return node.Level;
    }
    
  }
  protected bool m_init = false;
  protected GraphNode Node
  {
    get
    {
      return node;
    }
    set
    {
      if (value == null) return;
      if (node != null)
        node.Leave(this);
      node = value;
      node.Enter(this);
      Vector3 newPosition = node.NodeCoords();
      newPosition.y = transform.position.y;
      transform.position = newPosition;
    }
  }
  GraphNode node;

  public GraphNode GetNode()
  {
    //Debug.Log(node);
    return node;
  }
  public abstract void OnUpdate();
  //protected void OnStart(){}
  protected void Awake()
  {
    Init();
  }
  public virtual void Init()
  {
    //Debug.Log(Level);
    if (m_init) return;
    int level = GetComponent<CustomObjectEditorSupply>().Level;
    Node = GraphNode.GetNodeByCoords(transform.position, level);

    Creator.AddObject(this);
    m_init = true;
  }

  protected void OnDestroy()
  {
    if (Application.isPlaying)
    {
      if (node != null)
        node.Leave(this);
      Creator.RemoveObject(this);
    }

  }
  public abstract void Interact(CustomObject obj, InteractType type);
  public void Move(int direction)
  {
    if (direction < 6)
      Node = Node.GetNodeByDirection(direction);
  }
  public void Move(GraphNode newNode)
  {
    Node = newNode;
  }

  protected void OnDrawGizmos()
  {
    //GameObject targetObject = (target as PlanerCore).gameObject;
    Gizmos.color = new Color(0, 0, 0, 0);
    Gizmos.DrawSphere(transform.position, 20);
  }
}
public enum ObjectType
{
  Planer,
  MeatChopper,
  Worm,
  WormSegment,
  Warming,
  Torpedo,
  Button
}
public enum InteractType
{
  Enter,
  Stay
}