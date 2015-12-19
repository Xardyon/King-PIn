using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CreateForestEdit))]

public class CreateForestEditEditor : Editor {
    
    public CreateForestEdit cf = null;
    
	public override void OnInspectorGUI()
    {   
        cf = (CreateForestEdit)target;
    	DrawDefaultInspector();
    	
    	
    	EditorGUILayout.BeginHorizontal();
// 			if(GUILayout.Button("Clean"))
// 			{
// 				cf.CleanTrees();
// 			}
			if(GUILayout.Button("Generate"))
			{
				cf.Generate();
			}
		EditorGUILayout.EndHorizontal();
    	
    
    }
}
