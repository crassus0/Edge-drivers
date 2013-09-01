using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
[System.Serializable]
public class LevelObjectsInfo
{
	public string name;
  public List<CustomObjectInfo> objectsInfo;
	public List<LevelInfo> info;
	public int defaultPortal;
	[System.Runtime.Serialization.OptionalField]
	public string mainTexture="";
	public static LevelObjectsInfo LoadLevelInfo(string levelName)
	{
   
    System.Type[] types;
    try
    {
		  types=new System.Type[Creator.prefabs.Count+1];
    }
    catch
    {
      Debug.Log(Creator.prefabs);
      throw;
    }
		for(int i=0; i<Creator.prefabs.Count; i++)
		{
			types[i]=Creator.prefabs[i].GetComponent<CustomObject>().SerializedType();
		}
		types[Creator.prefabs.Count]=typeof(LevelInfo);
		XmlSerializer serializer=new XmlSerializer(typeof(LevelObjectsInfo), types);
		TextAsset info= Resources.Load(levelName) as TextAsset;
		
		MemoryStream stream=new MemoryStream(info.bytes);
		LevelObjectsInfo x=serializer.Deserialize(stream) as LevelObjectsInfo;
		return x;
	}
}