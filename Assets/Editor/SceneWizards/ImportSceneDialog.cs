using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public static class ImportSceneDialog
{
  static string m_sceneName;
  public static void Import()
  {
    string importPath = EditorUtility.OpenFolderPanel("Export to", "", DeleteSceneDialog.GetSceneNameFromPath(EditorApplication.currentScene));
    //Debug.Log(exportPath);
    if (importPath.Length == 0)
      return;
    string[] files = Directory.GetFiles(importPath);
    //Debug.Log(importPath);
    string curentPath = "Assets/Levels/";
    string levelName = "";

    foreach (string x in files)
    {
      string y = x.Replace('\\', '/');
      //Debug.Log(y);
      //Debug.Log(x);
      if (y.Contains(".unity"))
      {
        levelName = DeleteSceneDialog.GetSceneNameFromPath(y);
        //Debug.Log(levelName);
        curentPath = curentPath + levelName;
      }
    }
    //Debug.Log(curentPath);
    Directory.CreateDirectory(curentPath);
    foreach (string x in files)
    {
      string y = x.Replace('\\', '/');
      //Debug.Log(y);
      if (!y.Contains(".meta"))
      {
        //Debug.Log(y.Remove(0, y.LastIndexOf("/")));
        File.Copy(y, curentPath + y.Remove(0, y.LastIndexOf("/")), true);
      }
    }
    List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
    m_sceneName=curentPath + "/" + levelName + ".unity";
    if (scenes.Find(SceneMatchPredicate)==null) 
      scenes.Add(new EditorBuildSettingsScene(m_sceneName, true));
    EditorBuildSettings.scenes = scenes.ToArray();
    EditorApplication.OpenScene(m_sceneName);
  }
  static bool SceneMatchPredicate(EditorBuildSettingsScene scene)
  {
    return scene.path == m_sceneName;
  }
}