using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorMenu : EditorWindow
{

  static EditorAdditionalGUI editor
  {
    get
    {
			if(EditorAdditionalGUI.EditorOptions==null)
			{
				
        GameObject m_editorPrefab = AssetDatabase.LoadAssetAtPath("Assets/ObjectPrefabs/EditorOptionsPrefab.prefab", typeof(GameObject)) as GameObject;
				EditorAdditionalGUI.EditorOptions=(GameObject.Instantiate(m_editorPrefab) as GameObject).GetComponent<EditorAdditionalGUI>();
				EditorAdditionalGUI.EditorOptions.gameObject.name="EditorOptions";
			}
      return EditorAdditionalGUI.EditorOptions;
    }
  }
  public static int m_selectedOption = 0;
  [MenuItem("EdgeDrivers/Editor Menu")]
  static void Init()
  {
    //Debug.Log("init");
    // Get existing open window or if none, make a new one:
		EditorMenu window = (EditorMenu)EditorWindow.GetWindow(typeof(EditorMenu));
    window.name = "Edge drivers components";
    
    window.Repaint();
  }


  void OnFocus()
  {
    Repaint();
    Selection.activeGameObject = editor.gameObject;
  }

  // Update is called once per frame
  void OnGUI()
  {
    //Debug.Log(System.Math.Sign(-1));
    if (Application.isPlaying)
    {
      return;
    }
    DrawButtons();
    KeyCheck();
    if (GUI.changed)
      CheckActiveState();
  }
  void KeyCheck()
  {
		
    if ((Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl) && Event.current.type == EventType.KeyDown)
      editor.RepeatButton = true;
    if ((Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl) && Event.current.type == EventType.KeyUp)
      editor.RepeatButton = false;
  }
  void DrawButtons()
  {
    //GUILayout.Label("BareerEdit");
    m_selectedOption = GUILayout.SelectionGrid(m_selectedOption, editor.buttonTextures, 4,  GUILayout.MinHeight(200));

  }
  void CheckActiveState()
  {

    Selection.activeObject = editor.gameObject;
    editor.selected = m_selectedOption;
  }
	public static void CreateObject(Vector3 coords)
	{
		Selection.activeObject = editor.gameObject;
    editor.selected = m_selectedOption;
    if (m_selectedOption >= 5)
    {
			
      CreateObject(editor.objectNames[m_selectedOption - 5], coords);
    }
	}
  static void CreateObject(string type, Vector3 coords)
  {

    GameObject planerPrefab = AssetDatabase.LoadAssetAtPath("Assets/ObjectPrefabs/" + type + "Prefab.prefab", typeof(GameObject)) as GameObject;
    GraphNode node=GraphNode.GetNodeByCoords(coords, editor.ActiveLevel);
		GameObject planer = Instantiate(planerPrefab, coords, Quaternion.Euler(0, 180*node.Index,0)) as GameObject;
    planer.name = type + EditorAdditionalGUI.EditorOptions.Objects.Count;
    planer.SetActive(true);
    EditorAdditionalGUI.EditorOptions.Objects.Add(planer.GetComponent<CustomObject>());
		planer.GetComponent<CustomObject>().Node=node;
		if(!editor.RepeatButton)
		{
			m_selectedOption=0;
			editor.selected=0;
			EditorWindow.GetWindow(typeof(EditorMenu)).Repaint();
		}
		EditorUtility.SetDirty(planer);
  }
  public void Repeat()
  {
    CheckActiveState();
  }
}
