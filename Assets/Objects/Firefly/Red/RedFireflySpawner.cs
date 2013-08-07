using UnityEngine;
using System.Collections;

public class RedFireflySpawner : CustomObject
{
  public override void OnStart()
  {
    Interact = OnInteract;
  }
  void OnInteract(CustomObject obj, InteractType type)
  {

    if (obj as PlanerCore != null&&(obj as PlanerCore).State==0)
    {
      RedFirefly x= ScriptableObject.CreateInstance<RedFirefly>();
      x.Activate(obj as PlanerCore);
    }
  }
	public override CustomObjectInfo SerializeObject ()
	{
		RedFireflySpawnerInfo x = new RedFireflySpawnerInfo();
		x.node=Node;
		x.instanceID=ObjectID;
		return x;
	}
}
[System.Serializable]
public class RedFireflySpawnerInfo:CustomObjectInfo
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
		return "RedFireflySpawner";
	}
}