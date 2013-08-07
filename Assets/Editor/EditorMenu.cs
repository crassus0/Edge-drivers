using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorMenu : EditorWindow
{

  EditorAdditionalGUI editor
  {
    get
    {
      return EditorAdditionalGUI.EditorOptions;
    }
  }
  GameObject m_editorPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/EditorOptionsPrefab.prefab", typeof(GameObject)) as GameObject;
  public int m_selectedOption = 0;
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
    //m_selectedOption=0;
    //editor.selected=0;

    if (EditorAdditionalGUI.EditorOptions == null)
    {
      GameObject newEditor = Instantiate(m_editorPrefab) as GameObject;
      newEditor.name = "EditorOptions";

    }
    
    //EditorAdditionalGUI.editorPrefab = m_editorPrefab;
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
    m_selectedOption = GUILayout.SelectionGrid(m_selectedOption, editor.buttonTextures, 4, GUILayout.Width(160), GUILayout.Height(80), GUILayout.MinHeight(160));

  }
  void CheckActiveState()
  {

    Selection.activeObject = editor.gameObject;
    editor.selected = m_selectedOption;
    if (m_selectedOption >= 5)
    {
      CreateObject(editor.objectNames[m_selectedOption - 5]);
    }
  }
  void CreateObject(string type)
  {

    GameObject planerPrefab = AssetDatabase.LoadAssetAtPath("Assets/ObjectPrefabs/" + type + "Prefab.prefab", typeof(GameObject)) as GameObject;
    GameObject planer = Instantiate(planerPrefab) as GameObject;
    planer.name = type + EditorAdditionalGUI.EditorOptions.Objects.Count;
    planer.SetActive(true);
    EditorAdditionalGUI.EditorOptions.Objects.Add(planer.GetComponent<CustomObject>());
    SceneDataSaver.SaveSceneData();
    editor.objectToMove = planer.GetComponent<CustomObject>();
    editor.objectToMove.GetComponent<CustomObject>().Level = editor.ActiveLevel;
  }
  public void Repeat()
  {
    CheckActiveState();
  }
}
