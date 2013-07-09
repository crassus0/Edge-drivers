using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(EditorAdditionalGUI))]
public class CreatorEditor : Editor
{

  //int selected=0;
  EditorAdditionalGUI targ;
  int m_activeLevelSize;
  //int count = 0;
  //int activeLevel=1;

  static GameObject m_editorPrefab;

  public override void OnInspectorGUI()
  {
    //KeyCheck();
    PreInit();
    ShowActiveLevel();
    ShowInstrumentSetup();
    ShowActiveLevelEditor();
  }
  void PreInit()
  {
    if (targ == null)
    {

      targ = target as EditorAdditionalGUI;
      targ.RepeatButton = false;
      targ.levels = GameObject.Find("Creator").GetComponent<Creator>().levels;

      m_activeLevelSize = targ.levels[(targ.ActiveLevel)].NumAreas;

      OnChangedActiveLevel();
    }
  }
  void ShowActiveLevel()
  {

    int[] levelValues = new int[targ.levels.Count];
    string[] levelNames = new string[levelValues.Length];
    for (int i = 0; i < levelValues.Length; i++)
    {
      levelValues[i] = i;
      levelNames[i] = targ.levels[i].name;
    }
    int activeLevel = targ.ActiveLevel;
    //	    Debug.Log(targ.ActiveLevel);
    //	    Debug.Log(levelNames);
    //	    Debug.Log(levelValues);
    activeLevel = EditorGUILayout.IntPopup("Active Level", activeLevel, levelNames, levelValues);
    //Debug.Log(activeLevel);

    targ.ActiveLevel = activeLevel;
    //Debug.Log(targ.ActiveLevel);
    if (GUI.changed)
    {

      OnChangedActiveLevel();
    }
  }
  void ShowInstrumentSetup()
  {
    targ.ShowInstrument = EditorGUILayout.Foldout(targ.ShowInstrument, "Instrument");
    if (!targ.ShowInstrument) return;
    targ.InstrumentRadius = EditorGUILayout.Slider("Radius", targ.InstrumentRadius / 16, 1, 50) * 16;
  }
  void OnChangedActiveLevel()
  {

    if (Application.isPlaying) return;
    for (int i = 0; i < targ.levels.Count; i++)
    {
      targ.levels[i].gameObject.SetActive(false);
      targ.levels[i].OnDeactivate();
      targ.levels[i].gameObject.hideFlags = 0;// HideFlags.HideInInspector | HideFlags.HideInHierarchy;
    }
    targ.levels[targ.ActiveLevel].Init();
    targ.levels[targ.ActiveLevel].gameObject.SetActive(true);
    foreach (CustomObject x in targ.Objects)
    {
      if (x != null)
      {
        if (x.Level != targ.ActiveLevel)
        {
          x.gameObject.SetActive(false);
        }
        else
        {
          x.gameObject.SetActive(true);
        }
      }
    }
    m_activeLevelSize = targ.levels[targ.ActiveLevel].NumAreas;
    //Debug.Log(m_activeLevelSize);
    targ.levels[targ.ActiveLevel].gameObject.hideFlags = 0;// HideFlags.HideInInspector | HideFlags.HideInHierarchy;
    targ.levels[targ.ActiveLevel].gameObject.SetActive(true);
  }
  void ShowActiveLevelEditor()
  {

    GUILayout.Space(10);
    GUILayout.Label("ActiveLevel");
    m_activeLevelSize = EditorGUILayout.IntSlider("NumAreas", m_activeLevelSize, 1, 8);
    //Debug.Log(m_activeLevelSize);
    if (m_activeLevelSize <= 0) m_activeLevelSize = 1;
    GUILayout.BeginHorizontal();
    {
      if (GUILayout.Button("Confirm"))
      {
        //Debug.Log("confirm");
        targ.levels[(targ.ActiveLevel)].NumAreas = m_activeLevelSize;
        targ.levels[(targ.ActiveLevel)].InitBareers();
        targ.levels[(targ.ActiveLevel)].Init();
      }
      if (GUILayout.Button("Unconfirm"))
      {
        m_activeLevelSize = targ.levels[(targ.ActiveLevel)].NumAreas;
      }
    }
    GUILayout.EndHorizontal();
    string name = EditorGUILayout.TextField("Name", targ.levels[(targ.ActiveLevel)].name);
    if (GUI.changed)
    {
      targ.levels[(targ.ActiveLevel)].name = name;
    }
    float t = EditorGUILayout.FloatField("Move phase duration", targ.levels[targ.ActiveLevel].MovePhaseDuration);
    if (t < 0) t = 0;
    targ.levels[targ.ActiveLevel].MovePhaseDuration = t;
    t = EditorGUILayout.FloatField("Action phase duration", targ.levels[targ.ActiveLevel].SelectionPhaseDuration);
    if (t < 0) t = 0;
    targ.levels[targ.ActiveLevel].SelectionPhaseDuration = t;
    targ.levels[targ.ActiveLevel].SelectionPhaseType = (PhaseType)EditorGUILayout.EnumPopup("Action phase type", targ.levels[targ.ActiveLevel].SelectionPhaseType);
    if (targ.levels.Count > 1)
    {
      if (GUILayout.Button("DeleteLevel") && EditorUtility.DisplayDialog("Delete level", "Are you sure?", "Yes", "No"))
      {
        BareerLevelControls x = targ.levels[(targ.ActiveLevel)];
        targ.levels.Remove(x);
        DestroyImmediate(x.gameObject);
        targ.ActiveLevel = 0;
        OnChangedActiveLevel();
      }
    }

    EditorUtility.SetDirty(targ.levels[(targ.ActiveLevel)]);
  }

