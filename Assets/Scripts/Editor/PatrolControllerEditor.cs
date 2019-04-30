using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolController)), CanEditMultipleObjects]
public class PatrolControllerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		PatrolController script = (PatrolController)target;

		DrawDefaultInspector();

		if(GUILayout.Button("Reset")) {
			Undo.RecordObject(script, "Reset point");

			script.points.Clear();

			Vector2 pos = script.transform.position;

			script.points.Add(pos + Vector2.right * 1f);
			script.points.Add(pos - Vector2.right * 1f);
		}
	}

	protected virtual void OnSceneGUI()
	{
		PatrolController script = (PatrolController)target;

		for(int i = 0; i < script.points.Count; i++) {

			EditorGUI.BeginChangeCheck();

			Vector2 newPoint = Handles.PositionHandle(script.points[i], Quaternion.identity);
		
			if(EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(script, "Change point");

				script.points[i] = newPoint;
				script.OnValidate();
			}
		}
	}
}