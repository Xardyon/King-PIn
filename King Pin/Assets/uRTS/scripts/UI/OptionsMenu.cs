using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour {
	
	
	[HideInInspector] public RTSMaster rtsm;

	public GameObject mainCanvas;
	
	public ButtonObject coverPanel;
	
	public ButtonObject optionsButton;	
	public ButtonObject groupingToogle;
	
	public GameObject go_gameSpeed;
	public ButtonObject tx_gameSpeed;
	
	public Slider slider1;
	public GameSpeed sliderGameSpeed;
	
	
	
	public void Build(){
    	mainCanvas = rtsm.mainCanvas;
		BuildMenu();
		RenameButtons();
	}
	
	void BuildMenu(){
	    BuildCoverPanel();
  		BuildOptionsButton();
  		BuildGroupingToogleButton();
  		BuildGameSpeed();
	}
	
	void BuildCoverPanel(){
		coverPanel = new ButtonObject();
  		coverPanel.rtsm = rtsm;
  		coverPanel.buttonCanvas = mainCanvas;
  		coverPanel.SetButton();
  		coverPanel.MedievalStyle();
  		coverPanel.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			coverPanel.rectTransform,
  			0.85f,0f,
			1f,1f,
			0
  		);
  		coverPanel.imageLocation.Add("textures/wood/brownWood");
		coverPanel.SetButtonImage();
		coverPanel.im_button[1].type = Image.Type.Tiled;
		
		coverPanel.buttonGo.SetActive(false);
	}
	
	
	void BuildOptionsButton(){
		optionsButton = new ButtonObject();
  		optionsButton.rtsm = rtsm;
  		optionsButton.buttonCanvas = mainCanvas;
        optionsButton.isChangeableText = true;
		optionsButton.textPixelRatio = 2f/3f;
		optionsButton.textChangeFactor = 0.05f;
  		optionsButton.SetButton();
  		optionsButton.MedievalStyle();
  		optionsButton.tx_button.text = "Options";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			optionsButton.rectTransform,
  			0.85f,0.95f,
			0.969f,1f,
			0
  		);

	}
	
	void BuildGroupingToogleButton(){
		groupingToogle = new ButtonObject();
  		groupingToogle.rtsm = rtsm;
  		groupingToogle.buttonCanvas = mainCanvas;
        groupingToogle.isChangeableText = true;
		groupingToogle.textPixelRatio = 2f/3f;
		groupingToogle.textChangeFactor = 0.05f;
  		groupingToogle.SetButton();
  		groupingToogle.MedievalStyle();
  		groupingToogle.tx_button.text = "Hide grouping menu";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			groupingToogle.rectTransform,
  			0.85f,0.90f,
			1f,0.949f,
			0
  		);
  		
  		groupingToogle.buttonGo.SetActive(false);

	}
	
	void BuildGameSpeed(){
		go_gameSpeed = (GameObject)Instantiate(Resources.Load<GameObject>("UI/Slider"));
		go_gameSpeed.name = "GameSpeed";
//		go_gameSpeed.SetActive(false);
		go_gameSpeed.transform.SetParent(mainCanvas.transform);
		rtsm.screenSizeChangeActions.AddPanelEditor(
  			go_gameSpeed.GetComponent<RectTransform>(),
  			0.85f,0.81f,
			1f,0.85f,
			0
  		);
  		
  		Transform[] ts = go_gameSpeed.GetComponentsInChildren<Transform>();
  		foreach (Transform t in ts){
  			if(t.gameObject.name == "Slider"){
  				rtsm.screenSizeChangeActions.AddPanelEditor(
					t.GetComponent<RectTransform>(),
					0f,0f,
					1f,1f,
					0
				);
				GameObject sliderGo = t.gameObject;
				sliderGameSpeed = sliderGo.AddComponent<GameSpeed>();
				
				slider1 = sliderGo.GetComponent<Slider>();
		//		sliderGameSpeed.slider = slider1;
				
  			}
  			if(t.gameObject.name == "Text"){
  				Text tx = t.GetComponent<Text>();
  				if(tx.text == "Game Speed"){
  					DestroyImmediate(tx.gameObject);
  				}
  			}
  		}
		
		tx_gameSpeed = new ButtonObject();
  		tx_gameSpeed.rtsm = rtsm;
  		tx_gameSpeed.buttonCanvas = mainCanvas;
        tx_gameSpeed.isChangeableText = true;
		tx_gameSpeed.textPixelRatio = 2f/3f;
		tx_gameSpeed.textChangeFactor = 0.05f;
  		tx_gameSpeed.SetButton();
  		tx_gameSpeed.MedievalTransparentStyle();
  		tx_gameSpeed.tx_button.text = "Game Speed";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			tx_gameSpeed.rectTransform,
  			0.85f,0.85f,
			1f,0.899f,
			0
  		);
		
		
		go_gameSpeed.SetActive(false);
		tx_gameSpeed.buttonGo.SetActive(false);
		

	}
	
	void RenameButtons(){
    	optionsButton.buttonGo.name = "optionsButton";	
		groupingToogle.buttonGo.name = "groupingToogle";
		tx_gameSpeed.buttonGo.name = "tx_gameSpeed";
    }

	
	void Start () {
		AddFunctionality();
	}
	
	void AddFunctionality(){
		optionsButton.button.onClick.AddListener(delegate {
			SwitchMenu();
		});
		groupingToogle.button.onClick.AddListener(delegate {
			GroupingToogleFunctionality();
		});
		
		slider1.onValueChanged.AddListener(delegate{
			sliderGameSpeed.ChangeGameSpeed();
		});
	}
	
	void SwitchMenu(){
	    coverPanel.buttonGo.SetActive(!optionsButton.isPressedIn);
		groupingToogle.buttonGo.SetActive(!optionsButton.isPressedIn);
		go_gameSpeed.SetActive(!optionsButton.isPressedIn);
		tx_gameSpeed.buttonGo.SetActive(!optionsButton.isPressedIn);

		optionsButton.isPressedIn = !optionsButton.isPressedIn;
	}
	
	void GroupingToogleFunctionality(){
	    if(groupingToogle.isPressedIn == false){
			groupingToogle.tx_button.text = "Show grouping menu";
		}
		else{
		    groupingToogle.tx_button.text = "Hide grouping menu";
		}
		rtsm.groupingMenu.SetActivity(groupingToogle.isPressedIn);
		groupingToogle.isPressedIn = !groupingToogle.isPressedIn;
	}
	
	
	
}
