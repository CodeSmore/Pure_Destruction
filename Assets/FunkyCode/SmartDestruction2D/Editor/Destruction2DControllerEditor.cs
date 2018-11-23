using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Destruction2DController))]
public class Destruction2DControllerEditor : Editor {
	static bool visualsFoldout = true;
	static bool foldout = true;
	static bool modifiersFoldout = true;

	override public void OnInspectorGUI()
	{
		Destruction2DController script = target as Destruction2DController;

		script.destructionType = (Destruction2DController.DestructionType)EditorGUILayout.EnumPopup ("Destruction Type", script.destructionType);
		script.destructionLayer.SetLayerType((Destruction2DLayerType)EditorGUILayout.EnumPopup ("Destruction Layer", script.destructionLayer.GetLayerType()));

		EditorGUI.indentLevel = EditorGUI.indentLevel + 2;

		if (script.destructionLayer.GetLayerType() == Destruction2DLayerType.Selected) {
			for (int i = 0; i < 8; i++) {
				script.destructionLayer.SetLayer(i, EditorGUILayout.Toggle ("Layer " + (i + 1), script.destructionLayer.GetLayerState(i)));
			}
		}

		EditorGUI.indentLevel = EditorGUI.indentLevel - 2;

		visualsFoldout = EditorGUILayout.Foldout(visualsFoldout, "Visuals" );
		if (visualsFoldout) {
			EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
			script.drawVisuals = EditorGUILayout.Toggle ("Enable Visuals", script.drawVisuals);

			if (script.drawVisuals  == true) {
				script.color = (Color)EditorGUILayout.ColorField ("Color", script.color);
				script.lineWidth = EditorGUILayout.FloatField ("Sidth", script.lineWidth);
				script.zPosition = EditorGUILayout.FloatField ("Z Position", script.zPosition);
				script.visualScale = EditorGUILayout.Slider("Scale", script.visualScale, 1f, 50f);
			}
			
			EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
		}

		DestructionTypesUpdate (script);

		if (Destruction2DController.DestructionType.Polygon == script.destructionType || Destruction2DController.DestructionType.Modifier == script.destructionType) {

			modifiersFoldout = EditorGUILayout.Foldout(modifiersFoldout, "Mofidiers" );
			if (modifiersFoldout) {
				EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
				
				script.modifierSize = EditorGUILayout.Vector2Field("Size", script.modifierSize);
				script.randomRotation = EditorGUILayout.Toggle("Random Rotation", script.randomRotation);
				if (script.randomRotation == false) {
					script.modifierRotation = EditorGUILayout.FloatField("Rotation", script.modifierRotation);
				}

				script.randomModifierID = EditorGUILayout.Toggle("Random Modifier", script.randomModifierID);
				if (script.randomModifierID == false) {
					script.modifierID = EditorGUILayout.IntField("ModifierID", script.modifierID);
				}

				SerializedProperty myIterator = serializedObject.FindProperty("modifierTextures");
				while (true) {
					Rect myRect = GUILayoutUtility.GetRect(0f, 16f);
					bool showChildren = EditorGUI.PropertyField(myRect, myIterator);
					
					if (myIterator.NextVisible(showChildren) == false) {
						break;
					}

				}
				serializedObject.ApplyModifiedProperties();

					
				EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
			}
		}
	}

	void DestructionTypesUpdate(Destruction2DController script){
		switch (script.destructionType) {

			case Destruction2DController.DestructionType.LinearCut:
				foldout = EditorGUILayout.Foldout(foldout, "Linear Cut Destruction" );
				if (foldout) {
					EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
					
					script.cutSize = EditorGUILayout.FloatField ("Linear Cut Size", script.cutSize);
					if (script.cutSize < 0.01f) {
						script.cutSize = 0.01f;
					}

					EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
				}
				break;

			case Destruction2DController.DestructionType.ComplexCut:
				foldout = EditorGUILayout.Foldout(foldout, "Complex Cut Destruction" );
				if (foldout) {
					EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
					
					script.cutSize = EditorGUILayout.FloatField ("Complex Cut Size", script.cutSize);
					if (script.cutSize < 0.01f) {
						script.cutSize = 0.01f;
					}

					EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
				}
				break;

			case Destruction2DController.DestructionType.Polygon:
				foldout = EditorGUILayout.Foldout(foldout, "Polygon Destruction");
				if (foldout) {
					EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

					script.polygonType = (Polygon2D.PolygonType)EditorGUILayout.EnumPopup ("Type", script.polygonType);
					script.polygonSize = EditorGUILayout.FloatField ("Size", script.polygonSize);
					script.minVertsDistance = EditorGUILayout.FloatField ("Vertices Size", script.minVertsDistance);
					if (script.polygonType == Polygon2D.PolygonType.Circle) {
						script.polygonEdgeCount = EditorGUILayout.IntField ("Edge Count", script.polygonEdgeCount);	
					}
					
					EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
				}
				break;

			case Destruction2DController.DestructionType.PolygonBrush:
				foldout = EditorGUILayout.Foldout(foldout, "Polygon Brush");
				if (foldout) {
					EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

					script.polygonType = (Polygon2D.PolygonType)EditorGUILayout.EnumPopup ("Type", script.polygonType);
					script.polygonSize = EditorGUILayout.FloatField ("Size", script.polygonSize);
					script.minVertsDistance = EditorGUILayout.FloatField ("Vertices Size", script.minVertsDistance);
					if (script.polygonType == Polygon2D.PolygonType.Circle) {
						script.polygonEdgeCount = EditorGUILayout.IntField ("Edge Count", script.polygonEdgeCount);	
					}
					
					EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
				}
				break;
		}
	}
}