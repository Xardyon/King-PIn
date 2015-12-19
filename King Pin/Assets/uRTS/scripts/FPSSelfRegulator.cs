using UnityEngine;
using System.Collections;

public class FPSSelfRegulator : MonoBehaviour {
	
	private FPSCount fpsCounter;
	[HideInInspector] public SpritesManagerMaster smm;
	[HideInInspector] public RTSMaster rtsm;
	private float M = 0;
	
	public bool displayMessage = true;
	
	// Use this for initialization
	void Start () {
	//	fpsCounter = this.gameObject.GetComponent<FPSCount>();
	    fpsCounter = rtsm.fpsCount;
		StartCoroutine(RuntimeCheck());
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	IEnumerator RuntimeCheck() {
		while(true) {
			

			    
			    if(fpsCounter.fps < 60f){
					if(Mathf.Abs(fpsCounter.fps-45f)<40){
						
						M = smm.maxPerfCount;
						M = M+(M*0.01f*(fpsCounter.fps-45f));
						if(M<100f){
							M = 100f;
						}
						else if(M>3000f){
							M = 3000f;
						}
						
						smm.maxPerfCount = M;
					
		//				M = M+(int)(M*0.01f*(fps-40f));
					}
					
				}
				
				
				
			
			yield return new WaitForSeconds (fpsCounter.messageUpdateTime);
		}
	}
	
	void OnGUI(){
		

	//		GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.85f, 500f, 20f),"Self-Regulator count: " + M);
			
		
	}
	
	
}
