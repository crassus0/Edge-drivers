using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ButtonObject : ScriptableObject
{

  public static GUITexture buttonPrefab;
  bool m_hasGUI = false;
  PlanerCore m_parentPlaner;
  int m_index;
  GUITexture m_button;

  public GUITexture Button
  {
    get { return m_button; }
    set { m_button = value; }
  }
  protected PlanerCore ParentPlaner
  {
    get { return m_parentPlaner; }
  }
  protected bool HasGUI
  {
    get { return m_hasGUI; }
  }
  protected int Index
  {
    get { return m_index; }
    set { m_index = value; }
  }
  protected bool Activated { get; set; }

  public abstract Texture2D GetObjectTexture();
  public abstract void Activate(bool isUp);
  protected virtual void InitGUI(int index)
  {
    if (!Application.isPlaying) return;
    Button = (GUITexture)Instantiate(buttonPrefab);
    Button.transform.parent = Creator.GUIFolder.transform;
    DontDestroyOnLoad(Button);
    Button.GetComponent<GUIButtonControls>().Init(this, Allign.Right, index);
    Button.name = "ControlButton";
  }
  public virtual void SetIndex(int i)
  {
    Button.transform.position = new Vector3(1, 1 - (float)((i + 2)) / GUIButtonControls.numColumnTextures, 0);
  }
  public void Init(PlanerCore planer, int index)
  {
    m_parentPlaner = planer;
    Activated = false;
    if (planer == Creator.Player)
    {
      m_index = index;
      InitGUI(index);
      m_hasGUI = true;
    }

  }
  protected void OnDestroy()
  {
    if (m_button != null)
      Destroy(m_button.gameObject);
  }

  protected abstract void OnLoop();
  public void OnUpdate() { if (Activated) OnLoop(); }
  public abstract string GetName();
}
