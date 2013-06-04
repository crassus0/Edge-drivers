using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System;

public class Creator : MonoBehaviour 
{ 
	public readonly string VERSION="0.3.0";
	public GameObject debugGraphNodes;
	
	public int size=1;
	public bool randomLevel=false;
	public List<BareerLevelControls> levels;
    public string levelName;


    public string m_nextLevel;
	
	public static readonly int Size=1;
	static Vector3 m_screenSize;
	static float m_ratio;
	static Creator m_creator;
	static int m_energy;
	static HashSet<CustomObject> m_objects;
	static HashSet<CustomObject> m_removeObjects;
    static HashSet<CustomObject> m_addObjects;
//    static int m_step=0;
	int numObjects;
	static bool m_init=false;
	static uint m_curentID=1;
	public PlanerCore m_player;
	static int numCreators=0;
	int direction;
	float turnTime=0.1f;

	public static string NextLevel {get{return creator.m_nextLevel;} set{creator.m_nextLevel=value;}}
	public static Vector3 ScreenSize{get{return m_screenSize;}}
	public static Creator creator{get{return m_creator;}}
	public static PlanerCore Player{get{return m_creator.m_player;}}
	public static bool OnPause{get; set;}
	public static int Energy{get{return Player.Energy;}}
    public static int Level{get{return Player.Level;}}
	//public static int Size{get{return m_staticSize;}}
	public static string DebugMessage{get;set;}
	public static bool Initialised{get{return m_init;}}
	
	public void Init () 
	{
//	   Debug.Log(Application.platform.ToString());
//	  Debug.Log ("creation started");

	  if(m_init)return;
	  numCreators++;
	  if(numCreators>1)
	  {
		Debug.LogError("Multiply Creators: "+numCreators.ToString());
		throw(new System.Exception("Only one creator is allowed"));
	  }
	  DebugMessage ="Initializing...";
	  OnPause=false;
	  m_creator=this;
      

	  


	  m_player.Init();
	  m_energy=m_player.Level;
	  levels[m_energy].Init();
	  
	  m_ratio=(float)Screen.width/Screen.height;
	  m_screenSize.y=((Camera.mainCamera.orthographicSize));
	  m_screenSize.x=(m_ratio)*Camera.mainCamera.orthographicSize*2;
	  
      //string fileName="levels/"+levelName+".edl";
	  //FileStream stream = new FileStream(fileName, FileMode.Open);
	  //BinaryReader fin = new BinaryReader(stream);
	  //string ver = fin.ReadString();
	  //Debug.Log(File.Exists(fileName));
      

	  Color activeColor=levels[m_energy].renderer.material.color;
	  activeColor.a=1;
      levels[m_energy].renderer.material.color=activeColor;
	  DebugMessage="Bareers loaded\n";
	  
	  	  
	  if(m_ratio>1)
	    m_screenSize.z=m_ratio;
	  else
		m_screenSize.z=1;
	  //backgroundLevel.InitLevel(maxLevel);//new BinaryReader(new FileStream("123.txt", FileMode.Create)));
//      initializedBackground=true;
	  DebugMessage=DebugMessage+"Background loaded\n";
		//Debug.Log(ratio);
	  Camera.main.GetComponent<CameraControls>().Init();
	  Camera.main.GetComponent<CameraControls>().ForceSetPosition(m_player.transform.position, m_energy);
	  m_init=true;
	  if(m_objects==null)
		m_objects=new HashSet<CustomObject>();

	}
	void SwitchLevel()
	{
	  int newEnergy=m_player.Level;
//	  Debug.Log(m_energy+","+newEnergy+","+m_player.Energy);
	  if(m_energy!=newEnergy)
	  {
		
		levels[m_energy].Deactivate();
	    levels[newEnergy].Activate();
        foreach(CustomObject x in m_objects)
	      if(x!=null)
		  {

		    x.gameObject.SetActiveRecursively(x.Level==newEnergy);
		  }

	    m_energy=newEnergy;
		if(m_energy>2)
		  m_energy=2;
	  }
	}
	void UpdateObjectList()
	{
	  if(m_addObjects==null)
		m_addObjects=new HashSet<CustomObject>();
	  foreach(CustomObject x in m_addObjects)
	    if(x!=null)
		{
		  if(!m_objects.Contains(x))
		    m_objects.Add(x); 
		  x.gameObject.SetActiveRecursively(x.Level==m_energy);
		}
	  m_addObjects.Clear();
	  if(m_removeObjects==null)
		m_removeObjects=new HashSet<CustomObject>();
	  foreach(CustomObject x in m_removeObjects)
	    if(x!=null)
		{
		  m_objects.Remove(x);
		}
	  
 	  m_removeObjects.Clear();
	}
	void Update()
	{
	  if(!m_init)return;
	  UpdateObjectList();
	  if(OnPause)return;
	  if(turnTime>=0)
	  {
		turnTime-=Time.deltaTime;
		return;
	  }
	  SwitchLevel();
	  turnTime=1f;
	  m_screenSize.y=((Camera.mainCamera.orthographicSize));
	  m_screenSize.x=(m_ratio)*Camera.mainCamera.orthographicSize*2;
//	  Debug.Log(Input.mousePosition);

	  
      if(m_objects==null)
		m_objects=new HashSet<CustomObject>();
	  if(m_removeObjects==null)
		m_removeObjects=new HashSet<CustomObject>();
	  if(m_addObjects==null)
		m_addObjects = new HashSet<CustomObject>();

	  
	  foreach(CustomObject x in m_objects)
	    if(x!=null)
	    {

	      x.OnUpdate();
	    }
      GraphNode.InteractAll();
	  
	  Camera.main.GetComponent<CameraControls>().SetNewTargetPosition(m_player.transform.position, m_player.Level, 1f);
	  

	  	  //bareerMaterial.color=new Color(1-energy, energy, energy,1);
	}
	public static uint GetID(){return m_curentID++;}
	public static byte[] GetBareerMap(int i)
	{
////	  Debug.Log(i);
	  return creator.levels[i].Bareers;
    }

	public static void LoadLevel(bool restart)
	{
	  if(restart)
		Application.LoadLevel(Application.loadedLevel);
	  else
		Application.LoadLevel(NextLevel);
	}
	public static void AddObject(CustomObject newObject)
	{
	  if(m_addObjects==null)
		m_addObjects = new HashSet<CustomObject>();
	  m_addObjects.Add(newObject);
	 // if(m_init)
	   // 
	}
	public static void RemoveObject(CustomObject obj)
	{
	  if(m_removeObjects==null)
		m_removeObjects=new HashSet<CustomObject>();
	  m_removeObjects.Add(obj);
	}
	public Creator()
	{
	  m_creator=this;
	}
	struct xTest
	{
	  int x;
	  GraphNode t;
	}
}
