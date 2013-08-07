using UnityEngine;
using System.Collections;

public class Whirlwind : CustomObject
{
  public int spin;
  public override void OnStart()
  {
    if (Node.Tag != NodeTag.None)
    {
      Creator.DestroyObject(this);
      return;
    }
    Node.Tag = NodeTag.Whirlwind;
    Node.TagModifier = spin;
    Interact = OnInteract;
    m_visualiser.animation["Rotate"].speed = -0.2f*spin;
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if (type == InteractType.Stay)
    {
      if (obj as IAutoMove != null)
        (obj as IAutoMove).Direction += spin;
    }
  }
  protected new void OnDestroy()
  {
    base.OnDestroy();
		Node.Tag =NodeTag.None;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		WhirlwindInfo x = new WhirlwindInfo();
		x.node=Node;
		x.instanceID=ObjectID;
		x.spin=spin;
		return x;
	}
}
public class WhirlwindInfo:CustomObjectInfo
{
	public int spin;
	public override CustomObject Deserialize ()
	{
		Whirlwind x = CreateInstance() as Whirlwind;
		x.spin=spin;
		return x;
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "Whirlwind";
	}
}