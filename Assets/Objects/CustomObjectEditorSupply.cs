using UnityEngine;
using System.Collections;
[ExecuteInEditMode()]
public class CustomObjectEditorSupply : MonoBehaviour {
	public int level=0;
	
	void Update()
	{
      //Debug.Log("Update");
	  GetComponent<CustomObject>().Level=level;
	}
	void Start()
	{
	  if(Application.isPlaying)return;
	  //Debug.Log(EditorAdditionalGUI.EditorOptions);
	  
	  CustomObject x = GetComponent<CustomObject>();
	  if(!EditorAdditionalGUI.EditorOptions.objects.Contains(x))
	    EditorAdditionalGUI.EditorOptions.objects.Add(x);
	  
	}
	void OnDestroy()
	{
	  if(Application.isPlaying)return;
	  CustomObject x = GetComponent<CustomObject>();
      if(EditorAdditionalGUI.EditorOptions!=null)
	    EditorAdditionalGUI.EditorOptions.objects.Remove(x);
	}
	public void SetNode(GraphNode Node)
	{
//	  Debug.Log("setNode");

	  transform.position=Node.NodeCoords();
	}
	public void SetFlags()
	{
	  
	  transform.hideFlags=HideFlags.HideInInspector;
	  int children=transform.childCount;
	  for(int i=0; i<children; i++)
		transform.GetChild(i).gameObject.hideFlags=HideFlags.HideInInspector|HideFlags.HideInHierarchy;
	}
}
