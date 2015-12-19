using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class MovementButtons : MonoBehaviour {

	[HideInInspector] public RTSMaster rtsm;
	
	[HideInInspector] public GameObject mainCanvas;
	public ButtonsGrid movementButtonGrid;
	
	



	public void Build () {
		
		mainCanvas = rtsm.mainCanvas;		
		BuildMenu();		
		
	}


	void BuildMenu(){
		movementButtonGrid = new ButtonsGrid();
		movementButtonGrid.canvas = mainCanvas;
		
		movementButtonGrid.gridGoName = "movementButtonGrid";
		
		movementButtonGrid.isChangeableWidth = true;
		movementButtonGrid.isChangeableHeight = true;
		movementButtonGrid.changeWidth = 0.07f;
		movementButtonGrid.changeHeight = 0.1f;
		
		
		movementButtonGrid.SetGrid();
	//	movementButtonGrid.rszGrid.lockRight = true;
		rtsm.screenSizeChangeActions.dynamicGrids.Add(movementButtonGrid.rszGrid);
		
		rtsm.screenSizeChangeActions.AddPanelEditor(
  			movementButtonGrid.gridGo.GetComponent<RectTransform>(),
  			0.85f,0f,
			1f,0.5f,
			0
  		);
		
// 		movementButtonGrid.MoveLayer(
// 			1f,0f,
// 			1f,0.5f,
// 			
// 			-164f,0f,
// 			0f,0f
// 		);
		movementButtonGrid.MoveLayer(
			0f,0f,
			1f,1f,
			
			-164f,0f,
			0f,0f
		);
		movementButtonGrid.gr_scrollContain.cellSize = new Vector2(80f, 80f);
		movementButtonGrid.gr_scrollContain.spacing = new Vector2(3f, 3f);

		
  		ButtonObject bo = new ButtonObject();
  		bo.buttonCanvas = mainCanvas;
  		bo.SetButton();
  		bo.MedievalStyle();
  		movementButtonGrid.DeActivateButton(bo);
  		bo.tx_button.text = "";
  		bo.tx_button.fontSize = 18;
		bo.imageLocation.Add("UI/icons/stop");
		bo.SetButtonImage();
  		

  		bo = new ButtonObject();
  		bo.buttonCanvas = mainCanvas;
  		bo.SetButton();
  		bo.MedievalStyle();
  		movementButtonGrid.DeActivateButton(bo);
  		bo.tx_button.text = "";
  		bo.tx_button.fontSize = 18;
		bo.imageLocation.Add("UI/icons/walk");
		bo.SetButtonImage();


  		bo = new ButtonObject();
  		bo.buttonCanvas = mainCanvas;
  		bo.SetButton();
  		bo.MedievalStyle();
  		movementButtonGrid.DeActivateButton(bo);
  		bo.tx_button.text = "";
  		bo.tx_button.fontSize = 18;
		bo.imageLocation.Add("UI/icons/attack");
		bo.SetButtonImage();


  		bo = new ButtonObject();
  		bo.buttonCanvas = mainCanvas;
  		bo.SetButton();
  		bo.MedievalStyle();
  		movementButtonGrid.DeActivateButton(bo);
  		bo.tx_button.text = "";
  		bo.tx_button.fontSize = 18;
		bo.imageLocation.Add("UI/icons/grouping");
		bo.SetButtonImage();
  		
  		bo = new ButtonObject();
  		bo.buttonCanvas = mainCanvas;
  		bo.SetButton();
  		bo.MedievalStyle();
  		movementButtonGrid.DeActivateButton(bo);
  		bo.tx_button.text = "RPG";
  		bo.tx_button.fontSize = 18;
	//	bo.imageLocation.Add("UI/icons/grouping");
	//	bo.SetButtonImage();
  		
  		
  		
	}	
	


	// Use this for initialization
	void Start () {
		AddFunctionality();
	}
	
	
	
	void AddFunctionality(){
		movementButtonGrid.buttonsPool[0].button.onClick.AddListener(delegate {
				rtsm.cameraOperator.StopDestinationsF();
		});
		movementButtonGrid.buttonsPool[1].button.onClick.AddListener(delegate {
				rtsm.cameraOperator.movementButtonMode = 1;
		});
		movementButtonGrid.buttonsPool[2].button.onClick.AddListener(delegate {
				rtsm.cameraOperator.movementButtonMode = 2;
		});
		movementButtonGrid.buttonsPool[3].button.onClick.AddListener(delegate {
			rtsm.unitsGrouping.GroupSelected();
//			rtsm.buildDiplomacyMenu.CreateNewGroup();
			rtsm.groupingMenu.CreateNewGroup();
		});
		movementButtonGrid.buttonsPool[4].button.onClick.AddListener(delegate {
			rtsm.cameraSwitcher.FlipSwitcher(rtsm.selectionMark.selectedGoPars[0]);
		});
	}
	
	
}
