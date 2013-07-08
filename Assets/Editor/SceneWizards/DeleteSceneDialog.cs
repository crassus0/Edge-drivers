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
    m_sceneName = GetSceneNameFromPath(EditorApplication.currentScene);
    if (m_sceneName == "BasicScene" || m_sceneName == "GlobalMap" || EditorBuildSettings.scenes.Length <= 1)
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
    foreach (string x in Directory.GetFiles("Assets/Levels/" + m_sceneName))
    {
      File.Delete(x);
    }
    Directory.Delete("Assets/Levels/" + m_sceneName);
    
    string path="Assets/Levels/" + m_sceneName + "/" + m_sceneName + ".unity";
    List<EditorBuildSettingsScene> scenes= new List<EditorBuildSettingsScene>();

    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++ )
    {
      if (!EditorBuildSettings.scenes[i].path.Equals(path))
      {
        scenes.Add(EditorBuildSettings.scenes[i]);
      }
    }
    EditorBuildSettings.scenes = scenes.ToArray();
    EditorApplication.OpenScene(EditorBuildSettings.scenes[0].path);
  }
  public static string GetSceneNameFromPath(string path)
  {
    string name = path.Remove(path.Length - 6);
    int i = name.LastIndexOf("/");
    name = name.Remove(0, i + 1);
    return name;
  }
}
