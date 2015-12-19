using UnityEngine;
using System.Collections;


public class ActiveScreen : MonoBehaviour {

	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	public ButtonObject activeScreen;	
	
	
	
	
	public void Build(){
		mainCanvas = rtsm.mainCanvas;
    	BuildMenu();	
    	RenameButtons();
    	
	}
	
	void BuildMenu(){
		activeScreen = new ButtonObject();
  		activeScreen.rtsm = rtsm;
  		activeScreen.buttonCanvas = mainCanvas;

  		activeScreen.SetButton();
  		activeScreen.MedievalTransparentStyle();
  		activeScreen.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			activeScreen.rectTransform,
  			0.02f,0.03f,
			0.85f,0.95f,
			0
  		);
  		ActiveScreenPEC activeScreenPEC = activeScreen.buttonGo.AddComponent<ActiveScreenPEC>();
  		activeScreenPEC.rtsm = rtsm;
	}
	
	void RenameButtons(){
    	activeScreen.buttonGo.name = "ActiveScreen";
    }
    
    

	void Start () {
	
	}
	

	
	
}
