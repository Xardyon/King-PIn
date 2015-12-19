using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WondererAI : MonoBehaviour {

	
	public List<UnitPars> wonderersPars = new List<UnitPars>();
 	public List<UnitPars> guardsPars = new List<UnitPars>();
 	
 	public List<List<UnitPars>> wonderersByNation = new List<List<UnitPars>>();
	

    public RTSMaster rtsm;
    public int nation = -1;
	
//	public int wonderersCount = 0;
	public int maxWonderersCount = 30;
	
	public int wonderingNation = -1;

// for moving to nation centre	
	public float R1 = 10f;
// for moving to safe distance	
	public float R2 = 0f;
	public float R3 = 0f;
	
	public Vector3 r1Point;
	public Vector3 r2Point;
	
	public float sumOfAllDistances = 0f;
	public float nationDistanceRatio = 0f;
	
	public List<float> nationDistanceRatios = new List<float>();
	public List<int> maxWonderersCounts = new List<int>();
	
	public List<UnitPars> allUnits = null;
	
	public List<int> nOponentsInside = new List<int>();
	
	public List<Wonderer> wonderers = new List<Wonderer>();
    public List<Wonderer> returners = new List<Wonderer>();
	
	
	void Start () {
	    if(nation != rtsm.diplomacy.playerNation){
			allUnits = rtsm.battleSystem.unitssUP;
			StartCoroutine(WondererPhase());
		}
	}
	
	
	
	
	
	
    public IEnumerator WondererPhase(){
        
        for(int i=0; i<rtsm.diplomacy.numberNations; i++){
        	maxWonderersCounts.Add(0);
        	wonderersByNation.Add(new List<UnitPars>());
        	nOponentsInside.Add(0);
        }
        
        wonderers = new List<Wonderer>();
        returners = new List<Wonderer>();
        
    	while(true){
    	    for(int i=0;i<(rtsm.diplomacy.numberNations-1);i++){
    	    	if(rtsm.nationPars[nation].isReady == true){
    	    	    wonderingNation = rtsm.nationPars[nation].sortedNationNeighbours[i];
					R3 = rtsm.nationPars[nation].rNations[wonderingNation];
			//		r1Point = rtsm.nationCenters[wonderingNation].transform.position;
					r1Point = TerrainVector(rtsm.nationCenters[wonderingNation].transform.position);
			
					ResetMaxWonderersCount();
					ResetWonderers();
			
			
					R2 = rtsm.nationPars[wonderingNation].rSafe;
					
					r2Point = GetR2Point(wonderingNation);
			//		Wonder(wonderingNation);
					
			        Guarding();
			        CountOponentsInside();
				}
    		}
    		
    	    StartWonderers();
    	    ReturnWonderers();
    	    CleanReturners();
    	    
    		
    		yield return new WaitForSeconds(Random.Range(0.57f,0.63f));
    	}
    	
    
    }
	
	
	
	
	
	
	public void StartWonderers(){
		if((wonderingNation >= 0) && (wonderingNation < maxWonderersCounts.Count)){
// 		    if(nation==4){
// 		    	print(wonderers.Count);
// 		    }
		    int iddleGuardCount = GetIddleGuardCount();
// 			if(iddleGuardCount > 0.2f*ExpectedGuardCount()){
// 				if(iddleGuardCount > 10){
// 					if(wonderers.Count == 0){
// 						if(returners.Count == 0){
// 							if(iddleGuardCount > 0.5f*guardsPars.Count){
// 								wonderers.Add(new Wonderer(this));
// 								int wCount = wonderers.Count-1;
// 								int gCount = (int) (1f*guardsPars.Count);
// 								int gCount2 = 0;
// 								for(int i=0; i<gCount; i++){
// 									if((guardsPars[i].isWondering1 == 0)&&(gCount2 < 0.5f*iddleGuardCount)){
// 										gCount2++;
// 										wonderers[wCount].AddUnit(guardsPars[i]);
// 									}
// 								}
// 								wonderers[wCount].SendUnits(TerrainVector(rtsm.nationCenters[rtsm.nationPars[nation].sortedNationNeighbours[0]].transform.position));
// 							}
// 						}
// 					}
// 				}
// 			}
			if(iddleGuardCount > 0.2f*ExpectedGuardCount()){
				if(iddleGuardCount > 10){
					if(returners.Count == 0){
						if(iddleGuardCount > 0.5f*guardsPars.Count){
							wonderers.Add(new Wonderer(this));
							int wCount = wonderers.Count-1;
							int gCount = (int) (1f*guardsPars.Count);
							int gCount2 = 0;
							for(int i=0; i<gCount; i++){
								if((guardsPars[i].isWondering1 == 0)&&(gCount2 < 0.5f*iddleGuardCount)){
									gCount2++;
									wonderers[wCount].AddUnit(guardsPars[i]);
									guardsPars[i].wonderingMode = -Mathf.Abs(guardsPars[i].wonderingMode);
								}
							}
							wonderers[wCount].SendUnits(TerrainVector(rtsm.nationCenters[rtsm.nationPars[nation].sortedNationNeighbours[0]].transform.position));
						}
					}
				}
			}
			
		}

	}
	
	
	public void ReturnWonderers(){
		for(int i=0;i<wonderers.Count;i++){
//		if(wonderers.Count > 0){
			if(wonderers[i].wonderersPars.Count > 0){
				int targNation = rtsm.nationPars[nation].sortedNationNeighbours[0];
				if(0.5f*wonderers[i].wonderersPars[0].thisNMA.speed * wonderers[i].TimeSpend() + 100f > 
					(
						TerrainVector(rtsm.nationCenters[targNation].transform.position) -
						TerrainVector(rtsm.nationCenters[nation].transform.position)
					).magnitude
				){
					if(rtsm.diplomacy.relations[nation][targNation] != 1){
						returners.Add(new Wonderer(this));
						int rCount = returners.Count-1;
						for(int j=0;j<wonderers[i].wonderersPars.Count;j++){
							UnitPars up = wonderers[i].wonderersPars[j];
							returners[rCount].AddUnit(up);
							up.wonderingMode = 110;
						}
						wonderers[i].CleanWonderers();
						wonderers.Remove(wonderers[i]);
					}
				}
			}
			else{
				wonderers.Remove(wonderers[i]);
			}
		}
		
		
// 		if(nation==4){
// 		    if(rtsm.nationPars[nation].sortedNationNeighbours.Count > 0){
// 				int targNation = rtsm.nationPars[nation].sortedNationNeighbours[0];
// 				print(rtsm.diplomacy.relations[nation][targNation]);
// 			}
// 		}

	}
	
	public void CleanReturners(){
		for(int i=0;i<returners.Count;i++){
	//	if(returners.Count > 0){
			if(returners[i].wonderersPars.Count > 0){
				if(0.5f*returners[i].wonderersPars[0].thisNMA.speed * returners[0].TimeSpend() + 100f > 
					(
						TerrainVector(rtsm.nationCenters[rtsm.nationPars[nation].sortedNationNeighbours[0]].transform.position) -
						TerrainVector(rtsm.nationCenters[nation].transform.position)
					).magnitude
				){
					returners[i].CleanWonderers();
					returners.Remove(returners[i]);
				}
			}
			else{
				returners.Remove(returners[i]);
			}
		}
	}
	
	
	
	public int GetIddleGuardCount(){
		int count1 = 0;
		for(int i=0; i<guardsPars.Count; i++){
		    bool pass = true;
			for(int j=0; j<wonderers.Count; j++){
				if(wonderers[j].WondererContains(guardsPars[i])==true){
	// 			if(IsInsideNation(guardsPars[i].transform.position, nation) == true){
				//	count1++;
					pass = false;
				}
 			}
 			for(int j=0; j<returners.Count; j++){
				if(returners[j].WondererContains(guardsPars[i])==true){
			//		count1++;
					pass = false;
				}
 			}
 			if(pass == true){
 				count1++;
 			}
 			
		}
		return count1;
	}
	
	
	
	public void ResetWonderers(){
    	
    	
    	for(int i=0; i<allUnits.Count; i++){
			UnitPars up = allUnits[i];
			
			if(up.nation == nation){
				if(up.militaryMode == 10){
					if(up.isBuilding == false){
						if(up.rtsUnitId != 15){
							if(wonderersByNation[wonderingNation].Count < maxWonderersCounts[wonderingNation]){
								if(
									(up.wonderingMode >= 0)
								){
								// adding new units
									if((! wonderersPars.Contains(up))&&(! guardsPars.Contains(up))){
										if((wonderersPars.Count < guardsPars.Count)&&(guardsPars.Count>10000)){
// 											wonderersPars.Add(up);
// 								
// 											if(up.wonderingMode == 0){
// 												up.wonderingMode = 10;
// 												up.nationToWonder = wonderingNation;
// 											}
// 						
// 											wonderersByNation[wonderingNation].Add(up);
										}
										else{
											guardsPars.Add(up);
											if(up.wonderingMode == 0){
												up.wonderingMode = 110;
											}
										}
									}
									
								}
							}
						}
					}
				}
			}
    			
    	}
    	
    	
    	
    	
    	
    }
    
    
    
    
    public int ExpectedGuardCount(){
    	return (rtsm.nationAIs[nation].GetExpectedMilitariesCount());
    }
    
    
    
//     public void Wonder(int iNat){
//         int poolN = 0;
//         
//          
//         
//     	for(int i=0; i<wonderersByNation[iNat].Count; i++){
//     		float R = (wonderersByNation[iNat][i].transform.position - r1Point).sqrMagnitude;
//     		if(R < (R2+10)*(R2+10)){
//     			poolN = poolN+1;
//     		}
//     	}
//     	
//     	int maxCountChecker = maxWonderersCounts[iNat] - ((int)(0.4f*maxWonderersCounts[iNat]));
//     	if(poolN > maxCountChecker){
//     		
//     		for(int i=0; i<wonderersByNation[iNat].Count; i++){
//     			wonderersByNation[iNat][i].searchDistance = rtsm.nationAIs[iNat].size;
//     		}
//     		
//     	}
//     	else{
// 			for(int i=0; i<wonderersByNation[iNat].Count; i++){
// 			    UnitPars up = wonderersByNation[iNat][i];
// 				if(up.militaryMode == 10){
// 					Vector3 actualPoint = r1Point;
// 					if(rtsm.diplomacy.relations[nation][iNat] == 1){
// 						actualPoint = r2Point;
// 						//rtsm.nationPars[nation].GetSafePointV3(iNat, wonderersPars[i].transform.position);
// 						//r2Point;
// 					}
// 			        if((iNat==0)&&(nation==1)){
// 				//		print(r1Point);
// 					}
// 			
// 					if(up.wonderingMode == 10){
// 					    if(up.wonderingResetCount < Random.Range(6,10)){
// 					    	up.wonderingResetCount = up.wonderingResetCount+1;
// 					    }
// 					    else{
// 					        up.wonderingResetCount = 0;
// 							up.wonderingMode = 20;
// 							up.MoveUnit(actualPoint, "Walk");
// 					//		MoveUnit(up, actualPoint);
// 						}
// 					}
// 					else if(up.wonderingMode == 20){
// 						if(CheckCriticalDistance(up, actualPoint, 8f) == true){
// 							up.StopUnit("Idle");
// 					//		StopUnit(up);
// 						}
// 					}
// 				}
// 			}
//     	}
//     	
//     }
    
    public void Guarding(){
    	for(int i=0; i<guardsPars.Count; i++){
    	    if(guardsPars[i].velocityVector.magnitude > 0.5f*guardsPars[i].thisNMA.speed){
    	    	if(guardsPars[i].rtsUnitId != 15){
    	    		if(guardsPars[i].thisSL.animName == "Idle"){
    	    			guardsPars[i].PlayAnimation("Walk");
    	    		}
    	    	}
    	    }
    	    if(guardsPars[i].velocityVector.magnitude < 0.5f*guardsPars[i].thisNMA.speed){
    	    	if(guardsPars[i].rtsUnitId != 15){
    	    		if(guardsPars[i].thisSL.animName == "Walk"){
    	    			guardsPars[i].PlayAnimation("Idle");
    	    		}
    	    	}
    	    }
    	    if(guardsPars[i].militaryMode == 10){
				if(guardsPars[i].wonderingMode == 110){
					bool pass = false;
					if(guardsPars[i].guardResetCount < 0){
						guardsPars[i].wondererAIdestination = RandomTerrainVector(rtsm.nationCenters[nation].transform.position, rtsm.nationPars[nation].nationSize);
						pass = true;
					}
					else if(guardsPars[i].guardResetCount < Random.Range(6,10)){
		    	        guardsPars[i].guardResetCount = guardsPars[i].guardResetCount + 1;
		    		}
		    		else{
		        		pass = true;
					}
					
// 					if(guardsPars[i].militaryMode != 10){
// 						pass = false;
// 					}
					
					if(pass == true){
						guardsPars[i].guardResetCount = 0;
		        		guardsPars[i].wonderingMode = 120;
		        		guardsPars[i].MoveUnit(guardsPars[i].wondererAIdestination,"Walk");
				//		MoveUnit(guardsPars[i], guardsPars[i].wondererAIdestination);
					}
					
				}
				else if(guardsPars[i].wonderingMode == 120){
					if(CheckCriticalDistance(guardsPars[i], guardsPars[i].wondererAIdestination, 8f) == true){
						guardsPars[i].StopUnit("Idle");
				//		StopUnit(guardsPars[i]);
						guardsPars[i].wonderingMode = 130;
					}
				}
    		}
    	}
    }
    
    
    public void CountOponentsInside(){
        
        for(int i=0;i<rtsm.diplomacy.numberNations;i++){
        	nOponentsInside[i] = 0;
        }
        
    	for(int i=0; i<allUnits.Count; i++){
    		UnitPars up = allUnits[i];
    		
    		int nat = up.nation;
    		if(up.isBuilding == false){
				if(up.rtsUnitId != 15){
					if(IsInsideNation(up.transform.position, nation)==true){
						nOponentsInside[nat] = nOponentsInside[nat] + 1;
					}
				}
			}
    		
    	}
    	
    }
    
    
    
    public Vector3 GetR2Point(int iNat){
    	Vector3 v3 = Vector3.zero;
    	if(R3 > R2){
    		Vector3 dr = rtsm.nationCenters[iNat].transform.position - rtsm.nationCenters[nation].transform.position;
    		float ratio = (R3-R2) / R3;
    		Vector3 dr2 = ratio * dr;
    		
    		v3 = TerrainVector(rtsm.nationCenters[nation].transform.position + dr2);
    	}
    	return v3;
    }
	
	
	
	
	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}
	
	
	
		
	public void RemoveUnit(UnitPars up){
		wonderersPars.Remove(up);
		guardsPars.Remove(up);
		for(int i=0; i<wonderersByNation.Count; i++){
			wonderersByNation[i].Remove(up);
		}
		
		for(int i=0; i<wonderers.Count; i++){
			List<UnitPars> upL = wonderers[i].wonderersPars;
			upL.Remove(up);
		}
		for(int i=0; i<returners.Count; i++){
			List<UnitPars> upL = returners[i].wonderersPars;
			upL.Remove(up);
		}
		
	}
	
	public bool CheckCriticalDistance(UnitPars up, Vector3 dest, float critR){
		bool isInside = false;
		float R = (up.transform.position - dest).sqrMagnitude;
		if(R < critR*critR){
			isInside = true;
		}
		return isInside;
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
	
	public void ResetMaxWonderersCount(){
	    List<UnitPars> militaryUnits = rtsm.nationAIs[nation].militaryUnits;
	    int countInside = 0;
	    int totCount = militaryUnits.Count;
	    for(int i=0; i<militaryUnits.Count; i++){
	    	if(IsInsideNation(militaryUnits[i].transform.position, nation) == true){
	    		countInside = countInside+1;
	    	}
	    }
		maxWonderersCount = (int)((1f*countInside - 0.5f*totCount) * rtsm.nationPars[nation].neighboursDistanceFrac[wonderingNation]);
		
		for(int i=0;i<rtsm.diplomacy.numberNations;i++){
			if(i != nation){
			    maxWonderersCounts[i] = (int)(0.5f*militaryUnits.Count*rtsm.nationPars[nation].neighboursDistanceFrac[i]);
		//		maxWonderersCounts[i] = (int)((1f*countInside - 0.5f*totCount) * rtsm.nationPars[nation].neighboursDistanceFrac[i]);
				if((nation==1)&&(i==2)){
		//			Debug.Log((wonderersByNation[i].Count).ToString()+" "+(maxWonderersCounts[i]).ToString());
				}
			}
		}
		
		
	}
	
	public Vector3 RandomTerrainVector(Vector3 origin, float vSize){	
		Vector2 randCircle = Random.insideUnitCircle * vSize;
		
		
		Vector3 rPos1 = origin + new Vector3(randCircle.x,0f,randCircle.y);
		float y1 = rtsm.manualTerrain.SampleHeight(rPos1);
		Vector3 randomPosition = new Vector3(rPos1.x, y1 ,rPos1.z);
		
		
		return randomPosition;
	
	}
	

	
	
}



