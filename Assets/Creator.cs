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

  public int size = 1;
  public bool randomLevel = false;
  public List<BareerLevelControls> levels;
  public string levelName;
  public string m_nextLevel;

  public static readonly int Size = 1;
  public float turnDuration = 1f;
  static Vector3 m_screenSize;
  static float m_ratio;
  static Creator m_creator;
  static int m_energy;
  static List<CustomObject> m_objects;
  static HashSet<CustomObject> m_removeObjects;
  static HashSet<CustomObject> m_addObjects;
  //    static int m_step=0;
  int numObjects;
  bool m_init = false;
  static uint m_curentID = 1;
  public PlanerCore m_player;
  int direction;
  float turnTime = 0.1f;
  float pauseTime;
  PhaseType m_phaseType=PhaseType.OnAction;

  public static string NextLevel { get { return creator.m_nextLevel; } set { creator.m_nextLevel = value; } }
  public static Vector3 ScreenSize { get { return m_screenSize; } }
  public static Creator creator { get { return m_creator; } }
  public static PlanerCore Player { get { return m_creator.m_player; } }
  public static bool OnPause { get; set; }
  public static int Energy { get { return Player.Energy; } }
  public static int Level { get { return Player.Level; } }
  //public static int Size{get{return m_staticSize;}}
  public static string DebugMessage { get; set; }
  public static bool Initialised { get { return m_creator.m_init; } }

  public void Init()
  {
    //	   Debug.Log(Application.platform.ToString());
    //	  Debug.Log ("creation started");
    //Debug.Log(m_init);
    if (m_init) return;

    DebugMessage = "Initializing...";
    OnPause = false;
    m_creator = this;
    m_ratio = (float)Screen.width / Screen.height;
    m_screenSize.y = ((Camera.mainCamera.orthographicSize));
    m_screenSize.x = (m_ratio) * Camera.mainCamera.orthographicSize * 2;


    DebugMessage = "Bareers loaded\n";

    m_player.Init();
    LoadGame();
    if (m_ratio > 1)
      m_screenSize.z = m_ratio;
    else
      m_screenSize.z = 1;
    //backgroundLevel.InitLevel(maxLevel);//new BinaryReader(new FileStream("123.txt", FileMode.Create)));
    //      initializedBackground=true;
    DebugMessage = DebugMessage + "Background loaded\n";
    //Debug.Log(ratio);
    Camera.main.GetComponent<CameraControls>().Init();
    Camera.main.GetComponent<CameraControls>().ForceSetPosition(m_player.transform.position);
    m_init = true;
    if (m_objects == null)
      m_objects = new List<CustomObject>();
    UnityEngine.Object[] customObjects = Resources.FindObjectsOfTypeAll(typeof(CustomObject));
    foreach (UnityEngine.Object x in customObjects)
    {
      if (!x.name.Contains("Prefab"))
        m_objects.Add((x as CustomObject));
    }

    m_energy = -1;
    SwitchLevel();
    turnTime = levels[m_energy].SelectionPhaseDuration;
  }
  void SwitchLevel()
  {
    int newEnergy = m_player.Level;

    // Debug.Log(m_energy+","+newEnergy);
    if (m_energy != newEnergy)
    {

      if (m_energy >= 0)
      {
        //Debug.Log(m_energy);
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
      pauseTime = levels[newEnergy].SelectionPhaseDuration;
      m_phaseType = levels[newEnergy].SelectionPhaseType;
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
      }
    m_addObjects.Clear();
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
    if (OnPause) return;
    Camera.main.GetComponent<CameraControls>().SetNewTargetPosition(m_player.Visualiser.transform.position, m_player.Level, turnDuration);
    if (turnTime >= 0)
    {
      turnTime -= Time.deltaTime;
      if (m_phaseType == PhaseType.Mixed)
      {
        if (!(m_player.HasTarget && turnTime - pauseTime < 0))
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
    if (m_objects == null)
      m_objects = new List<CustomObject>();
    if (m_removeObjects == null)
      m_removeObjects = new HashSet<CustomObject>();
    if (m_addObjects == null)
      m_addObjects = new HashSet<CustomObject>();
    foreach (CustomObject x in m_objects)
      if (x != null)
      {
        x.OnUpdate();
      }
    GraphNode.InteractAll();

    SwitchLevel();
  }
  public void RenewTurnTime()
  {
    if (m_phaseType == PhaseType.OnAction)
      levels[m_energy].SelectionPhaseDuration = 0;
    turnTime = turnDuration + levels[m_energy].SelectionPhaseDuration;
  }
  public static uint GetID() { return m_curentID++; }
  public static byte[] GetBareerMap(int i)
  {
    ////	  Debug.Log(i);
    return creator.levels[i].Bareers;
  }

  public static void LoadLevel(bool restart)
  {
    if (restart)
      Application.LoadLevel(Application.loadedLevel);
    else
      Application.LoadLevel(NextLevel);
  }
  public static void AddObject(CustomObject newObject)
  {
    //Debug.Log(newObject.Node);
    if (m_addObjects == null)
      m_addObjects = new HashSet<CustomObject>();
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
    return PlayerSaveData.SetPlanerData(m_player);
  }
  void SetSpeed()
  {
    foreach (CustomObject x in m_objects)
    {
      x.SetSpeed();
    }
  }
  struct xTest
  {
    int x;
    GraphNode t;
  }
}
