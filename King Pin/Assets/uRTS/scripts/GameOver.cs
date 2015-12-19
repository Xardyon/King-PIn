using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	
	public RTSMaster rtsm;
	private bool runUpdate = false;
	

	
	
	void Start(){
		runUpdate = true;
	}
	
	void Update(){
		if(runUpdate){
			CheckCastleExistance();
		}
	}
	
	public void CheckCastleExistance(){
		if(rtsm.selectionMark.selectedGoPars.Count == 0){
			if(rtsm.nationAIs[rtsm.diplomacy.playerNation].spawnedBuildings.Count > 0){
				
				if(rtsm.numberOfUnitTypes[rtsm.diplomacy.playerNation][0] == 0){
					if(rtsm.buildingButtons.buildingButtonGrid[9].IsActiveAny() == false){
						ButtonObject bo = rtsm.buildingButtons.buildingButtonGrid[9].buttonsPool[0];
						rtsm.buildingButtons.buildingButtonGrid[9].ActivateButton(bo);
						
					}
				}
				
				if(rtsm.numberOfUnitTypes[rtsm.diplomacy.playerNation][0] > 0){
					if(rtsm.buildingButtons.buildingButtonGrid[9].IsActiveAny() == true){
						rtsm.buildingButtons.buildingButtonGrid[9].DeactivateAll();
					}
				}
			}
		}
		
		if(rtsm.selectionMark.selectedGoPars.Count > 0){
			if(rtsm.buildingButtons.buildingButtonGrid[9].IsActiveAny() == true){
				rtsm.buildingButtons.buildingButtonGrid[9].DeactivateAll();
			}
		}
		
		
	}
	
	
	
}
