using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelObjectsInfo
{
	public string name;
  public List<CustomObjectInfo> objectsInfo;
	public List<LevelInfo> info;
	public static LevelObjectsInfo LoadLevelInfo(string levelName)
	{
		return null;
	}
}