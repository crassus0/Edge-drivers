using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AdvancedPlanerAI : BasicPlanerAI
{
  static List<AdvancedPlanerAI> opponents;
  public override void Init(PlanerCore planer)
  {
	base.Init(planer);
	if(opponents==null)opponents=new List<AdvancedPlanerAI>();
	opponents.Add(this);
  }
  public override void OnUpdate()
  {
	GraphNode playerNode=Creator.Player.GetNode();
	if(Planer.GetNode().Distance(Creator.Player.GetNode())>20)
	{
	  Destroy(Planer.gameObject);
	  
	}
	
	if(Target==null||(Target.Distance(playerNode)>5&&Planer.GetNode().Distance(playerNode)>10))
	  SetTarget(Creator.Player.GetNode());
	while(Target==null)
	{
	  GraphNode node=Planer.GetNode().GetNodeByDirection(Random.Range(0,6));
	  if(node.NodeValue(EntityValue)<3)
	    SetTarget(node);
	  
	}
	Planer.Mines[0].Activate();
	Planer.Mines[1].Activate();
	base.OnUpdate();
  }	
  void OnDestroy()
  {	 
	  if(opponents!=null)
	    opponents.Remove(this);
	  if(!Creator.Initialised)return;
	  Vector3 randomVector=new Vector3(Random.Range(0f,1f),0,Random.Range(0f,1f)).normalized;
	  randomVector*=15*GraphNode.graphDistance;
	  Planer.GetComponent<CustomObjectEditorSupply>().Level=Creator.Level;
	  
      GameObject newPlaner =Instantiate(Planer.gameObject, Creator.Player.transform.position+randomVector, Planer.transform.rotation) as GameObject;
	  newPlaner.GetComponent<PlanerCore>().Energy=25;

  }
}
