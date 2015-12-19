using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraOperator : MonoBehaviour {
	
	[HideInInspector] public List<Texture2D> cursorTexture = new List<Texture2D>();
	[HideInInspector] public int cursorMode = 0;
	
	private Texture2D selectionHighlight = null;
	public static Rect selection = new Rect(0,0,0,0);
	private Vector3 startClick = -Vector3.one;
	
	static private Vector3 destinationClick = -Vector3.one;
	
	private static Vector3 moveToDestination = Vector3.zero;
	private static List<string> passables = new List<string>() { "Terrain" };
	
	[HideInInspector] public List<UnitPars> selectablesPars = new List<UnitPars>();
	
	[HideInInspector] public List<UnitPars> allUnits = null;
	
	
	private bool isSelectByClickRunning = false;
	
	private bool lockRectangle = false;
	
	
	
	private UnitPars tGo = null;
	
	static private BattleSystem bs;
	static private Diplomacy dip;
	
	static private SelectionMark selM;
		
	[HideInInspector] public bool isMouseOnActiveScreen = true;
	
	[HideInInspector] public RTSMaster rtsm = null;
	
//	[HideInInspector] public int nGroups = 0;
//	[HideInInspector] public List<int> groupCount = new List<int>();
	
	[HideInInspector] public int movementButtonMode = 0;
	
	
	void Start() {
		bs = rtsm.battleSystem;
		dip = rtsm.diplomacy;
		selM = rtsm.selectionMark;
		
		selectionHighlight = new Texture2D(2,2);
		selectionHighlight = Texture2D.whiteTexture;
		
		allUnits = rtsm.battleSystem.unitssUP;
		
		InstanciateCursor();
		StartCoroutine(Cnt());
	}
	
	
	void InstanciateCursor(){
		Cursor.visible = false;
		cursorTexture.Add(Resources.Load<Texture2D>("UI/icons/cursor"));
		cursorTexture.Add(Resources.Load<Texture2D>("UI/icons/move"));
		cursorTexture.Add(Resources.Load<Texture2D>("UI/icons/attack"));
		cursorMode = 0;
	}
	
	
	
	
	void Update () {
		if(isMouseOnActiveScreen==true){
			if(movementButtonMode == 0){
				CheckCamera();
			}
			if(movementButtonMode == 1){
				CheckMovementButtonCamera();
			}
			if(movementButtonMode == 2){
				CheckAttackButtonCamera();
			}
			
		}
		else{
			if(Input.GetMouseButtonUp(0)){
				CheckCamera();
			}
		}
		
	}
	
	private void CheckCamera(){
		
		if(Input.GetMouseButtonDown(0)){
			startClick = Input.mousePosition;
			if(rtsm.buildMark.projector.activeSelf == false){
				SelectByClick(startClick,0);
			}
		
		}
			
		else if(Input.GetMouseButtonUp(0)){
		    if(lockRectangle == false){
				if(selection.width != 0){
					if(selection.height != 0){
						SelectByRect();
					}
				}
				startClick = -Vector3.one;
				selection = new Rect(0,0,0,0);
			}
			lockRectangle = false;
			
		}
		
		if(Input.GetMouseButton(0)){
			if(lockRectangle == false){
				selection = new Rect(startClick.x,InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
			
				if(selection.width < 0){
					selection.x += selection.width;
					selection.width = -selection.width;
				}
				if(selection.height < 0){
					selection.y += selection.height;
					selection.height = -selection.height;
				}
			}
		}	
			
		if(selM.selectedGoPars.Count > 0){	
		    if(selM.selectedGoPars[0].isBuilding == false){
				if(Input.GetMouseButton(1)){
					cursorMode = 1;
					tGo = null;
					destinationClick = Input.mousePosition;
					RightClick(destinationClick);
					if(tGo != null){
						cursorMode = 2;
					}
					
				}
				if(Input.GetMouseButtonUp(1)){
					cursorMode = 0;
					tGo = null;
					destinationClick = Input.mousePosition;
			
					RightClick(destinationClick);
					GetDestination();
					StartCoroutine(SetDestinations());
			
				}
			}
		}
		if(Input.GetMouseButton(1)){
			rtsm.diplomacyMenu.CloseAllMenus();
			rtsm.buildingButtons.CloseBuildMenus();
		}
		
	}
	
	
	private void CheckMovementButtonCamera(){
		
		cursorMode = 1;
		if(Input.GetMouseButtonUp(0)){
			cursorMode = 0;
			
			
			destinationClick = Input.mousePosition;
			RightClick(destinationClick);
			GetDestination();
			tGo = null;
			StartCoroutine(SetDestinations());
			movementButtonMode = 0;
		}
		if(Input.GetMouseButtonUp(1)){
			cursorMode = 0;
			movementButtonMode = 0;
		}
	}
	
	private void CheckAttackButtonCamera(){
		cursorMode = 3;
		tGo = null;
		destinationClick = Input.mousePosition;
		RightClick(destinationClick);
		if(tGo != null){
			cursorMode = 4;
		}
		if(Input.GetMouseButtonUp(0)){
			cursorMode = 0;
			
			
			tGo = null;
			destinationClick = Input.mousePosition;
			RightClick(destinationClick);
			GetDestination();
			if(tGo != null){
				StartCoroutine(SetDestinations());
			}
			movementButtonMode = 0;
		}
		if(Input.GetMouseButtonUp(1)){
			cursorMode = 0;
			movementButtonMode = 0;
		}
	}
	
	
	
	
	private void OnGUI(){
		GUI.color = new Color(1, 1 ,1 ,0.5f);
		GUI.DrawTexture(selection, selectionHighlight);
		Cursor.visible = false;
		
		if(cursorMode == 0){
			GUI.color = new Color(0.9f,0.9f,0.7f,1f);
			//new Color(0.25f,0.2f,0.01f,1f);
			GUI.DrawTexture (new Rect(Event.current.mousePosition.x-32, Event.current.mousePosition.y-32, 64, 64), cursorTexture[0]);
		}
		if(cursorMode == 1){
			GUI.color = new Color(0.61f,0.50f,0.11f,1f);
			float gg = Mathf.PingPong(Time.time, 1f)*0.5f+0.5f;
			int i = (int)(32f*gg);
			GUI.DrawTexture (new Rect(Event.current.mousePosition.x-i, Event.current.mousePosition.y-i, 2*i, 2*i), cursorTexture[1]);
		}
				
		if(cursorMode == 2){
			float gg = Mathf.PingPong(Time.time, 1f)*0.5f;
			GUI.color = new Color(1, gg ,gg ,1f);
			GUI.DrawTexture (new Rect(Event.current.mousePosition.x-16, Event.current.mousePosition.y-16, 32, 32), cursorTexture[2]);
		}
		
		if(cursorMode == 3){
			GUI.color = new Color(1, 1 ,1 ,1f);
			GUI.DrawTexture (new Rect(Event.current.mousePosition.x-16, Event.current.mousePosition.y-16, 32, 32), cursorTexture[2]);
		}
		if(cursorMode == 4){
			GUI.color = new Color(1, 0 ,0 ,1f);
			GUI.DrawTexture (new Rect(Event.current.mousePosition.x-16, Event.current.mousePosition.y-16, 32, 32), cursorTexture[2]);
		}

		
		
	}
	
	public static float InvertMouseY(float y){
		
		return Screen.height - y;
	}
	

	
	public static void GetDestination(){
	
			RaycastHit hit;
			Ray r = Camera.main.ScreenPointToRay(destinationClick);
			
			if(Physics.Raycast(r,out hit)){
				while (!passables.Contains(hit.transform.gameObject.name)){
					
					if(!Physics.Raycast(hit.transform.position, r.direction, out hit)){
						break;
					}
				}
				
				
			}
			if(hit.transform!=null){
				moveToDestination = hit.point;
			}
	
	}
	
	public void StopDestinationsF(){
		StartCoroutine(StopDestinations());
	}
	
	
	public IEnumerator StopDestinations(){
		while(isSelectByClickRunning==true){
			yield return new WaitForSeconds(0.1f);
		}
		LoadSelectables(1);
		
		UnitPars goPars;
		NavMeshAgent goNav;

		for(int i=0;i<selectablesPars.Count;i++){
			    
			goPars = selectablesPars[i];	
			goNav = goPars.thisNMA;
			
			if(goPars.isSelected){
			
				if(goPars.targetUP != null){
					goPars.targetUP.noAttackers = goPars.targetUP.noAttackers-1;
					goPars.targetUP.attackers.Remove(goPars);
					goPars.targetUP = null;
				}
				goPars.strictApproachMode = false;
				goPars.onManualControl = true;
			
				goPars.isMovingMC = true;
				bs.UnSetSearching(goPars);
				goPars.manualDestination = goPars.transform.position;
				
			
				if(goNav.enabled == true){
					goNav.ResetPath();
				}
			    
			    if(goPars.thisSL != null){
			        SpriteLoader spL = goPars.thisSL;
					spL.animName = "Idle";
					rtsm.spritesManagerMaster.SetAnimation(spL,goPars.rtsUnitId);
				}
			
			}
		}
		
	}
	
	
	public IEnumerator Cnt(){
		while(true){
		    int selk = 0;
			for(int i=0;i<selectablesPars.Count;i++){
			    UnitPars goPars = selectablesPars[i];
				if(goPars.isSelected){
					selk = selk+1;
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
	}
	
	
	
	public IEnumerator SetDestinations(){
		
		while(isSelectByClickRunning==true){
			yield return new WaitForSeconds(0.02f);
		}
		
		LoadSelectables(1);
		
		
		bool proceedToWar = false;
		
		
		if(tGo != null){
		   if(rtsm.selectionMark.selectedGoPars.Count > 0){
			   if(tGo.nation!=dip.playerNation){
				 if((dip.warNoticeWarning==true)&&(dip.relations[dip.playerNation][tGo.nation]!=1)){
					rtsm.diplomacyMenu.MakeReport("Attacking the object will lead to War!");
					dip.warNoticeWarning=false;
					StartCoroutine("ResetWarNotice");
				  }
				  else
				  {	
					StopCoroutine("ResetWarNotice");
					proceedToWar = true;
					dip.SetRelation(dip.playerNation, tGo.nation, 1);
				
				  }
				}
				else{
					proceedToWar = true;
				}
			}
		}
		
		UnitPars goPars;
		NavMeshAgent goNav;
		
		List<Vector3> formationDest = LoadFormationDestinations();
		int iManual = 0;
		
		for(int i=0;i<selectablesPars.Count;i++){
			    
			goPars = selectablesPars[i];

			
			goNav = goPars.thisNMA;
			
			if(goNav != null){
				if(goPars.isSelected){
					
					
					if(goPars.isOnBS==true){
						
						
					}
					
					goPars.failedDist = 0;
					if(tGo != null){
						if(proceedToWar == true){
							
							if(goPars != tGo){
								if(goPars.rtsUnitId != 15){
									goPars.strictApproachMode = true;
									goPars.targetUP = tGo;
									goPars.targetNMA = tGo.thisNMA;
									tGo.noAttackers = tGo.noAttackers+1;
									tGo.attackers.Add(goPars);
									tGo.isApproachable = true;
									goPars.militaryMode = 20;
								}
								else if(goPars.rtsUnitId == 15){
									if(tGo.isBuilding == false){
										goPars.strictApproachMode = true;
										goPars.targetUP = tGo;
										goPars.targetNMA = tGo.thisNMA;
										tGo.noAttackers = tGo.noAttackers+1;
										tGo.attackers.Add(goPars);
										tGo.isApproachable = true;
										goPars.militaryMode = 20;
										
									}
								
								}
							}
						}
					}
					else{
						if(goPars.rtsUnitId != 15){
							if(goPars.targetUP != null){
								goPars.targetUP.noAttackers = goPars.targetUP.noAttackers-1;
								goPars.targetUP.attackers.Remove(goPars);
								goPars.targetUP = null;
							}
							goPars.strictApproachMode = false;
							goPars.onManualControl = true;
						
							goPars.isMovingMC = true;
							bs.UnSetSearching(goPars);
							
							Vector3 manDest = formationDest[iManual];
							
							goPars.manualDestination = manDest;
						
//							if(goNav.enabled == true){
//								goNav.SetDestination(manDest);
					//		goPars.MoveUnit(manDest, "Walk");
					//		rtsm.unitsMover.AddMover(goPars,manDest,0);
							rtsm.unitsMover.AddMilitaryAvoider(goPars,manDest,0);
//							}
							
							iManual = iManual+1;
						
// 							if(goPars.thisSL != null){
// 								SpriteLoader spL = goPars.thisSL;
// 								spL.animName = "Walk";
// 								rtsm.spritesManagerMaster.SetAnimation(spL,goPars.rtsUnitId);
// 								
// 							}
							
						}
						else if(goPars.rtsUnitId == 15){
							if(movementButtonMode == 1){
								rtsm.resourcesCollection[rtsm.diplomacy.playerNation].WalkTo(moveToDestination);
							}
						}
					
					}
					
					
					
				
					
				}
			}				
		}
		
		tGo = null;
		yield return new WaitForSeconds(0.02f);
	
	}
	
	
	
	public List<Vector3> LoadFormationDestinations(){
		UnitPars goPars;
		NavMeshAgent goNav;
		
		List<Vector3> destinations = null;
		List<UnitPars> manualUP = new List<UnitPars>();
		
		for(int i=0;i<selectablesPars.Count;i++){
			goPars = selectablesPars[i];
			goNav = goPars.thisNMA;
			if(goNav != null){
				if(goPars.isSelected){
					if(tGo == null){
						manualUP.Add(goPars);
					}
				}
			}
			
		}
		if(manualUP.Count > 0){
		    Formation form = null;
		    bool pass = false;
		    if(manualUP[0].formation != null){
				if(manualUP[0].formation.strictMode == 1){
					if(manualUP[0].unitsGroup != null){
						if(manualUP[0].unitsGroup.members.Count > 0){
							if(manualUP[0].unitsGroup.members[0].formation == manualUP[0].formation){
								pass = true;
							}
						}
						else if(manualUP[0].formation.strictMode == 1){
							pass = true;
						}
					}
					
				}
		    }
		    
			if(pass == true){
				form = manualUP[0].formation;
			}
			else{
				form = new Formation();
				form.rtsm = rtsm;
			
				rtsm.formations.AddUnitsToFormations(manualUP,form);
			}
			
			destinations = form.GetDestinations(manualUP, moveToDestination);
		}
		
		return destinations;
	}
	
	
	
	public void SelectByClick(Vector3 clickPos, int clickMode) {
		
		isSelectByClickRunning = true;
		Ray r = Camera.main.ScreenPointToRay(clickPos);
		Vector3 camVec = Camera.main.transform.position;
		
		LoadSelectables(clickMode);
		
		
		//Ray world vector
		Vector3 rayDirection = r.direction;
		
		
		float distFromRay = 0.0f;
					
		UnitPars goPars;
		Vector3 goVec;
		
		UnitPars selCandidate_Pars = null;
		
		float minDist = 999999.9f;
		
		bool selectionCleaned = false;
		
		if(rtsm.buildMark.projector.activeSelf == false){
			DeselectAll();
			selectionCleaned = true;
		}
		
		
		for(int i=0;i<selectablesPars.Count;i++){
			
			
			goPars = selectablesPars[i];
			
			
			
			goVec = goPars.transform.position;
		
			float distFromCamera = (goVec - camVec).magnitude;
		
		
			distFromRay=Vector3.Distance(rayDirection*distFromCamera , goVec - camVec);
			
			if(distFromRay < goPars.rEnclosed){
				if(distFromCamera < minDist){
					minDist = distFromCamera;
					selCandidate_Pars = goPars;
				}
						
				
			}
		
			
		}
		
		
		
		if(selCandidate_Pars != null){
			SelectObject(selCandidate_Pars);
			
			lockRectangle = true;
			selectionCleaned = false;
			
			SelectedUnitsInfo();
			
			if(selCandidate_Pars.isBuilding){
				ActivateBuildingsMenu(selCandidate_Pars.rtsUnitId);
				if(selCandidate_Pars.rtsUnitId == 5){
					
					if(selCandidate_Pars.resourceType == 0){
						rtsm.mineLabel.SelectIron(selCandidate_Pars.remainingResources);
					}
					else if(selCandidate_Pars.resourceType == 1){
						rtsm.mineLabel.SelectGold(selCandidate_Pars.remainingResources);
					}
				}
			// activating buildProgressNum
				if(
					(selCandidate_Pars.rtsUnitId == 0)||
					(selCandidate_Pars.rtsUnitId == 1)
				){
					if(selCandidate_Pars.gameObject.GetComponent<SpawnPoint>() != null){
						SpawnPoint selCandidate_sp = selCandidate_Pars.gameObject.GetComponent<SpawnPoint>();
						if(selCandidate_sp.isSpawning == true){
							rtsm.cancelSpawnButton.Activate();
							rtsm.buildProgressNum.UpdateText(selCandidate_sp.GetProgressString());
							DeActivateBuildingsMenu(selCandidate_Pars.rtsUnitId);
						}
						else{
							rtsm.cancelSpawnButton.DeActivate();
							ActivateBuildingsMenu(selCandidate_Pars.rtsUnitId);
						}
					}
				}
				else{
					rtsm.cancelSpawnButton.DeActivate();
					ActivateBuildingsMenu(selCandidate_Pars.rtsUnitId);
				}
			}
			else if(selCandidate_Pars.isBuilding == false){
				UnsetPropertiesButton();
				ActivateUnitsMenu();
			}
			
			
			
		}
		if(selectionCleaned==true){
			UnsetPropertiesButton();
			rtsm.selectedObjectInfo.DeActivate();
			
		}
		
		
		isSelectByClickRunning = false;
		
	}
	
	
	public void RightClick(Vector3 clickPos){
	
		Ray r = Camera.main.ScreenPointToRay(clickPos);
		Vector3 camVec = Camera.main.transform.position;
		
		LoadSelectables(1);
		
		Vector3 rayDirection = r.direction;
		float distFromRay = 0.0f;
		
		UnitPars goPars;
		Vector3 goVec;
		
		for(int i=0;i<selectablesPars.Count;i++){
		
			goPars = selectablesPars[i];
			
			goVec = goPars.transform.position;			
			float distFromCamera = (goVec - camVec).magnitude;
		
			distFromRay=Vector3.Distance(rayDirection*distFromCamera , goVec - camVec);
			if(distFromRay < goPars.rEnclosed){
				tGo = goPars;
			}
		}
		
	
	}	
	
	
	
	
	public void SelectByRect(){
	
		LoadSelectables(0);
		
		Camera camera = Camera.main;
		
		UnitPars goPars;
		Vector3 goPos;
		Vector3 camPos;
		
		for(int i=0;i<selectablesPars.Count;i++){
			
			goPars = selectablesPars[i];
			
	
			goPos = goPars.transform.position;
			
			// if ManualControl is attached to gameobject
			camPos = camera.WorldToScreenPoint(goPos);
			camPos.y = CameraOperator.InvertMouseY(camPos.y);
		
			
			if(selection.Contains(camPos)){
				if(goPars.isBuilding==false){
					SelectObject(goPars);
					
					ActivateUnitsMenu();
					
				}
	
			}
			
		}
		if(selM.selectedGoPars.Count>0){
			SelectedUnitsInfo();
		}
	
	}
	
	
	
	public void LoadSelectables(int mode){
		
		
		selectablesPars.Clear();
		
		for(int i=0;i<allUnits.Count;i++){
			UnitPars goPars = allUnits[i];
			if(mode == 0){
				if(goPars.nation == rtsm.diplomacy.playerNation){
					selectablesPars.Add(goPars);
				}
			}
			else if(mode == 1){
				selectablesPars.Add(goPars);
			}
		}
		
	}
	
	
	public void SelectedUnitsInfo(){
		if(selM.selectedGoPars.Count == 1){
			UnitPars up = selM.selectedGoPars[0];
			
			rtsm.selectedObjectInfo.Activate(
				up.unitName,
		//		rtsm.buildDiplomacyMenu.unitsIcons[up.rtsUnitId],
				rtsm.uiMaster.unitsIcons[up.rtsUnitId],
				up.health/up.maxHealth
			);
		}
		else if(selM.selectedGoPars.Count > 1){
			
			rtsm.selectedObjectInfo.Activate(
				"troops ("+selM.selectedGoPars.Count.ToString()+")",
				rtsm.uiMaster.troopsIcon,
				1f
			);
			
		}
	}
	
	
	
	public IEnumerator ResetWarNotice(){
		yield return new WaitForSeconds(8.0f);
		dip.warNoticeWarning = true;
	}
	
	
	
	public void ActiveScreenTrue(){
		isMouseOnActiveScreen = true;
	}
	
	public void ActiveScreenFalse(){
		isMouseOnActiveScreen = false;
	}
	
	
	
	
	
	public void UnsetPropertiesButton(){
		rtsm.cancelSpawnButton.DeActivate();
		rtsm.mineLabel.DeActivate();
	}
	
	
	
	public void StopSelectedSpawning(){
		if(selM.selectedGoPars.Count == 1){
			SpawnPoint sp = selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>();
			sp.StopSpawning();
			
		}
	}
	
	
	
	public void ActivateUnitsMenu(){
		rtsm.movementButtons.movementButtonGrid.ActivateAll();
	}
	
	
	public void DeActivateUnitsMenu(){
		rtsm.movementButtons.movementButtonGrid.DeactivateAll();
	}
	
	
	public void ActivateBuildingsMenu(int id){
	    if(id<rtsm.buildingButtons.buildingButtonGrid.Count){
	        if(selM.selectedGoPars.Count == 1){
	        	if(selM.selectedGoPars[0].isBuildFinished == true){
					rtsm.buildingButtons.buildingButtonGrid[id].ActivateAll();
				}
			}
		}
	}
	
	public void DeActivateBuildingsMenu(int id){
		if(id<rtsm.buildingButtons.buildingButtonGrid.Count){
			rtsm.buildingButtons.buildingButtonGrid[id].DeactivateAll();
			rtsm.buildingButtons.CloseBuildMenus();
		}
	}
	
	
	
	
	public void DeselectAll(){
		for(int i=0; i<selM.selectedGoPars.Count; i++){
			DeselectObject(selM.selectedGoPars[i]);
		}
		for(int i=0;i<allUnits.Count;i++){
			DeselectObject(allUnits[i]);
		}
		
		selM.selectedGoPars.Clear();
	}
	
	
	public void DeselectObject(UnitPars goPars){
		if(goPars.isSelected == true){
			goPars.isSelected = false;
			selM.selectedGoPars.Remove(goPars);
		
			rtsm.cancelSpawnButton.DeActivate();
		    
		    if(selM.selectedGoPars.Count<1){
				rtsm.selectedObjectInfo.DeActivate();
			}
			
			selM.totalSelectedHealth = selM.totalSelectedHealth-goPars.maxHealth;
								
			if(selM.selectedGoPars.Count<1){
				rtsm.selectedObjectInfo.DeActivate();
				rtsm.cancelSpawnButton.DeActivate();
			}
			else{
				rtsm.selectedObjectInfo.selectedObjectInfo_text.tx_button.text = "troops ("+selM.selectedGoPars.Count.ToString()+")";
				rtsm.selectedObjectInfo.SetHealth(selM.remainingSelectedHealth/selM.totalSelectedHealth);
			}
	        
	        if(selM.selectedGoPars.Count<1){
				DeActivateUnitsMenu();
			}
			if(goPars.isBuilding == true){	
				DeActivateBuildingsMenu(goPars.rtsUnitId);
			}
			if(selM.selectedGoPars.Count == 0){
				rtsm.movementButtons.movementButtonGrid.DeactivateAll();
			}
		}
	}
	
	
	public void SelectObject(UnitPars goPars){
		goPars.isSelected = true;
		selM.selectedGoPars.Add(goPars);
		CheckFormation(goPars);
	}
	
	public void SelectObjectS(UnitPars goPars){
		goPars.isSelected = true;
		selM.selectedGoPars.Add(goPars);
	}
	
	public void CheckFormation(UnitPars goPars){
		if(goPars.formation != null){
			Formation form = goPars.formation;
			if(form.strictMode == 1){
				for(int i=0;i<form.units.Count;i++){
					UnitPars up = form.units[i];
					if(up != goPars){
						if(up.isDying == false){
							if(up.isSinking == false){
								selM.selectedGoPars.Remove(up);
								SelectObjectS(up);
							}
						}
					}
				}
			}
		}
	}
	
	
}
