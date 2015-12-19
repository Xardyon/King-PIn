using UnityEngine;
using System.Collections;

public class CancelSpawnButton : MonoBehaviour {
	
	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	public ButtonObject cancelSpawnButton;
	
	
	
	public void Build(){
		mainCanvas = rtsm.mainCanvas;
		BuildMenu();
		DeActivate();
		RenameButtons();
	}
	
	
	
	public void BuildMenu(){
		cancelSpawnButton = new ButtonObject();
  		cancelSpawnButton.rtsm = rtsm;
  		cancelSpawnButton.buttonCanvas = mainCanvas;
  		cancelSpawnButton.SetButton();
  		cancelSpawnButton.MedievalStyle();
  		cancelSpawnButton.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			cancelSpawnButton.rectTransform,
  			0.921f,0.4f,
			0.992f,0.5f,
			0
  		);
  		
  		cancelSpawnButton.imageLocation.Add("UI/icons/cancel_ico");
		cancelSpawnButton.SetButtonImage();
		cancelSpawnButton.im_button[1].color = Color.red;

	}
	
	void RenameButtons(){
    	cancelSpawnButton.buttonGo.name = "CancelSpawnButton";
    }
	
	
	
	public void DeActivate(){
		cancelSpawnButton.buttonGo.SetActive(false);
	}
	
	public void Activate(){
		cancelSpawnButton.buttonGo.SetActive(true);
	}
	
	
	void Start () {
		SetFunctionalities();
	}
	
	
	void SetFunctionalities(){
		cancelSpawnButton.button.onClick.AddListener(delegate {
    		rtsm.cameraOperator.StopSelectedSpawning();
    		DeActivate();
    	});
	}
	
}
