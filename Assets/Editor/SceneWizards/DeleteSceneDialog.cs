using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static  class DeleteSceneDialog 
{
  static string m_sceneName;
  public static void Enable()
  {
    m_sceneName = Creator.creator.SceneName;
    if (m_sceneName == "SafeHouse" || m_sceneName == "GlobalMap" || EditorBuildSettings.scenes.Length <= 1)
    {
      EditorUtility.DisplayDialog("Delete scene", "Unable delete scene", "OK");
      return;
    }
    if (EditorUtility.DisplayDialog("Delete scene", "Are you sure? This action is unrevertable.", "Yes", "No"))
    {
      DeleteCurentScene();
    }
    
    
  }
  static void DeleteCurentScene()
  {
		File.Delete("Assets/Resources/"+m_sceneName+".xml");
		File.Delete("Assets/Resources/"+m_sceneName+".xml.meta");
	  CreatorEditor.LoadLevel("SafeHouse");
	}
  static string GetSceneNameFromPath(string path)
  {
    string name = path.Remove(path.Length - 6);
    int i = name.LastIndexOf("/");
    name = name.Remove(0, i + 1);
    return name;
  }
}
