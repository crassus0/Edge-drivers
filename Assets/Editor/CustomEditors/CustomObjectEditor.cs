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
  CustomObject edited;
  void OnEnable()
  {
    
    creator = GameObject.Find("Creator").GetComponent<Creator>();
    edited = (target as CustomObjectEditorSupply).GetComponent<CustomObject>();

    (target as CustomObjectEditorSupply).SetFlags();
    CustomObject x = edited.GetComponent<CustomObject>();
    //x.Level = Level;
    if (Application.isPlaying) return;
    //	Debug.Log(target);
    if (!EditorAdditionalGUI.EditorOptions.Objects.Contains(x))
      EditorAdditionalGUI.EditorOptions.Objects.Add(x);
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
}
