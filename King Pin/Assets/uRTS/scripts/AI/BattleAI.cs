using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleAI : MonoBehaviour {
	
	public SearcherAI iddles;
	public SearcherAI approachers;
	
//	public WondererAI wonderers = null;
	
    public RTSMaster rtsm;
    public int nation = 0;
	
	
	void Start () {
		iddles = new SearcherAI();
		approachers = new SearcherAI();
		
		
		
		iddles.rtsm = rtsm;
		iddles.nation = nation;
		iddles.allUnits = rtsm.battleSystem.unitssUP;
		
		approachers.rtsm = rtsm;
		approachers.nation = nation;
		approachers.allUnits = rtsm.battleSystem.unitssUP;
		
		
		
		StartCoroutine(iddles.SearchPhase());
		StartCoroutine(approachers.SearchApproacherPhase());
		
		if(nation != rtsm.diplomacy.playerNation){
//		    wonderers = new WondererAI();
//		    wonderers.rtsm = rtsm;
//			wonderers.nation = nation;
//			wonderers.allUnits = rtsm.battleSystem.unitssUP;
//			StartCoroutine(wonderers.WondererPhase());
			
// 			for(int i=0; i<rtsm.diplomacy.numberNations; i++){
// 				if(i != nation){
// 					wonderers.Add(new WondererAI());
// 					wonderers[i].rtsm = rtsm;
// 					wonderers[i].nation = nation;
// 					wonderers[i].wonderingNation = i;
// 					wonderers[i].allUnits = rtsm.battleSystem.unitssUP;
// 					
// 					StartCoroutine(wonderers[i].WondererPhase());
// 				}
// 				else{
// 					wonderers.Add(null);
// 				}
// 			}
		}
		
		
	}


}





public class SearcherAI {
    
    [HideInInspector] public List<UnitPars> attackersPars = new List<UnitPars>();
    
    [HideInInspector] public List<UnitPars> approachersPars = new List<UnitPars>();
    
    [HideInInspector] public List<UnitPars> targetsPars = new List<UnitPars>();
    [HideInInspector] public KDTree targetsKD;
    
    
    
    public RTSMaster rtsm;
    public int nation = 0;
    
    public float minimumApproachDistance = 30f;
    
    public List<UnitPars> allUnits = null;
	

    public IEnumerator SearchPhase(){
    	while(true){
    	    
    	    ResetFreeTargets();
    	    ResetAttackers();
    	    FindTargets();
    	    
    	    
    		yield return new WaitForSeconds(Random.Range(0.57f,0.63f));
    	}
    	
    
    }
    public IEnumerator SearchApproacherPhase(){
    	while(true){
    	    
    	    ResetFreeTargets();
    	    ResetApproachers();
    	    FindApproacherTargets();
    	    
    	    
    		yield return new WaitForSeconds(Random.Range(0.57f,0.63f));
    	}
    	
    
    }
    
    
    
    
    
    public void RebuildTargetsKD(){
        if(targetsPars.Count > 0){
			Vector3[] targPos = new Vector3[targetsPars.Count];
		
			for(int i=0; i<targetsPars.Count; i++){
				targPos[i] = targetsPars[i].transform.position;
			}
			targetsKD = KDTree.MakeFromPoints(targPos);
    	}
    	
    }
    
    
    
    
    
    public void ResetTargets(){
    	targetsPars.Clear();
    	
    	for(int i=0; i<allUnits.Count; i++){
			UnitPars up = allUnits[i];
			
			if(rtsm.diplomacy.relations[nation][up.nation] == 1){
				targetsPars.Add(up);
			}		
    	}
    	RebuildTargetsKD();
    	
    	
    }
    public void ResetFreeTargets(){
  // uses check for maximum number of attackers  
    	targetsPars.Clear();
    	
    	for(int i=0; i<allUnits.Count; i++){
			UnitPars up = allUnits[i];
			
			if(rtsm.diplomacy.relations[nation][up.nation] == 1){
				if(CheckMaxumumAttackersNumber(up,1)==true){
					if((up.isDying == false)&&(up.isSinking==false)){
						targetsPars.Add(up);
					}
				}
			}
    			
    	}
    	RebuildTargetsKD();
    	
    }
    
    
    
    public void AddTarget(UnitPars upTarget){
        if(upTarget != null){
    		targetsPars.Add(upTarget);
    		RebuildTargetsKD();
    	}    	
    }
    
    public void RemoveTarget(UnitPars upTarget){
        if(upTarget != null){
    		targetsPars.Remove(upTarget);
    		RebuildTargetsKD();
    	}    	
    }
    
    
    
    public void ResetAttackers(){
    	attackersPars.Clear();
    	
    	for(int i=0; i<allUnits.Count; i++){
			UnitPars up = allUnits[i];
			
			if(up.nation == nation){
				if(up.militaryMode == 10){
					if(up.isBuilding == false){
						if(up.rtsUnitId != 15){
							attackersPars.Add(up);
						}
					}
				}
			}
    			
    	}
    	
    }
    public void ResetApproachers(){
    	approachersPars.Clear();
    	
    	int count = 0;
    	
    	for(int i=0; i<allUnits.Count; i++){
			UnitPars up = allUnits[i];
			
			if(up.nation == nation){
				if(up.militaryMode == 20){
					if(up.isBuilding == false){
						if(up.strictApproachMode == false){
							if(up.rtsUnitId != 15){
								if(count < 140){
									if(up.approachersLoadCount > Random.Range(5,7)){
										approachersPars.Add(up);
										up.approachersLoadCount = 0;
										count = count+1;
									}
									else{
										up.approachersLoadCount = up.approachersLoadCount+1;
									}
								}
							}
						}
					}
				}
			}
    			
    	}
    	
    }
    
    
    
