using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DiplomacyMenu : MonoBehaviour {
	
	
	[HideInInspector] public RTSMaster rtsm;
	
	[HideInInspector] public GameObject mainCanvas;
	
	public ButtonsGrid playerDialogConsole;
	public DiplomacyButtonsGrid playerDialogConsoleTheir;
	public ButtonsGrid playerAnswersConsole;
	
//	public ButtonsGrid nationsMenuGrid;
	
	public ButtonPool nationsMenuGrid_im;
	public ButtonPool nationsMenuGrid_tx;
	public ButtonPool nationsMenuGrid_rel;
	
	public DiplomacyButtonsGrid playerOffersMenuGrid;
	
	[HideInInspector] public int nationDialogOpen = -1;
	
    [HideInInspector] public int numberNations = 0;
    [HideInInspector] public int playerNation = 0;
    
    [HideInInspector] public string war_Option_String = ", prepare to die";
    [HideInInspector] public string peace_Option_String = "Lets live in peace, ";
    [HideInInspector] public string slavery_Option_String = "We have Mercy, ";    
    [HideInInspector] public string mastery_Option_String = "I accept your Mercy, but you will pay for it, "; 
    [HideInInspector] public string masteryDecline_Option_String = "You will die, ";     
    [HideInInspector] public string slaveryLeave_Option_String = "We paid enough, you will get nothing more from us, "; 
    [HideInInspector] public string alliance_Option_String = "Let's join alliance, "; 
    [HideInInspector] public string allianceLeave_Option_String = "We are breaking our alliance, "; 
    [HideInInspector] public string allianceAccept_Option_String = "Let it be, ";
    [HideInInspector] public string allianceDecline_Option_String = "I will think about your offer, "; 
    
    
    [HideInInspector] public Button but_diplomacyButton;
    [HideInInspector] public ButtonObject taxesInfo;
    
    [HideInInspector] public List<List<int>> theirActiveOffers = new List<List<int>>();
    
    private int fSize = 18;
	
	void Update(){
		if (Input.GetKey("q")){
			ActivateAllianceOffer(1);
		//	ActivateMercyOffer(1);
		}
		if (Input.GetKey("r")){
			rtsm.economy.LargeResources(rtsm.diplomacy.playerNation);
		}
	}
	
	public void Build () {
		
		mainCanvas = rtsm.mainCanvas;
		numberNations = rtsm.diplomacy.numberNations;
		playerNation = rtsm.diplomacy.playerNation;
		
//		but_diplomacyButton = rtsm.buildDiplomacyMenu.but_diplomacyButton;
		
		fSize = 20;
		//(int)(Screen.dpi/5f);
		
		BuildDiplomacyButton();
		BuildMenu();
		
		
	}
	
	
	
	void BuildDiplomacyButton(){
		ButtonObject diplomacyButton = new ButtonObject();
  		diplomacyButton.rtsm = rtsm;
  		diplomacyButton.buttonCanvas = mainCanvas;
 // 		bo.imageLocation.Add(null);
        diplomacyButton.isChangeableText = true;
		diplomacyButton.textPixelRatio = 2f/3f;
		diplomacyButton.textChangeFactor = 0.05f;
  		diplomacyButton.SetButton();
  		diplomacyButton.MedievalStyle();
  		diplomacyButton.tx_button.text = "Diplomacy";
//  		diplomacyButton.tx_button.resizeTextForBestFit = true;
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			diplomacyButton.rectTransform,
  			0.85f,0f,
			1f,0.05f,
			0
  		);
  		but_diplomacyButton = diplomacyButton.button;
  		
	}
	
	
	

	void BuildMenu(){

// our proposals
	
		playerDialogConsole = new ButtonsGrid();
		playerDialogConsole.canvas = mainCanvas;
		playerDialogConsole.rtsm = rtsm;
		
		playerDialogConsole.gridGoName = "playerDialogConsole";
		playerDialogConsole.isChangeableWidth = true;
		playerDialogConsole.isChangeableHeight = true;
		playerDialogConsole.changeWidth = 0.6f;
		playerDialogConsole.changeHeight = 0.1f;
		playerDialogConsole.isChangeableText = true;
		playerDialogConsole.textPixelRatio = 2f/3f;
		playerDialogConsole.textChangeFactor = 0.1f;
		
		playerDialogConsole.SetGrid();
		rtsm.screenSizeChangeActions.dynamicGrids.Add(playerDialogConsole.rszGrid);
		
		
  		ButtonObject bo = new ButtonObject();
  		bo.rtsm = rtsm;
  		bo.buttonCanvas = mainCanvas;
 // 		bo.imageLocation.Add(null);
        bo.isChangeableText = true;
		bo.textPixelRatio = 2f/3f;
		bo.textChangeFactor = 0.1f;
  		bo.SetButton();
  		bo.MedievalTransparentStyle();
  		playerDialogConsole.DeActivateButton(bo);
  		bo.tx_button.text = "Our proposals";
  //		bo.tx_button.fontSize = fSize;
  		
  		for(int i=0; i<9; i++){
			bo = new ButtonObject();
			bo.rtsm = rtsm;
			bo.buttonCanvas = mainCanvas;
			bo.isChangeableText = true;
			bo.textPixelRatio = 2f/3f;
			bo.textChangeFactor = 0.1f;
			bo.SetButton();
			bo.MedievalTransparentStyle();	
			playerDialogConsole.DeActivateButton(bo);
//			bo.tx_button.fontSize = fSize;
  		}

// their proposals
		playerDialogConsoleTheir = new DiplomacyButtonsGrid();
		playerDialogConsoleTheir.rtsm = rtsm;
		playerDialogConsoleTheir.canvas = mainCanvas;
		
		playerDialogConsoleTheir.gridGoName = "playerDialogConsoleTheir";
		
		playerDialogConsoleTheir.isChangeableWidth = true;
		playerDialogConsoleTheir.isChangeableHeight = true;
		playerDialogConsoleTheir.changeWidth = 0.6f;	
		playerDialogConsoleTheir.changeHeight = 0.1f;	
		playerDialogConsoleTheir.SetGrid();
		rtsm.screenSizeChangeActions.dynamicGrids.Add(playerDialogConsoleTheir.rszGrid);
		
		
  		bo = new ButtonObject();
  		bo.rtsm = rtsm;
  		bo.buttonCanvas = mainCanvas;
  		bo.isChangeableText = true;
		bo.textPixelRatio = 2f/3f;
		bo.textChangeFactor = 0.1f;
  		bo.SetButton();
  		bo.MedievalTransparentStyle();
  		playerDialogConsoleTheir.DeActivateButton(bo);
  		bo.tx_button.text = "Their proposals";
  		bo.tx_button.fontSize = fSize;
  		
        playerDialogConsoleTheir.AddButton_war();
		playerDialogConsoleTheir.AddButton_mercy();
		playerDialogConsoleTheir.AddButton_mercyAccept();
		playerDialogConsoleTheir.AddButton_mercyDecline();
		playerDialogConsoleTheir.AddButton_escapeSlavery();
		playerDialogConsoleTheir.AddButton_alliance();
		playerDialogConsoleTheir.AddButton_allianceAccept();
		playerDialogConsoleTheir.AddButton_allianceDecline();
		playerDialogConsoleTheir.AddButton_allianceLeave();
		
		
// answers console
        playerAnswersConsole = new DiplomacyButtonsGrid();
		playerAnswersConsole.canvas = mainCanvas;
		
		playerAnswersConsole.gridGoName = "playerAnswersConsole";
		
		playerAnswersConsole.isChangeableWidth = true;
		playerAnswersConsole.isChangeableHeight = true;
		playerAnswersConsole.changeWidth = 0.6f;	
		playerAnswersConsole.changeHeight = 0.1f;	
		playerAnswersConsole.SetGrid();
		rtsm.screenSizeChangeActions.dynamicGrids.Add(playerAnswersConsole.rszGrid);
		
		
  		bo = new ButtonObject();
  		bo.rtsm = rtsm;
  		bo.buttonCanvas = mainCanvas;
  		bo.isChangeableText = true;
		bo.textPixelRatio = 2f/3f;
		bo.textChangeFactor = 0.1f;

  		bo.SetButton();
  		bo.MedievalTransparentStyle();
  		playerAnswersConsole.DeActivateButton(bo);
  		bo.tx_button.text = "Answer";
  		bo.tx_button.fontSize = fSize;
		
  		for(int i=0; i<9; i++){
			bo = new ButtonObject();
			bo.rtsm = rtsm;
			bo.buttonCanvas = mainCanvas;
			bo.isChangeableText = true;
			bo.textPixelRatio = 2f/3f;
			bo.textChangeFactor = 0.1f;

			bo.SetButton();
			bo.MedievalTransparentStyle();	
			playerAnswersConsole.DeActivateButton(bo);
			bo.tx_button.fontSize = fSize;
  		}
		
		
		
		
		

// offers console		
		playerOffersMenuGrid = new DiplomacyButtonsGrid();
		playerOffersMenuGrid.rtsm = rtsm;
		playerOffersMenuGrid.canvas = mainCanvas;
		
		playerOffersMenuGrid.gridGoName = "playerOffersMenuGrid";
		playerOffersMenuGrid.isChangeableWidth = true;
		playerOffersMenuGrid.isChangeableHeight = true;
		playerOffersMenuGrid.changeWidth = 0.6f;	
		playerOffersMenuGrid.changeHeight = 0.1f;	
		playerOffersMenuGrid.SetGrid();
		rtsm.screenSizeChangeActions.dynamicGrids.Add(playerOffersMenuGrid.rszGrid);
		
		
		bo = new ButtonObject();
		bo.rtsm = rtsm;
		bo.buttonCanvas = mainCanvas;
		bo.isChangeableText = true;
		bo.textPixelRatio = 2f/3f;
		bo.textChangeFactor = 0.1f;

		bo.SetButton();
		bo.MedievalTransparentStyle();	
		playerOffersMenuGrid.DeActivateButton(bo);
		bo.tx_button.fontSize = fSize;
		taxesInfo = bo;
		
		for(int i=0; i<rtsm.diplomacy.numberNations; i++){
			playerOffersMenuGrid.AddButton_war();
			playerOffersMenuGrid.AddButton_mercy();
			playerOffersMenuGrid.AddButton_mercyAccept();
			playerOffersMenuGrid.AddButton_mercyDecline();
			playerOffersMenuGrid.AddButton_escapeSlavery();
			playerOffersMenuGrid.AddButton_alliance();
			playerOffersMenuGrid.AddButton_allianceAccept();
			playerOffersMenuGrid.AddButton_allianceDecline();
			playerOffersMenuGrid.AddButton_allianceLeave();
		}
		
		
		
  		
  		
// top diplomacy menu grid (for selecting nation)




		nationsMenuGrid_im = new ButtonPool();
		nationsMenuGrid_im.uim = rtsm.uiMaster;
		nationsMenuGrid_im.SetGridHor(
			4, 3, rtsm.diplomacy.numberNations-1,
			0.1f, 0.1f,
			0.85f, 0.9f,
			0.05f, 0.05f,
			"",
			2
		);
		
		int j = 0;
		for(int i=0; i<rtsm.diplomacy.numberNations; i++){
			if(i != rtsm.diplomacy.playerNation){
				bo = nationsMenuGrid_im.poolBO[j];
				bo.imageLocation.Add("textures/faces/"+(i).ToString());
				bo.SetButtonImage();
				j++;
			}
		}

		nationsMenuGrid_tx = new ButtonPool();
		nationsMenuGrid_tx.uim = rtsm.uiMaster;
		nationsMenuGrid_tx.SetGridHor(
			4, 3, rtsm.diplomacy.numberNations-1,
			0.1f, 0.19f,
			0.85f, 0.99f,
			0.05f, 0.05f,
			"a",
			2
		);
		
		j=0;
		for(int i=0; i<rtsm.diplomacy.numberNations; i++){
			if(i != rtsm.diplomacy.playerNation){
				bo = nationsMenuGrid_tx.poolBO[j];
				bo.tx_button.text = rtsm.nationAIs[i].nationName;
				bo.tx_button.alignment = TextAnchor.MiddleCenter;
				j++;
			}
		}
		
		nationsMenuGrid_rel = new ButtonPool();
		nationsMenuGrid_rel.uim = rtsm.uiMaster;
		nationsMenuGrid_rel.SetGridHor(
			4, 3, rtsm.diplomacy.numberNations-1,
			0.1f, 0.02f,
			0.85f, 0.82f,
			0.05f, 0.05f,
			"",
			3
		);
		
		j=0;
		for(int i=0; i<rtsm.diplomacy.numberNations; i++){
		    if(i != rtsm.diplomacy.playerNation){
				bo = nationsMenuGrid_rel.poolBO[j];
				bo.imageLocation.Add("UI/icons/peace");
				bo.SetButtonImage();
				j++;
			}
		}

		
		nationsMenuGrid_im.DeActivateAll();
		nationsMenuGrid_tx.DeActivateAll();
		nationsMenuGrid_rel.DeActivateAll();


  			
// 		nationsMenuGrid = new ButtonsGrid();
// 		nationsMenuGrid.canvas = mainCanvas;
// 		nationsMenuGrid.aMin = new Vector3(0f, 0f);
// 		nationsMenuGrid.aMax = new Vector3(1f, 1f);
// 		nationsMenuGrid.aPos = new Vector2(0f, 0f);
// 		nationsMenuGrid.sDelt = new Vector2(0f, 0f);
// 		
// 		nationsMenuGrid.gridGoName = "nationsMenuGrid";
// 		
// 		nationsMenuGrid.SetGrid();
// 		
// 		nationsMenuGrid.GridLimits(4);
// 		
// 		nationsMenuGrid.gr_scrollContain.cellSize = new Vector2(160f, 120f);
// 		nationsMenuGrid.gr_scrollContain.spacing = new Vector2(60f, 30f);
// 		nationsMenuGrid.gr_scrollContain.childAlignment = TextAnchor.MiddleCenter;
// 		
// 		for(int i=0;i<(rtsm.diplomacy.numberNations);i++){
// 			bo = new ButtonObject();
// 			bo.buttonCanvas = mainCanvas;
// 			bo.sDelt = new Vector3(60f, 60f);
// 			bo.imageLocation.Add("textures/faces/"+(i).ToString());
// 			bo.SetButton();
// 			bo.MedievalTransparentStyle();	
// 			bo.im_button[bo.im_button.Count-1].color = new Color(1f,1f,1f,1f);
// 			nationsMenuGrid.DeActivateButton(bo);
// 			
// 			bo.tx_button.text = rtsm.nationAIs[i].nationName;
// 			bo.tx_button.fontSize = 18;
// 			
// 			bo.im_button[bo.im_button.Count-1].gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-50f,-30f);
// 			bo.im_button[bo.im_button.Count-1].gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(50f,30f);
// 			
// 			bo.tx_button.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f,-90f);
// 			
// 			
// 			
// 			bo.imageLocation.Add("UI/icons/peace");
// 			bo.SetButtonImage();
// 			bo.im_button[bo.im_button.Count-1].color = new Color(1f,1f,1f,1f);
// 			bo.im_button[bo.im_button.Count-1].gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-60f,0f);
// 			bo.im_button[bo.im_button.Count-1].gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(60f,80f);
// 			
// 			
// 			
// 		}
		

	}

	
	
	
	public IEnumerator AddTheirProposal(int nat, int propId){
		if(! theirActiveOffers[nat].Contains(propId)){
		    theirActiveOffers[nat].Add(propId);
			yield return new WaitForSeconds(30.0f);
			theirActiveOffers[nat].Remove(propId);
		}
	}
	
	
	public void MakeReport(string reportText){
		int i = 0;
		foreach (Transform child in playerDialogConsole.scrollContain.transform){
			if(child != null){
				i++;
			}
		}
	//	foreach (Transform child in nationsMenuGrid.scrollContain.transform){
// 		foreach (Transform child in nationsMenuGrid_im.poolCanvas.transform){
// 			if(child != null){
// 				i++;
// 			}
// 		}
		if(nationsMenuGrid_im.IsActiveAny() == true){
			i++;
		}

		if(i == 0){
			int j = 0;
			foreach (Transform child in playerOffersMenuGrid.scrollContain.transform){
				if(child != null){
					j++;
				}
			}
			if(j<5){
	//		    print("qqq");
			    StartCoroutine(playerOffersMenuGrid.ActivateButtonDelay(taxesInfo, 5f));
				taxesInfo.tx_button.text = reportText;
			}
			else{
				
			}
		}
	}
	
	
	public void ActivateWarOffer(int nat){
	    if(! theirActiveOffers[nat].Contains(1)){
//			nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_war(nat));	
			playerOffersMenuGrid.warSug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName + war_Option_String;
	//		StartCoroutine(AddTheirProposal(nat,1));
		}
	}
	
	public void ActivateMercyOffer(int nat){
		if(! theirActiveOffers[nat].Contains(2)){
//			nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_mercy(nat));	
			playerOffersMenuGrid.mercySug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + slavery_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			StartCoroutine(AddTheirProposal(nat,2));
		}
	}
	
	public void ActivateMercyAcceptOffer(int nat){
		if(! theirActiveOffers[nat].Contains(1)){
//			nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_mercyAccept(nat));	
			playerOffersMenuGrid.mercyAcceptSug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + mastery_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
	//		StartCoroutine(AddTheirProposal(nat,3));
		}
	}
	
	public void ActivateMercyDeclineOffer(int nat){
		if(! theirActiveOffers[nat].Contains(1)){
	//		nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_mercyDecline(nat));	
			playerOffersMenuGrid.mercyDeclineSug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + masteryDecline_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
	//		StartCoroutine(AddTheirProposal(nat,4));
		}
	}

	public void ActivateEscapeSlaveryOffer(int nat){
		if(! theirActiveOffers[nat].Contains(1)){
	//		nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_escapeSlavery(nat));	
			playerOffersMenuGrid.escapeSlaverySug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + slaveryLeave_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
	//		StartCoroutine(AddTheirProposal(nat,5));
		}
	}

	public void ActivateAllianceOffer(int nat){
		if(! theirActiveOffers[nat].Contains(6)){
//			nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_alliance(nat));	
			playerOffersMenuGrid.allianceSug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + alliance_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			StartCoroutine(AddTheirProposal(nat,6));
		}
	}
	
	
	public void ActivateAllianceAcceptOffer(int nat){
		if(! theirActiveOffers[nat].Contains(1)){
//			nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_allianceAccept(nat));	
			playerOffersMenuGrid.allianceAcceptSug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + allianceAccept_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
	//		StartCoroutine(AddTheirProposal(nat,7));
		}
	}
	
	public void ActivateAllianceDeclineOffer(int nat){
		if(! theirActiveOffers[nat].Contains(1)){
//			nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_allianceDecline(nat));	
			playerOffersMenuGrid.allianceDeclineSug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + allianceDecline_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
	//		StartCoroutine(AddTheirProposal(nat,8));
		}
	}
	
	public void ActivateAllianceLeaveOffer(int nat){
		if(! theirActiveOffers[nat].Contains(1)){
	//		nationsMenuGrid.DeactivateAll();
			
			nationsMenuGrid_im.DeActivateAll();
			nationsMenuGrid_tx.DeActivateAll();
			nationsMenuGrid_rel.DeActivateAll();
			
			playerDialogConsole.DeactivateAll();
			StartCoroutine(playerOffersMenuGrid.ActivateButton_allianceLeave(nat));	
			playerOffersMenuGrid.allianceLeaveSug[nat].tx_button.text = rtsm.nationAIs[nat].nationName + " : " + allianceLeave_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
	//		StartCoroutine(AddTheirProposal(nat,9));
		}
	}
	
	
	
	
	
	void AddFunctionality(){
		playerDialogConsole.buttonsPool[0].button.onClick.AddListener(delegate {
				playerDialogConsole.DeactivateAll();
				FillDiplomacyOffersTheir();
		});
	
	// war	
		playerDialogConsole.buttonsPool[1].button.onClick.AddListener(delegate {
				rtsm.diplomacyMenu.SetWar();
				playerDialogConsole.DeactivateAll();
		});
	// mercySug	
		playerDialogConsole.buttonsPool[2].button.onClick.AddListener(delegate {
				rtsm.diplomacyMenu.CheckSlaveryDecision();
				playerDialogConsole.DeactivateAll();
		});
	// escapeSlaverySug	
		playerDialogConsole.buttonsPool[5].button.onClick.AddListener(delegate {
				rtsm.diplomacyMenu.SetPeace();
				playerDialogConsole.DeactivateAll();
		});
	// allianceSug	
		playerDialogConsole.buttonsPool[6].button.onClick.AddListener(delegate {
				rtsm.diplomacyMenu.CheckAllianceDecision();
				playerDialogConsole.DeactivateAll();
		});
	// allianceLeaveSug	
		playerDialogConsole.buttonsPool[9].button.onClick.AddListener(delegate {
				rtsm.diplomacyMenu.SetPeace();
				playerDialogConsole.DeactivateAll();
		});





	// mercyAcceptSug	
		playerAnswersConsole.buttonsPool[3].button.onClick.AddListener(delegate {
				rtsm.diplomacyMenu.SetMastery();
				theirActiveOffers[nationDialogOpen].Remove(2);
				playerAnswersConsole.DeactivateAll();
		});
	// mercyDeclineSug	
		playerAnswersConsole.buttonsPool[4].button.onClick.AddListener(delegate {
		        theirActiveOffers[nationDialogOpen].Remove(2);
				playerAnswersConsole.DeactivateAll();
		});
	// allianceAcceptSug
		playerAnswersConsole.buttonsPool[7].button.onClick.AddListener(delegate {
				rtsm.diplomacyMenu.SetAllianceAnswer();
				theirActiveOffers[nationDialogOpen].Remove(6);
				playerAnswersConsole.DeactivateAll();
				
		});
	// allianceDeclineSug
		playerAnswersConsole.buttonsPool[8].button.onClick.AddListener(delegate {
				theirActiveOffers[nationDialogOpen].Remove(6);
				playerAnswersConsole.DeactivateAll();
		});








		
		
		playerDialogConsoleTheir.buttonsPool[0].button.onClick.AddListener(delegate {
				playerDialogConsoleTheir.DeactivateAll();
				FillDiplomacyOffers();
		});
		
		
		int j = 0;
		for(int i=0; i<(rtsm.diplomacy.numberNations); i++){
//		    int natId = i;
// 			nationsMenuGrid.buttonsPool[i].button.onClick.AddListener(delegate {
// 			    nationDialogOpen = natId;
// 			    FillDiplomacyOffers();
// 				nationsMenuGrid.DeactivateAll();
// 			});
			
			
			
			if(i != rtsm.diplomacy.playerNation){
			    int natId1 = i;
				nationsMenuGrid_im.poolBO[j].button.onClick.AddListener(delegate {
					nationDialogOpen = natId1;
					FillDiplomacyOffers();
					nationsMenuGrid_im.DeActivateAll();
					nationsMenuGrid_tx.DeActivateAll();
					nationsMenuGrid_rel.DeActivateAll();
				});
				nationsMenuGrid_tx.poolBO[j].button.onClick.AddListener(delegate {
					nationDialogOpen = natId1;
					FillDiplomacyOffers();
					nationsMenuGrid_im.DeActivateAll();
					nationsMenuGrid_tx.DeActivateAll();
					nationsMenuGrid_rel.DeActivateAll();
				});
				nationsMenuGrid_rel.poolBO[j].button.onClick.AddListener(delegate {
					nationDialogOpen = natId1;
					FillDiplomacyOffers();
					nationsMenuGrid_im.DeActivateAll();
					nationsMenuGrid_tx.DeActivateAll();
					nationsMenuGrid_rel.DeActivateAll();
				});
				
				
// 			nationsMenuGrid_tx.DeActivateAll();
// 			nationsMenuGrid_rel.DeActivateAll();
			
				j++;
			}
			
			
		}
		
		but_diplomacyButton.onClick.AddListener(delegate {
//			nationsMenuGrid.FlipAll();
			
			nationsMenuGrid_im.FlipAll();
			nationsMenuGrid_tx.FlipAll();
			nationsMenuGrid_rel.FlipAll();
			
// 			int pNation = rtsm.diplomacy.playerNation;
// 			ButtonObject bo = nationsMenuGrid.buttonsPool[pNation];
// 			nationsMenuGrid.DeActivateButton(bo);
			playerDialogConsole.DeactivateAll();
			playerDialogConsoleTheir.DeactivateAll();
			playerOffersMenuGrid.DeactivateAll();
		});
		
		taxesInfo.button.onClick.AddListener(delegate {
			playerOffersMenuGrid.DeActivateButton(taxesInfo);
		});
		
		for(int i=0; i<(rtsm.diplomacy.numberNations); i++){
			int natId = i;
			playerOffersMenuGrid.warSug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				FillDiplomacyOffers();
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.mercySug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				playerAnswersConsole.DeactivateAll();
				
				playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[0]);
				playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[3]);
				playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[4]);
				
				playerAnswersConsole.buttonsPool[3].tx_button.text = mastery_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
				playerAnswersConsole.buttonsPool[4].tx_button.text = masteryDecline_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
				
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.mercyAcceptSug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				FillDiplomacyOffers();
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.mercyDeclineSug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				FillDiplomacyOffers();
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.escapeSlaverySug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				FillDiplomacyOffers();
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.allianceSug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				playerAnswersConsole.DeactivateAll();
				
				playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[0]);
				playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[7]);
				playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[8]);
				
				playerAnswersConsole.buttonsPool[7].tx_button.text = allianceAccept_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
				playerAnswersConsole.buttonsPool[8].tx_button.text = allianceDecline_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
				
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.allianceAcceptSug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				FillDiplomacyOffers();
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.allianceDeclineSug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				FillDiplomacyOffers();
				playerOffersMenuGrid.DeactivateAll();
			});
			playerOffersMenuGrid.allianceLeaveSug[natId].button.onClick.AddListener(delegate {
				nationDialogOpen = natId;
				FillDiplomacyOffers();
				playerOffersMenuGrid.DeactivateAll();
			});
		}
		
		
		playerDialogConsoleTheir.mercySug[0].button.onClick.AddListener(delegate {
			playerDialogConsole.DeactivateAll();
			
			playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[0]);
			playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[3]);
			playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[4]);
			
			playerAnswersConsole.buttonsPool[3].tx_button.text = mastery_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
			playerAnswersConsole.buttonsPool[4].tx_button.text = masteryDecline_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
			
			playerDialogConsoleTheir.DeactivateAll();
		});
		playerDialogConsoleTheir.allianceSug[0].button.onClick.AddListener(delegate {
			playerAnswersConsole.DeactivateAll();
			
			playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[0]);
			playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[7]);
			playerAnswersConsole.ActivateButton(playerAnswersConsole.buttonsPool[8]);
			
			playerAnswersConsole.buttonsPool[7].tx_button.text = allianceAccept_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
			playerAnswersConsole.buttonsPool[8].tx_button.text = allianceDecline_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
			
			playerDialogConsoleTheir.DeactivateAll();
		});


	}
	
	
	public void FillDiplomacyOffers(){
		if(rtsm.diplomacy.relations[playerNation][nationDialogOpen] == 0){
		    playerDialogConsole.DeactivateAll();
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[0]);
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[1]);
			
			playerDialogConsole.buttonsPool[1].tx_button.text = rtsm.nationAIs[nationDialogOpen].nationName + war_Option_String;
			
			
		}
		if(rtsm.diplomacy.relations[playerNation][nationDialogOpen] == 1){
		    playerDialogConsole.DeactivateAll();
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[0]);
			
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[6]);
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[2]);
			
			playerDialogConsole.buttonsPool[6].tx_button.text = alliance_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
			playerDialogConsole.buttonsPool[2].tx_button.text = slavery_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
			
		}
		if(rtsm.diplomacy.relations[playerNation][nationDialogOpen] == 2){
			playerDialogConsole.DeactivateAll();
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[0]);
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[5]);
			
			playerDialogConsole.buttonsPool[5].tx_button.text = slaveryLeave_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
		}
		if(rtsm.diplomacy.relations[playerNation][nationDialogOpen] == 3){
			playerDialogConsole.DeactivateAll();
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[0]);
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[1]);
			
			playerDialogConsole.buttonsPool[1].tx_button.text = rtsm.nationAIs[nationDialogOpen].nationName + war_Option_String;
		}		
		if(rtsm.diplomacy.relations[playerNation][nationDialogOpen] == 4){	
			playerDialogConsole.DeactivateAll();
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[0]);
			
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[9]);
			playerDialogConsole.ActivateButton(playerDialogConsole.buttonsPool[1]);
			
			playerDialogConsole.buttonsPool[9].tx_button.text = allianceLeave_Option_String + rtsm.nationAIs[nationDialogOpen].nationName;
			playerDialogConsole.buttonsPool[1].tx_button.text = rtsm.nationAIs[nationDialogOpen].nationName + war_Option_String;
		}	
		
	}
	
	public void FillDiplomacyOffersTheir(){
		playerDialogConsoleTheir.DeactivateAll();
		playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.buttonsPool[0]);
		for(int i=0;i<theirActiveOffers[nationDialogOpen].Count;i++){
			int k = theirActiveOffers[nationDialogOpen][i];
			if(k==1){
		    	playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.warSug[0]);
				playerDialogConsoleTheir.warSug[0].tx_button.text = rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName + war_Option_String;	
			}
			else if(k==2){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.mercySug[0]);
				playerDialogConsoleTheir.mercySug[0].tx_button.text = slavery_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
			else if(k==3){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.mercyAcceptSug[0]);
				playerDialogConsoleTheir.mercyAcceptSug[0].tx_button.text = mastery_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
			else if(k==4){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.mercyDeclineSug[0]);
				playerDialogConsoleTheir.mercyDeclineSug[0].tx_button.text = masteryDecline_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
			else if(k==5){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.escapeSlaverySug[0]);
				playerDialogConsoleTheir.escapeSlaverySug[0].tx_button.text = slaveryLeave_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
			else if(k==6){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.allianceSug[0]);
				playerDialogConsoleTheir.allianceSug[0].tx_button.text = alliance_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
			else if(k==7){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.allianceAcceptSug[0]);
				playerDialogConsoleTheir.allianceAcceptSug[0].tx_button.text = allianceAccept_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
			else if(k==8){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.allianceDeclineSug[0]);
				playerDialogConsoleTheir.allianceDeclineSug[0].tx_button.text = allianceDecline_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
			else if(k==9){
				playerDialogConsoleTheir.ActivateButton(playerDialogConsoleTheir.allianceLeaveSug[0]);
				playerDialogConsoleTheir.allianceLeaveSug[0].tx_button.text = allianceLeave_Option_String + rtsm.nationAIs[rtsm.diplomacy.playerNation].nationName;
			}
		}
	}
    
    public void CheckSlaveryDecision(){
 //   	int pNation = rtsm.diplomacy.playerNation;
    	SetSlavery();
    }
    
    public void CheckAllianceDecision(){
        int pNation = rtsm.diplomacy.playerNation;
    	if((rtsm.nationAIs[nationDialogOpen].beatenUnits[pNation] > 20)&&
    	   (rtsm.scores.nUnits[nationDialogOpen] > 20)
    	){
    		SetAlliance();
    	}
    	else if(rtsm.scores.nUnits[nationDialogOpen] <= 20){
    		SetAlliance();
    	}
    	else{
    		ActivateAllianceDeclineOffer(nationDialogOpen);
    	}
    }
	
	
	public void SetPeace(){
		int pNation = rtsm.diplomacy.playerNation;		
	    rtsm.diplomacy.SetRelation(pNation,nationDialogOpen,0);
	    
	}
	
	public void SetWar(){
		int pNation = rtsm.diplomacy.playerNation;
	    rtsm.diplomacy.SetRelation(pNation,nationDialogOpen,1);
	}
	
	public void SetAlliance(){
		int pNation = rtsm.diplomacy.playerNation;
		ActivateAllianceAcceptOffer(nationDialogOpen);
		rtsm.diplomacy.SetRelation(pNation,nationDialogOpen,4);
	}
	
	public void SetAllianceAnswer(){
		int pNation = rtsm.diplomacy.playerNation;
		rtsm.diplomacy.SetRelation(pNation,nationDialogOpen,4);
	}
	
	public void SetSlavery(){
		int pNation = rtsm.diplomacy.playerNation;
	    ActivateMercyAcceptOffer(nationDialogOpen);
		rtsm.diplomacy.SetRelation(pNation,nationDialogOpen,2);
	}
	public void SetMastery(){
		int pNation = rtsm.diplomacy.playerNation;
		rtsm.diplomacy.SetRelation(pNation,nationDialogOpen,3);
	}
	
	
	
	
	public void ChangeRelationIcons(){
	    int pNation = rtsm.diplomacy.playerNation;
	    int j = 0;
		for(int i=0; i<rtsm.diplomacy.numberNations; i++){
			if(i != pNation){
			    int rel = rtsm.diplomacy.relations[pNation][i];
			    nationsMenuGrid_rel.poolBO[j].im_button[1].sprite = rtsm.uiMaster.relationIcons[rel];
				
// 				if(rtsm.diplomacy.relations[pNation][i] == 0){
// 					nationsMenuGrid.buttonsPool[i].im_button[1].sprite = Resources.Load<Sprite>("UI/icons/peace");
// 				}
// 				else if(rtsm.diplomacy.relations[pNation][i] == 1){
// 					nationsMenuGrid.buttonsPool[i].im_button[1].sprite = Resources.Load<Sprite>("UI/icons/war");
// 				}
// 				else if(rtsm.diplomacy.relations[pNation][i] == 2){
// 					nationsMenuGrid.buttonsPool[i].im_button[1].sprite = Resources.Load<Sprite>("UI/icons/crown");
// 				}
// 				else if(rtsm.diplomacy.relations[pNation][i] == 3){
// 					nationsMenuGrid.buttonsPool[i].im_button[1].sprite = Resources.Load<Sprite>("UI/icons/bags");
// 				}
// 				else if(rtsm.diplomacy.relations[pNation][i] == 4){
// 					nationsMenuGrid.buttonsPool[i].im_button[1].sprite = Resources.Load<Sprite>("UI/icons/handshake");
// 				}
				j++;
			}
		}
	}
	
	
	
	public void CloseAllMenus(){
		playerDialogConsole.DeactivateAll();
		playerDialogConsoleTheir.DeactivateAll();
		playerAnswersConsole.DeactivateAll();
//		nationsMenuGrid.DeactivateAll();
		
		nationsMenuGrid_im.DeActivateAll();
		nationsMenuGrid_tx.DeActivateAll();
		nationsMenuGrid_rel.DeActivateAll();
		
		playerOffersMenuGrid.DeactivateAll();
	}
	

	void Start () {
	    for(int i=0; i<rtsm.diplomacy.numberNations; i++){
			theirActiveOffers.Add(new List<int>());
		}
		AddFunctionality();
	}
	
}





