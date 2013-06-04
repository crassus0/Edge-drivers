using UnityEngine;
using System.Collections;

public abstract class ButtonObject : ScriptableObject {

	
	public static GUITexture buttonPrefab;
	bool m_hasGUI=false;
	PlanerCore m_parentPlaner;
	int m_index;
	GUITexture m_button;
	
	protected GUITexture Button
	{
		get{return m_button;}
		set{m_button=value;}
	}
	protected PlanerCore ParentPlaner
	{
	  get{return m_parentPlaner;}
	}
	protected bool HasGUI
	{
	  get{return m_hasGUI;}
	}
	protected int Index
	{
	  get{return m_index;}
	  set{m_index=value;}
	}
	protected bool Activated{get;set;}
	
	public abstract Texture2D GetObjectTexture();
	public abstract void Activate();
	protected virtual void InitGUI (int index)
    {
	  Button= (GUITexture)Instantiate(buttonPrefab);
	  Button.transform.position=new Vector3(1,1-(float)((index+2))/GUIButtonControls.numColumnTextures,0);
	  Button.GetComponent<GUIButtonControls>().Init(this, Allign.Right);
	  Button.name="ControlButton";	
    }
	public virtual void SetIndex (int i)
	{
	  Button.transform.position=new Vector3(1,1-(float)((i+2))/GUIButtonControls.numColumnTextures,0);
	}
	public void Init(PlanerCore planer, int index)
	{
	  m_parentPlaner=planer;
	  Activated=false;
	  if(planer==Creator.Player)
	  {
		
		m_index=index;
		InitGUI(index);
		m_hasGUI=true;
	  }

	}
	protected abstract void OnLoop();
	public void OnUpdate(){if(Activated) OnLoop();}
}