    public void FindTargets(){
    	// assign simple neighbour targets
    	if(targetsPars.Count > 0){
			for(int i=0; i<attackersPars.Count; i++){
				int k = targetsKD.FindNearest(attackersPars[i].transform.position);
			    if((k>=0)&&(k<targetsPars.Count)){
			        if(i>=attackersPars.Count){
			        	Debug.Log("a1");
			        }
			        if(i<0){
			        	Debug.Log("a2");
			        }
			        if(k>=targetsPars.Count){
			        	Debug.Log("a3");
			        }
			        if(k<0){
			        	Debug.Log("a4");
			        }
			    	float R = (attackersPars[i].transform.position-targetsPars[k].transform.position).sqrMagnitude;
			    	float searchDist = attackersPars[i].searchDistance * attackersPars[i].searchDistance;
			    	
			    	if(R < searchDist){
						AssignTarget_MaxCheck(i, k);
					}
					else if(IsInsideNation(targetsPars[k].transform.position, nation)&&
							IsInsideNation(attackersPars[i].transform.position, nation)){
						AssignTarget_MaxCheck(i, k);
					}
					else if(IsInsideNation(targetsPars[k].transform.position, targetsPars[k].nation)&&
							IsInsideNation(attackersPars[i].transform.position, targetsPars[k].nation)){
						AssignTarget_MaxCheck(i, k);
					}
					
				}
			}
    	}
    
    }
    
    
    
    public void FindApproacherTargets(){
    	// assign simple neighbour targets
    	if(targetsPars.Count > 0){
			for(int i=0; i<approachersPars.Count; i++){
				int k = targetsKD.FindNearest(approachersPars[i].transform.position);
				if((k>=0)&&(k<targetsPars.Count)){
					float R1 = (approachersPars[i].transform.position - targetsPars[k].transform.position).sqrMagnitude;
					float R2 = (approachersPars[i].transform.position - approachersPars[i].targetUP.transform.position).sqrMagnitude;
				
					if(R1 < 0.25f*R2){
						AssignApproacherTarget_MaxCheck(i, k);
					}
				}
			}
    	}
    
    }

    
    
    public void AssignTarget_MaxCheck(int i, int k){
    
        if(k>=targetsPars.Count){
        	Debug.Log("aa");
        }
        if(i>=attackersPars.Count){
        	Debug.Log("bb");
        }
		
		if(targetsPars[k].isDying == true){
			Debug.Log("dying");
		}
		if(targetsPars[k].isSinking == true){
			Debug.Log("sinking");
		}
        
        SetTarget(attackersPars[i],targetsPars[k]);
        attackersPars[i].targetNMA = targetsPars[k].thisNMA;
		
		attackersPars[i].militaryMode = 20;
		if((attackersPars[i].wonderingMode > 10)&&(attackersPars[i].wonderingMode < 100)){
			attackersPars[i].wonderingMode = 10;
		}
		else if(attackersPars[i].wonderingMode > 110){
		    attackersPars[i].wonderingMode = 110;
		    // if(attackersPars[i].guardResetCount < Random.Range(6,10)){
// 		    	attackersPars[i].guardResetCount = attackersPars[i].guardResetCount + 1;
// 		    }
// 		    else{
// 		        attackersPars[i].guardResetCount = 0;
// 				
// 			}
		}
		
		if(CheckMaxumumAttackersNumber(targetsPars[k],1)==false){
			RemoveTarget(targetsPars[k]);
		}
		
    }
    
    
    public void AssignApproacherTarget_MaxCheck(int i, int k){
        
        SetTarget(approachersPars[i],targetsPars[k]);
        approachersPars[i].targetNMA = targetsPars[k].thisNMA;
		
		if(CheckMaxumumAttackersNumber(targetsPars[k],1)==false){
			RemoveTarget(targetsPars[k]);
		}
		    
		
    }
    
    
    public bool CheckMaxumumAttackersNumber(UnitPars up, int mode){
    	bool check = false;
    	if(mode == 1){
			if(up.noAttackers < up.maxAttackers){
				check = true;
			}
    	}
		if(mode == 2){
			if(up.noAttackers < up.maxHealth*0.2f+5f){
				check = true;
			}
    	}    	
    	
    	return check;
    }
    
    public bool IsInsideNation(Vector3 pos, int nationId){
    	bool isInside = false;
    	float rSq = (pos - rtsm.nationCenters[nationId].transform.position).sqrMagnitude;
    	float nationSize = rtsm.nationAIs[nationId].size * rtsm.nationAIs[nationId].size;
    	if(rSq < nationSize){
    		isInside = true;
    	}
    	return isInside;
    }
	
	
	
	public void SetTarget(UnitPars attPars, UnitPars targPars){
		UnitPars prevTargetPars = attPars.targetUP;
		
		if(prevTargetPars != null){
			prevTargetPars.attackers.Remove(attPars);
			prevTargetPars.noAttackers = prevTargetPars.attackers.Count;
			attPars.targetUP = null;
		}
		
		if(targPars != null){
			attPars.targetUP = targPars;
			targPars.attackers.Add(attPars);
			targPars.noAttackers = targPars.attackers.Count;
		}
	}

	
	
	public UnitPars GetTarget(Vector3 attPos){
		UnitPars targ = null;
		if(targetsPars.Count > 0){
			int k = targetsKD.FindNearest(attPos);
			targ = targetsPars[k];
		}
		return targ;
	}
	
	

}
