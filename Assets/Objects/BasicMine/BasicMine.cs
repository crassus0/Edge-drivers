using UnityEngine;
using System;
using System.Collections;

public class BasicMine : CustomObject, IFireflyDestroyable
{
  //public BasicMineVisualiser m_visualiser;
  int damage = 1;
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
      m_visualiser.transform.parent = transform.parent;
      (m_visualiser.GetComponent<BasicMineVisualiser>()).OnDestroyed();
      Creator.DestroyObject(this);
    }
  }
  public void FireflyDestroy(YellowFirefly firefly)
  {
    firefly.Direction--;
    Creator.DestroyObject(this);
    Destroyed = true;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		BasicMineInfo x = new BasicMineInfo();
		x.BasicSerialization(this);
	  return x;
	}
	public override Type SerializedType ()
	{
		return typeof(BasicMineInfo);
	}
}
[System.Serializable]
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