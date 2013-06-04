using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CustomObjectEditorSupply))]
public abstract class CustomObject : MonoBehaviour {
	
	public ObjectType Type{get; set;}
    public int Level{get; set;}
	
	bool awaken=false;
	protected GraphNode Node
	{
	  get
	  {
		return node;
	  }
	  set
	  {
		if(value==null)return;
		if(node!=null)
		  node.Leave(this);
		node=value;
		Level=node.Level;
		node.Enter(this);
		Vector3	newPosition=node.NodeCoords();
		newPosition.y=transform.position.y;
		transform.position=newPosition;
	  }
	}
    GraphNode node;
	
	public GraphNode GetNode()
	{
	  return node;
	}
	public abstract void OnUpdate();
	//protected void OnStart(){}
	protected void Awake()
	{
	  if(awaken)return;
	  //Destroy(GetComponent<CustomObjectEditorSupply>());
//.	  Creator.creator.Init();
//	  Debug.Log(gameObject.name);
	  
	
//		Debug.Log(Level);
	    Node=GraphNode.GetNodeByCoords(transform.position, Level);
	  
	  Creator.AddObject(this);
	  awaken=true;
	}

	protected void OnDestroy()
	{
	  if(Application.isPlaying)
	  {
	    if(node!=null)
	  	  node.Leave(this);
	    Creator.RemoveObject(this);
	  }
	  
	}
	public abstract void Interact(CustomObject obj, InteractType type);
	public void Move(int direction)
	{
	  if(direction<6)
	    Node=Node.GetNodeByDirection(direction);
	  if(direction==6)
	  {
//		Debug.Log("up");
	    Node=Node.GetUpperNode();
			gameObject.SetActiveRecursively(Object.ReferenceEquals(Creator.Player, this));
		  
	  }
	  if(direction==7)
	  {
		Node=Node.GetLowerNode();
		  gameObject.SetActiveRecursively(Object.ReferenceEquals(Creator.Player, this));
	  }
	}
	protected virtual void OnDrawGizmos()
	{
	  //GameObject targetObject = (target as PlanerCore).gameObject;
	  Gizmos.color=new Color(0,0,0,0);
	  Gizmos.DrawSphere(transform.position,  20);
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