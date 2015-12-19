using UnityEngine;
using System.Collections;

public class FPSCount : MonoBehaviour {


	private float startTime = 0f;
	private float updateTime = 0f;
	
	[HideInInspector] public float fps = 0.0f;
	public float messageUpdateTime = 1.0f;
	
	private int frameCount = 0;
	
	
	public bool displayMessage = true;
	
	[HideInInspector] public RTSMaster rtsm;
	
	
	IEnumerator CalculateFPS() {
		while(true) {
			fps = frameCount/(updateTime-startTime);
			startTime = Time.realtimeSinceStartup;
			frameCount = 0;
			yield return new WaitForSeconds (messageUpdateTime);
		}
	}
	
	
	
	
	void Start () {
	
		startTime = Time.realtimeSinceStartup;
	    updateTime = startTime;
	    
		StartCoroutine (CalculateFPS());
	
	}
	
	
	void Update () {
		updateTime = Time.realtimeSinceStartup;
		frameCount++;
	}
	
	
	void OnGUI(){
		

			GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.9f, 500f, 20f),"FPS: " + fps);
			
		
	}
	
	
	
	
}
