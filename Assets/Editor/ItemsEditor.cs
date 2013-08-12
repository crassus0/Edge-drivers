using UnityEngine;
using System.Collections;
using UnityEditor;
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
    level.transform.parent = GameObject.Find("Creator").transform;
    level.Init();
  }
  [MenuItem("EdgeDrivers/Show Hidden")]
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
}
