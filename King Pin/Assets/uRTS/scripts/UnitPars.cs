using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitPars : MonoBehaviour {
	public int rtsUnitId = 0;
	
	public int statusBS = 0;
	
	public float buildTime = 1f;
	[HideInInspector] public bool isBuildFinished = false;
	
	[HideInInspector] public bool isFakeObject = false;
	public bool isMovable = true;
	public bool isBuilding = false;
	
	public bool onManualControl = false;
	
	public bool isArcher = false;
	public GameObject arrow = null;
	
	[HideInInspector] public Vector3 prevPos = Vector3.zero;
	[HideInInspector] public float prevTime = 0.0f;
	
	public float velArrow = 20.0f;
	
	public int militaryMode = 0;
	
	public int wonderingMode = 0;
	public int nationToWonder = -1;
	public float searchDistance = 30f;
	
//    public bool isReadyBeg = false;
//    public bool isReadyEnd = false;
//	public bool isApproaching = false;
	public bool strictApproachMode = false;
	public bool isAttacking = false;

	public bool isApproachable = true;
	public bool isAttackable = true;
	public bool onTargetSearch = false;
	public bool isHealing = false;
	public bool isImmune = false;
	public bool isDying = false;
	public bool isSinking = false;
	
	[HideInInspector] public Vector3 velocityVector = Vector3.zero;
	[HideInInspector] public Vector3 lastPosition;
	
	
//	[HideInInspector] public Transform thisTransform = null;
	[HideInInspector] public NavMeshAgent thisNMA = null;
	public NavMeshObstacle thisNMO = null;
	
	[HideInInspector] public SpriteLoader thisSL = null;
	[HideInInspector] public Animation thisAnim = null;
	[HideInInspector] public SpawnPoint thisSpawn = null;
	
	
	
//	public GameObject target = null;
	[HideInInspector] public UnitPars targetUP = null;
	[HideInInspector] public NavMeshAgent targetNMA = null;
	
	public List<UnitPars> attackers = new List<UnitPars>();
	
	public int noAttackers = 0;
	public int maxAttackers = 20;
	public int maxStrictAttackers = 20;
	
	[HideInInspector] public int approachersLoadCount = 0;
	
	public float stopDistIn = 2.0f;
	public float stopDistOut = 2.5f;
	
	
	
	[HideInInspector] public float prevR;
	public int failedR = 0;
	public int critFailedR = 7;
	
	public float attackWaiter = 3.0f;
	[HideInInspector] public float timeMark = 0.0f;
	
	public float health = 100.0f;
	public float maxHealth = 100.0f;
	public float selfHealFactor = 4.0f;
	
	public float strength = 10.0f;
	public float defence = 10.0f;
	

	
	[HideInInspector] public int deathCalls = 0;
	public int maxDeathCalls = 5;
	
	[HideInInspector] public int sinkCalls = 0;
	public int maxSinkCalls = 5;
	
	[HideInInspector] public int unsetCalls = 0;
	[HideInInspector] public int maxUnsetCalls = 5;
	
	
	
//	[HideInInspector] public bool changeMaterial = true;
	[HideInInspector] public int animationMode = 100;
	
	public int nation = 0;
	
// MC options
    
    public bool isSelected = false;
	public bool prepareMovingMC = false;
	public bool isMovingMC = false;
	
	public bool isOnBS = false;
	
	
    [HideInInspector] public float prevDist = 0.0f;
    [HideInInspector] public int failedDist = 0;
    public int critFailedDist = 10;
	
	public Vector3 manualDestination;
	public Vector3 wondererAIdestination;
	
	public int wonderingResetCount = 0;
	public int guardResetCount = -1;
	
	public float rEnclosed = 0.0f;	
	
	
	public int animationToUse = 1;
	// 0 - none
	// 1 - 3d sprites
	// 2 - model animations
	
	
	
	
	public string unitName = "";
	
	public int costIron = 0;
	public int costGold = 0;
	public int costLumber = 0;
	public int costPopulation = 0;
	
    
//    public int propertiesMenuList = -1;
    
    
    public int chopTreeId = -1; // gathering node index (i.e. id of tree if chopping or id of mine if collecting iron or gold)
    public int chopTreePhase = -1;
  // -1 - no chopping
  //  1 - approaching
  //  2 - chopping
  //  3 - returning logs  
  //  4 - waiting for sawmill (if none are build)
    public int chopHits = 0;
    public int maxChopHits = 35;
    
    [HideInInspector] public int targetSawmillId = -1;
    [HideInInspector] public int resourceType = -1;
    public int remainingResources = -1;
    
    public int selectionGroupId = 0;
    public UnitsGroup unitsGroup = null;
    
    public Formation formation = null;
    
    
    
    
    
// Levels    
    public List<string> levelNames = new List<string>();
    public List<float> levelExp = new List<float>();
    public List<int> levelValues = new List<int>();
    public int totalLevel = 0;
    
    [HideInInspector] public RTSMaster rtsm;

// 0 - life points    
// 1 - attack 
// 2 - defence 
// 3 - building
// 4 - wood cutting     
// 5 - resource collection





// path resetter

    [HideInInspector] public int failPath = 0;
    [HideInInspector] public int maxFailPath = 10;
    [HideInInspector] public float remainingPathDistance = 1000000000000f;
    
    [HideInInspector] public int fakePathMode = 0;
    [HideInInspector] public int fakePathCount = 0;
    [HideInInspector] public int maxFakePathCount = 2;
    [HideInInspector] public Vector3 restoreTruePath = Vector3.zero;
    
    
    
// unitsMover    
    [HideInInspector] public Vector3 um_staticPosition;
    [HideInInspector] public Transform um_Follower;
    [HideInInspector] public int um_completionMark;
    [HideInInspector] public int um_complete;
    [HideInInspector] public float um_stopDistance;
    [HideInInspector] public string um_animationOnMove;
    [HideInInspector] public string um_animationOnComplete;
    
    [HideInInspector] public int isWondering1;


    void Awake(){
        rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
    	levelNames.Add("life points");
    	levelNames.Add("attack");
    	levelNames.Add("defence");
    	levelNames.Add("building");
    	levelNames.Add("wood cutting");
    	levelNames.Add("resource collection");
    	
    	for(int i=0;i<6;i++){
    		levelExp.Add(0f);
    		levelValues.Add(0);
    	}
    	
    	
    }      


	// Use this for initialization
	void Start () {
	
	}
	
	public void UpdateLevel(int id){
	    int oldLevel = levelValues[id];
	    int newLevel = (int)Mathf.Sqrt(levelExp[id]/50f);
	    if(newLevel>oldLevel){
			levelValues[id] = newLevel;
			if(id==0){
			    float addHealth = 100f*0.2f*(newLevel-oldLevel);
				maxHealth = maxHealth + addHealth;
				if(isSelected == true){
					rtsm.selectionMark.totalSelectedHealth = rtsm.selectionMark.totalSelectedHealth+addHealth;
				}
				if(health>0f){
					health = health+addHealth;
					if(isSelected == true){
						rtsm.selectionMark.remainingSelectedHealth = rtsm.selectionMark.remainingSelectedHealth+addHealth;
					}
				}
			}
			if(id==1){
				float addStrength = 10f*0.2f*(newLevel-oldLevel);
				strength = strength+addStrength;
			}
			if(id==2){
				float addDefence = 10f*0.2f*(newLevel-oldLevel);
				defence = defence+addDefence;
			}
			if(nation == rtsm.diplomacy.playerNation){
				float levelForScores = newLevel*newLevel;
				if(levelForScores < 200){
					rtsm.scores.AddToMasterScoreDiff(0.1f*newLevel*newLevel);
				}
				else{
					rtsm.scores.AddToMasterScoreDiff(0.1f*200f);
				}
			}
			UpdateTotalLevel();
		}
	}
	
	public void UpdateTotalLevel(){
		totalLevel = 0;
		for(int i=0; i<levelValues.Count; i++){
			totalLevel = totalLevel + levelValues[i];
		}
	}
	
	
	
	
	public void PlayAnimation(string animationName){
		if(thisSL.animName != animationName){
			thisSL.animName = animationName;
			rtsm.spritesManagerMaster.SetAnimation(thisSL,rtsUnitId);
		}
	}
	
	
	
	public void MoveUnit(Vector3 dest){
		
		if(thisNMA.enabled == true){
			thisNMA.SetDestination(TerrainVector(dest));
		}
	}

	public void MoveUnit(Vector3 dest, string animationName){
		
		PlayAnimation(animationName);
// 		if(thisNMO != null){
// 			thisNMO.enabled = false;
// 		}
	    if(rtsm.agentMover.useManualAgentsSystem == true){
			rtsm.agentMover.AddAgent(this,transform,TerrainVector(dest),thisNMA.stoppingDistance,thisNMA.speed, thisNMA.angularSpeed, 0.5f*thisNMA.height, thisNMA.radius);
		}
		else{
			if(thisNMA.enabled == true){
				thisNMA.SetDestination(TerrainVector(dest));
			}
		}
	}


	
	public void StopUnit(){
// 	    if(thisNMO != null){
// 	    	thisNMO.enabled = true;
// 	    }
		if(thisNMA.enabled == true){
			thisNMA.ResetPath();
		}
		
	}


	public void StopUnit(string animationName){
	    
	    PlayAnimation(animationName);
// 	    if(thisNMO != null){
// 	    	thisNMO.enabled = true;
// 	    }
		if(thisNMA.enabled == true){
			thisNMA.ResetPath();
		}
		
	}


	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}

	
	public void GrowBuilding(){
		if(isBuilding == true){
			StartCoroutine(GrowBuildingI());
		}
	}
	
	
	public IEnumerator GrowBuildingI(){
		bool isGrowing = true;
		int i=0;
		Vector3 finalPos = transform.position;
		
		float buildTimeTot = buildTime;
		float tStep1 = 0.1f;
		int nIterations = (int) (buildTimeTot / tStep1);
		
		NavMeshObstacle navObs = GetComponent<NavMeshObstacle>();
		
		float size1 = navObs.size.y;
//		float size1 = rEnclosed;
		
		float growStep = size1 / nIterations;
		float healthStep = 0.9f*maxHealth / nIterations;
		
		transform.position = new Vector3(finalPos.x, finalPos.y - size1, finalPos.z);
		health = 0.1f*maxHealth;
		
		while(isGrowing == true){
		    i++;
		    if(i>nIterations){
		    	isGrowing = false;
		    }
		    if((isDying==false)&&(isSinking==false)){
		    	float yOffset = finalPos.y - size1 + growStep*i;
				if(yOffset > finalPos.y){
					yOffset = finalPos.y;
				}
				transform.position = new Vector3(finalPos.x, yOffset, finalPos.z);
				
				health = health + healthStep;
				if(health>maxHealth){
					health = maxHealth;
				}
				
				
		    }
		    else{
		    	isGrowing = false;
		    }
		    
		    
			yield return new WaitForSeconds(tStep1);
		}
		
		if((isDying==false)&&(isSinking==false)){
			transform.position = finalPos;
			isBuildFinished = true;
			if(isSelected == true){
				rtsm.cameraOperator.ActivateBuildingsMenu(rtsUnitId);
			}
		}
		
	}
	
	
	public bool isEnoughResources(Economy eco, int nat){
		bool isEnough = false;
		if(eco.iron[nat] > costIron){
			if(eco.gold[nat] > costGold){
				if(eco.lumber[nat] > costLumber){
					if(eco.population[nat] > costPopulation){
						isEnough = true;
					}
				}
			}
		}
		return isEnough;
	}
	
	

}
