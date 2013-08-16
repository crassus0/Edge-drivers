using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

[ExecuteInEditMode()]

public class EditorAdditionalGUI : MonoBehaviour
{
  public Texture2D[] buttonTextures;
  public string[] objectNames;
	public List<GameObject> prefabs;
  public int selected { get; set; }
	public EmptyObject showObject;
  public List<BareerLevelControls> levels { get; set; }
  public List<CustomObject> Objects
  {
    get
    {
      return m_objects;
    }
		set
		{
			m_objects=value;
		}
  }
  List<CustomObject> m_objects = new List<CustomObject>();
  public int ActiveLevel { get; set; }
  public bool ShowInstrument { get; set; }
  public float InstrumentRadius { get; set; }
  public bool RepeatButton { get; set; }

  public static EditorAdditionalGUI EditorOptions
  {
    get
    {
			if(m_editor==null)
			{
				try
				{
				  m_editor=GameObject.Find("EditorOptions").GetComponent<EditorAdditionalGUI>();
				}
				catch(System.NullReferenceException)
				{
				}
			}
      return m_editor;
    }
		set
		{
			m_editor=value;
		}
  }
  static EditorAdditionalGUI m_editor;
  public CustomObject objectToMove;
	public void ClearScene()
	{
		foreach(CustomObject x in m_objects)
		{
			if(x!=null)
			  DestroyImmediate(x.gameObject);
		}
		BareerLevelControls.loadingLevel=true;
		foreach(BareerLevelControls x in levels)
		{
			if(x!=null)
			 DestroyImmediate(x.gameObject);
		}
		BareerLevelControls.loadingLevel=false;
	}

  void Start()
  {
    m_editor = this;
    Object[] customObjects = Resources.FindObjectsOfTypeAll(typeof(CustomObject));
    foreach (Object x in customObjects)
    {
      if (!x.name.Contains("Prefab")&&(x as EmptyObject==null))
			{
        Objects.Add((x as CustomObject));
			}
    }
		
		
    levels = GameObject.Find("Creator").GetComponent<Creator>().levels;
    //GraphNode.creator=GameObject.Find("Creator").GetComponent<Creator>();
    SetFlags();
    selected = 0;
    ShowInstrument = true;

    InstrumentRadius = 5;

  }

  void Update()
  {
    //	  Debug.Log(ActiveLevel);
    SetFlags();
		for(int i=0; i<m_objects.Count; i++)
		{
			if(m_objects[i]==null)
			{
				m_objects.RemoveAt(i);
				i--;
			}
			else
			{
				m_objects[i].ObjectID=i;
			}
		}
		
    if (Application.isPlaying)
    {

      Destroy(this);

    }
    //gameObject.hideFlags=0;

  }
  void OnGUI()
  {

  }
	public void SceneSave()
	{
		
	}
  void SetFlags()
  {

  }
}
