using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour {

	public GameObject model;
	private UnitPars modelPars;

	public bool readynow = true;
	public float timestep = 0.01f;
	public int count = 0;
	public int numberOfObjects = 100;
	
	public int formationSize = 1;
	
	public float size = 1.0f;

	private float timeStart_loc = 0f;
	private float timeStart_gl = 0f;

	public float progress_loc = 0f;
	public float progress_gl = 0f;

	private bool isUpdateTimerRunning = false;

	public bool enoughResources = true;

	public int nation = 0;


	public bool addToBS = true;

	//public bool isLocked = false;
	public bool isSpawning = false;


	public bool useAutoRespawn = false;
	public int autoRespawnThreshold = 50;

	private Scores sc = null;
	private Diplomacy dip = null;
	private Economy ec;

	[HideInInspector] public RTSMaster rtsm = null;

	[HideInInspector] public bool isManualPosition = false;
	[HideInInspector] public List<Vector3> manualPosition = new List <Vector3>();
	[HideInInspector] public List<Quaternion> manualRotation = new List <Quaternion>();
	public UnitPars thisPars;
	[HideInInspector] public BattleSystem bsScript;
	[HideInInspector] public NationAI thisNationAI;

	//private IEnumerator spawner1;


	void Start () {
		rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
		bsScript = rtsm.battleSystem;
		sc = rtsm.scores;
		dip = rtsm.diplomacy;
		ec = rtsm.economy;
	
		if(rtsm.nationCenters[nation].GetComponent<NationAI>() != null){
			thisNationAI = rtsm.nationCenters[nation].GetComponent<NationAI>();
		}

		thisPars = this.gameObject.GetComponent<UnitPars>();
		RefreshTimeStep();
	
	//    spawner1 = Spawner();
	
		StartCoroutine(Spawner());
	}

 
	public IEnumerator Spawner(){
	 
		 isSpawning = true;
	 
		 while(ec == null){
			if(ec == null){
				yield return new WaitForSeconds(0.2f);
			}
		 }
	 
	 
	     
	 
 
 
		 count = 0;
	 	 
	 	 modelPars = model.GetComponent<UnitPars>();
	 	 
		 int costIron = modelPars.costIron;
		 int costGold = modelPars.costGold;
		 int costLumber = modelPars.costLumber;
		 int costPopulation = modelPars.costPopulation;
	 
		 if(isUpdateTimerRunning == false){
			StartCoroutine(UpdateTimer());
		 }
	 
		 timeStart_gl = Time.time;
		 
	 	 int nSplits = 1;
	 	 if(modelPars.isBuilding == false){
	 	 	nSplits = formationSize;
	 	 }
	 	 int nIter = (int)(1f*numberOfObjects / nSplits);
	 	 
	 
		 for(int i=0; i<nIter; i++){
			if(
				(ec.iron[nation] - costIron*nSplits >= 0) &&
				(ec.gold[nation] - costGold*nSplits >= 0) &&
				(ec.lumber[nation] - costLumber*nSplits >= 0) &&
				(ec.population[nation] - costPopulation*nSplits >= 0) &&
				((thisPars == null)||(thisPars.isBuildFinished == true))
			){
				timeStart_loc = Time.time;
				enoughResources = true;
			
				timestep = RefreshTimeStep();
				
				List<UnitPars> formUnits = new List<UnitPars>();
			
				yield return new WaitForSeconds(timestep*nSplits);
				
				for(int j=0; j<nSplits; j++){
					ec.iron[nation] = ec.iron[nation] - costIron;
					ec.gold[nation] = ec.gold[nation] - costGold;
					ec.lumber[nation] = ec.lumber[nation] - costLumber;
					ec.population[nation] = ec.population[nation] - costPopulation;
			
					if(nation==dip.playerNation){
						ec.textIron.text = (ec.iron[dip.playerNation]).ToString();
						ec.textGold.text = (ec.gold[dip.playerNation]).ToString();
						ec.textLumber.text = (ec.lumber[dip.playerNation]).ToString();
						ec.textPopulation.text = (ec.population[dip.playerNation]).ToString();
					}
			
					readynow=false;
			

					float rand1 = Random.Range(-size,size);
					float rand2 = Random.Range(-size,size);


					Vector3 randomPosition = Vector3.zero;


					randomPosition = transform.localPosition + (Quaternion.Euler(0, -90, 0) * transform.forward * 7) + new Vector3(rand1, 0f, rand2);    
					float x = randomPosition.x;
					float z = randomPosition.z;
					randomPosition = new Vector3(x, rtsm.manualTerrain.SampleHeight(new Vector3(x,0f,z)), z);
			
					float randAngle = Random.Range(0f, 360f);
					Quaternion randomRotation = Quaternion.Euler( 0f, randAngle , 0f);
			
					GameObject go = null;
					if(isManualPosition == false){
						go = (GameObject)Instantiate(model, TerrainVector(randomPosition), randomRotation);
				
					
				
					}
					else{
						Vector3 pos = manualPosition[0];
						randAngle = manualRotation[0].eulerAngles.y;
						go = (GameObject)Instantiate(model, TerrainVector(pos), manualRotation[0]);
				
						manualPosition.RemoveAt(0);
						manualRotation.RemoveAt(0);
				
						if(manualPosition.Count<1){
							isManualPosition = false;
						}
					
				
					}
					UnitPars goPars = null;

					SpriteLoader sl = null;
			
			
			
					if(go.GetComponent<SpriteLoader>() != null){
						sl = go.GetComponent<SpriteLoader>();

						go.transform.position = go.transform.position - new Vector3(0f,0.5f*sl.spriteSize,0f);
					}

					if(go.GetComponent<UnitPars>() != null){
			
						goPars = go.GetComponent<UnitPars>();
				
						if(go.GetComponent<NavMeshAgent>() != null){
							goPars.thisNMA = go.GetComponent<NavMeshAgent>();
							if(goPars.isBuilding == false){
								goPars.thisNMA.enabled = !rtsm.agentMover.useManualAgentsSystem;
							}
						}
						if(go.GetComponent<NavMeshObstacle>() != null){
							goPars.thisNMO = go.GetComponent<NavMeshObstacle>();
						}
						if(go.GetComponent<SpriteLoader>() != null){
							SpriteLoader sl1 = go.GetComponent<SpriteLoader>();
							goPars.thisSL = sl1;
							sl1.spriteUP = goPars;
					
						}
						if(go.GetComponent<Animation>() != null){
							goPars.thisAnim = go.GetComponent<Animation>();
						}
						if(go.GetComponent<SpawnPoint>() != null){
							goPars.thisSpawn = go.GetComponent<SpawnPoint>();
						}
				
				
						goPars.militaryMode = 10;
						goPars.rEnclosed = go.GetComponent<MeshRenderer>().GetComponent<Renderer>().bounds.extents.magnitude;
						if(goPars.isBuilding == true){
							goPars.rEnclosed = 0.6f*goPars.rEnclosed;
							goPars.GrowBuilding();
						}
						goPars.nation = nation;
					}

					if(go.GetComponent<SpawnPoint>() != null){
						go.GetComponent<SpawnPoint>().nation = nation;
					}

					if(goPars != null){
						bsScript.AddSelfHealer(goPars);

						bsScript.unitsBuffer.Add(goPars);

						if(goPars.isBuilding == false){
							goPars.isBuildFinished = true;
							sc.nUnits[nation] = sc.nUnits[nation]+1;
							if(nation == rtsm.diplomacy.playerNation){
								rtsm.scores.AddToMasterScoreDiff(0.1f);
							}
					
							goPars.transform.position = TerrainVector(goPars.transform.position);
							
							if(formationSize > 1){
								formUnits.Add(goPars);
							}
						}
						else{
							sc.nBuildings[nation] = sc.nBuildings[nation]+1;
							if(nation == rtsm.diplomacy.playerNation){
								rtsm.scores.AddToMasterScoreDiff(1f);
							}
						}
				
				
						if(thisNationAI != null){
							thisNationAI.SetUnit(goPars);
						}
				
						rtsm.numberOfUnitTypes[nation][goPars.rtsUnitId] = rtsm.numberOfUnitTypes[nation][goPars.rtsUnitId] + 1;
						rtsm.resourcesCollection[nation].AddToResourcesCollection(go);
						rtsm.unitsListByType[goPars.rtsUnitId].Add(goPars);
				
						if(goPars.rtsUnitId == 15){
							NavMeshAgent nav = go.GetComponent<NavMeshAgent>();
							nav.radius = nav.radius+0.05f*nav.radius*Random.Range(-1f,1f);
							nav.speed = nav.speed+0.05f*nav.speed*Random.Range(-1f,1f);
						}
				
				
				
					}
			
			


					readynow=true;
					count = count+1;
			
					if(rtsm.buildProgressNum.buildProgressNum_text.buttonGo.activeSelf == true){
						if(thisPars != null){
							if(thisPars.isSelected == true){
								rtsm.buildProgressNum.UpdateText(GetProgressString());
							}
						}
					}
					
					
				}
				if(formUnits.Count > 0){
					rtsm.formations.CreateNewStrictFormation(formUnits);
// 					Formation form = new Formation();
// 					form.rtsm = rtsm;
// 					rtsm.formations.AddUnitsToFormations(formUnits,form,true);
// 			
// 					form.GetDestinations(formUnits, form.CurrentMassCentre(formUnits));
				}
			}
			else{
				enoughResources = false;
				timeStart_gl = timeStart_gl+0.2f;
				yield return new WaitForSeconds(0.2f);
			}


		 }
	 
		 isUpdateTimerRunning = false;
		 if(rtsm.buildProgressNum.buildProgressNum_text.buttonGo.activeSelf == true){
			if(thisPars != null){
				if(thisPars.isSelected == true){
					rtsm.cancelSpawnButton.DeActivate();
					rtsm.buildingButtons.buildingButtonGrid[thisPars.rtsUnitId].ActivateAll();
				}
			}
		 }
			 
	
		 isSpawning = false;


	}



	public IEnumerator UpdateTimer(){
		isUpdateTimerRunning = true;
		while(isUpdateTimerRunning == true){
			if(modelPars != null){
				int nSplits = 1;
				if(modelPars.isBuilding == false){
					nSplits = formationSize;
				}
//				int nIter = (int)(1f*numberOfObjects / nSplits);
				
				
				progress_loc = (Time.time - timeStart_loc) / (timestep*nSplits);
				if(progress_loc > 1f){
					progress_loc = 1f;
				}
			
			
				progress_gl = (1f*count + progress_loc*nSplits)/numberOfObjects;
				
		        if(rtsm.buildProgressNum.buildProgressNum_text.buttonGo.activeSelf == true){
					if(thisPars != null){
						if(thisPars.isSelected == true){
							rtsm.buildProgressNum.UpdateText(GetProgressString());
						}
					}
				}
		
			}
			yield return new WaitForSeconds(0.02f);
		
		}
		isUpdateTimerRunning = false;
	}


	public string GetProgressString(){
		int nSplits = 1;
		if(modelPars != null){
			if(modelPars.isBuilding == false){
				nSplits = formationSize;
			}
		}
		
		return ((count+nSplits).ToString()+"/"+numberOfObjects.ToString());
	}





	Vector3 RotAround(float rotAngle, Vector3 original, Vector3 direction){
		
		
		
			Vector3 cross1 = Vector3.Cross(original,direction);
	
			Vector3 pr = Vector3.Project(original,direction);
			Vector3 pr2 = original - pr;
	
	
			Vector3 cross2 = Vector3.Cross(pr2,cross1);
	
			Vector3 rotatedVector = (Quaternion.AngleAxis( rotAngle, cross2)*pr2)+pr;
	

			return rotatedVector;
	
	}

	public void StopSpawning(){
		numberOfObjects = 0;
		if(isSpawning == true){
			StopCoroutine("Spawner");
		}
		isSpawning = false;
		isUpdateTimerRunning = false;
		int id = thisPars.rtsUnitId;
		rtsm.cameraOperator.ActivateBuildingsMenu(id);
	}

	public void StartSpawning(){
	
		if(isSpawning == true){
			StopCoroutine("Spawner");
		}
	
		StartCoroutine("Spawner");
	}


	public Vector3 RandomTerrainVector(Vector3 origin, float vSize){	
		Vector2 randCircle = Random.insideUnitCircle * vSize;
	
	
		Vector3 rPos1 = origin + new Vector3(randCircle.x,0f,randCircle.y);
		float y1 = rtsm.manualTerrain.SampleHeight(rPos1);
		Vector3 randomPosition = new Vector3(rPos1.x, y1 ,rPos1.z);
	
	
		return randomPosition;

	}


	public float RefreshTimeStep(){
		float tStep = timestep;
		UnitPars up = model.GetComponent<UnitPars>();
		if(up.isBuilding == false){
			int numberMills = rtsm.numberOfUnitTypes[nation][8] + 1;
		
			tStep = up.buildTime;
		
			if(numberMills > 0){
				tStep = up.buildTime / numberMills;
			}
			if(tStep < 0.1f*up.buildTime){
				tStep = 0.1f*up.buildTime;
			}
		
			if(rtsm.cheats.godMode == 1){
				if(nation == rtsm.diplomacy.playerNation){
					tStep = 0.1f*up.buildTime;
				}
			}
		
		}
		return tStep;
	
	}

	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
	
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}




}