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
	public List<Texture2D> additionalTextures;
  public int selected { get; set; }
	
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


  void Start()
  {
    m_editor = this;
    //	  Debug.Log(Editor);

    Object[] customObjects = Resources.FindObjectsOfTypeAll(typeof(CustomObject));
    foreach (Object x in customObjects)
    {
      if (!x.name.Contains("Prefab"))
        Objects.Add((x as CustomObject));
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
    //	  Debug.Log(levels.Count);

    for (int i = 0; i < levels.Count; i++)
    {
      //		Debug.Log(i);
      levels[i].gameObject.hideFlags = 0;// HideFlags.HideInInspector | HideFlags.HideInHierarchy;
    }
    //levels[ActiveLevel].gameObject.hideFlags=0;
    gameObject.transform.hideFlags = 0;// HideFlags.HideInInspector | HideFlags.HideInHierarchy;
		hideFlags=0;
		gameObject.hideFlags=0;
    //	  Debug.Log(transform.hideFlags);
    //gameObject.hideFlags = HideFlags.HideInHierarchy ;
    //hideFlags=0;
  }
}
