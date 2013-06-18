using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
public class BasicPlanerAI: ScriptableObject {
  PlanerCore m_planer;
  AStarNode m_target; 
  Queue<int> route;
  Queue<float>weights;
  int m_maxDistance=50;
  public AStarNode Target{get{return m_target;}}
  protected PlanerCore Planer{get{return m_planer;}}
  public virtual void Init(PlanerCore planer)
  {
	m_planer=planer;
	route=new Queue<int>();
	weights=new Queue<float>();
  }
  public void SetTarget(GraphNode target, int direction, int maxDistance=50)
  {
	m_maxDistance=maxDistance;
	m_target=new AStarNode(target, direction, float.MaxValue, null);
//	Debug.Log(m_target);
	if(!AStarSearch())
	{
	  m_target=null;
   	  //Debug.Log("FAIL");
	}
  }
  public void SetTarget(GraphNode target)
  {
	
	SetTarget(target, -1);
  }
  public virtual void OnUpdate()
  {
	AStarNode currentNode=new AStarNode(m_planer.GetNode(), m_planer.Direction, 0, null);
	if(m_planer.Stopped>0)return;
	if(currentNode.EqualWithRandomRotation(m_target))
	  m_target=null;
    if(m_target==null)return;
	ApplyDirection();
  }
  public void ChangeLevel(int level)
  {
	if(m_target==null)return;
  if (level != m_target.Level) m_target = null;
  else SetTarget(m_target.node, m_target.direction );

  }
  void ApplyDirection()
  {
	int newDir=route.Dequeue();
	float newWeight=weights.Dequeue();
	if(newWeight<m_planer.GetNode().GetNodeByDirection(newDir).NodeValue(EntityValue)-0.5f)
	  AStarSearch();
	m_planer.SetNewDirection(newDir);
  }
  bool AStarSearch()
  {
	if(m_target.Distance(m_planer.GetNode())>m_maxDistance)
	{
	
		return false;
	}
//    long time=System.DateTime.Now.Ticks;
	List<AStarNode> checkedNodes= new List<AStarNode>();
	List<AStarNode> adjaccentNodes = new List<AStarNode>();
	//TODO rewrite collection
	checkedNodes.Clear();
	route.Clear();
	weights.Clear();
	//Initialising search
	AStarNode startNode=new AStarNode(m_planer.GetNode(), m_planer.Direction, 0, null);
	checkedNodes.Add(startNode);
	UpdateAdjacent(startNode, ref adjaccentNodes, ref checkedNodes);
	bool found=false;
	float foundDist=float.MaxValue;
	AStarNode targetNode=null;
	//Iterating
	while(!found)
	{
	  
      //if(checkedNodes.Count>200)return false;
	  AStarNode toAddNode=null;
	  float heuristic=float.MaxValue;
	  //Find best adjacent
	  foreach(AStarNode x in adjaccentNodes)
	  {
		 float nodeHeuristic=x.distance+m_target.Distance(x.node);
         if(nodeHeuristic<heuristic&&nodeHeuristic<foundDist)
		 {
			heuristic=nodeHeuristic;
			toAddNode=x;
		 }
	  }
      //Adding best if found
	  if(toAddNode!=null)
	  {
		int index = adjaccentNodes.BinarySearch(toAddNode);
		adjaccentNodes.RemoveAt(index);
		index=checkedNodes.BinarySearch(toAddNode);
		checkedNodes.Insert(~index, toAddNode);
		bool isTarget=toAddNode.EqualWithRandomRotation(m_target);
		if(isTarget)
		{
		  foundDist=toAddNode.distance;
		  targetNode=toAddNode;
		}
		else
		{
		  //CheckingNode  
		  UpdateAdjacent(toAddNode, ref adjaccentNodes, ref checkedNodes);
		}
	  }
	  else
	  {
		//If nothing to add
		if(targetNode==null)
		{ 
		  
		  return false;
		}
	    AStarNode current=targetNode;
//		Debug.Log(current);
		LinkedList<AStarNode> queue=new LinkedList<AStarNode>();

		while(!current.Equals(startNode))
		{

			//Debug.DrawRay(current.previous.node.NodeCoords(), 8*new Vector3(Mathf.Cos(current.prevDirection*Mathf.PI/3),0,Mathf.Sin(current.prevDirection*Mathf.PI/3)), Color.red);

			queue.AddFirst(current);
			current=current.previous;
		}
		//Debug.Log(m_target+"-target");
		///Debug.Log(m_planer.GetNode()+"-current");
		foreach(AStarNode x in queue)
		{
		  //Debug.Log(x.node);
		  //Debug.Log(x.direction);
		  route.Enqueue(x.prevDirection);
		  weights.Enqueue (x.previous==null?0:x.distance-x.previous.distance);
		}
		found=true;
		//Debug.Log((float)(System.DateTime.Now.Ticks-time));
		//Debug.Break();
	  }
	}
	
	//m_planer.Rotate(newDir-m_planer.direction);
	return true;
  }
  void UpdateAdjacent(AStarNode node, ref List<AStarNode> adj, ref List<AStarNode> check)
  {
	int maxRotateAngle=m_planer.MaxRotateAngle;
	int minDirection=(node.direction+6-maxRotateAngle);
	int maxDirection=(node.direction+6+maxRotateAngle);
	for(int i=minDirection; i<=maxDirection; i++)
	{
	  int index=i%6;
      UpdateDirection(node, ref adj, ref check, index);
	}
  }
  void UpdateDirection(AStarNode node, ref List<AStarNode> adj, ref List<AStarNode> check, int index)
  {
	  AStarNode newNode;
	  bool []directions=node.node.GetDirections();
	  if(directions[index])
	  {
	    newNode=new AStarNode(node.node.GetNodeByDirection(index), index, node.distance+1+0.01f*Mathf.Abs((node.direction+6-index)%6), node);
	   	newNode.distance+=newNode.node.NodeValue(EntityValue);
	  }
	  else
	  {
		newNode=new AStarNode(node.node, node.node.GetHitDirection(index), node.distance+1+0.01f*Mathf.Abs((node.direction+6-index)%6), node);
		newNode.prevDirection=index;
	  }
	  int foundIndex=check.BinarySearch(newNode);
	  bool found=false;
	  AStarNode existNode;
	  if(foundIndex>=0)
	  {
	    found=true;
	    existNode=check[foundIndex];
	    if(existNode.distance>newNode.distance)
	    {
		  existNode.previous.children[index]=null;
		  existNode.distance=newNode.distance;
		  existNode.previous=node;
		
		  for(int i=0; i<6; i++)
  		  {
		    if(existNode.children[i]!=null)
		      UpdateDirection(existNode, ref  adj, ref check, i);
		  }
		  node.children[index]=existNode;
		  return;
	    }
	  }
	  foundIndex=adj.BinarySearch(newNode);
	  if(foundIndex>=0)
	  {
	    found=true;
	    existNode=adj[foundIndex];
	    if(existNode.distance>newNode.distance)
	    {
		  existNode.previous.children[index]=null;
		  existNode.distance=newNode.distance;
		  existNode.previous=node;
		  node.children[index]=existNode;
		  return;
		}
	  }
	  if(found==false&&node.distance<m_maxDistance*1.2f)
	  {
	    node.children[index]=newNode;
	    adj.Insert(~foundIndex, newNode);
	  }
  }
  public virtual float EntityValue(CustomObject entity)
  {
	if(entity.GetType()==typeof(BasicMine))return 0.1f;
	return 0;
  }
}
