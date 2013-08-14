using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
[CustomEditor(typeof(CustomObjectEditorSupply))]
[CanEditMultipleObjects()]
//[ExecuteInEditMode()]

public class CustomObjectEditor : Editor
{
  GraphNode m_currentNode;
  Creator creator;
  CustomObject edited;
	List<CustomObject> adjacent;
	string[] adjNames;
	List<int>adjIndexes;
	int selectedObject=-1;
  void OnEnable()
  {
    
    creator = GameObject.Find("Creator").GetComponent<Creator>();
    edited = (target as CustomObjectEditorSupply).GetComponent<CustomObject>();

    (target as CustomObjectEditorSupply).SetFlags();

		GetAdjacentObjects();
  }
  public override void OnInspectorGUI()
  {
    
    //base.OnInspectorGUI();
    //if(Application.isPlaying)return;
    //Debug.Log("1412351345");
    try
    {
      m_currentNode = edited.Node;
    }
    catch
    {
      Debug.Log(edited);
    }
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("X", GUILayout.MaxWidth(45));
    int xCoord = EditorGUILayout.IntField(m_currentNode.X, GUILayout.ExpandWidth(true));
    EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(45));
    int yCoord = EditorGUILayout.IntField(m_currentNode.Y, GUILayout.ExpandWidth(true));
    EditorGUILayout.EndHorizontal();


    int index = EditorGUILayout.IntField("Index", m_currentNode.Index);


    string[] levelNames = new string[creator.levels.Count];
    int[] levels = new int[levelNames.Length];
    for (int i = 0; i < levelNames.Length; i++)
    {
      levels[i] = i;
      levelNames[i] = creator.levels[i].name;
    }
    int level = EditorGUILayout.IntPopup("Level", edited.Level, levelNames, levels);
		int selected;
    try
		{
			selected=EditorGUILayout.IntPopup("Selected object", selectedObject, adjNames, adjIndexes.ToArray());
		}
		catch
		{
			Debug.Log(selectedObject);
			Debug.Log(adjNames);
			Debug.Log(adjIndexes);
      throw;
		}
		if(GUILayout.Button("Delete"))
			if(EditorUtility.DisplayDialog("Delete", "Are you sure", "Yes", "Cancel"))
				DestroyObject();
    if (GUI.changed)
    {


      //Debug.Log(creator.levels.Count);
      //		Debug.Log(m_currentNode);
      //Debug.Log(edited.name);
      m_currentNode = GraphNode.GetNodeByParameters(xCoord, yCoord, index, level);//.GetNodeByCoords(m_currentNode.NodeCoords(), level);
      //Debug.Log(m_currentNode);
      edited.Node = m_currentNode;
      //edited.transform.position = m_currentNode.NodeCoords();
      
      //edited.GetComponent<CustomObject>().Level = level;

      edited.gameObject.SetActive(level == EditorAdditionalGUI.EditorOptions.ActiveLevel);


      edited.transform.position = m_currentNode.NodeCoords();
			if(selected!=selectedObject)
			{
				Selection.activeGameObject=adjacent[selected].gameObject;
			}
    }
    //edited.transform.hideFlags=0;
    (target as CustomObjectEditorSupply).SetFlags();
    EditorUtility.SetDirty(edited);
  }
  /*CustomObjectEditor()
  {
  HideComponents();
  }*/

  public void OnSceneGUI()
  {


      m_currentNode = GraphNode.GetNodeByCoords(edited.transform.position, edited.Level);

    edited.Node = m_currentNode;
    edited.transform.position = m_currentNode.NodeCoords();

  }
	void GetAdjacentObjects()
	{
		adjacent=new List<CustomObject>();
		GraphNode node=(target as CustomObjectEditorSupply).GetComponent<CustomObject>().GetNode();
		node=GraphNode.GetNodeByParameters(node.X, node.Y, node.Index, node.Level);
		int iD=(target as CustomObjectEditorSupply).GetComponent<CustomObject>().ObjectID;
		int i=0;
		adjIndexes=new List<int>();
		if(!Application.isPlaying)
		  foreach(CustomObject x in EditorAdditionalGUI.EditorOptions.Objects)
		  {
			  if(x.Node.Equals(node))
			  { 
				  adjacent.Add(x);
				  if(x.ObjectID==iD)
					  selectedObject=i;
				  adjIndexes.Add(i++);
			  }
		  }
		else
		{
			
			foreach(CustomObject x in node.Objects)
		  {
				adjacent.Add(x);
				if(x.ObjectID==iD)
				  selectedObject=i;
				adjIndexes.Add(i++);
			}
		}
		adjNames=adjacent.ConvertAll<string>(x=>x.name).ToArray();
	}
	void DestroyObject()
	{
		if(Application.isPlaying)return;
		DestroyImmediate((target as CustomObjectEditorSupply).gameObject);
	}
}
