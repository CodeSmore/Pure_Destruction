using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Destruction2D))]
public class Destruction2DEditor : Editor {

	override public void OnInspectorGUI()
	{
		Destruction2D script = target as Destruction2D;

		script.textureType = (Destruction2D.TextureType)EditorGUILayout.EnumPopup ("Texture Type", script.textureType);
		script.destructionLayer = (DestructionLayer)EditorGUILayout.EnumPopup ("Destruction Layer", script.destructionLayer);
		script.centerOfMass = (Destruction2D.CenterOfMass)EditorGUILayout.EnumPopup ("Center of Mass", script.centerOfMass);

		script.enableSplit = EditorGUILayout.Toggle("Split", script.enableSplit);

		if (script.enableSplit) {
			script.splitLimit = GUILayout.Toggle(script.splitLimit, "Split Limit");

			if (script.splitLimit) {
				script.maxSplits = EditorGUILayout.IntSlider("Max Splits", script.maxSplits, 1, 10);
			}
		}

		//script.createChunks = EditorGUILayout.Toggle("Create Chunks", script.createChunks);

		//script.recalculateCenterPivot = EditorGUILayout.Toggle("Recalculate Pivot", script.recalculateCenterPivot);
	}
}
 