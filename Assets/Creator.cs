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
  public GameObject debugGraphNodes;
  int subStep = 0;
  public int size = 1;
  public bool randomLevel = false;
  public List<BareerLevelControls> levels;
  public string levelName;
  public string m_nextLevel;
  public static string PreviousLevel { get; set; }
  public Texture m_concentrationBackground;
  public Texture m_concentrationProgress;
  public static bool IsLoading { get; set; }
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
  static Vector3 m_screenSize;
  static float m_ratio;
  static Creator m_creator;
  static int m_energy;
  static List<CustomObject> m_objects;
  static HashSet<CustomObject> m_removeObjects;
  static HashSet<CustomObject> m_addObjects;
  static HashSet<CustomObject> m_startObjects;
  //    static int m_step=0;
  int numObjects;
  bool m_init = false;
  static uint m_curentID = 1;
  public PlanerCore m_player;
  int m_direction;
  float m_turnTime = 0.1f;
  float m_pauseTime;
  PhaseType m_phaseType=PhaseType.OnAction;
  static bool firstLoad=true;
  public static string NextLevel { get { return creator.m_nextLevel; } set { creator.m_nextLevel = value; } }
  public static Vector3 ScreenSize { get { return m_screenSize; } }
  public static Creator creator { get { return m_creator; } }
  public static PlanerCore Player { get { return m_creator.m_player; } }
  public static bool OnPause { get; set; }
  public static int Level { get { return Player.Level; } }
  //public static int Size{get{return m_staticSize;}}
  public static string DebugMessage { get; set; }
  public static bool Initialised { get { return m_creator.m_init; } }

  public void Start()
  {
    if (m_init) return;
    IsLoading = false;
    OnPause = false;
    m_creator = this;
    m_ratio = (float)Screen.width / Screen.height;
    m_screenSize.y = ((Camera.mainCamera.orthographicSize));
    m_screenSize.x = (m_ratio) * Camera.mainCamera.orthographicSize * 2;
    m_player.Init();
    LoadGame();
    if (m_ratio > 1)
      m_screenSize.z = m_ratio;
    else
      m_screenSize.z = 1;
    Camera.main.GetComponent<CameraControls>().Init();
    for (int i = 0; i < levels.Count; i++)
      levels[i].Level = i;
    //Camera.main.GetComponent<CameraControls>().ForceSetPosition(m_player.transform.position);
    m_init = true;
    if (m_objects == null)
      m_objects = new List<CustomObject>();
    if(m_startObjects==null)
      m_startObjects=new HashSet<CustomObject>();
    UnityEngine.Object[] customObjects = Resources.FindObjectsOfTypeAll(typeof(CustomObject));
    //levels[m_player.Level].Activate();
    foreach (UnityEngine.Object x in customObjects)
    {
      //Debug.Log(x.name);
      if (!x.name.Contains("Prefab"))
      {
        //Debug.Log(x.name);
        m_objects.Add((x as CustomObject));
        (x as CustomObject).gameObject.SetActive(true);
        m_startObjects.Add(x as CustomObject);
      }
    }
    m_energy = -1;
    SwitchLevel();
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
      if (m_energy > 2)
        m_energy = 2;
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
    foreach (CustomObject x in m_addObjects)
      if (x != null)
      {
        if (!m_objects.Contains(x))
          m_objects.Add(x);
        x.gameObject.SetActive(x.Level == m_energy);
        m_startObjects.Add(x);

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
        m_objects.Remove(x);
      }

    m_removeObjects.Clear();
  }
  void Update()
  {
    if (!m_init) return;
    UpdateObjectList();
    GraphNode.InteractAll();
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
    m_screenSize.y = ((Camera.mainCamera.orthographicSize));
    m_screenSize.x = (m_ratio) * Camera.mainCamera.orthographicSize * 2;
    subStep++;
    foreach (CustomObject x in m_objects)
      if (x != null&&x.OnUpdate!=null&&(subStep%x.GetStepCount()==0))
      {
        x.OnUpdate();
      }
    

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
    if (m_addObjects == null)
      m_addObjects = new HashSet<CustomObject>();
    if (m_startObjects == null)
      m_startObjects = new HashSet<CustomObject>();
    m_addObjects.Add(newObject);
    // if(m_init)
    // 
  }
  public static void RemoveObject(CustomObject obj)
  {
    if (m_removeObjects == null)
      m_removeObjects = new HashSet<CustomObject>();
    m_removeObjects.Add(obj);
  }
  public Creator()
  {
    m_creator = this;
  }
  void SaveGame()
  {
    PlayerSaveData.Save(m_player, null, false);
  }
  bool LoadGame()
  {
    bool t =PlayerSaveData.SetPlanerData(m_player, firstLoad);
    firstLoad=false;
    return t;
    
  }
  void SetSpeed()
  {
    Time.timeScale = 1/ TurnDuration;
    Time.fixedDeltaTime *= 1 / TurnDuration;
  }
  void OnDestroy()
  {
    PreviousLevel = Application.loadedLevelName;
    GraphNode.ClearAll();
    try
    {
      m_addObjects.Clear();
      m_objects.Clear();
      m_removeObjects.Clear();
    }
    catch (System.NullReferenceException)
    {
    }
  }
  struct xTest
  {
    int x;
    GraphNode t;
  }
  void OnGUI()
  {
    DrawConcentration();
  }
  void DrawConcentration()
  {

    int screenWidth = Screen.width;
    Rect coords = new Rect(0, 0, screenWidth, Screen.height * 0.1f);

    Rect porgressCoords = new Rect(0, 0, ((float)screenWidth)*(m_player.Concentration/m_player.MaxConcentration), Screen.height * 0.05f);
    Rect texCoords = new Rect(0,0,  m_player.Concentration/m_player.MaxConcentration,1 );
    //Debug.Log(porgressCoords);
    GUI.DrawTexture(coords, m_concentrationBackground);
    GUI.DrawTextureWithTexCoords(porgressCoords, m_concentrationProgress, texCoords);//, progressTexCoords);
  }
}
