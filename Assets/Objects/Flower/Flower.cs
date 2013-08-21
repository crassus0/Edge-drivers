using UnityEngine;
using System.Collections;

public class Flower : CustomObject, IActivatable, IDeactivatable
{
	public bool ActivateOnStart{get {return false;}}
	public Texture2D basicTexture;
	public Texture2D activatedTexture;
  public override void OnStart ()
	{
		m_visualiser.renderer.material=new Material(Shader.Find("Transparent/Diffuse"));
		m_visualiser.renderer.material.mainTexture=basicTexture;
	}
	public void Activate ()
	{
		m_visualiser.renderer.material.mainTexture=activatedTexture;
  }
	
	public void Deactivate ()
	{
		m_visualiser.renderer.material.mainTexture=basicTexture;
	}
	
	public override System.Type SerializedType ()
	{
		return typeof(FlowerInfo);
	}
	public override CustomObjectInfo SerializeObject ()
	{
		FlowerInfo x = new FlowerInfo();
		x.BasicSerialization(this);
		if(activatedTexture!=null)
		  x.activatedTexture=activatedTexture.name;
		if(basicTexture!=null)
		  x.basicTexture=basicTexture.name;
		x.position=m_visualiser.transform.position;
		x.rotation=m_visualiser.transform.rotation;
		x.scale=m_visualiser.transform.localScale;
		return x;
	}
	public void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "Flower");
	}
}
public class FlowerInfo:CustomObjectInfo
{
	public string basicTexture="";
	public string activatedTexture="";
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
	public override CustomObject Deserialize ()
	{
		Flower x = CreateInstance() as Flower;
		if(basicTexture.Length>0)
		{
			x.basicTexture=Resources.Load("Textures/"+basicTexture, typeof(Texture2D)) as Texture2D;
			x.m_visualiser.renderer.sharedMaterial=new Material(Shader.Find("Transparent/Diffuse"));
		  x.m_visualiser.renderer.sharedMaterial.mainTexture=x.basicTexture;
		}
		if(activatedTexture.Length>0)
		  x.activatedTexture=Resources.Load("Textures/"+activatedTexture, typeof(Texture2D)) as Texture2D;
		x.m_visualiser.transform.position=position;
		x.m_visualiser.transform.rotation=rotation;
		x.m_visualiser.transform.localScale=scale;
		return x;
	}
	public override void EstablishConnections ()
	{
		
	}
	public override string GetName ()
	{
		return "Flower";
	}
}