using UnityEngine;
using System.Collections;

public class WeaponPrototype : CustomObject, IDeactivatable
{
  public string UpgradeName{get;set;}
  public int IsActive { get; set; }
  public bool ActivateOnStart { get { return false; } }
  public override void OnStart()
  {
    Interact = OnInteract;
    
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if(ReferenceEquals(obj, Creator.Player))
    {
      if (IsActive==0)
      {
        Activate();
        //TODO
      }
      else if(IsActive==1)
      {
        Deactivate();
        //TODO
      }
    }
  }
  public void Activate()
  {

    IsActive = 1;
    transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
  }
  public void Deactivate()
  {

    IsActive = 0;
    transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		WeaponPrototypeInfo x = new WeaponPrototypeInfo();
		x.BasicSerialization(this);
		x.upgradeName=UpgradeName;
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(WeaponPrototypeInfo);
	}
}
public class WeaponPrototypeInfo:CustomObjectInfo
{
	public string upgradeName;
	public override CustomObject Deserialize ()
	{
		WeaponPrototype x = CreateInstance() as WeaponPrototype;
		x.UpgradeName=upgradeName;
		return x;
	}
	public override void EstablishConnections ()
	{
		
	}
	public override string GetName ()
	{
		return "WeaponPrototype";
	}
}