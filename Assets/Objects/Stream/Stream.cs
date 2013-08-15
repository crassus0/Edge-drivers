using UnityEngine;
using System.Collections;

public class Stream : CustomObject
{
  public int power;
	public int direction;
//	bool m_initialized=false;
  public override void OnStart()
  {

    Node.Tag = NodeTag.Stream;
    Node.TagModifier = power;
		Node.TagTarget=direction;
		transform.rotation=Quaternion.Euler(Vector3.down*direction*60);
		
    //m_visualiser.animation["Rotate"].speed = -0.2f*spin;
  }

  protected new void OnDestroy()
  {
    base.OnDestroy();
		Node.Tag =NodeTag.None;
  }
	public override CustomObjectInfo SerializeObject ()
	{
		StreamInfo x = new StreamInfo();
		x.BasicSerialization(this);
		x.direction=direction;
		x.power=power;
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(StreamInfo);
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "Stream.png");
		Gizmos.color=Color.yellow;
    float ang = (1f/3f)*Mathf.PI* direction;  
    Vector3 dest=Vector3.right*Mathf.Cos (ang)+Vector3.forward*Mathf.Sin(ang);
    Gizmos.DrawRay(transform.position, dest*18);
	}
}
[System.Serializable]
public class StreamInfo:CustomObjectInfo
{
	public int power;
	public int direction;
	public override CustomObject Deserialize ()
	{
		Stream x = CreateInstance() as Stream;
		x.power=power;
		x.direction=direction;
		return x;
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "Stream";
	}
}