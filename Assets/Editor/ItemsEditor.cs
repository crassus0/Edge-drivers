using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
public static class ItemsEditor
{

  [MenuItem("EdgeDrivers/Add Level")]
  static void AddLevel()
  {
    GameObject levelPrefab = AssetDatabase.LoadAssetAtPath("Assets/Terrain/BareerLevels/LevelPrefab.prefab", typeof(GameObject)) as GameObject;
    BareerLevelControls level = (Object.Instantiate(levelPrefab) as GameObject).GetComponent<BareerLevelControls>();
    //	  Debug.Log(EditorAdditionalGUI.EditorOptions);
    level.name = "Level" + EditorAdditionalGUI.EditorOptions.levels.Count;
    EditorAdditionalGUI.EditorOptions.levels.Add(level);
    level.transform.parent = GameObject.Find("Levels").transform;
    level.Init();
  }
  //[MenuItem("EdgeDrivers/Show Hidden")]
  static void ShowHidden()
  {
    Object[] x = Resources.FindObjectsOfTypeAll(typeof(Object));
    foreach (Object a in x)
    {
      a.hideFlags = 0;
    }
  }
  //[MenuItem("EdgeDrivers/Hide Selected")]
  static void HideSelected()
  {
    foreach (GameObject x in Selection.objects)
    {
      x.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
    }
  }
  [MenuItem("EdgeDrivers/Scene/Add Scene")]
  static void AddScene()
  {
    ScriptableWizard.DisplayWizard<AddSceneDialog>("Add Scene");
  }
	[MenuItem("EdgeDrivers/Scene/Save Scene")]
	static void SaveLevel()
	{
		CreatorEditor.SaveLevel();
	}
  [MenuItem("EdgeDrivers/Scene/Select Scene")]
  static void SelectScene()
  {
    ScriptableWizard.DisplayWizard<SelectSceneDialog>("Select Scene");
  }
  [MenuItem("EdgeDrivers/Scene/Delete Scene")]
  static void RemoveScene()
  {
    DeleteSceneDialog.Enable();
  }
	
  [MenuItem("EdgeDrivers/Clear saves")]
  static void ClearSaves()
  {
    PlayerSaveData.Clear();
  }
  [MenuItem("EdgeDrivers/Import texture/Background")]
  static void ImportBackground()
  {
    ImportTexture("Background");
  }
  [MenuItem("EdgeDrivers/Import texture/InfoPicture")]
  static void ImportInfoPicture()
  {
    ImportTexture("InfoPictures");
  }
  [MenuItem("EdgeDrivers/Import texture/Texture")]
  static void ImportTexture()
  {
    ImportTexture("Textures");
  }
  static void ImportTexture(string folder)
  {
    string path = EditorUtility.OpenFilePanel("Select texture", "", "png");
    if(path.Length<1)return;
    string newPath=path.Substring(path.LastIndexOf('/')+1);

    newPath=newPath.Substring(newPath.LastIndexOf('\\')+1);
    newPath="Assets/Resources/"+folder+"/"+newPath;
    File.Copy(path, newPath);
    AssetDatabase.Refresh();
  }
}
