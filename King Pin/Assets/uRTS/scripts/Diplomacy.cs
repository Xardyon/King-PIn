using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Diplomacy : MonoBehaviour {
	
	public int numberNations = 2;
	public int playerNation = 0;
	
	public bool warNoticeWarning = true;
//	public List<bool> warWarnings = new List<bool>();
	
	
	public List<List<int>> relations = new List<List<int>>();
	public RTSMaster rtsm;
	
	
	
	/////////// relation values and meanings //////////////////
	// relations[i,j] = 0; - peace
	// relations[i,j] = 1; - war
	// relations[i,j] = 2; - slavery
	// relations[i,j] = 3; - mastery
	// relations[i,j] = 4; - alliance
	///////////////////////////////////////////////////////////
	
	////////// unitPars statusBS values and meanings //////////
	// statusBS = 0; - is not on BS
	// statusBS = 1; - is on BS
	// statusBS = 2; - remove from BS
	// statusBS = 3; - set to BS
	///////////////////////////////////////////////////////////
	
	void Awake(){
//		rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
		for(int i=0;i<numberNations;i++){
			relations.Add(new List<int> { 0 });
	
		}
		
		
		for(int i=0;i<numberNations;i++){
			for(int j=0;j<numberNations;j++){
				relations[i].Add(0);
			}
		}

	}
	
	
	void Start () {
	    
	  /*  for(int i=0;i<numberNations;i++){
	    	
	    	warWarnings.Add(true);
	    	if((i==0)||(i==playerNation)){
	    		warWarnings[i] = false;
	    	}
	    	if(warNoticeWarning==false){
	    		warWarnings[i] = false;
	    	}
	    }*/

		
		
		
        SetAllPeace();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.F))
		{
			SetRelation(0, 1, 1);
			SetRelation(1, 2, 1);
	//		SetRelation(0, 2, 1);
			print("war");
		}
		if (Input.GetKeyDown (KeyCode.P))
		{
			SetRelation(0, 1, 0);
	//		SetRelation(0, 2, 0);
			print("peace");
		}
	
	}
	
	public void SetAllPeace(){
		for (int i = 0; i < numberNations; i++){
			for (int j = 0; j < numberNations; j++){
				if(i!=j){
					relations[i][j] = 0;
				}
			}
		}
//		print("peace");
	}
	
	public void SetAllWar(){
		for (int i = 0; i < numberNations; i++){
			for (int j = 0; j < numberNations; j++){
				if(i!=j){
					relations[i][j] = 1;
				}
			}
		}
//		print("war");
	}
	
	public void SetRelation(int firstNation, int secondNation, int relation){
//	    print(firstNation.ToString() + secondNation.ToString() + relation.ToString());
		if((firstNation!=secondNation) && (relation!=relations[firstNation][secondNation])){
	    	if(relation == 2){
	    	    
	    	    SlavedNation(firstNation, secondNation);
	    		relations[firstNation][secondNation] = 2;
	    		relations[secondNation][firstNation] = 3;
	    //		print(secondNation);
	    		
	    	}
	    	else if(relation == 3){
	    	    SlavedNation(secondNation, firstNation);
	    		relations[firstNation][secondNation] = 3;
	    		relations[secondNation][firstNation] = 2;
	    		
	    	}
			else{
			    LeaveSlaveryStraight(firstNation, secondNation);
				relations[firstNation][secondNation] = relation;
				relations[secondNation][firstNation] = relation;
			}
			
	//		ResetUnitsBehaviour(firstNation, secondNation, relation);
	        if((firstNation == playerNation) || (secondNation == playerNation)){
				rtsm.diplomacyMenu.ChangeRelationIcons();
			}
			
		}
	}
	
	
	
	
	public void ResetUnitsBehaviour(int firstNation, int secondNation, int relation){
		BattleSystem bs = rtsm.battleSystem;
		List<UnitPars> allUnits = bs.unitssUP;
		
		if(relation == 1){
			warNoticeWarning = false;
			
			for(int i=0; i<allUnits.Count; i++){
	//		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Unit"))
	//		{
			
				
				UnitPars goPars = allUnits[i];
				
				
				if(goPars.rtsUnitId != 15){
					if((goPars.prepareMovingMC==false)&&(goPars.isMovingMC==false)){
						if(goPars.nation == firstNation){
							bs.ResetSearching(goPars);
						}
						else if(goPars.nation == secondNation){
							bs.ResetSearching(goPars);
						}
					}
				}
			
				
				
			}
		}
		else{
			warNoticeWarning = true;
			
			
			for(int i=0; i<allUnits.Count; i++){
	//		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Unit"))
	//		{
			
				
				UnitPars goPars = allUnits[i];
				//go.GetComponent<UnitPars>();
		
					
				if(goPars.rtsUnitId != 15){
				
					if((goPars.prepareMovingMC==false)&&(goPars.isMovingMC==false)){
					// remove enemy targets on peace
					
						if(goPars.nation == firstNation){
						//	bs.UnSetSearching(go);
							bs.ResetSearching(goPars);
						}
						else if(goPars.nation == secondNation){
						//	bs.UnSetSearching(go);
							bs.ResetSearching(goPars);
						}
					}
				
					if(goPars.strictApproachMode == true){
						UnitPars tgPars = goPars.targetUP;
							if(tgPars != null){
						//		UnitPars tgPars = target.GetComponent<UnitPars>();
								if(tgPars.nation != goPars.nation){
							
								
									if(goPars.nation == firstNation){
										goPars.strictApproachMode = false;
									}
									else if(goPars.nation == secondNation){
										goPars.strictApproachMode = false;
									}
								}
							}
					}
				}
		
			}
		}

	}
	
	
	
	public IEnumerator Invoke(){
	
		bool invokeEnd = false;
		while(invokeEnd == false){
		
			if(invokeEnd == false){
				SetAllPeace();
			}
			
			invokeEnd = true;
			yield return new WaitForSeconds(0.5f);
		}
	
	}
	
	
	void SlavedNation(int slave, int master){
		for(int i=0; i<numberNations; i++){
			if((i!=slave)&&(i!=master)){
		//		SetRelation(master, i, 1);
		        if(rtsm.nationAIs[i].masterNationId != master){
					relations[master][i] = 1;
					relations[i][master] = 1;
				}
			}
		}
		
		LeaveSlavery(slave);
		rtsm.nationAIs[slave].masterNationId = master;
		
		
	}
	
	void LeaveSlavery(int slave){
	    int master = rtsm.nationAIs[slave].masterNationId;
		if(master > -1){
		//	rtsm.diplomacy.SetRelation(slave, rtsm.nationAIs[slave].masterNationId, 0);
		    
			relations[slave][master] = 0;
			relations[master][slave] = 0;
		}
	}
	void LeaveSlaveryStraight(int slave, int master){
		if(rtsm.nationAIs[slave].masterNationId == master){
	//		rtsm.diplomacy.SetRelation(slave, master, 0);
			relations[slave][master] = 0;
			relations[master][slave] = 0;

		}
	}
	
	
}
