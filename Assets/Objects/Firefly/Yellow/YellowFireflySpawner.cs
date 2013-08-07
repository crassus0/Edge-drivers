using UnityEngine;
using System.Collections;

public class YellowFireflySpawner : ObjectSpawner
{
  public int spin=1;
  public override void OnStart()
  {
    base.OnStart();
    transform.rotation=Quaternion.identity;
    transform.Rotate(new Vector3(0, Direction * -60, 0));
  }
  protected override void OnUpdated()
  {

    base.OnUpdated();
    if (step == 1)
    {
      transform.Rotate(new Vector3(0, spin * -60, 0));
      Direction = Direction + spin;
    }
  }
  protected new void OnDrawGizmos()
  { 
    Gizmos.color = new Color(0, 0, 0, 0);
    Gizmos.DrawSphere(transform.position, 20);
  }
  public override CustomObjectInfo SerializeObject ()
	{
	  YellowFireflySpawnerInfo x = new YellowFireflySpawnerInfo();
		x.node=Node;
		x.instanceID=ObjectID;
		x.prefabName=prefab.name.Remove(prefab.name.Length-6);
	  x.cooldown=cooldown;	
		x.spin=spin;
		return x;
	}
}
[System.Serializable]
public class YellowFireflySpawnerInfo:ObjectSpawnerInfo
{
	public int spin;
	public override CustomObject Deserialize ()
	{
	  YellowFireflySpawner x = base.Deserialize() as YellowFireflySpawner;
		x.spin=spin;
		return x;
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "YellowFireflySpawner";
	}
}