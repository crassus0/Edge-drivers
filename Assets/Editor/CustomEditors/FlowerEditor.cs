using System;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Flower))]
public class FlowerEditor:Editor
{
	Flower targ;
	void OnEnable()
	{
		targ=target as Flower;
		try
		{
			if(targ.m_visualiser.GetComponent<Renderer>().sharedMaterial==null)
		  {
			  targ.m_visualiser.GetComponent<Renderer>().sharedMaterial=new Material(Shader.Find("Transparent/Diffuse"));
		  }
		  if(targ.basicTexture!=null)
 		  {
			  targ.m_visualiser.GetComponent<Renderer>().sharedMaterial.mainTexture=targ.basicTexture;
		  }
		}
		catch
		{
		}
	}

  public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();
		
		if(GUILayout.Button("Select visualizer"))
		{
			Selection.activeGameObject=targ.m_visualiser;
		}
		if(GUI.changed)
		{
			if(targ.basicTexture!=null)
 		  {
			  targ.m_visualiser.GetComponent<Renderer>().sharedMaterial.mainTexture=targ.basicTexture;
		  }
		}
		if(GUILayout.Button("Visualizer home"))
		{
			targ.m_visualiser.transform.localPosition=Vector3.zero;
			targ.m_visualiser.transform.rotation=Quaternion.identity;
			targ.m_visualiser.transform.localScale=Vector3.one;
		}
	}

}