  //SCENE  GUI
  void OnSceneGUI()
  {
    PreInit();
    OnInstrument();
    //OnButtons();

    OnBareerEditCheck();
    OnObjectMove();
    CheckEvent();
  }
  void CheckEvent()
  {
    Event currentEvent = Event.current;
    if (currentEvent.type == EventType.ScrollWheel) return;
    if (currentEvent.button == 2) return;
    if (currentEvent.type == EventType.Repaint) return;
    if (currentEvent.type == EventType.KeyDown || currentEvent.type == EventType.KeyUp) return;
    Event.current.Use();
  }
  void OnInstrument()
  {
    if ((targ.selected > 0) && (targ.selected < 4))
    {
      Vector3 mouseCoords = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
      mouseCoords.y = 0;
      Color instrumentColor = Color.red;
      instrumentColor.a = 0.2f;
      Handles.color = instrumentColor;
      Handles.DrawSolidDisc(mouseCoords, Vector3.up, targ.InstrumentRadius);
    }
  }

  void OnBareerEditCheck()
  {
    int index = targ.selected;
    if (targ.selected == 0 || targ.selected > 3) return;
    if (index == 0) return;
    index = 3 - index;
    if (index == 2) index++;

    if (Event.current.type != EventType.MouseDrag || Event.current.button != 1) return;
    //Debug.Log("Index " +index);
    float triangleWidth = BareerAreaControls.triangleWidth;
    float triangleHeight = BareerAreaControls.triangleHeight;
    float radius = targ.InstrumentRadius;

    Vector3 center = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
    center.y = 0;
    Vector3 mouseCoords = center;
    center.z += radius;
    int numVertecies = 0;
    while (center.z > -radius)
    {
      bool found = true;
      Vector3 left, right;
      GraphNode levelNode = GraphNode.GetNodeByCoords(center, targ.ActiveLevel);
      left = levelNode.NodeCoords();
      right = left;
      if (levelNode.Index == 1)
      {
        left.x -= triangleWidth / 2;

        right.x += triangleWidth / 2;
      }
      while (found)
      {
        found = false;
        Vector3[] leftFound = GraphNode.GetNodeByCoords(left, targ.ActiveLevel).BoundsCoords();
        Vector3[] rightFound = GraphNode.GetNodeByCoords(right, targ.ActiveLevel).BoundsCoords();
        byte[] leftActives = { 6, 6, 6 };
        byte[] rightActives = { 6, 6, 6 };
        for (int i = 0; i < 3; i++)
        {
          //			Debug.Log(leftFound[i]);
          Vector3 point = (leftFound[2 * i] + leftFound[2 * i + 1]) / 2;
          if (Vector3.Distance(leftFound[2 * i], mouseCoords) < radius || Vector3.Distance(leftFound[2 * i + 1], mouseCoords) < radius || Vector3.Distance(point, mouseCoords) < radius)
          {
            leftActives[i] = (byte)index;
            found = true;
            //Debug.Log(i);
          }
          point = (rightFound[2 * i] + rightFound[2 * i + 1]) / 2;
          if (Vector3.Distance(rightFound[2 * i], mouseCoords) < radius || Vector3.Distance(rightFound[2 * i + 1], mouseCoords) < radius || Vector3.Distance(point, mouseCoords) < radius)
          {
            rightActives[i] = (byte)index;
            found = true;
            //Debug.Log(i);
          }
        }

        if (found)
        {

          GraphNode.GetNodeByCoords(left, targ.ActiveLevel).ChangeState(leftActives, targ.levels);
          GraphNode.GetNodeByCoords(right, targ.ActiveLevel).ChangeState(rightActives, targ.levels);
          left.x -= triangleWidth;
          right.x += triangleWidth;
          numVertecies += 2;
        }
      }
      center.z -= triangleHeight;
    }
  }
  void OnObjectMove()
  {
    if (targ.selected < 4) return;
    EditorMenu curentWindow = EditorWindow.GetWindow<EditorMenu>();
    //	  if(targ.selected<4)return;
    //	  Debug.Log(targ.selected);
    Vector3 mouseCoords = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
    mouseCoords.y = 0;
    if (targ.objectToMove != null)
    {
      targ.objectToMove.transform.position = GraphNode.GetNodeByCoords(mouseCoords, targ.ActiveLevel).NodeCoords();
      targ.objectToMove.Node = GraphNode.GetNodeByCoords(targ.objectToMove.transform.position, targ.ActiveLevel);
    }
    if (Event.current.button == 1 && Event.current.type == EventType.MouseUp)
    {

      if (targ.RepeatButton)
      {
        curentWindow.Repeat();
      }
      else
      {
        EditorWindow.GetWindow<EditorMenu>().m_selectedOption = 0;
        
        targ.objectToMove = null;
      }
    }
  }
  void OnDestroy()
  {
    SceneDataSaver.SaveSceneData();
  }
}
