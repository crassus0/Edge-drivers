using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(CustomObjectEditorSupply))]
[CanEditMultipleObjects()]
//[ExecuteInEditMode()]

public class CustomObjectEditor : Editor 
{
  GraphNode m_currentNode;
  Creator creator;
  CustomObjectEditorSupply edited;
  void OnEnable()
  {
//	Debug.Log(target);
	creator = GameObject.Find("Creator").GetComponent<Creator>();
	edited = target as CustomObjectEditorSupply;
	m_currentNode=GraphNode.GetNodeByCoords(edited.transform.position, edited.level);
    edited.SetFlags();
  }
  public override void OnInspectorGUI()
  {
	//base.OnInspectorGUI();
	//if(Application.isPlaying)return;
	//Debug.Log("1412351345");
	EditorGUILayout.BeginHorizontal();
	EditorGUILayout.LabelField("X", GUILayout.MaxWidth(45));
	int xCoord=EditorGUILayout.IntField(m_currentNode.X,  GUILayout.ExpandWidth(true));
	EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(45));
	int yCoord=EditorGUILayout.IntField(m_currentNode.Y, GUILayout.ExpandWidth(true));
	EditorGUILayout.EndHorizontal();
	

	int index=EditorGUILayout.IntField("Index",m_currentNode.Index);
		
	
	string[] levelNames = new string[creator.levels.Count];
	int []levels=new int[levelNames.Length];
	for(int i=0; i<levelNames.Length; i++)
	{
	  levels[i]=i;
	  levelNames[i]=creator.levels[i].name;
	}
	int level=EditorGUILayout.IntPopup("Level", m_currentNode.Level, levelNames, levels);
	
	
	if(GUI.changed)
	{
	  
	  if(m_currentNode.Level!=level)
	  {
		Debug.Log(creator.levels.Count);
//		Debug.Log(m_currentNode);
		m_currentNode=GraphNode.GetNodeByCoords(m_currentNode.NodeCoords(), level);
		//Debug.Log(m_currentNode);
	    edited.transform.position=m_currentNode.NodeCoords();
		edited.level=level;
		
		xCoord=EditorGUILayout.IntField("X", m_currentNode.X);
		yCoord=EditorGUILayout.IntField("Y", m_currentNode.Y);
		index=EditorGUILayout.IntField("Index", m_currentNode.Index);
		level=EditorGUILayout.IntField("Level", m_currentNode.Level);
			Debug.Log(EditorAdditionalGUI.EditorOptions);
		edited.gameObject.SetActiveRecursively(level==EditorAdditionalGUI.EditorOptions.ActiveLevel);
	
			
	  }
	  m_currentNode=GraphNode.GetNodeByParameters(xCoord, yCoord, index, level);
	  edited.transform.position=m_currentNode.NodeCoords();
	}
	//edited.transform.hideFlags=0;
	edited.SetFlags();
  }
  /*CustomObjectEditor()
  {
	HideComponents();
  }*/

  public void OnSceneGUI()
  {

	  CustomObjectEditorSupply edited = target as CustomObjectEditorSupply;
	  m_currentNode=GraphNode.GetNodeByCoords(edited.transform.position, edited.level);
	  edited.transform.position=m_currentNode.NodeCoords();

  }
}
