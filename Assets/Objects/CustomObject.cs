using UnityEngine;
using System.Collections;
using System;

[Serializable]
[RequireComponent(typeof(CustomObjectEditorSupply))]
public abstract class CustomObject : MonoBehaviour
{
  
  public GameObject m_visualiser;
  public ObjectType Type { get; set; }
  public bool Hidden{get; set;}
  public abstract void OnStart();
  public bool Destroyed { get; set; }

  public virtual int GetStepCount()
  {
    return 4;
  }
  public int Level
  {
    get
    {
      return node.Level;
    }
    set
    {
      Node = GraphNode.GetNodeByParameters(Node.X, Node.Y, Node.Index, value);
    }
  }
  protected bool m_init = false;
  public GraphNode Node
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
      //Debug.Log(name);
      node = GraphNode.GetNodeByParameters(value.X, value.Y, value.Index, value.Level);
      node.Enter(this);
      Vector3 newPosition = node.NodeCoords();
      newPosition.y = transform.position.y;
      transform.position = newPosition;
    }
  }
  //[HideInInspector]
  [SerializeField]
  GraphNode node=new GraphNode();

  //public abstract void OnStart();
  public GraphNode GetNode()
  {
    //Debug.Log(node);
    return node;
  }
  public Action OnUpdate;
  
  //protected void OnStart(){}
  protected void Awake()
  {
    
    if (m_init) return;
    try
    {
      Node = GraphNode.GetNodeByParameters(node.X, node.Y, node.Index, node.Level);
    }
    catch (System.Exception x)
    {
      Debug.Log(x);
      Debug.Log(name);
    }
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
  public Action<CustomObject, InteractType> Interact;
  public Action Activate; 
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