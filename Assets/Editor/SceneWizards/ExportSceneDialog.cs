using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public static class ExportSceneDialog
{
  public static void Export()
  {
    string exportPath = EditorUtility.SaveFolderPanel("Export to", "", DeleteSceneDialog.GetSceneNameFromPath(EditorApplication.currentScene));
    //Debug.Log(exportPath);
    if (exportPath.Length == 0)
      return;
    string currentPath = EditorApplication.currentScene.Remove(EditorApplication.currentScene.LastIndexOf("/"));
    foreach (string x in Directory.GetFiles(currentPath))
    {
      if (!x.Contains(".meta"))
        File.Copy(x, exportPath + x.Remove(0, EditorApplication.currentScene.LastIndexOf("/")), true);
    }

  }
}
