using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

//[CustomEditor(typeof(ScreenSizeChangeActions))]

[ExecuteInEditMode]
public class ScreenSizeChangeActionsEditor : MonoBehaviour {
	
	
	public ScreenSizeChangeActions ssca = null;
	public Vector2 screenSize = new Vector2(0f,0f);
	public Vector2 prevScreenSize = new Vector2(0f,0f);
	
	[HideInInspector] public RTSMaster rtsm;
	
// 	public override void OnInspectorGUI(){
//         ssca = (ScreenSizeChangeActions)target;
//         if(ssca.gameMode == false){
//         	screenSize = GetMainGameViewSize();
//         	if(screenSize != prevScreenSize){
//         		
//         		ssca.UpdateEditor();
//         		
//         		prevScreenSize = screenSize;
//         	}
//         }
//         
//         
//         if(GUILayout.Button("Refresh")){
//         	ssca.UpdateEditor();
//         }
//         
//     	DrawDefaultInspector();
//     	
//     	
//     }
    
    void Update(){
    	if(ssca == null){
    	    ssca = GameObject.Find("MainCanvas").GetComponent<ScreenSizeChangeActions>();
    	}
    	if(ssca.gameMode == false){
        	screenSize = GetMainGameViewSize();
        	if(screenSize != prevScreenSize){
        		
        		ssca.UpdateEditor();
        		
        		prevScreenSize = screenSize;
        	}
        }
    }
    
    public static Vector2 GetMainGameViewSize(){
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
		return (Vector2)Res;
	}

}
