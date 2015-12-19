using UnityEngine;
using System.Collections;

public class FullScreenButton : MonoBehaviour {

	[HideInInspector] public RTSMaster rtsm;

	public GameObject mainCanvas;
	
	public ButtonObject fullScreenButton;	
	
	
	
	public void Build(){
    	mainCanvas = rtsm.mainCanvas;
		BuildMenu();
		RenameButtons();
	}	

	
	
	void BuildMenu(){
		fullScreenButton = new ButtonObject();
  		fullScreenButton.rtsm = rtsm;
  		fullScreenButton.buttonCanvas = mainCanvas;
        fullScreenButton.isChangeableText = true;
		fullScreenButton.textPixelRatio = 2f/3f;
		fullScreenButton.textChangeFactor = 0.05f;
  		fullScreenButton.SetButton();
  		fullScreenButton.MedievalStyle();
  		fullScreenButton.tx_button.text = "FS";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			fullScreenButton.rectTransform,
  			0.97f,0.95f,
			1f,1f,
			0
  		);
	}
	
	
	void RenameButtons(){
    	fullScreenButton.buttonGo.name = "FullScreenButton";
    }

	
	
	void Start () {
		AddFunctionality();
	}
	
	void AddFunctionality(){
		fullScreenButton.button.onClick.AddListener(delegate {
			EnterFullScreen();
		});
	}

	
	public void EnterFullScreen(){
		Screen.fullScreen = !Screen.fullScreen;
	}
	
	
}
