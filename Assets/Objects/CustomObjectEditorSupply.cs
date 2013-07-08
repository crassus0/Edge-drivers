using UnityEngine;
using System.Collections;
//[ExecuteInEditMode()]
public class CustomObjectEditorSupply : MonoBehaviour {

	

	
	void OnDestroy()
	{
	  if(Application.isPlaying)return;
	  CustomObject x = GetComponent<CustomObject>();
      if(EditorAdditionalGUI.EditorOptions!=null)
	    EditorAdditionalGUI.EditorOptions.Objects.Remove(x);
	}
  
	public void SetNode(GraphNode Node)
	{
//	  Debug.Log("setNode");

	  transform.position=Node.NodeCoords();
	}
	public void SetFlags()
	{

    transform.hideFlags = 0;// HideFlags.HideInInspector;
	  int children=transform.childCount;
	  for(int i=0; i<children; i++)
      transform.GetChild(i).gameObject.hideFlags = 0;// HideFlags.HideInInspector | HideFlags.HideInHierarchy;
	}
}
