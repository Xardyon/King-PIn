using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class SelectedObjectInfo : MonoBehaviour {

	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	public ButtonObject selectedObjectInfo_image;
	public ButtonObject selectedObjectInfo_barGreen;
	public ButtonObject selectedObjectInfo_barRed;
	public ButtonObject selectedObjectInfo_text;	
	
	public ResizablePanel selectedObjectInfo_barRed_rp;


    public void Build(){
    	mainCanvas = rtsm.mainCanvas;
		
		BuildImage();
		BuildBar();
		BuildText();
		
		DeActivate();
		RenameButtons();
	
	}
	
	public void BuildImage(){
		selectedObjectInfo_image = new ButtonObject();
  		selectedObjectInfo_image.rtsm = rtsm;
  		selectedObjectInfo_image.buttonCanvas = mainCanvas;
  		selectedObjectInfo_image.SetButton();
  		selectedObjectInfo_image.MedievalStyle();
  		selectedObjectInfo_image.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			selectedObjectInfo_image.rectTransform,
  			0.86f,0.69f,
			0.96f,0.9f,
			0
  		);
  		
  		selectedObjectInfo_image.imageLocation.Add("textures/icons/barracks_ico");
  		
  		selectedObjectInfo_image.SetButtonImage();
  		
	}
	
	public void BuildBar(){
		selectedObjectInfo_barGreen = new ButtonObject();
  		selectedObjectInfo_barGreen.rtsm = rtsm;
  		selectedObjectInfo_barGreen.buttonCanvas = mainCanvas;
  		selectedObjectInfo_barGreen.SetButton();
        selectedObjectInfo_barGreen.im_button[selectedObjectInfo_barGreen.im_button.Count-1].color = Color.green;
  		selectedObjectInfo_barGreen.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			selectedObjectInfo_barGreen.rectTransform,
  			0.96f,0.69f,
			0.965f,0.9f,
			0
  		);
  		
		selectedObjectInfo_barRed = new ButtonObject();
  		selectedObjectInfo_barRed.rtsm = rtsm;
  		selectedObjectInfo_barRed.buttonCanvas = mainCanvas;
  		selectedObjectInfo_barRed.SetButton();
        selectedObjectInfo_barRed.im_button[selectedObjectInfo_barGreen.im_button.Count-1].color = Color.red;
  		selectedObjectInfo_barRed.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			selectedObjectInfo_barRed.rectTransform,
  			0.96f,0.83f,
			0.965f,0.9f,
			0
  		);
  		
  		selectedObjectInfo_barRed_rp = rtsm.screenSizeChangeActions.GetResizablePanel(selectedObjectInfo_barRed.rectTransform);
  		
  		
  		
	}
	
	public void BuildText(){
		selectedObjectInfo_text = new ButtonObject();
  		selectedObjectInfo_text.rtsm = rtsm;
  		selectedObjectInfo_text.buttonCanvas = mainCanvas;
        selectedObjectInfo_text.isChangeableText = true;
		selectedObjectInfo_text.textPixelRatio = 2f/3f;
		selectedObjectInfo_text.textChangeFactor = 0.05f;
  		selectedObjectInfo_text.SetButton();
  		selectedObjectInfo_text.MedievalTransparentStyle();
  		selectedObjectInfo_text.tx_button.text = "Barracks";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			selectedObjectInfo_text.rectTransform,
  			0.86f,0.9f,
			0.96f,0.95f,
			0
  		);
  		
	}
	
	
	void RenameButtons(){
    	selectedObjectInfo_image.buttonGo.name = "selectedObjectInfo_image";
    	selectedObjectInfo_barGreen.buttonGo.name = "selectedObjectInfo_barGreen";
    	selectedObjectInfo_barRed.buttonGo.name = "selectedObjectInfo_barRed";
    	selectedObjectInfo_text.buttonGo.name = "selectedObjectInfo_text";
    }
	
	
	
	public void SetHealth(float health){
		if(health < 0f){
			health = 0f;
		}
		if(health > 1f){
			health = 1f;
		}
		
		float lowLimit = health*0.21f + 0.69f;
		rtsm.screenSizeChangeActions.UpdatePanel(
  			selectedObjectInfo_barRed_rp,
  			0.96f,lowLimit,
			0.965f,0.9f,
			0
  		);
		
	}
	
	
	
	
	public void DeActivate(){
		selectedObjectInfo_image.buttonGo.SetActive(false);
		selectedObjectInfo_barGreen.buttonGo.SetActive(false);
		selectedObjectInfo_barRed.buttonGo.SetActive(false);
		selectedObjectInfo_text.buttonGo.SetActive(false);
	}
	
	public void Activate(
		string text,
		Sprite icon,
		float value
	){
		selectedObjectInfo_text.tx_button.text = text;
		selectedObjectInfo_image.im_button[1].sprite = icon;
		SetHealth(value);
		selectedObjectInfo_image.buttonGo.SetActive(true);
		selectedObjectInfo_barGreen.buttonGo.SetActive(true);
		selectedObjectInfo_barRed.buttonGo.SetActive(true);
		selectedObjectInfo_text.buttonGo.SetActive(true);
	}
	
	
	

	void Start () {
	
	}
}
