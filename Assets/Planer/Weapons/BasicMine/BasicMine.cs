using UnityEngine;
using System;
using System.Collections;

public class BasicMine : CustomObject, IFireflyDestroyable
{
  //public BasicMineVisualiser m_visualiser;
  int damage = 5;
  public override void OnStart()
  {
    Interact = OnInteract;
  }
  public void Init(PlanerCore parent)
  {
    //Destroy(GetComponent<CustomObjectEditorSupply>());
    Node = parent.Node;
    gameObject.SetActive(true);
    
    (m_visualiser.GetComponent<BasicMineVisualiser>()).OnInstall(parent);
  }

  void OnInteract(CustomObject obj, InteractType type)
  {
    //Debug.Log("interact");
    if (type == InteractType.Stay)
    {
      PlanerCore planer = obj as PlanerCore;
      if (planer == null) return;
      planer.OnDamageDealt(damage);
      (m_visualiser.GetComponent<BasicMineVisualiser>()).OnDestroy();
    }
  }
  public void FireflyDestroy(YellowFirefly firefly)
  {
    firefly.Direction--;
    Destroy(gameObject);
    Destroyed = true;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		BasicMineInfo x = new BasicMineInfo();
		x.node=Node;
		x.instanceID=ObjectID;
	  return x;
	}
}
public class BasicMineInfo:CustomObjectInfo
{
	public override CustomObject Deserialize ()
	{
		return CreateInstance();
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "Mine";
	}
}