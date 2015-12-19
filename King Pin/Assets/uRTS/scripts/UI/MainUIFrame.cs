using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class MainUIFrame : MonoBehaviour {


	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	public ButtonObject topBar;	
	public ButtonObject rightBar;	
	public ButtonObject bottomBar;	
	public ButtonObject leftBar;	
    
    
    
    public void Build(){
    	mainCanvas = rtsm.mainCanvas;
    	BuildMenu();
    	RenameButtons();
    }
    
    
    void BuildMenu(){
    	SetBar(ref topBar,
    		0f,0.95f,
			1f,1f
    	);
    	SetBar(ref rightBar,
    		0.85f,0f,
			1f,1f
    	);
    	SetBar(ref bottomBar,
    		0f,0f,
			1f,0.03f
    	);
    	SetBar(ref leftBar,
    		0f,0f,
			0.02f,1f
    	);
    	
    	
    }
    
    
    
    void SetBar(
    	ref ButtonObject bar,
    	float wMin,
    	float hMin,
    	float wMax,
    	float hMax
    ){
    	bar = new ButtonObject();
  		bar.buttonCanvas = mainCanvas;
  		
  		bar.SetButton();
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar.rectTransform,
  			wMin,hMin,
			wMax,hMax,
			0
  		);
  		
  	//	topBar.MedievalStyle();
  	//	movementButtonGrid.DeActivateButton(bo);
  		bar.tx_button.text = "";
  //		topBar.tx_button.fontSize = 18;
		bar.imageLocation.Add("textures/wood/brownWood");
		bar.SetButtonImage();
		bar.im_button[1].type = Image.Type.Tiled;
    }
    
    
    
    void RenameButtons(){
    	topBar.buttonGo.name = "TopBar";
    	rightBar.buttonGo.name = "RightBar";
    	bottomBar.buttonGo.name = "BottomBar";
    	leftBar.buttonGo.name = "LeftBar";
    }
    

	void Start () {
	
	}
	
}
