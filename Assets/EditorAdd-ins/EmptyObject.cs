using UnityEngine;
using System.Collections;

public class EmptyObject : CustomObject
{
	string m_texture;
  public override void OnStart ()
	{
		
	}
	public void Init(string texture)
	{
		m_texture=texture;
	}
	void OnDrawGizmos()
	{
		  Gizmos.DrawIcon(transform.position, m_texture+".png");
	}
	public override System.Type SerializedType ()
	{
		throw new System.NotImplementedException ();
	}
	public override CustomObjectInfo SerializeObject ()
	{
		throw new System.NotImplementedException ();
	}
}
