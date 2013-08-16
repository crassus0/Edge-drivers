using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class Creator : MonoBehaviour
{
  public readonly string VERSION = "0.3.0";
  int subStep = 0;
  public bool testBuild = false;
	bool m_loading=false;
  public IPortalExit testEnter 
  {
    get 
    {
      if(testPortal!=null)
        return testPortal.GetComponent<CustomObject>() as IPortalExit;
      return null;
    }
  }
  public string SceneName;
  public GameObject testPortal;
  public List<BareerLevelControls> levels;
  public static string PreviousLevel { get; set; }
  public static bool IsLoading { get; set; }
	public GameObject levelPrefab;
  public GameObject playerPrefab;
	public GameObject initializerPrefab;
  public DistantPortalExit defaultPortal;
	//public 
	public static List<GameObject> prefabs;
	public static List<Texture2D> textures;
  public float TurnDuration
  {
    get
    {
      return m_turnDuration; 
    }
    set
    {
      m_turnDuration = value;
      SetSpeed();
    }
  }
  [SerializeField]
  float m_turnDuration = 1f;
  static Creator m_creator;
  static int m_energy;
  public List<CustomObject> m_objects{get; set;}
  HashSet<CustomObject> m_removeObjects;
  HashSet<CustomObject> m_addObjects;
  HashSet<CustomObject> m_startObjects;
	static GraphNode s_savedNode;
  static int s_savedDirection;
	static List<CustomObject> s_objects;
	static string s_prevName;
  //    static int m_step=0;
  int numObjects;
  bool m_init = false;
  static uint m_curentID = 1;
  public static PlanerCore m_player;
  int m_direction;
  float m_turnTime = 0.1f;
  float m_pauseTime;
  PhaseType m_phaseType=PhaseType.OnAction;
  static bool firstLoad=true;
  bool isMainCreator=true;
  public static Creator creator
	{
		get 
		{
			if(m_creator==null)
				m_creator=GameObject.Find("Creator").GetComponent<Creator>();
			return m_creator; 
		} 
	}
  public static PlanerCore Player { get { return m_player; } }
  public static bool OnPause { get; set; }
  public static int Level 
	{
		get
		{
			try	{	return Player.Level;}
			catch	{	return 0;	}
		} 
	}
  public static bool Initialised { get { return m_creator.m_init; } }
	
	public void Awake()
	{
		Instantiate(initializerPrefab);
		prefabs=new List<GameObject>(EditorAdditionalGUI.EditorOptions.prefabs);
		textures=new List<Texture2D>(EditorAdditionalGUI.EditorOptions.additionalTextures);
	  m_addObjects=new HashSet<CustomObject>(EditorAdditionalGUI.EditorOptions.Objects.ToArray());
		Destroy(EditorAdditionalGUI.EditorOptions.gameObject);
	}
  public void Start()
  {
    if (m_init) return;
		if(!(testBuild&&Application.isEditor))
		{
		  foreach(UnityEngine.Object x in Resources.FindObjectsOfTypeAll(typeof(CustomObject)))
		  {
		    if(!(x).name.Contains("Prefab"))
		    {
			    if(x as EmptyObject!=null)continue;
			    Destroy((x as CustomObject).gameObject);
		    }
		  }
		}
    if(m_player==null)
    {
      m_player=(Instantiate(playerPrefab) as GameObject).GetComponent<PlanerCore>();
      m_player.Init();
      m_player.name="Player";
    }
    m_player.Hidden=false;
    Camera.main.GetComponent<CameraControls>().Init();
		string levelName="SafeHouse";
		if(testBuild&&Application.isEditor)
		{
			levelName=SceneName;
		  m_energy=-1;
			UpdateObjectList();
		}
		else
		{
			LoadLevel(levelName);
		}
		LoadGame();
    m_init = true;
		AddObject(m_player);
		SwitchLevel();
		GraphNode.InteractAll();
		m_turnTime = levels[m_energy].SelectionPhaseDuration;
  }
  void SwitchLevel()
  {
    int newEnergy = m_player.Level;
    if (m_energy != newEnergy)
    {
      if (m_energy >= 0)
      {
        levels[m_energy].Deactivate();
        Camera.main.animation.Play("CameraAnimation");
      }
     
      levels[newEnergy].Activate();
      levels[newEnergy].gameObject.SetActive(true);
      foreach (CustomObject x in m_objects)
        if (x != null)
        {
          x.gameObject.SetActive(x.Level == newEnergy);
        }
      m_energy = newEnergy;
      TurnDuration = levels[newEnergy].MovePhaseDuration;
      m_pauseTime = levels[newEnergy].SelectionPhaseDuration;
      m_phaseType = levels[newEnergy].SelectionPhaseType;
      Camera.main.GetComponent<CameraControls>().ForceSetPosition(m_player.transform.position);
    }
  }
  void UpdateObjectList()
  {
    if (m_addObjects == null)
      m_addObjects = new HashSet<CustomObject>();
		if(m_objects==null)
			m_objects=new List<CustomObject>();
    foreach (CustomObject x in m_addObjects)
      if (x != null)
      {
        x.SetNode();
        if (!m_objects.Contains(x))
          m_objects.Add(x);
        x.gameObject.SetActive(x.Level == m_energy);
        m_startObjects.Add(x);
        x.transform.parent=transform;
      }
    m_addObjects.Clear();
    foreach (CustomObject x in m_startObjects)
    {
      x.OnStart();
    }
    m_startObjects.Clear();
    if (m_removeObjects == null)
      m_removeObjects = new HashSet<CustomObject>();
    foreach (CustomObject x in m_removeObjects)
      if (x != null)
      {
        
        if(m_objects.Contains(x))
        {
          m_objects.Remove(x);
          x.Node.LeaveImmidiately(x);
          Destroy(x.gameObject);

        }
      }

    m_removeObjects.Clear();
  }
  void Update()
  {
    if (!m_init) return;
    if(!isMainCreator)
    {

      return;
    }
    UpdateObjectList();
    
    if (OnPause) return;
    Camera.main.GetComponent<CameraControls>().SetNewTargetPosition(m_player.Visualiser.transform.position, m_player.Level, m_turnDuration);
    if (m_turnTime >= 0)
    {
      m_turnTime -= Time.deltaTime;
      if (m_phaseType == PhaseType.Mixed)
      {
        if (!(m_player.HasTarget && m_turnTime - m_pauseTime < 0))
          return;
      }
      else
      {
        return;
      }
    }
    if ((m_phaseType == PhaseType.OnAction) && !m_player.HasTarget)
      return;
    SaveGame();//TODO
    RenewTurnTime();
    subStep++;
    foreach (CustomObject x in m_objects)
      if (x != null&&x.OnUpdate!=null&&(subStep%x.GetStepCount()==0))
      {
        x.OnUpdate();
		    if(m_loading)
			  {
				  m_loading=false;
				  break;
			  }
      }

    GraphNode.InteractAll();
    SwitchLevel();
  }
  public void RenewTurnTime()
  {
    if (m_phaseType == PhaseType.OnAction)
      levels[m_energy].SelectionPhaseDuration = 0;

    m_turnTime += 0.25f;
    if(subStep%m_player.GetStepCount()==0)
      m_turnTime+=levels[m_energy].SelectionPhaseDuration;
  }
  public static uint GetID() { return m_curentID++; }
  public static byte[] GetBareerMap(int i)
  {
    return creator.levels[i].Bareers;
  }
  public static void AddObject(CustomObject newObject)
  {
    //Debug.Log(newObject.Node);
    if (creator.m_addObjects == null)
      creator.m_addObjects = new HashSet<CustomObject>();
    if (creator.m_startObjects == null)
      creator.m_startObjects = new HashSet<CustomObject>();
    creator.m_addObjects.Add(newObject);
    // if(m_init)
    // 
  }
  public static void DestroyObject(CustomObject obj)
  {
    if (creator.m_removeObjects == null)
      creator.m_removeObjects = new HashSet<CustomObject>();
    creator.m_removeObjects.Add(obj);
  }
	void ClearScene()
	{
		Debug.Log("Clear");
		BareerLevelControls.loadingLevel=true;
		foreach(BareerLevelControls x in levels)
		{
			Destroy(x.gameObject);
		}
		levels.Clear();
		BareerLevelControls.loadingLevel=false;
		ClearObjects();
		GraphNode.ClearAll();

	}
	void ClearObjects()
	{
	  if(m_objects==null)return;
		foreach(CustomObject x in m_objects)
		{
			if(!ReferenceEquals(x, m_player))
			  Destroy(x.gameObject);
		}
		
		m_objects.Clear();
		foreach(CustomObject x in m_addObjects)
		{
			  Destroy(x.gameObject);
		}
		
		m_addObjects.Clear();
		foreach(CustomObject x in m_removeObjects)
		{
			  Destroy(x.gameObject);
		}
		m_removeObjects.Clear();
		foreach(CustomObject x in m_startObjects)
		{
			  Destroy(x.gameObject);
		}
		
		m_startObjects.Clear();
	}
	public void LoadLevel(string levelName)
	{
		LevelObjectsInfo x = LevelObjectsInfo.LoadLevelInfo(levelName);
		GraphNode.ClearAll();
		ClearScene();
		levels=x.info.ConvertAll<BareerLevelControls>(y=>y.Deserialize());
		m_objects=x.objectsInfo.ConvertAll<CustomObject>(y=>y.Deserialize());
		x.objectsInfo.ForEach(y=>y.EstablishConnections());
		defaultPortal=CustomObjectInfo.GetObjectByID(x.defaultPortal) as DistantPortalExit;
		SceneName=x.name;
		AddObject(m_player);
    PlayerSaveData.SetPlanerData(m_player, false);
		m_energy=-1;
		m_loading=true;
		SwitchLevel();
	}
  public void LoadHome()
  {
    s_objects=m_objects;
		m_objects=null;
		s_savedNode=m_player.Node;
		s_savedDirection=m_player.Direction;
		s_prevName=SceneName;
		foreach(CustomObject x in s_objects)
		{
			x.gameObject.SetActive(false);
		}
		LoadLevel("SafeHouse");
  }
  public void LoadPrev()
  {
    LoadLevel(s_prevName);
		ClearObjects();
		foreach(CustomObject x in s_objects)
			AddObject(x);
		m_player.Node=s_savedNode;
		m_player.SetNewDirection(s_savedDirection, true);
  }
  void SaveGame()
  {
    PlayerSaveData.Save(m_player, null,-1, false);
  }
  bool LoadGame()
  {

    bool t =PlayerSaveData.SetPlanerData(m_player, firstLoad);

    firstLoad=!t;
    SaveGame();
		Camera.main.GetComponent<CameraControls>().ForceSetPosition(m_player.transform.position);
    return t;
    
  }
	public void Pause()
	{
		if(OnPause)
		{
			OnPause=false;
			SetSpeed();
		}
		else
		{
			OnPause=true;
			SetSpeed(0);
		}
	}
  void SetSpeed(float modifier=1)
  {
    Time.timeScale = modifier/ TurnDuration;
    //Time.fixedDeltaTime *= 1 / TurnDuration;
  }
  void OnApplicationQuit()
  {
    PlayerSaveData.Clear();
  }
  struct xTest
  {
    int x;
    GraphNode t;
  }

	public Creator()
	{
		m_creator=this;
	}
}
