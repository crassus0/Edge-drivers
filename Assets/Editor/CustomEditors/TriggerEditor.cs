using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Trigger))]
public class TriggerEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		Trigger targ = target as Trigger;
		targ.ActivateOnStart=EditorGUILayout.Toggle("Activate on start", targ.ActivateOnStart);
		base.OnInspectorGUI ();
	}
}
