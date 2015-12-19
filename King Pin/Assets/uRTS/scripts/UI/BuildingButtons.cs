using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingButtons : MonoBehaviour {
	
	[HideInInspector] public RTSMaster rtsm;
	
	[HideInInspector] public GameObject mainCanvas;
	[HideInInspector] public GameObject buildingCanvas;
	public List<ButtonsGrid> buildingButtonGrid = new List<ButtonsGrid>();
	public List<ButtonsGrid> buildingCreationGrid = new List<ButtonsGrid>();
	
	public List<string> castleRestore = new List<string>();
	public List<string> castleCreationIcons = new List<string>();
	public List<string> barracksCreationIcons = new List<string>();
	public List<string> factoryCreationIcons = new List<string>();
	
	public ButtonObject butCounter = null;
	public ScrollCountController scc_butCounter = null;
	
	
	public FormationSizeController fsc;
	public Slider fsc_slider;
	
	public GameObject fsc_sliderGo;
	public GameObject fsc_sliderGoTx;
	
	
	
	public int numberScrollMode = 0;
	public int scrollButtonId = -1;
	
	
	
	
	
	
	
	public void Build () {
		
		mainCanvas = rtsm.mainCanvas;	
		CreationIcons();	
		BuildMenu();		
		BuildSubMenu();
		
	}
	
	
	void BuildMenu(){
	    
	    ButtonObject bo = new ButtonObject();
	    bo.buttonCanvas = mainCanvas;
	    bo.SetEmpty();
	    buildingCanvas = bo.buttonGo;
	    buildingCanvas.name = "buidings canvas";
	    
 	    butCounter = new ButtonObject();
 	    butCounter.buttonCanvas = buildingCanvas;
 	    butCounter.SetButton();
 	    butCounter.MoveLayer(
 	    	1f,0.5f,
 	    	1f,0.5f,
 	    	-164f,20f,
 	    	0f,40f
 	    );
 	    butCounter.MedievalTransparentStyle();
 	    butCounter.tx_button.text = "0";
 	    butCounter.HideButton();
 	    
 	    scc_butCounter = butCounter.buttonGo.AddComponent<ScrollCountController>();
 	    
 	    
	    BuildFormationScroll();
	    
	    
	    for(int i=0; i<10; i++){
	    
			buildingButtonGrid.Add(new ButtonsGrid());
			buildingButtonGrid[i].canvas = buildingCanvas;
			buildingButtonGrid[i].gridGoName = "grid "+i.ToString();
			
			buildingButtonGrid[i].isChangeableWidth = true;
			buildingButtonGrid[i].isChangeableHeight = true;
			buildingButtonGrid[i].changeWidth = 0.07f;
			buildingButtonGrid[i].changeHeight = 0.1f;

		
			buildingButtonGrid[i].SetGrid();
			rtsm.screenSizeChangeActions.dynamicGrids.Add(buildingButtonGrid[i].rszGrid);
			
			rtsm.screenSizeChangeActions.AddPanelEditor(
				buildingButtonGrid[i].gridGo.GetComponent<RectTransform>(),
				0.85f,0f,
				1f,0.5f,
				0
			);
		
// 			buildingButtonGrid[i].MoveLayer(
// 				1f,0f,
// 				1f,0.5f,
// 			
// 				-164f,0f,
// 				0f,0f
// 			);
			buildingButtonGrid[i].MoveLayer(
				0f,0f,
				1f,1f,
			
				-164f,0f,
				0f,0f
			);
		
			buildingButtonGrid[i].gr_scrollContain.cellSize = new Vector2(80f, 80f);
			buildingButtonGrid[i].gr_scrollContain.spacing = new Vector2(3f, 3f);
			
			int jmax = 4;
			if((i==0)||(i==1)||(i==6)||(i==9)){
				bo = new ButtonObject();
				bo.buttonCanvas = mainCanvas;
				bo.SetButton();
				bo.MedievalStyle();
				buildingButtonGrid[i].DeActivateButton(bo);
				bo.tx_button.text = "";
				bo.tx_button.fontSize = 18;
			//	bo.tx_button.fontSize = (int) (Screen.height*0.04f);
				bo.imageLocation.Add("UI/icons/eye_ico");
				bo.SetButtonImage();
			}
			else{
				jmax = 5;
			}
		
			for(int j=0; j<jmax; j++){
				bo = new ButtonObject();
				bo.buttonCanvas = mainCanvas;
				bo.col_button = Color.clear;
				bo.SetButton();
				buildingButtonGrid[i].DeActivateButton(bo);
				bo.tx_button.text = "";
			}

		
	//		if(i!=9){
			bo = new ButtonObject();
			bo.buttonCanvas = mainCanvas;
			bo.col_button = Color.red;
			bo.SetButton();
			bo.MedievalStyle();
			buildingButtonGrid[i].DeActivateButton(bo);
			bo.tx_button.text = "";
			bo.tx_button.fontSize = 18;
			bo.imageLocation.Add("UI/icons/cancel_ico");
			bo.SetButtonImage();
	//		}
		}


	}
	
	
	
	
	void BuildFormationScroll(){
		GameObject go = (GameObject)Instantiate(Resources.Load<GameObject>("UI/Slider"));
		go.name = "FormationScroll";
		
		fsc_sliderGo = go;
		
//		go_gameSpeed.SetActive(false);
		go.transform.SetParent(mainCanvas.transform);
		rtsm.screenSizeChangeActions.AddPanelEditor(
  			go.GetComponent<RectTransform>(),
  			0.85f,0.58f,
			1f,0.62f,
			0
  		);
  		
  		Transform[] ts = go.GetComponentsInChildren<Transform>();
  		foreach (Transform t in ts){
  			if(t.gameObject.name == "Slider"){
  				rtsm.screenSizeChangeActions.AddPanelEditor(
					t.GetComponent<RectTransform>(),
					0f,0f,
					1f,1f,
					0
				);
				GameObject sliderGo = t.gameObject;
				fsc = sliderGo.AddComponent<FormationSizeController>();
				
				fsc_slider = sliderGo.GetComponent<Slider>();
				fsc_slider.wholeNumbers = true;
				fsc_slider.minValue = 1;
				fsc_slider.maxValue = 20;
				fsc_slider.value = 1;
				
				fsc.slider = fsc_slider;
				fsc.rtsm = rtsm;
				
  			}
  			if(t.gameObject.name == "Text"){
  				Text tx = t.GetComponent<Text>();
  				if(tx.text == "Game Speed"){
  					DestroyImmediate(tx.gameObject);
  				}
  			}
  		}
		
		ButtonObject tx_go = new ButtonObject();
  		tx_go.rtsm = rtsm;
  		tx_go.buttonCanvas = mainCanvas;
        tx_go.isChangeableText = true;
		tx_go.textPixelRatio = 2f/3f;
		tx_go.textChangeFactor = 0.05f;
  		tx_go.SetButton();
  		tx_go.MedievalTransparentStyle();
  		tx_go.tx_button.text = "Formation (1)";
  		
  		fsc.tx_slider = tx_go.tx_button;
  		fsc_sliderGoTx = tx_go.buttonGo;
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			tx_go.rectTransform,
  			0.85f,0.62f,
			1f,0.669f,
			0
  		);
		
		fsc_sliderGo.SetActive(false);
		fsc_sliderGoTx.SetActive(false);
//		go.SetActive(false);
//		tx_go.buttonGo.SetActive(false);
	}
	
	void BuildSubMenu(){
	    
	    
	    for(int i=0; i<10; i++){
			buildingCreationGrid.Add(new ButtonsGrid());
			buildingCreationGrid[i].canvas = buildingCanvas;
			buildingCreationGrid[i].gridGoName = "creationButtons "+i.ToString();
			
			buildingCreationGrid[i].isChangeableWidth = true;
			buildingCreationGrid[i].isChangeableHeight = true;
			buildingCreationGrid[i].changeWidth = 0.05f;
			buildingCreationGrid[i].changeHeight = 0.1f;
			
			
			buildingCreationGrid[i].SetGrid();
			rtsm.screenSizeChangeActions.dynamicGrids.Add(buildingCreationGrid[i].rszGrid);
			
			rtsm.screenSizeChangeActions.AddPanelEditor(
				buildingCreationGrid[i].gridGo.GetComponent<RectTransform>(),
				0.8f,0.2f,
				0.85f,0.95f,
				0
			);
		    
		    buildingCreationGrid[i].MoveLayer(
				0f,0f,
				1f,1f,
			
				-164f,0f,
				0f,0f
			);

		    
// 			buildingCreationGrid[i].MoveLayer(
// 				1f,0f,
// 				1f,1f,
// 			
// 				-230f,0f,
// 				-161f,0f
// 			);
			
			
			
			buildingCreationGrid[i].gr_scrollContain.cellSize = new Vector2(64f, 64f);
			buildingCreationGrid[i].gr_scrollContain.spacing = new Vector2(1f, 1f);
			buildingCreationGrid[i].gr_scrollContain.childAlignment = TextAnchor.MiddleCenter;
			
			

			
		
		}
		ButtonObject bo = null;
		
		
		for(int i=0; i<1; i++){
			bo = new ButtonObject();
			bo.buttonCanvas = mainCanvas;
			bo.SetButton();
			bo.MedievalStyle();
			buildingCreationGrid[9].DeActivateButton(bo);
			bo.tx_button.text = "";
			bo.tx_button.fontSize = 18;
			bo.imageLocation.Add(castleRestore[i]);
			bo.SetButtonImage();
			PointerEventsController pec = bo.buttonGo.AddComponent<PointerEventsController>();
			pec.spawnGoUP = rtsm.rtsUnitTypePrefabs[i].GetComponent<UnitPars>();
			bo.rtsId = i;
		}
		
		
		
		for(int i=0; i<9; i++){
			bo = new ButtonObject();
			bo.buttonCanvas = mainCanvas;
			bo.SetButton();
			bo.MedievalStyle();
			buildingCreationGrid[0].DeActivateButton(bo);
			bo.tx_button.text = "";
			bo.tx_button.fontSize = 18;
			bo.imageLocation.Add(castleCreationIcons[i]);
			bo.SetButtonImage();
			PointerEventsController pec = bo.buttonGo.AddComponent<PointerEventsController>();
			if(i<8){
				pec.spawnGoUP = rtsm.rtsUnitTypePrefabs[i+1].GetComponent<UnitPars>();
				bo.rtsId = i+1;
	//			print(bo.rtsId);
			}
			else{
				pec.spawnGoUP = rtsm.rtsUnitTypePrefabs[15].GetComponent<UnitPars>();
				bo.rtsId = 15;
			}
		}
		for(int i=0; i<4; i++){
			bo = new ButtonObject();
			bo.buttonCanvas = mainCanvas;
			bo.SetButton();
			bo.MedievalStyle();
			buildingCreationGrid[1].DeActivateButton(bo);
			bo.tx_button.text = "";
			bo.tx_button.fontSize = 18;
			bo.imageLocation.Add(barracksCreationIcons[i]);
			bo.SetButtonImage();
			PointerEventsController pec = bo.buttonGo.AddComponent<PointerEventsController>();
			pec.spawnGoUP = rtsm.rtsUnitTypePrefabs[i+11].GetComponent<UnitPars>();
			bo.rtsId = i+11;
		}
		for(int i=0; i<2; i++){
			bo = new ButtonObject();
			bo.buttonCanvas = mainCanvas;
			bo.SetButton();
			bo.MedievalStyle();
			buildingCreationGrid[6].DeActivateButton(bo);
			bo.tx_button.text = "";
			bo.tx_button.fontSize = 18;
			bo.imageLocation.Add(factoryCreationIcons[i]);
			bo.SetButtonImage();
			PointerEventsController pec = bo.buttonGo.AddComponent<PointerEventsController>();
			pec.spawnGoUP = rtsm.rtsUnitTypePrefabs[i+9].GetComponent<UnitPars>();
			bo.rtsId = i+9;
		}
		
		buildingCreationGrid[0].buttonsEnableMask[5] = 0;
		buildingCreationGrid[0].buttonsEnableMask[6] = 0;
		buildingCreationGrid[0].buttonsEnableMask[7] = 0;
		
		buildingCreationGrid[1].buttonsEnableMask[2] = 0;
		buildingCreationGrid[1].buttonsEnableMask[3] = 0;
		
		
	}
	
	
	void CreationIcons(){
		string prefix = "textures/icons/";
		
		castleRestore.Add(prefix+"castle_ico");
		
		castleCreationIcons.Add(prefix+"barracks_ico");
		castleCreationIcons.Add(prefix+"sawmill_ico");
		castleCreationIcons.Add(prefix+"farm_ico");
		castleCreationIcons.Add(prefix+"laboratory_ico");
		castleCreationIcons.Add(prefix+"mine_ico");
		castleCreationIcons.Add(prefix+"factory_ico");
		castleCreationIcons.Add(prefix+"stables_ico");
		castleCreationIcons.Add(prefix+"windmill_ico");
		castleCreationIcons.Add(prefix+"worker_ico");
		
		barracksCreationIcons.Add(prefix+"archer_ico");
		barracksCreationIcons.Add(prefix+"swordsman_ico");
		barracksCreationIcons.Add(prefix+"arsonist_ico");
		barracksCreationIcons.Add(prefix+"knight_ico");
		
		factoryCreationIcons.Add(prefix+"fence_ico");
		factoryCreationIcons.Add(prefix+"tower_ico");
		
	}
	
	
	
	void Start () {
		AddFunctionality();
	}
	
	
	
	
	void AddFunctionality(){
		for(int i=0; i<10; i++){
			buildingButtonGrid[i].buttonsPool[5].button.onClick.AddListener(delegate {
				rtsm.DestroyUnit(rtsm.selectionMark.selectedGoPars[0]);
				LeaveNumberScrollMode();
				rtsm.mineLabel.DeActivate();
			});
		}
		buildingButtonGrid[9].buttonsPool[0].button.onClick.AddListener(delegate {
		    buildingCreationGrid[9].FlipAll();
		    LeaveNumberScrollMode();
		});
		
		buildingButtonGrid[0].buttonsPool[0].button.onClick.AddListener(delegate {
		    buildingCreationGrid[0].FlipAll();
		    LeaveNumberScrollMode();
		});
		buildingButtonGrid[1].buttonsPool[0].button.onClick.AddListener(delegate {
		    buildingCreationGrid[1].FlipAll();
		    LeaveNumberScrollMode();
		});
		buildingButtonGrid[6].buttonsPool[0].button.onClick.AddListener(delegate {
		    buildingCreationGrid[6].FlipAll();
		    LeaveNumberScrollMode();
		});
		
		
		for(int i=0; i<1; i++){
			SetBuildButtonClick(9, i);
		}
		
		for(int i=0; i<8; i++){
		    SetBuildButtonClick(0, i);
		}
		SetUnitButtonClick(0, 8, 0);
		
		for(int i=0; i<4; i++){
			SetUnitButtonClick(1, i, 1);
		}
		
		for(int i=0; i<2; i++){
			SetBuildButtonClick(6, i);
		}
		
		fsc_slider.onValueChanged.AddListener(delegate{
			fsc.LoadSliderToSpawnPoint();
		});
		
	}
	
	void SetBuildButtonClick(int gridId, int butId){
		ButtonObject but = buildingCreationGrid[gridId].buttonsPool[butId];
		ButtonsGrid grd = buildingCreationGrid[gridId];
		but.button.onClick.AddListener(delegate {
			if(numberScrollMode == 0){
				int id = but.rtsId;
				rtsm.buildMark.objectToSpawn = rtsm.rtsUnitTypePrefabs[id];
				rtsm.buildMark.up_objectToSpawn = rtsm.rtsUnitTypePrefabs[id].GetComponent<UnitPars>();
				rtsm.buildMark.enabled = true;
				rtsm.buildMark.ActivateProjector();
				grd.DeactivateAll();
			}
		});
	}
	
	void SetUnitButtonClick(int gridId, int butId, int bCounterId){
		ButtonObject but = buildingCreationGrid[gridId].buttonsPool[butId];
		ButtonsGrid grd = buildingCreationGrid[gridId];
		
//		GameObject bCounter = rtsm.buildDiplomacyMenu.list_bCounter[bCounterId];
//		ScrollCountController scc_bCounter = bCounter.GetComponent<ScrollCountController>();
		
		int id = but.rtsId;
		GameObject prefab = rtsm.rtsUnitTypePrefabs[id];
		
		but.button.onClick.AddListener(delegate {
		    if(numberScrollMode == 0){
		    	EnterNumberScrollMode(id);
		    	butCounter.buttonGo.SetActive(true);
		    }
		    else if(numberScrollMode == 1){
		    	if(id == scrollButtonId){
					grd.DeactivateAll();
					butCounter.buttonGo.SetActive(false);
					buildingButtonGrid[gridId].DeactivateAll();
					scc_butCounter.model = prefab;
					scc_butCounter.StartSpawnng();
					LeaveNumberScrollMode();
				}
		    }
		});

	}
	
	void EnterNumberScrollMode(int id){
		numberScrollMode = 1;
		scrollButtonId = id;
		rtsm.rtsCamera.enabled = false;
		scc_butCounter.counter = 1;
		butCounter.tx_button.text = scc_butCounter.counter.ToString();
		butCounter.tx_button.resizeTextForBestFit = true;
//		print(scc_butCounter.counter);
		butCounter.ShowButton();
		fsc_sliderGo.SetActive(true);
		fsc_sliderGoTx.SetActive(true);
		fsc.GetSelectedSpawnPoint();
		fsc.LoadSpawnPointToSlider();
	}
	void LeaveNumberScrollMode(){
		numberScrollMode = 0;
		scrollButtonId = -1;
		if(rtsm.rpgCamera.enabled == false){
			rtsm.rtsCamera.enabled = true;
		}
		butCounter.HideButton();
		fsc_sliderGo.SetActive(false);
		fsc_sliderGoTx.SetActive(false);
	//	scc_butCounter.counter = 1;
	}
	
	
	public void CloseBuildMenus(){
		if(numberScrollMode != 0){
			LeaveNumberScrollMode();
		}
		for(int i=0;i<buildingCreationGrid.Count;i++){
			buildingCreationGrid[i].DeactivateAll();
		}
		
	}
	
	
	
}
