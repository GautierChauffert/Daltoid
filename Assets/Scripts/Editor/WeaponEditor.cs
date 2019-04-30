using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon), true), CanEditMultipleObjects]
public class WeaponEditor : Editor
{
	public override void OnInspectorGUI()
	{
		Weapon script = (Weapon)target;

		DrawDefaultInspector();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Handle", EditorStyles.boldLabel);
		if(script.transform.parent != null) {
			if(GUILayout.Button("Set Handle")) {
				Undo.RecordObject(script, "Set handle point");
				script.handle = new Vector2(-script.transform.localPosition.z, -script.transform.localPosition.y);
			}
		}

		if(GUILayout.Button("Reset Handle")) {
			Undo.RecordObject(script, "Reset handle point");
			script.handle = -0.5f * Vector2.right;
		}
	}

	protected virtual void OnSceneGUI()
	{
		Weapon script = (Weapon)target;

		EditorGUI.BeginChangeCheck();

		Vector2 point = Handles.PositionHandle((Vector2)script.transform.position + script.handle, Quaternion.identity);

		Handles.color = Color.yellow;
		Handles.SphereHandleCap(0, point, Quaternion.identity, 0.1f, EventType.Repaint);
	
		if(EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(script, "Change handle point");
			script.handle = point - (Vector2)script.transform.position;
		}
	}
}