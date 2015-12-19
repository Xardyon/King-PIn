using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NationAI : MonoBehaviour {
    
    public string nationName;
    
    public GameObject model;
    public int maxNumBuildings = 20;
    
    public GameObject testGo = null;
    
    
    private List<int> buildMask = new List<int>();
    
    private List<int> numb = new List<int>();
    private List<int> maxNumb = new List<int>();
    
    private List<int> minMaxNumb = new List<int>();
    private List<int> maxMaxNumb = new List<int>();
    
    private List<int> rtsIds = new List<int>();
    private List<int> rtsRevIds = new List<int>();
    
    [HideInInspector] public float size = 100f;
    private List<float> buildingRadii = new List<float>();
    
    [HideInInspector] public List<UnitPars> spawnedBuildings = new List<UnitPars>();
    
    [HideInInspector] public List<UnitPars> barracks = new List<UnitPars>();
//    [HideInInspector] public List<SpawnPoint> barracksSpawn = new List<SpawnPoint>();

    [HideInInspector] public List<UnitPars> castle = new List<UnitPars>();
//    [HideInInspector] public List<SpawnPoint> castleSpawn = new List<SpawnPoint>();
    
    [HideInInspector] public List<UnitPars> workers = new List<UnitPars>();
    
    [HideInInspector] public List<UnitPars> workersIron = new List<UnitPars>();
    [HideInInspector] public List<UnitPars> workersGold = new List<UnitPars>();
    [HideInInspector] public List<UnitPars> workersLumber = new List<UnitPars>();
    
    public List<int> countAllianceWarning = new List<int>();
    
//     public int nWorkersIron = 3;
//     public int nWorkersGold = 3;
//     public int nWorkersLumber = 3;
    
    [HideInInspector] public List<UnitPars> ironMines = new List<UnitPars>();
    [HideInInspector] public List<UnitPars> goldMines = new List<UnitPars>();
    
    
    public int nIronMines = 0;
    public int nGoldMines = 0;
    
    public int nMaxIronMines = 1;
    public int nMaxGoldMines = 1;
    
    
    
    private SpawnPoint thisSpawnPoint;
    
    [HideInInspector] public RTSMaster rtsm;
    public int nation = 2;
    
    [HideInInspector] public List<UnitPars> militaryUnits = new List<UnitPars>();
    
    [HideInInspector] public List<int> beatenUnits = new List<int>();
    [HideInInspector] public List<int> allianceAcceptanceTimes = new List<int>();
    
    public int masterNationId = -1;
    
    [HideInInspector] public int nEntitiesUnderAttack = 0;
    
//     void Start () {
//     	StartCoroutine(StartDelay());
//     }
//     IEnumerator StartDelay(){
//     	yield return new WaitForSeconds(1f);
//     	Starter();
//     }
    
	void Start () {
	    rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
	    thisSpawnPoint = this.gameObject.GetComponent<SpawnPoint>();
	    
// 	    for(int i=0; i<rtsm.diplomacy.numberNations; i++){
// 	    	beatenUnits.Add(0);
// 	    	allianceAcceptanceTimes.Add(0);
// 	    }
	    
	    
	    for(int i=0; i<rtsm.rtsUnitTypePrefabs.Count; i++){
	    	rtsRevIds.Add(-1);
	    }
	    nIronMines = 0;
	    nGoldMines = 0;
	    
	    
	    rtsIds.Add(0);
	    rtsIds.Add(1);
	    rtsIds.Add(2);
	    rtsIds.Add(3);
	    rtsIds.Add(11);
	    rtsIds.Add(15);
	    
	    rtsRevIds[0] = 0; // castle
	    rtsRevIds[1] = 1; // barracks
	    rtsRevIds[2] = 2; // sawmill
	    rtsRevIds[3] = 3; // farm
	    rtsRevIds[11] = 4; // militaries
	    rtsRevIds[15] = 5; // workers
	    
	    numb.Add(0);
	    numb.Add(0);
	    numb.Add(0);
	    numb.Add(0);
	    numb.Add(0);
	    numb.Add(0);
	    
	    maxNumb.Add(1);
	    maxNumb.Add(1);
	    maxNumb.Add(1);
	    maxNumb.Add(10);
	    maxNumb.Add(30);
	    maxNumb.Add(10);
	    
	    minMaxNumb.Add(1);
	    minMaxNumb.Add(1);
	    minMaxNumb.Add(1);
	    minMaxNumb.Add(10);
	    minMaxNumb.Add(30);
	    minMaxNumb.Add(10);
	    
	    maxMaxNumb.Add(1);
	    maxMaxNumb.Add(1);
	    maxMaxNumb.Add(1);
	    maxMaxNumb.Add(25);
	    maxMaxNumb.Add(45);
	    maxMaxNumb.Add(20);
	    
	    
	    
        for(int i=0;i<4;i++){
            buildingRadii.Add(0f);
        	buildMask.Add(1);
        }
        
        if(nation != rtsm.diplomacy.playerNation){
			StartCoroutine(CastleBuilder());
	//		StartCoroutine(BarracksBuilder());
	//		StartCoroutine(SawmillBuilder());
	//		StartCoroutine(FarmBuilder());
		
			StartCoroutine(CastleControl());
		//	StartCoroutine(CheckBuildings());
			StartCoroutine(SetWorkers());
			StartCoroutine(IronMinesControl());
		//	StartCoroutine(GoldMinesControl());
			StartCoroutine(RecalculateLimits());
			StartCoroutine(TroopsControl());
			StartCoroutine(MilitaryChecks());
		}
		else{
			SetPlayerCamera();
		}
	}




	public void FillLists(){
		beatenUnits.Clear();
		allianceAcceptanceTimes.Clear();
		countAllianceWarning.Clear();
		for(int i=0; i<rtsm.diplomacy.numberNations; i++){
	    	beatenUnits.Add(0);
	    	allianceAcceptanceTimes.Add(0);
	    	countAllianceWarning.Add(0);
	    }
	}	
	
	
	
	public IEnumerator CastleBuilder(){
		yield return new WaitForSeconds(0.5f);
		while(true){
		
			int ii = builderDecider();
			
			if(ii >= 0){
				SetBuilding(ii);
			}
			else{
				yield return new WaitForSeconds(1f);
			}
			yield return new WaitForSeconds(Random.Range(10f,20f));
			
			
		}
	
	}
	
	
	
	
	public int builderDecider(){
		int buildingId = -1;
		
		if(castle.Count == 0){
			buildingId = 0;
		}
		else if(numb[3] ==0){
			buildingId = 3;
		}
		else if(numb[1] ==0){
			buildingId = 1;
		}
		else if(numb[2] ==0){
			buildingId = 2;
		}
		else{
			List<int> allowedModes = new List<int>();
			if(numb[1] < maxNumb[1]){
				allowedModes.Add(1);
			}
			if(numb[2] < maxNumb[2]){
				allowedModes.Add(2);
			}
		//	if(numb[3] < maxNumb[3]){
		//	if(numb[3] < maxNumb[3]){
				allowedModes.Add(3);
		//	}
			
			if(allowedModes.Count > 0){
				int chosenMode = Random.Range(0,allowedModes.Count);
				buildingId = allowedModes[chosenMode];
			}
			
		}
		
	// if castle is not completed	
		if(buildingId > 0){
			if(castle.Count > 0){
				if(castle[0].isBuildFinished == false){
					buildingId = -1;
				}
			}
		}
		
		return buildingId;
	}

	
	
	public IEnumerator CastleControl(){
		yield return new WaitForSeconds(0.5f);
		while(true){
			if(numb[0] > 0){
					if(numb[5]<maxNumb[5]){
					    if(castle.Count > 0){
						//	if(castle[0].thisSpawn.isLocked == true){
							if(castle[0].thisSpawn.isSpawning == false){
								
									castle[0].thisSpawn.count = 0;
									castle[0].thisSpawn.numberOfObjects = 1;
						//			castle[0].thisSpawn.isLocked = false;
									castle[0].thisSpawn.StartSpawning();
						//			innerLock = true;
									numb[5] = numb[5]+1;
									
									
								
							}
						}
					}
					
			}
			
			yield return new WaitForSeconds(0.2f);
		}
	
	}
	
	
	
	
	public IEnumerator TroopsControl(){
		yield return new WaitForSeconds(0.5f);
		while(true){
			
			if(numb[1] > 0){
				if(numb[4]<maxNumb[4]){
					if(barracks.Count > 0){
						for(int i=0; i<barracks.Count; i++){
							if(barracks[i].thisSpawn.isSpawning == false){
							//		print(maxNumb[4]);
									int randMilitaryUnit = Random.Range(11,13);
									barracks[i].thisSpawn.model = rtsm.rtsUnitTypePrefabs[randMilitaryUnit];
									barracks[i].thisSpawn.count = 0;
									barracks[i].thisSpawn.numberOfObjects = 1;
									barracks[i].thisSpawn.StartSpawning();
								//	innerLock = true;
									numb[4] = numb[4]+1;
							
							}
						}
					}
				}
				
			}
			
			yield return new WaitForSeconds(0.1f);
		}
	
	}
	
	
	
	
	
	
	
	public IEnumerator IronMinesControl(){
		 yield return new WaitForSeconds(0.5f);
		 while(true){
		 	int mineMode = Random.Range(0,2);
		 	if(CreateMine(mineMode)==true){
		 		yield return new WaitForSeconds(Random.Range(1f,3f));
		 	}
		 	yield return new WaitForSeconds(0.2f);
	//	 	yield return new WaitForSeconds(Random.Range(1f,3f));
		 }
	
	}
	
	
	
	public bool CreateMine(int mineMode){ // 0 for iron, 1 for gold
		//		 	int mineMode = 0; 
		 	
		bool locationFound = false;
		Vector3 candidateLocation = Vector3.zero;
		if(castle.Count > 0){
			int nMines = 0;
			int nMaxMines = 0;
			KDTree minesKD = null;
			List <Vector3> minesLoc = null;
			
			if(mineMode == 0){
				nMines = nIronMines;
				nMaxMines = nMaxIronMines;
				minesKD = rtsm.resourcePoint.kd_ironLocations;
				minesLoc = rtsm.resourcePoint.ironLocations;
			}
			else{
				nMines = nGoldMines;
				nMaxMines = nMaxGoldMines;
				minesKD = rtsm.resourcePoint.kd_goldLocations;
				minesLoc = rtsm.resourcePoint.goldLocations;
			}
			
			if(nMines < nMaxMines){
				int i = minesKD.FindNearestK(castle[0].transform.position, Random.Range(1,3));
				Vector3 pointLocation = minesLoc[i];
				float Rsq = (pointLocation-castle[0].transform.position).sqrMagnitude;
				if(Rsq < 8f*size*size){
					float x = Random.Range(pointLocation.x-7f, pointLocation.x+7f);
					float z = Random.Range(pointLocation.z-7f, pointLocation.z+7f);
					candidateLocation = new Vector3(x, rtsm.manualTerrain.SampleHeight(new Vector3(x,0f,z)), z);
					if((candidateLocation-pointLocation).sqrMagnitude < 49f){
						if(GetNeighbourDist(candidateLocation)>30f){
							if(rtsm.terrainProperties.TerrainSteepness(candidateLocation, 7f) < 30f){
						//		GameObject minePrefab = rtsm.rtsUnitTypePrefabs[5];
								if(thisSpawnPoint.isSpawning == false){
									locationFound = true;
								}
							}
						}
						
					}
				}
			}
		}
		if(locationFound == true){
			if(castle.Count > 0){
				if(castle[0].isBuildFinished == true){
					float randAngle = Random.Range(0f, 360f);
					Quaternion randomRotation = Quaternion.Euler( 0f, randAngle , 0f);
	

					thisSpawnPoint.model = rtsm.rtsUnitTypePrefabs[5];
					thisSpawnPoint.isManualPosition = true;
					thisSpawnPoint.manualPosition.Add(candidateLocation);
					thisSpawnPoint.manualRotation.Add(randomRotation);
					thisSpawnPoint.numberOfObjects = 1;
					thisSpawnPoint.StartSpawning();
			
					if(mineMode == 0){
						nIronMines = nIronMines+1;
					}
					else{
						nGoldMines = nGoldMines+1;
					}
				}
			}
		}
		if(locationFound == true){
			if(castle.Count > 0){
				if(castle[0].isBuildFinished == false){
					locationFound = false;
				}
			}
		}
		
		return locationFound;
	}
	
	
	
	
	
	
	
	
	public float GetNeighbourDist(Vector3 query){
		float R = 1000000f;
		for(int j=0; j<rtsm.diplomacy.numberNations; j++){
			if(rtsm.nationAIs[j] != null){
				for(int i=0; i<rtsm.nationAIs[j].spawnedBuildings.Count; i++){
					Vector3 target = rtsm.nationAIs[j].spawnedBuildings[i].transform.position;
					float Rcand = (query-target).sqrMagnitude;
					if(Rcand<R){
						R = Rcand;
					}
				}
			}
		}
		return (Mathf.Sqrt(R));
	}
	
	
	public IEnumerator SetWorkers(){
	    yield return new WaitForSeconds(0.5f);
	    while(true){
	        
	        if(workers.Count>0){
	      //  	int mod = 0;
	      //  	int count = -1;
	        	
	        	List<int> wTypes = new List<int>();
	        	List<int> wTypesIndex = new List<int>();
	        	
	        	if(ironMines.Count>0){
	        		wTypes.Add(workersIron.Count);
	        		wTypesIndex.Add(0);
	        	}
	        	if(goldMines.Count>0){
	        		wTypes.Add(workersGold.Count);
	        		wTypesIndex.Add(1);
	        	}
	        	if(numb[2]>0){
	        		wTypes.Add(workersLumber.Count);
	        		wTypesIndex.Add(2);
	        	}
	        	
	        	if(wTypes.Count > 0){
					int mod = wTypes.IndexOf(wTypes.Min());
					mod = wTypesIndex[mod];
				
					if(mod == 0){
						UnitPars worker = workers[0];
						workersIron.Add(worker);
						workers.Remove(worker);
					
						rtsm.resourcesCollection[nation].SetAutoMiner(worker, 0);
					}
					else if(mod == 1){
						UnitPars worker = workers[0];
						workersGold.Add(worker);
						workers.Remove(worker);
					
						rtsm.resourcesCollection[nation].SetAutoMiner(worker, 1);
					}
					else if(mod == 2){
						UnitPars worker = workers[0];
						workersLumber.Add(worker);
						workers.Remove(worker);
					
						rtsm.resourcesCollection[nation].SetAutoChopper(worker);
					}
	        	}
	        	
	        }
	        
		
			yield return new WaitForSeconds(2f);
		}
	}
	
	
	
	public void SetUnit(UnitPars goPars){
	
//		UnitPars goPars = go.GetComponent<UnitPars>();
		if(goPars.isBuilding == true){
			spawnedBuildings.Add(goPars);
		}
		if(goPars.rtsUnitId == 1){
			barracks.Add(goPars);
	//		barracksSpawn.Add(go.GetComponent<SpawnPoint>());
		}
		if(goPars.rtsUnitId == 0){
			castle.Add(goPars);
	//		castleSpawn.Add(go.GetComponent<SpawnPoint>());
		}
		if((goPars.rtsUnitId>=11)&&(goPars.rtsUnitId<=14)){
			militaryUnits.Add(goPars);
		}
		if(goPars.rtsUnitId == 15){
			workers.Add(goPars);
		}
		if(goPars.rtsUnitId == 5){
		    StartCoroutine(ResourceTypeDelayMine(goPars));
		}
	    
	}
	
	public void UnsetUnit(UnitPars goPars){
		int id = goPars.rtsUnitId;
		if(id != -1){
			int idThis = rtsRevIds[id];
			if((id >= 11) && (id <= 15)){
				idThis = rtsRevIds[11];
			}
			if(goPars.isDying != true){
				spawnedBuildings.Remove(goPars);
			}
			if(id == 1){
				barracks.Remove(goPars);
			}
			if(id == 0){
				castle.Remove(goPars);
			}
			if((id>=11)&&(id<=14)){
				militaryUnits.Remove(goPars);
			}
			
			if(id == 15){
				workers.Remove(goPars);
				workersIron.Remove(goPars);
				workersGold.Remove(goPars);
				workersLumber.Remove(goPars);
			}
			if(id == 5){
				if(goPars.resourceType == 0){
					ironMines.Remove(goPars);
					StartCoroutine(UnsetDelayMine(0));
				}
				else if(goPars.resourceType == 1){
					goldMines.Remove(goPars);
					StartCoroutine(UnsetDelayMine(1));
				}
			}
			
			if(id != 5){
				StartCoroutine(UnsetDelay(idThis));
			}
		}
	}
	
	
	
	
	public IEnumerator UnsetDelay(int id){
		yield return new WaitForSeconds(10f);
		if((id>=0)&&(id<numb.Count)){
			numb[id] = numb[id] - 1;
			if(id == 0){
				numb[id] = castle.Count;
			}
			if(id == 1){
				numb[id] = barracks.Count;
			}
			if(id == 4){
				numb[id] = militaryUnits.Count;
			}
		}
		
	}
	public IEnumerator UnsetDelayMine(int id){
		yield return new WaitForSeconds(10f);
		if(id == 0){
			nIronMines = nIronMines - 1;
		}
		if(id == 1){
			nGoldMines = nGoldMines - 1;
		}
		
	}
	
	public IEnumerator ResourceTypeDelayMine(UnitPars goPars){
		yield return new WaitForSeconds(3f);
		
		if(goPars.resourceType == 0){
			ironMines.Add(goPars);
		}
		else if(goPars.resourceType == 1){
			goldMines.Add(goPars);
		}
		
		
	}
	
	public IEnumerator RecalculateLimits(){
		while(true){
		    if(rtsm.economy.iron.Count == rtsm.diplomacy.numberNations){
		        
				int minRes = rtsm.economy.iron[nation];
				if(rtsm.economy.gold[nation] < minRes){
					minRes = rtsm.economy.gold[nation];
				}
				if(rtsm.economy.lumber[nation] < minRes){
					minRes = rtsm.economy.lumber[nation];
				}
			    
			    
			  
			    
			// units
			    maxNumb[4] = (int) (Mathf.Pow((numb[3]*1f),1.5f));
			    if(maxNumb[4] > 2000){
			    	maxNumb[4] = 2000;
			    }
// 			    if(nation==4){
// 			    	print(maxNumb[4]);
// 			    }
// 			    
            // sawmills
				maxNumb[2] = (int) (numb[3]*1f/12f + 1);
				if(maxNumb[2] > 5){
			    	maxNumb[2] = 5;
			    }
				

			// farms    
			    maxNumb[3] = GetMaxNumb(1, 200, minRes, 1, 100);
			// workers	
			    maxNumb[5] = numb[3];
			    if(maxNumb[5] > 20){
			    	maxNumb[5] = 20;
			    }
			// barracks  
			    maxNumb[1] = (int) (numb[3]*1f/20f + 1);
				
				
			// nation size    
			    float predictedSize = Mathf.Pow((1f*(minRes+1000)),0.5f)+Mathf.Pow((20f*numb[3]),0.5f);
			//    float predictedSize = Mathf.Pow((20f*numb[3]),0.5f)+40f;
				if(predictedSize < 40f){
					size = 40f;
				}
				else if(predictedSize > 150f){
					size = 150f;
				}
				else{
					size = predictedSize;
				}
				rtsm.nationPars[nation].nationSize = size;
				rtsm.nationPars[nation].RefreshStaticPars();
				
				
				buildingRadii[0] = 40f;
				buildingRadii[1] = 0.2f*size;
				if(buildingRadii[1]<40f){
					buildingRadii[1] = 40f;
				}
				buildingRadii[2] = size;
				buildingRadii[3] = 2f*size;
				
				
			}
			yield return new WaitForSeconds(3f);
		}
	}
	
	public int GetExpectedMilitariesCount(){
	    if(maxNumb.Count < 4){
	    	return(1);
	    }
	    else{
			return(maxNumb[4]);
		}
	}
	
	public int GetMaxNumb(int value, int tot, int c1, int limMin, int limMax){
		int maxNumb = 0;
		if(((int)(1f*c1*value/(1f*tot)))<limMin){
			maxNumb = limMin;
		}
		else if(((int)(1f*c1*value/(1f*tot)))>limMax){
			maxNumb = limMax;
		}
		else{
			maxNumb = (int)(1f*c1*value/(1f*tot));
		}
		
		return maxNumb;
		
	}
	

    




	public IEnumerator MilitaryChecks(){
		while(true){
		    MilitaryChecksF();
			yield return new WaitForSeconds(1f);
		}
	}	
	
	public void MilitaryChecksF(){
		
	//	    if(militaryUnits.Count>5){
		// threat of growing nation	
		        int NN = rtsm.diplomacy.numberNations;
				for(int i=0; i<NN; i++){
					if(
						(i != nation) //&&
					//	(rtsm.nationAIs[i]!=null)
					){
					
							
					// war conditions	
						if(rtsm.diplomacy.relations[nation][i]==0){
							float R = (rtsm.nationCenters[i].transform.position-rtsm.nationCenters[nation].transform.position).magnitude;
							if(R < (size+rtsm.nationAIs[i].size)){
								if(rtsm.nationAIs[i].militaryUnits.Count < militaryUnits.Count){
									if(rtsm.nationAIs[i].militaryUnits.Count > 25){
										rtsm.diplomacy.SetRelation(nation, i, 1);
										if((nation!=rtsm.diplomacy.playerNation)&&(i==rtsm.diplomacy.playerNation)){
											rtsm.diplomacyMenu.ActivateWarOffer(nation);
										}
									}
								}
							}
							
							
							if(rtsm.wondererAIs[nation].nOponentsInside[i] > 6){
						//	if(nOponentUnits > 6){
							//	if(rtsm.nationAIs[i].militaryUnits.Count < militaryUnits.Count){
									if(militaryUnits.Count > 15){
										rtsm.diplomacy.SetRelation(nation, i, 1);
										if((nation!=rtsm.diplomacy.playerNation)&&(i==rtsm.diplomacy.playerNation)){
											rtsm.diplomacyMenu.ActivateWarOffer(nation);
										}
									}
							//	}
							}
							
							
							
						}
					
					// war change checks
						else if(rtsm.diplomacy.relations[nation][i]==1){
						// alliance condition
							if(i<=-1){
								print(i);
							}
							if(i>=rtsm.scores.nUnits.Count){
								print(i);
							}
							if(nation<=-1){
								print(nation);
							}
							if(nation>=rtsm.scores.nUnits.Count){
								print(nation);
							}
						
							if( 
								(rtsm.nationAIs[i].beatenUnits[nation] > rtsm.scores.nUnits[nation]*1f/(allianceAcceptanceTimes[i]+1)) &&
								(rtsm.scores.nUnits[i] > 40) &&
								(rtsm.wondererAIs[nation].nOponentsInside[i] < 10)
							  ){
									if(i != rtsm.diplomacy.playerNation){
										rtsm.diplomacy.SetRelation(nation, i, 4);
							
										rtsm.nationAIs[i].beatenUnits[nation] = 0;
										beatenUnits[i] = 0;
							
										allianceAcceptanceTimes[i] = allianceAcceptanceTimes[i]+1;
										rtsm.nationAIs[i].allianceAcceptanceTimes[nation] = rtsm.nationAIs[i].allianceAcceptanceTimes[nation] + 1;
									}
									else{
										rtsm.diplomacyMenu.ActivateAllianceOffer(nation);
									}
							
							
							}	

						
						// slavery conditions	
						    else if((rtsm.wondererAIs[nation].nOponentsInside[i] > 15)&&(militaryUnits.Count < 6)){	
					//		else if(rtsm.nationAIs[i].militaryUnits.Count > 2f*militaryUnits.Count){
								if(i != rtsm.diplomacy.playerNation){
									rtsm.diplomacy.SetRelation(nation, i, 2);
								}
								else{
									rtsm.diplomacyMenu.ActivateMercyOffer(nation);
								}
// 								if(masterNationId > -1){
// 									rtsm.diplomacy.SetRelation(nation, masterNationId, 0);
// 								}
// 								masterNationId = i;
								rtsm.nationAIs[i].beatenUnits[nation] = 0;
								beatenUnits[i] = 0;
								
							
							}
						
						}
				// leaving slavery
						else if(rtsm.diplomacy.relations[nation][i]==2){
						    if(rtsm.nationAIs[i].militaryUnits.Count > 10){
					//		if(rtsm.nationAIs[i].militaryUnits.Count < 1.5f*militaryUnits.Count){
								rtsm.diplomacy.SetRelation(nation, i, 0);
								masterNationId = -1;
					//			rtsm.diplomacy.LeaveSlaveryStraight(nation, i);
							}
							if(i == rtsm.diplomacy.playerNation){
								rtsm.diplomacyMenu.ActivateEscapeSlaveryOffer(nation);
								rtsm.diplomacy.SetRelation(nation, i, 0);
							}
						}
				// leaving alliance if in 40 seconds other nation does not take back its units		
					    else if(rtsm.diplomacy.relations[nation][i]==4){
					    	if(rtsm.wondererAIs[nation].nOponentsInside[i] > 15){
					    	    if(militaryUnits.Count > 15){
									countAllianceWarning[i] = countAllianceWarning[i] + 1;
									if(countAllianceWarning[i] > 40){
										countAllianceWarning[i] = 0;
										rtsm.diplomacy.SetRelation(nation, i, 0);
									}
					    		}
					    	}
					    }
					
						
						
						
					}
				}
	//		}
		
	
	}
	
	
	
	
	
	public void SetBuilding(int id){
//	    bool isSet = false;
		Vector3 randomPosition = RandomTerrainVector(transform.position, buildingRadii[id]);
		bool positionFound = false;
		
		if(thisSpawnPoint.isSpawning == false){
			float neighDist = GetNeighbourDist(randomPosition);
			if(
				((neighDist>30f)&&(id != 3))||
				((neighDist>30f)&&(neighDist<36f)&&(id == 3))
			
			){
	//			if(rtsm.createForest.kd_treePositions != null){
				int itr = rtsm.createForest.kd_treePositions.FindNearest(randomPosition);
				
				if((itr>=0)&&(itr < rtsm.createForest.treePositions.Count)){
				
					if(
						(randomPosition-rtsm.createForest.treePositions[
							itr
						]).sqrMagnitude > 30f*30f
					){
						if(
							(randomPosition-rtsm.resourcePoint.allResLocations[rtsm.resourcePoint.kd_allResLocations.FindNearest(randomPosition)]).sqrMagnitude
							>
							30f*30f*1.2f
						
						){
						
							if(rtsm.terrainProperties.TerrainSteepness(randomPosition, 30f) < 30f){
							//	GameObject castleModel = rtsm.rtsUnitTypePrefabs[rtsIds[0]];
							//	positionFound = true;
			
								
							}
					//		isSet = true;
						}
			
					}
				}
	//			}
			}
		}
		
		FindPosition(ref randomPosition, ref positionFound, id, 30);
		
		
		
	//	if(rtsm.rtsUnitTypePrefabs[rtsIds[id]].GetComponent<UnitPars>().isEnoughResources(rtsm.economy,nation) == false){
	//    Debug.Log(nation);
		if(rtsm.buildMark.isEnoughResources(nation, rtsm.rtsUnitTypePrefabs[rtsIds[id]].GetComponent<UnitPars>()) == false){
			positionFound = false;
		}
		
		
		
		if(positionFound == true){
			float randAngle = Random.Range(0f, 360f);
			Quaternion randomRotation = Quaternion.Euler( 0f, randAngle , 0f);


			thisSpawnPoint.model = rtsm.rtsUnitTypePrefabs[rtsIds[id]];
			thisSpawnPoint.isManualPosition = true;
			thisSpawnPoint.manualPosition.Add(randomPosition);
			thisSpawnPoint.manualRotation.Add(randomRotation);
			thisSpawnPoint.numberOfObjects = 1;
			thisSpawnPoint.StartSpawning();
			
			numb[id] = numb[id]+1;
	
			if(id==0){
				rtsm.nationCenters[nation].transform.position = randomPosition;
				for(int i=0; i<rtsm.diplomacy.numberNations; i++){
					rtsm.nationPars[i].RefreshDistances();
				}
			}
		}
		
	//	return positionFound;	
	//	return isSet;	
	}
	
	
	
	
	public void FindPosition(ref Vector3 position, ref bool isFound, int id, int nIterations){
		int i=0;
		while((isFound == false) && (i<nIterations)){
		
			i++;
		
			Vector3 randomPosition = RandomTerrainVector(transform.position, buildingRadii[id]);
	//		bool positionFound = false;
		
			if(thisSpawnPoint.isSpawning == false){
				float neighDist = GetNeighbourDist(randomPosition);
				if(
					((neighDist>30f)&&(id != 3))||
					((neighDist>30f)&&(neighDist<36f)&&(id == 3))
			
				){
					int itr = rtsm.createForest.kd_treePositions.FindNearest(randomPosition);
				
					if((itr>=0)&&(itr < rtsm.createForest.treePositions.Count)){
				
						if(
							(randomPosition-rtsm.createForest.treePositions[
								itr
							]).sqrMagnitude > 30f*30f
						){
							if(
								(randomPosition-rtsm.resourcePoint.allResLocations[rtsm.resourcePoint.kd_allResLocations.FindNearest(randomPosition)]).sqrMagnitude
								>
								30f*30f*1.2f
						
							){
						
								if(rtsm.terrainProperties.TerrainSteepness(randomPosition, 30f) < 30f){
								//	GameObject castleModel = rtsm.rtsUnitTypePrefabs[rtsIds[0]];
								//	positionFound = true;
									isFound = true;
									position = randomPosition;
								
								}
						//		isSet = true;
							}
			
						}
					}
				
				}
			}
		}
// 		if(positionFound == true){
// 			
// 		}
		
	}
	
	
	
	public Vector3 RandomTerrainVectorB(Vector3 origin, float vSize){
		float rand1 = Random.Range(-vSize,vSize);
		float rand2 = Random.Range(-vSize,vSize);
		
		Vector3 rPos1 = origin + new Vector3(rand1,0f,rand2);
		float y1 = rtsm.manualTerrain.SampleHeight(rPos1);
		
		Vector3 randomPosition = new Vector3(rPos1.x, y1 ,rPos1.z);
		
		return randomPosition;
	}
	
	public Vector3 RandomTerrainVector(Vector3 origin, float vSize){	
		Vector2 randCircle = Random.insideUnitCircle * vSize;
		
		
		Vector3 rPos1 = origin + new Vector3(randCircle.x,0f,randCircle.y);
		float y1 = rtsm.manualTerrain.SampleHeight(rPos1);
		Vector3 randomPosition = new Vector3(rPos1.x, y1 ,rPos1.z);
		
		
		return randomPosition;
	
	}
	
	
	
	
	
	
	public void SetPlayerCamera(){
	    
		Transform camTransform = Camera.main.transform;
		CameraLooker cl = new CameraLooker();
		cl.LookAtTransform(camTransform, rtsm.nationCenters[rtsm.diplomacy.playerNation].transform.position, 80f, 0f, -25f);
//		camTransform.position = (rtsm.nationCenters[rtsm.diplomacy.playerNation].transform.position + new Vector3(-90f,40f,0f));
	}
	
	
	
}
