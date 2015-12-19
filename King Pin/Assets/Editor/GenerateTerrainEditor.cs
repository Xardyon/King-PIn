using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GenerateTerrain))]

public class GenerateTerrainEditor : Editor {

	public GenerateTerrain generateTerrain;

	public override void OnInspectorGUI(){
		generateTerrain = (GenerateTerrain)target;
    	DrawDefaultInspector();
    	
    	EditorGUILayout.BeginHorizontal();
    	    if(GUILayout.Button("Clean Up"))
			{
				generateTerrain.CleanUp();
			}
			if(GUILayout.Button("Generate"))
			{
				generateTerrain.Generate();
			}
		EditorGUILayout.EndHorizontal();
    	
	}
	
}
