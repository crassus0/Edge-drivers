using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 

using System.Text; 
 
[CustomEditor(typeof(EditorAdditionalGUI))]
public class CreatorEditor : Editor
{

  EditorAdditionalGUI targ;
  int m_activeLevelSize;
	static bool loaded=true;
  bool m_ShowBackground;
	public Creator creator;
  //int count = 0;
  //int activeLevel=1;

  static GameObject m_editorPrefab;
	void OnEnable()
	{
		targ = target as EditorAdditionalGUI;
    GameObject x = GameObject.Find("Creator") as GameObject;
    m_ShowBackground= Camera.main.GetComponent<CameraControls>().Background.activeSelf;
    if(x!=null)
		  creator=x.GetComponent<Creator>();
		Creator.prefabs=targ.prefabs;
	}
  public override void OnInspectorGUI()
  {
    //KeyCheck();
    PreInit();
    ShowInstrumentSetup();
    ShowActiveLevel();
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
    targ.m_ShowSceneSetup=EditorGUILayout.Foldout(targ.m_ShowSceneSetup, "Scene options");
    if(!targ.m_ShowSceneSetup)return;
		creator.SceneName=EditorGUILayout.TextField("  Scene name", creator.SceneName);
		Texture2D texture=Camera.main.GetComponent<CameraControls>().BackgroundTexture.GetComponent<Renderer>().sharedMaterial.mainTexture as Texture2D;
		texture=EditorGUILayout.ObjectField("  Background", texture, typeof(Texture2D),false) as Texture2D;
    string texturePath = AssetDatabase.GetAssetPath(texture);
    if (!(texturePath.StartsWith("Assets/Resources/Background/") || texturePath.StartsWith("Assets\\Resources\\Background\\")))
    {
      if(texture!=null)
        Debug.LogWarning("Textures should be from \"Resources/Background\" folder.");
      
      texture = null;
    }
		Camera.main.GetComponent<CameraControls>().BackgroundTexture.GetComponent<Renderer>().sharedMaterial.mainTexture=texture;
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
    activeLevel = EditorGUILayout.IntPopup("  Active Level", activeLevel, levelNames, levelValues);
    //Debug.Log(activeLevel);

    targ.ActiveLevel = activeLevel;
    //Debug.Log(targ.ActiveLevel);
    if (GUI.changed||loaded)
    {
			loaded=false;
      OnChangedActiveLevel();
    }
  }
  void ShowInstrumentSetup()
  {
    targ.ShowInstrument = EditorGUILayout.Foldout(targ.ShowInstrument, "Instrument");
    if (!targ.ShowInstrument) return;
    targ.InstrumentRadius = EditorGUILayout.Slider("  Radius", targ.InstrumentRadius / 16, 1, 50) * 16;
    m_ShowBackground=EditorGUILayout.Toggle("  Show Background", m_ShowBackground);
    Creator.creator.testBuild=EditorGUILayout.Toggle("  Test build", Creator.creator.testBuild);
    Creator.creator.testPortal=EditorGUILayout.ObjectField("  Test portal", Creator.creator.testPortal, typeof(GameObject), true) as GameObject;
    if(GUI.changed)
      Camera.main.GetComponent<CameraControls>().Background.SetActive(m_ShowBackground);
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
    targ.m_ShowLevelSetup=EditorGUILayout.Foldout(targ.m_ShowLevelSetup,"Level options");
    if(!targ.m_ShowLevelSetup)return;
    GUILayout.Space(10);
    m_activeLevelSize = EditorGUILayout.IntSlider("  NumAreas", m_activeLevelSize, 1, 8);
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
    string name = EditorGUILayout.TextField("  Name", targ.levels[(targ.ActiveLevel)].name);
    if (GUI.changed)
    {
      targ.levels[(targ.ActiveLevel)].name = name;
    }
    float t = EditorGUILayout.FloatField("  Move phase duration", targ.levels[targ.ActiveLevel].MovePhaseDuration);
    if (t < 0) t = 0;
    targ.levels[targ.ActiveLevel].MovePhaseDuration = t;
    t = EditorGUILayout.FloatField("  Action phase duration", targ.levels[targ.ActiveLevel].SelectionPhaseDuration);
    if (t < 0) t = 0;
    targ.levels[targ.ActiveLevel].SelectionPhaseDuration = t;
    targ.levels[targ.ActiveLevel].SelectionPhaseType = (PhaseType)EditorGUILayout.EnumPopup("  Action phase type", targ.levels[targ.ActiveLevel].SelectionPhaseType);
    if (targ.levels.Count > 1)
    {
      if (GUILayout.Button("DeleteLevel") && EditorUtility.DisplayDialog("Delete level", "Are you sure?", "Yes", "No"))
      {
        BareerLevelControls x = targ.levels[(targ.ActiveLevel)];
        foreach(CustomObject z in targ.Objects)
        {
          if(z==null)continue;
          if(z.Level>targ.ActiveLevel)
          {
            z.Level--;
          }
          else if(z.Level==targ.ActiveLevel)
          {
						
            DestroyImmediate(z.gameObject);
          }
        }
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
    OnObjectCreateCheck();
    CheckEvent();		
		if (loaded)
    {
			loaded=false;
      OnChangedActiveLevel();
    }
  }
	
  void CheckEvent()
  {
    Event currentEvent = Event.current;
    if (currentEvent.type == EventType.ScrollWheel) return;
    if (currentEvent.button == 2) return;
    if (currentEvent.type == EventType.Repaint) return;
    if (currentEvent.type == EventType.Layout) return;
    if (currentEvent.type == EventType.KeyDown || currentEvent.type == EventType.KeyUp) return;
    
    Event.current.Use();
  }
  void OnInstrument()
  {
		Vector3 mouseCoords = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
      mouseCoords.y = 0;
    if ((targ.selected > 0) && (targ.selected < 5))
    {
      
      Color instrumentColor = Color.red;
      instrumentColor.a = 0.2f;
      Handles.color = instrumentColor;
      Handles.DrawSolidDisc(mouseCoords, Vector3.up, targ.InstrumentRadius);
			
    }
		if(targ.selected>=5)
		{	
			targ.showObject.Init(targ.objectNames[targ.selected-5]);
			targ.showObject.transform.position=GraphNode.GetNodeByCoords(mouseCoords, targ.ActiveLevel).NodeCoords();
		}
		else
		{
			targ.showObject.transform.position=new Vector3(-1000, 1000, -1000);
			targ.showObject.Init ("");
		}
  }

  void OnBareerEditCheck()
  {
    int index = targ.selected;
    if (targ.selected == 0 || targ.selected > 4) return;
    if (index == 0) return;
    index = 4 - index;
    
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

          GraphNode.GetNodeByCoords(left, targ.ActiveLevel).ChangeState(leftActives, targ.levels, true);
          GraphNode.GetNodeByCoords(right, targ.ActiveLevel).ChangeState(rightActives, targ.levels, true);
          left.x -= triangleWidth;
          right.x += triangleWidth;
          numVertecies += 2;
        }
      }
      center.z -= triangleHeight;
    }
  }

	void OnObjectCreateCheck()
	{
		if (targ.selected <= 4) return;
		if (Event.current.type != EventType.MouseDown || Event.current.button != 1) return;
		Vector3 mouseCoords = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
    mouseCoords.y = 0;
		EditorMenu.CreateObject(GraphNode.GetNodeByCoords(mouseCoords, targ.ActiveLevel).NodeCoords());
	}

	public static void SaveLevel()
	{
		string m_path="Assets/Resources";
		string name= Creator.creator.SceneName+".xml";
        Encoding encoding = Encoding.GetEncoding("UTF-8");
        StreamWriter fout = new StreamWriter(m_path + "/" + name, false, encoding);
		MemoryStream stream=new MemoryStream();
		LevelObjectsInfo objects=new LevelObjectsInfo();
		objects.objectsInfo=EditorAdditionalGUI.EditorOptions.Objects.ConvertAll<CustomObjectInfo>(x=>x.SerializeObject());
		objects.info=EditorAdditionalGUI.EditorOptions.levels.ConvertAll<LevelInfo>(x=>x.SerializeLevel());
		objects.name=Creator.creator.SceneName;
		objects.defaultPortal=Creator.creator.defaultPortal.ObjectID;
		objects.mainTexture=Camera.main.GetComponent<CameraControls>().BackgroundTexture.GetComponent<Renderer>().sharedMaterial.mainTexture.name;
		System.Type[] types=new System.Type[EditorAdditionalGUI.EditorOptions.prefabs.Count+1];
		for(int i=0; i<EditorAdditionalGUI.EditorOptions.prefabs.Count; i++)
		{
			types[i]=EditorAdditionalGUI.EditorOptions.prefabs[i].GetComponent<CustomObject>().SerializedType();
		}
		types[EditorAdditionalGUI.EditorOptions.prefabs.Count]=typeof(LevelInfo);
		XmlSerializer serializer=new XmlSerializer(typeof(LevelObjectsInfo), types);
		//XmlTextWriter writer=new XmlTextWriter(stream, Encoding.UTF8);
        serializer.Serialize(fout, objects);
		//Debug.Log(stream.Length);
		
	    //fout.Write(stream);
		fout.Close();
		AssetDatabase.Refresh();
		
		//EditorUtility.SetDirty();
	}
	public static void LoadLevel(string levelName)
	{
		Creator.prefabs=EditorAdditionalGUI.EditorOptions.prefabs;
		LevelObjectsInfo x = LevelObjectsInfo.LoadLevelInfo(levelName);
		EditorAdditionalGUI targ = EditorAdditionalGUI.EditorOptions;
		targ.ClearScene();
    GraphNode.ClearAll();
		targ.levels=x.info.ConvertAll<BareerLevelControls>(y=>y.Deserialize());
		Creator.creator.levels=targ.levels;
		targ.Objects=x.objectsInfo.ConvertAll<CustomObject>(y=>y.Deserialize());
		Creator.creator.defaultPortal=CustomObjectInfo.GetObjectByID(x.defaultPortal) as DistantPortalExit;
		x.objectsInfo.ForEach(y=>y.EstablishConnections());
		if(x.mainTexture.Length>0)
		  Camera.main.GetComponent<CameraControls>().BackgroundTexture.GetComponent<Renderer>().sharedMaterial.mainTexture=Resources.Load("Background/"+x.mainTexture, typeof(Texture2D)) as Texture2D;

		Creator.creator.SceneName=x.name;
		loaded=true;
		Selection.activeObject=null;
		Selection.activeObject=targ;
		
	}

}
