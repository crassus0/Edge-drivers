using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Trigger : CustomObject, IActivatable, IDeactivatable
{

  bool targetEntered;
	public Texture2D texture;
	[HideInInspector]
	public string TargetTriggerName;
	public bool MultiUseTrigger;
	public bool OnObjectStay;
  System.Type TargetTrigger;
  bool activeTrigger =false;
  public bool ActivateOnStart 
  {
    get{return activateOnStart;}
    set{activateOnStart=value;}
  }
  [SerializeField]
  bool activateOnStart;
  public int delay=0;
	int stepCount=-1;
  public List<CustomObject> toActivate;
  public List<CustomObject> toDestroy;
  public List<CustomObject> toDeactivate;
	public static GameObject triggerPrefab;
	
  public override int GetStepCount ()
	{
		return 1;
	}
  public void Activate()
  {
    
    if (!activeTrigger)
    {
      ActivateTrigger();
			return;
    }
    if(delay>0)
		{
			if(stepCount<0)
		    stepCount=delay;
		}
	  else
			OnAction();

  }
	void ActivateTrigger()
	{
		activeTrigger = true;
    
		OnUpdate=OnUpdated;
		if(OnObjectStay)
			OnUpdate+=CheckExistance;
		else
			Interact = OnInteractEnter;
	}
	public void Deactivate ()
	{
		activeTrigger = false;
    
		OnUpdate=null;
		Interact = null;
	}
	void DeactivateTrigger()
	{
    activeTrigger = false;
    Interact = null;
		OnUpdate=null;
	}
  public void OnDrawGizmos()
  {

    if (!ActivateOnStart)
    {
      
        Color z = Color.yellow;
			  z.a=0.5f;
			Gizmos.color=z;
      Gizmos.DrawSphere(transform.position - 100 * Vector3.up, 18);
    }

  }
  public void OnDrawGizmosSelected()
  {
    if (toDestroy != null)
      foreach (CustomObject x in toDestroy)
      {
        if (x == null) continue;
			  Color z = Color.red;
			  z.a=0.5f;
        Gizmos.color = z;
        Gizmos.DrawSphere(x.transform.position - 1000 * Vector3.up, 18);
      }
    if (toActivate != null)
      foreach (CustomObject x in toActivate)
      {
        if (x == null) continue;
        Color z = Color.green;
			  z.a=0.5f;
        Gizmos.color = z;
        Gizmos.DrawSphere(x.transform.position - 1000 * Vector3.up, 18);
      }
    if (toDeactivate != null)
      foreach (CustomObject x in toDeactivate)
      {
        if (x == null) continue;
        Color z = Color.yellow;
			  z.a=0.5f;
        Gizmos.color = z;
        Gizmos.DrawSphere(x.transform.position - 1000 * Vector3.up, 18);
      }
  }
  public override void OnStart()
  {
		//ActivateOnStart=true;
		TargetTrigger=System.Type.GetType(TargetTriggerName, false, true);
    if (ActivateOnStart)
    {
      Activate ();
    }
		if(texture!=null)
		{
			m_visualiser.renderer.material=new Material(m_visualiser.renderer.material);
			m_visualiser.renderer.material.mainTexture=texture;
			m_visualiser.renderer.material.color=Color.white;
		}
		else
		{
			Destroy(m_visualiser);
		}
  }
  public void OnInteractEnter(CustomObject obj, InteractType type)
  {
    if (TargetTrigger == null) return;
    if (type == InteractType.Enter) return;
    if (TargetTrigger.IsAssignableFrom(obj.GetType()))
		{ 
				Activate();
		}
  }
	public void OnUpdated()
	{
		if(stepCount<0)return;
	  if(stepCount--==0)
			OnAction();
	}
	public void CheckExistance()
	{
  	if (TargetTrigger == null) return;
		
		bool exists=Node.HasObjectOfType(TargetTrigger);
		if(!exists)
		{
			
			Activate();
		}
	}
	void OnAction()
	{
    
	  foreach (CustomObject x in toActivate)
    {
      if (x as IActivatable != null)
      {
        (x as IActivatable).Activate();
      }
    }
    foreach (CustomObject x in toDeactivate)
    {
      if (x as IDeactivatable != null)
			{
				(x as IDeactivatable).Deactivate();
			}
    }
    foreach (CustomObject x in toDestroy)
    {
      Creator.DestroyObject(x);
    }
		if(!MultiUseTrigger)
			DeactivateTrigger();
	}
	public override CustomObjectInfo SerializeObject ()
	{
		TriggerInfo z = new TriggerInfo();
		z.BasicSerialization(this);
    if (texture != null)
      z.texture = texture.name;
    else
      z.texture = "";
		z.TargetTriggerName=TargetTriggerName;
		z.MultiUseTrigger=MultiUseTrigger;
		z.OnObjectStay=OnObjectStay;
		z.ActivateOnStart=ActivateOnStart;
		z.delay=delay;
		z.toActivate=toActivate.ConvertAll<int>(x=>x.ObjectID);
		z.toDeactivate=toDeactivate.ConvertAll<int>(x=>x.ObjectID);
		z.toDestroy=toDestroy.ConvertAll<int>(x=>x.ObjectID);
		return z;
	}
	public override Type SerializedType ()
	{
		return typeof(TriggerInfo);
	}
}
[System.Serializable]
public class TriggerInfo:CustomObjectInfo
{
	public string texture;
	public string TargetTriggerName;
	public bool MultiUseTrigger;
	public bool OnObjectStay;
	public bool ActivateOnStart;
  public int delay;
	public List<int> toActivate;
  public List<int> toDestroy;
  public List<int> toDeactivate;
	[System.NonSerializedAttribute]
	Trigger trigger;
	public override CustomObject Deserialize()
	{
		
		trigger = CreateInstance() as Trigger;
		
    if(texture!=null&&texture.Length>1)
		{
			trigger.texture= Resources.Load("Textures/"+texture, typeof(Texture2D)) as Texture2D;
		}
		trigger.TargetTriggerName=TargetTriggerName;
		trigger.MultiUseTrigger=MultiUseTrigger;
		trigger.OnObjectStay=OnObjectStay;
		trigger.ActivateOnStart=ActivateOnStart;
		trigger.delay=delay;
		
		return trigger;
	}
	public override void EstablishConnections ()
	{
	  trigger.toActivate=toActivate.ConvertAll<CustomObject>(x=>GetObjectByID(x));
		trigger.toDeactivate=toDeactivate.ConvertAll<CustomObject>(x=>GetObjectByID(x));
		trigger.toDestroy= toDestroy.ConvertAll<CustomObject>(x=>GetObjectByID(x));
	}
  public override string GetName ()
	{
		return "Trigger";
	}
}
