using UnityEngine;
using System.Collections;
using UnityEditor;
public class ItemsEditor : MonoBehaviour 
{
	[MenuItem("EdgeDrivers/CreatePlaner")]
    static void CreatePlaner()
	{
//	  Debug.Log("3412345");
	  
	  GameObject planerPrefab=AssetDatabase.LoadAssetAtPath("Assets/Planer/planer.prefab", typeof(GameObject)) as GameObject;
	  GameObject planer=Instantiate(planerPrefab) as GameObject;
	  planer.name="Planer"+EditorAdditionalGUI.EditorOptions.objects.Count;
	  EditorAdditionalGUI.EditorOptions.objects.Add(planer.GetComponent<CustomObject>());
	  planer.name="Planer";
	  
	}
    [MenuItem("EdgeDrivers/AddLevel")]
    static void AddLevel()
	{
	  GameObject levelPrefab=AssetDatabase.LoadAssetAtPath("Assets/Terrain/BareerLevels/LevelPrefab.prefab", typeof(GameObject)) as GameObject;
	  BareerLevelControls level=(Instantiate(levelPrefab) as GameObject).GetComponent<BareerLevelControls>();
//	  Debug.Log(EditorAdditionalGUI.EditorOptions);
	  level.name="Level"+EditorAdditionalGUI.EditorOptions.levels.Count;
	  EditorAdditionalGUI.EditorOptions.levels.Add(level);
	  level.transform.parent=GameObject.Find("Creator").transform;
	  level.Init();
	}
}
