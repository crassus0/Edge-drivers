using UnityEngine;
using System.Collections;

public class RedFirefly : CustomObject
{
	public override void OnStart ()
	{
		OnUpdate=OnUpdated;
	}
	public void OnUpdated()
	{
		Node=Creator.Player.Node;
	}
	public override CustomObjectInfo SerializeObject ()
	{
		throw new System.NotImplementedException ();
	}
	public override System.Type SerializedType ()
	{
		throw new System.NotImplementedException ();
	}
}
