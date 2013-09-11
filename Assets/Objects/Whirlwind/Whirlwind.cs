using UnityEngine;
using System.Collections;

public class Whirlwind : CustomObject
{
  public int spin;
//	bool m_initialized=false;
  public override void OnStart()
  {
    if (spin != 0)
    {
      Node.Tag = NodeTag.Whirlwind;
      Node.TagModifier = spin;
    }
    else
      Node.HiddenTag = true;
  }

  protected new void OnDestroy()
  {
    base.OnDestroy();
		Node.Tag =NodeTag.None;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		WhirlwindInfo x = new WhirlwindInfo();
		x.BasicSerialization(this);
		x.spin=spin;
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(WhirlwindInfo);
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "Whirlwind.png");
	}
}
[System.Serializable]
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