public class Wonderer {
	
	public WondererAI wondererAI;
	public List<UnitPars> wonderersPars = new List<UnitPars>();
	
	
	public RTSMaster rtsm;
	
	public float zeroTime = 0f;
	
	public Wonderer(WondererAI wAI){
		wondererAI = wAI;
		rtsm = wondererAI.rtsm;
		zeroTime = Time.time;
	}
	
	
	public void AddUnit(UnitPars up){
		wonderersPars.Add(up);
		up.isWondering1 = 1;
	}
	
	public void RemoveUnit(UnitPars up){
		wonderersPars.Remove(up);
		up.isWondering1 = 0;
	}
	
	
	
	
	
	public IEnumerator Wonder(){
		while(true){
		    
			yield return new WaitForSeconds(Random.Range(0.57f,0.63f));
		}
	}
	
	public void SendUnits(Vector3 pos){
		for(int i=0; i<wonderersPars.Count; i++){
			rtsm.unitsMover.AddMilitaryAvoider(wonderersPars[i], pos, 0);
	//	    wonderersPars.
		}
	}
	
	public void CleanWonderers(){
	    for(int i=0;i<wonderersPars.Count;i++){
	    	wonderersPars[i].isWondering1 = 0;
	    	rtsm.unitsMover.CompleteMovent(wonderersPars[i]);
	    }
		wonderersPars.Clear();
	}
	
	public float TimeSpend(){
		return (Time.time - zeroTime);
	}
	
	public bool WondererContains(UnitPars up){
		return(wonderersPars.Contains(up));
	}
	
	
	
	
	
	
}

