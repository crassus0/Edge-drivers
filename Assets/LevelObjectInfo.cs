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
	public static LevelObjectsInfo LoadLevelInfo(string levelName)
	{
		System.Type[] types=new System.Type[EditorAdditionalGUI.EditorOptions.prefabs.Count+1];
		for(int i=0; i<EditorAdditionalGUI.EditorOptions.prefabs.Count; i++)
		{
			types[i]=Creator.prefabs[i].GetComponent<CustomObject>().SerializedType();
		}
		types[EditorAdditionalGUI.EditorOptions.prefabs.Count]=typeof(LevelInfo);
		XmlSerializer serializer=new XmlSerializer(typeof(LevelObjectsInfo), types);
		TextAsset info= Resources.Load(levelName) as TextAsset;
		
		MemoryStream stream=new MemoryStream(info.bytes);
		LevelObjectsInfo x=serializer.Deserialize(stream) as LevelObjectsInfo;
		return x;
	}
}