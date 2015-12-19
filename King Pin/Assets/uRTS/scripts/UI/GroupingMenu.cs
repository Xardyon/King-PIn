using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GroupingMenu : MonoBehaviour {
	
	[HideInInspector] public RTSMaster rtsm;
	[HideInInspector] public UIMaster uim;

	public GameObject mainCanvas;
	
	public ButtonObject bg_panel;
	public ButtonObject cleanGroups;
	public ButtonObject formationMode;
	
	public List<ButtonObject> groupingMenuButtons;
	

	

	public void Build(){
	    uim = rtsm.uiMaster;
		mainCanvas = uim.SetSubCanvas("GroupingMenu");
		BuildMenu();
	}
	
	void BuildMenu(){
		BuildPanel();
		BuildCleanGroups();
		BuildFormationMode();
	}
	
	void BuildPanel(){
		bg_panel = new ButtonObject();
  		bg_panel.rtsm = rtsm;
  		bg_panel.buttonCanvas = mainCanvas;
  		bg_panel.SetButton();
  		bg_panel.MedievalStyle();
  		bg_panel.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bg_panel.rectTransform,
  			0f,0f,
			0.05f,1f,
			0
  		);
  		
  		bg_panel.imageLocation.Add("textures/wood/brownWood");
		bg_panel.SetButtonImage();
		bg_panel.im_button[1].type = Image.Type.Tiled;
	}
		
		
		
	void BuildCleanGroups(){
		cleanGroups = new ButtonObject();
  		cleanGroups.rtsm = rtsm;
  		cleanGroups.buttonCanvas = mainCanvas;
  		cleanGroups.isChangeableText = true;
		cleanGroups.textPixelRatio = 2f/3f;
		cleanGroups.textChangeFactor = 0.05f;
  		cleanGroups.SetButton();
  		cleanGroups.MedievalStyle();
  		cleanGroups.tx_button.text = "Clean";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			cleanGroups.rectTransform,
  			0f,0f,
			0.05f,0.05f,
			0
  		);

	}	


	void BuildFormationMode(){
		formationMode = new ButtonObject();
  		formationMode.rtsm = rtsm;
  		formationMode.buttonCanvas = mainCanvas;
  		formationMode.isChangeableText = true;
		formationMode.textPixelRatio = 2f/3f;
		formationMode.textChangeFactor = 0.05f;
  		formationMode.SetButton();
  		formationMode.MedievalStyle();
  		formationMode.SwitchStyle(3);
  		formationMode.tx_button.text = "F-Mode";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			formationMode.rectTransform,
  			0f,0.051f,
			0.05f,0.1f,
			0
  		);

	}	
		
		
		
		
		
		
	
	void Start () {
		Functionalities();
	}
	
	
	
	
	
	void CleanAllGroups(){
	    int N = groupingMenuButtons.Count;
		for(int i=0; i<N; i++){
			Destroy(groupingMenuButtons[0].buttonGo);
			rtsm.screenSizeChangeActions.RemovePanel(groupingMenuButtons[0].rectTransform);
			groupingMenuButtons.RemoveAt(0);
		}
		rtsm.unitsGrouping.CleanUpGroups();
	}
	
	public void RemoveGroup(int iGroup){
		Destroy(groupingMenuButtons[iGroup].buttonGo);
		rtsm.screenSizeChangeActions.RemovePanel(groupingMenuButtons[iGroup].rectTransform);
		groupingMenuButtons.RemoveAt(iGroup);
		int N = groupingMenuButtons.Count;
		for(int i=iGroup; i<N; i++){
		    ButtonObject groupingMenuButton = groupingMenuButtons[i];
			
			rtsm.screenSizeChangeActions.UpdatePanel(
  				rtsm.screenSizeChangeActions.GetResizablePanel(groupingMenuButton.rectTransform),
  				0f,(1f-0.05f*(i+1)),
				0.05f,(1f-0.05f*i-0.002f),
				0
  			);

	        groupingMenuButton.tx_button.text = (i+1).ToString();
			groupingMenuButton.buttonGo.GetComponent<UIPingPong>().selGroupButtonId = i;
		}
		
	}
	
	public void CreateNewGroup(){
	        int i = groupingMenuButtons.Count;
	        groupingMenuButtons.Add(new ButtonObject());
			
		    
			ButtonObject groupingMenuButton = groupingMenuButtons[i];
			groupingMenuButton.rtsm = rtsm;
			groupingMenuButton.buttonCanvas = mainCanvas;
			groupingMenuButton.isChangeableText = true;
			groupingMenuButton.textPixelRatio = 2f/3f;
			groupingMenuButton.textChangeFactor = 0.05f;
			groupingMenuButton.SetButton();
			groupingMenuButton.MedievalStyle();
			groupingMenuButton.tx_button.text = (i+1).ToString();
		
		
			rtsm.screenSizeChangeActions.AddPanelEditor(
				groupingMenuButton.rectTransform,
				0f,(1f-0.05f*(i+1)),
				0.05f,(1f-0.05f*i-0.002f),
				0
			);
		
			
			UIPingPong uipp = groupingMenuButton.buttonGo.AddComponent<UIPingPong>();
			uipp.selGroupButtonId = i;
			
			groupingMenuButton.button.onClick.AddListener(delegate {
				rtsm.unitsGrouping.SelectGroup(uipp.selGroupButtonId+1);
				
			});
			
			rtsm.unitsGrouping.AssignGroupButton(groupingMenuButton, uipp.selGroupButtonId+1);
			UnitsGroup ug = rtsm.unitsGrouping.GetUnitsGroup(uipp.selGroupButtonId+1);
			if(ug != null){
				if(ug.formationMode == 0){
					groupingMenuButton.SwitchStyle(3);
				}
				if(ug.formationMode == 1){
					groupingMenuButton.SwitchStyle(2);
				}
				
			}
	}

	
	void Functionalities(){
		cleanGroups.button.onClick.AddListener(delegate {
			CleanAllGroups();
		});
		
		formationMode.button.onClick.AddListener(delegate {
			formationMode.isPressedIn = !(formationMode.isPressedIn);
			if(formationMode.isPressedIn){
				formationMode.SwitchStyle(2);
			}
			else{
				formationMode.SwitchStyle(3);
			}
		});
	}
	
	
	
	public bool GetFormationMode(){
		return formationMode.isPressedIn;
	}
	
	
	public void SetActivity(bool setter){	
		mainCanvas.SetActive(setter);
	}
	
	
	
	
}
