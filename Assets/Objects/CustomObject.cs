using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[RequireComponent(typeof(CustomObjectEditorSupply))]
public abstract class CustomObject : MonoBehaviour
{

  public GameObject m_visualiser;
  public ObjectType Type { get; set; }
  public bool Hidden { get; set; }
  public abstract void OnStart();
  public bool Destroyed { get; set; }
	public int ObjectID { get; set; }
  public virtual int GetStepCount()
  {
    return 4;
  }
  public new static void Destroy(UnityEngine.Object obj)
  {
    //Debug.Log(obj);
    if (obj as GameObject != null&&(obj as GameObject).GetComponent<CustomObject>()!=null)
      throw new Exception("Custom object destroy exeption. Use Creator.Destroy instead.");
    UnityEngine.Object.Destroy(obj);
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
  GraphNode node = new GraphNode();

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
    if (m_visualiser != null && m_visualiser.GetComponent<CustomObjectVisualiser>())
      m_visualiser.GetComponent<CustomObjectVisualiser>().SetSpeed(GetStepCount());
    Creator.AddObject(this);
    m_init = true;
  }
  public void SetNode()
  {
    Node = GraphNode.GetNodeByParameters(node.X, node.Y, node.Index, node.Level);

  }
  protected void OnDestroy()
  {
    if (Application.isPlaying)
    {
      if (node != null)
        node.Leave(this);
      
    }

  }
  public Action<CustomObject, InteractType> Interact;
  public void Move(int direction)
  {
    if (direction < 6)
      Node = Node.GetNodeByDirection(direction);
  }
  public void Move(GraphNode newNode)
  {
    Node = newNode;
  }
  public abstract CustomObjectInfo SerializeObject();

	public abstract System.Type SerializedType();
	
}
[System.Serializable]
public abstract class CustomObjectInfo
{
	public NodeInformation node;
	public int instanceID;
	public string instanceName;
	public abstract CustomObject Deserialize();
	public abstract void EstablishConnections();
	public abstract string GetName();
	protected CustomObject CreateInstance()
	{
		GameObject gameObject =  GetPrefabByName(GetName());
		CustomObject customObject =gameObject.GetComponent<CustomObject>();
		customObject.Node =GraphNode.GetNodeByParameters(node.i,node.j, node.index, node.level);
		customObject.ObjectID=instanceID;
		customObject.name=instanceName;
		if(Application.isPlaying)
		  Creator.AddObject(customObject);
		else
			EditorAdditionalGUI.EditorOptions.Objects.Add(customObject);
		return customObject;
		
	}
	public static GameObject GetPrefabByName(string name)
	{
		List<GameObject> prefabs;
		if(Application.isPlaying)
			prefabs=Creator.prefabs;
		else
			prefabs=EditorAdditionalGUI.EditorOptions.prefabs;
		try
		{
			return GameObject.Instantiate(prefabs.Find(x=>x.name== name+"Prefab")) as GameObject;
		}
		catch(System.Exception x)
		{
			Debug.Log(name);
			throw(x);
		}
	}
	public void BasicSerialization(CustomObject x)
	{
		instanceName=x.name;
		instanceID=x.ObjectID;
		node= new NodeInformation(x.Node);
	}
	public static CustomObject GetObjectByID(int id)
	{
		CustomObject[] objects;
		if(Application.isPlaying)
			objects=Creator.creator.m_objects.ToArray();
		else
			objects=EditorAdditionalGUI.EditorOptions.Objects.ToArray();
		return objects[id];
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