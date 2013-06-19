using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

[ExecuteInEditMode()]

public class EditorAdditionalGUI : MonoBehaviour
{
  public Texture2D[] buttonTextures;
  public string[] objectNames;

  public int selected { get; set; }
  public List<BareerLevelControls> levels { get; set; }
  public List<CustomObject> objects
  {
    get
    {
      return m_objects;
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
      if (m_editor == null)
      {
        GameObject x = GameObject.Find("EditorOptions");
        if (x != null)
          m_editor = x.GetComponent<EditorAdditionalGUI>();
      }
      return m_editor;
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
        objects.Add((x as CustomObject));
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

    if (!Application.isEditor)
    {

      Destroy(this);

    }
    //gameObject.hideFlags=0;

  }
  void OnGUI()
  {

  }
  void SetFlags()
  {
    //	  Debug.Log(levels.Count);

    for (int i = 0; i < levels.Count; i++)
    {
      //		Debug.Log(i);
      levels[i].gameObject.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
    }
    //levels[ActiveLevel].gameObject.hideFlags=0;
    //	  Debug.Log(transform.hideFlags);
    gameObject.transform.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
    //	  Debug.Log(transform.hideFlags);
    //gameObject.hideFlags=HideFlags.HideInHierarchy;
    //hideFlags=0;
  }
}
