using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(RTSCompiler))]

public class RTSCompilerEditor : Editor {
    
    public RTSCompiler rtsc = null;
    
	public override void OnInspectorGUI()
    {
        rtsc = (RTSCompiler)target;
    	DrawDefaultInspector();
    	
    	EditorGUILayout.BeginHorizontal();
    	    if(GUILayout.Button("Clean Up"))
			{
				rtsc.CleanUp();
			}
			if(GUILayout.Button("Generate"))
			{
				rtsc.Compile();
			}
		EditorGUILayout.EndHorizontal();
    }
}
