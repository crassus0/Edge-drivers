using UnityEngine;
using System.Collections;

  public class AStarNode: System.IComparable<AStarNode>
  {
	public GraphNode node{get;set;}
	public int direction{get;set;}
	public int prevDirection{get;set;}
	public float distance{get;set;}
	public AStarNode previous{get;set;}
	public AStarNode[] children{get;set;}
	public int Level {get{ return node.Level;}}
	public AStarNode(GraphNode node, int direction, float distance, AStarNode prev)
	{
	  children=new AStarNode[6];
	  this.direction=direction;
	  this.prevDirection=direction;
	  this.node=node;
	  this.distance=distance;
	  previous=prev;
	}
	public float Distance(AStarNode x)
	{
	  float dist=node.Distance(x.node);
	  if(direction>=0)
		dist+=Mathf.Abs(rotationDistance(direction, x.direction));
	  return dist;
	  
	}
	public float Distance(GraphNode x)
	{
	  return node.Distance(x);
	}
	int rotationDistance(int rotation, int newRotation)
	{
		int dist=newRotation-rotation;
		if(dist>3)dist-=6;
		if(dist<-3)dist+=6;
		return dist;
	}
    public int CompareTo (AStarNode other)
	{
	  int comparer=node.CompareTo(other.node);
	  if(comparer!=0)
	    return comparer;
	  return Utility.Sign(direction-other.direction);
	}
   public override string ToString ()
	{
		return string.Format ("[AStarNode: node={0}, direction={1}]", node, direction);
	}
	public override bool Equals(object o)
	{
	  if(o==null)return false;
      AStarNode o1=o as AStarNode;
	  if(o1==null)	return false;
	  bool eq=node.Equals(o1.node);
	  eq=eq&&direction==o1.direction;
	  return eq;
	}
	public bool EqualWithRandomRotation(AStarNode x)
	{
	  if(x==null)return false;
	  bool equals=node.Equals(x.node);
	  if(direction>=0&&x.direction>=0)
		equals=equals&&direction==x.direction;
	  return equals;
	}
	public override int GetHashCode ()
		{
			return node.GetHashCode ()^direction;
		}
  }