using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildMark : MonoBehaviour {
    
 //   public GameObject goTerrain = null;
    
    
    public GameObject projector = null;
    public GameObject nationCentre = null;
    private SpawnPoint sp = null;
    
    public Terrain ter = null;
    
//    private float xTer = 0f;
//    private float zTer = 0f;
    
//    private int baseRes = 0;
    
    private float markRadius = 10f;
//    private float norm_markRadius;
    
//    private float highestSteepness = 0f;
    
    [HideInInspector] public bool buildingAllowed = false;
    
//    private float townRadius = 150f;
    
    private BattleSystem bs;
    
    public GameObject objectToSpawn = null;
    public UnitPars up_objectToSpawn = null;
    [HideInInspector] public int objectToSpawnId = 0;
    
    private float rEnclosed = 0f;
    
    public RTSMaster rtsm;
    
    private int nLeftClicks = 0;
    
    private List<GameObject> projectorFixed = new List<GameObject>();
    private List<Vector3> spawnLocations = new List<Vector3>();
    private List<Quaternion> spawnRotations = new List<Quaternion>();
    private List<bool> fenceAllowed = new List<bool>();
    
    
    
    
    public void ActivateProjector(){
        rEnclosed = objectToSpawn.GetComponent<MeshRenderer>().GetComponent<Renderer>().bounds.extents.magnitude;
    	projector.SetActive(true);
    	projector.GetComponent<Projector>().orthographicSize = rEnclosed;
    }
    void Awake(){
 //   	rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
 //   	rtsm.buildMark = this;
        nationCentre = rtsm.nationCenters[rtsm.diplomacy.playerNation];
        projector = rtsm.projector;
        projector.SetActive(false);
    	
    }
    
	// Use this for initialization
	void Start () {
//	    projector = rtsm.projector;
	    
	    bs = rtsm.battleSystem;
	    sp = nationCentre.GetComponent<SpawnPoint>();
	    
	    ter = rtsm.manualTerrain;
// 		ter = Terrain.activeTerrain;
// 		xTer = ter.terrainData.size.x;
// 		zTer = ter.terrainData.size.z;
// 		
// 		baseRes = ter.terrainData.baseMapResolution;
// 		norm_markRadius = markRadius/xTer;
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    
	    float height = 0f;
		RaycastHit hit;
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (ter.GetComponent<TerrainCollider>().Raycast (ray, out hit, 10000f)) {
                 height = ter.SampleHeight(hit.point);
        //         print(height);
             }
        
        projector.transform.position = new Vector3(hit.point.x, height+20f, hit.point.z);
        
//         float ttx = hit.point.x/xTer;
//         float ttz = hit.point.z/zTer;
//         
//         float tileXbase = ttx*baseRes;
//         float tileZbase = ttz*baseRes;
//         float delta_tileRadius = norm_markRadius*baseRes;
//         
//         int tilexmin = (int)(tileXbase - delta_tileRadius);
//         int tilexmax = (int)(tileXbase + delta_tileRadius);
//         int tilezmin = (int)(tileZbase - delta_tileRadius);
//         int tilezmax = (int)(tileZbase + delta_tileRadius);
//         
//         highestSteepness = 0f;
//         
//         for(int i=tilexmin; i<=tilexmax; i++){
//         	if(i>=0){
//         		if(i<baseRes){
//         			for(int j=tilezmin; j<=tilezmax; j++){
//         				if(j>=0){
// 							if(j<baseRes){
// 							
// 						// if inside the circle		
// 								if(
// 									((1f*i/baseRes-ttx)*(1f*i/baseRes-ttx))+
// 									((1f*j/baseRes-ttz)*(1f*j/baseRes-ttz))
// 									 <
// 									norm_markRadius*norm_markRadius
// 								){
// 									if(highestSteepness < ter.terrainData.GetSteepness(1f*i/baseRes, 1f*j/baseRes)){
// 										highestSteepness = ter.terrainData.GetSteepness(1f*i/baseRes, 1f*j/baseRes);
// 									}
// 								}
// 							
// 								
// 							}
// 						}	
//         			}
//         		}
//         	}
//         }
        
        if(rtsm.terrainProperties.TerrainSteepness(hit.point, markRadius) > 30f){
  //      if(highestSteepness > 30f){
        	projector.GetComponent<Projector>().material.color = Color.red;
        	buildingAllowed = false;
        }
        else if(
			(hit.point-rtsm.createForest.treePositions[
				rtsm.createForest.kd_treePositions.FindNearest(hit.point)
			]).sqrMagnitude < rEnclosed*rEnclosed
        ){
        	projector.GetComponent<Projector>().material.color = Color.red;
        	buildingAllowed = false;
        }
        else if(isEnoughResources(rtsm.diplomacy.playerNation, up_objectToSpawn) == false){
        	projector.GetComponent<Projector>().material.color = Color.red;
        	buildingAllowed = false;
        }
        else{
        	projector.GetComponent<Projector>().material.color = Color.green;
        	buildingAllowed = true;
        }
        
        
        
        if(nationCentre != null){
            if((up_objectToSpawn.rtsUnitId != 0) && (up_objectToSpawn.rtsUnitId != 2) && (up_objectToSpawn.rtsUnitId != 5)){
                
	//			if((hit.point-nationCentre.transform.position).sqrMagnitude>townRadius*townRadius){
	//				projector.GetComponent<Projector>().material.color = Color.red;
	//				buildingAllowed = false;
	//			}
        	}
        	else if(up_objectToSpawn.rtsUnitId == 5){
        	    int neigh = rtsm.resourcePoint.kd_allResLocations.FindNearest(hit.point);
        		if((rtsm.resourcePoint.allResLocations[neigh] - hit.point).sqrMagnitude>49f){
        			projector.GetComponent<Projector>().material.color = Color.red;
					buildingAllowed = false;
        		}
        	}
        }
        
        if(bs != null){
        	int ucount = bs.unitssUP.Count;
        	float smallestDist2 = 10000f;
        	for(int ii=0; ii<ucount; ii++){
        		if(bs.unitssUP[ii].isBuilding == true){
        		    float stopDistOut = (bs.unitssUP[ii].rEnclosed + rEnclosed)*(bs.unitssUP[ii].rEnclosed + rEnclosed);
        		    float dist2 = (hit.point-bs.unitssUP[ii].transform.position).sqrMagnitude;
        		    if(dist2 < smallestDist2){
        		    	if(bs.unitssUP[ii].nation == rtsm.diplomacy.playerNation){
        		    		smallestDist2 = dist2;
        		    	}
        		    }
        			if(dist2<stopDistOut){
        				projector.GetComponent<Projector>().material.color = Color.red;
						buildingAllowed = false;
					}
        		}
        	}
        	if((smallestDist2 > 1600f)&&
        	  ((up_objectToSpawn.rtsUnitId != 0) && (up_objectToSpawn.rtsUnitId != 2) && (up_objectToSpawn.rtsUnitId != 5))
        	){
        		projector.GetComponent<Projector>().material.color = Color.red;
				buildingAllowed = false;
        	}
        }
        
        
        if(Input.GetMouseButtonDown(0)){
        	if(buildingAllowed == true){
        		
        		UnitPars objectToSpawnPars = objectToSpawn.GetComponent<UnitPars>();
        	
        	// if continuous building (fence)	
        		if(objectToSpawnPars.rtsUnitId == 9){
        			nLeftClicks = nLeftClicks+1;
        		//	print(nLeftClicks);
        			if(nLeftClicks == 1){
        				projectorFixed.Add((GameObject)Instantiate(projector, projector.transform.position, projector.transform.rotation));
        				spawnLocations.Add(hit.point);
        				spawnRotations.Add(projector.transform.rotation);
        				fenceAllowed.Add(true);
        			}
        			else if(nLeftClicks == 2){
        				
        				nLeftClicks = 0;
        				sp.model = objectToSpawn;
        				
        				objectToSpawn.GetComponent<NavMeshAgent>().radius = 0.5f*rEnclosed;
        				
        				sp.numberOfObjects = 0;
        				
        				if(projectorFixed.Count>1){
        					spawnRotations[0] = spawnRotations[1];
        				}
        				
        				for(int i=0;i<projectorFixed.Count;i++){
        				    
        				    sp.isManualPosition = true;
        				    
        				    if(fenceAllowed[i] == true){
								sp.manualPosition.Add(spawnLocations[i]);
								sp.manualRotation.Add(spawnRotations[i]);
								sp.numberOfObjects = sp.numberOfObjects+1;
							}
							
        				    
        				    
        					
        				}
        				
        				sp.StartSpawning();
        				
        				for(int i=0;i<projectorFixed.Count;i++){
        					Destroy(projectorFixed[i].gameObject);
        					
        				}
        				
        				
        				projectorFixed.Clear();
        				spawnLocations.Clear();
        				spawnRotations.Clear();
        				fenceAllowed.Clear();
        				
        				
        				projector.SetActive(false);
        				this.enabled = false;
        				rtsm.bottomBarInfo.DisableBBI();
        				
        			}
        		}
        	// if regular building made in single click	
        		else{
        			float rot1 = Random.Range(0f,360f);
        			BuildBuilding(hit.point, rot1);
        	//		AroundBuilding(objectToSpawnPars, hit.point, rot1);
        		// mines resources	
        			
        		}
        		
        	}
        }
        else if(Input.GetMouseButtonDown(1)){
        	projector.SetActive(false);
        	this.enabled = false;
        	rtsm.bottomBarInfo.DisableBBI();
        	if(nLeftClicks == 1){
        		for(int i=0;i<projectorFixed.Count;i++){
					Destroy(projectorFixed[i].gameObject);
				}
        	}
        }
        
        
        if(nLeftClicks == 1){
            int nProjectorFixed = projectorFixed.Count;
            bool isAllowed = false;
            bool isPrevChanged = false;

            KDTree kd = KDTree.MakeFromPointsGo(projectorFixed);
            
            float rNearest = kd.FindNearest_R(projector.transform.position);
            
            if(rNearest > rEnclosed){
            	isAllowed = true;
            }
            
            float rPrev = (projectorFixed[nProjectorFixed-1].transform.position - projector.transform.position).sqrMagnitude;
            
            if(rPrev > rEnclosed*rEnclosed){
            	isPrevChanged = true;
            }
            
            if(isPrevChanged == true){
                spawnLocations.Add(hit.point);
				spawnRotations.Add(Quaternion.LookRotation(projectorFixed[nProjectorFixed-1].transform.position - projector.transform.position));
				projectorFixed.Add((GameObject)Instantiate(projector, projector.transform.position, projector.transform.rotation));
				if(isAllowed == true){
					fenceAllowed.Add(true);
				}
				else{
					fenceAllowed.Add(false);
				}
        	}
        }
        
        
        
  //      print(ter.terrainData.GetSteepness(ttx, ttz));
        
	}
	
	void BuildBuilding(Vector3 pos, float rot){
		        sp.isManualPosition = true;
        		sp.manualPosition.Add(pos);
        		sp.manualRotation.Add(Quaternion.Euler( 0f, rot , 0f));
        		sp.numberOfObjects = 1;
        		sp.model = objectToSpawn;
        		sp.StartSpawning();
        		
        		projector.SetActive(false);
        		this.enabled = false;
        		rtsm.bottomBarInfo.DisableBBI();

	}
	
	public bool isEnoughResources(int nat, UnitPars up){
		bool isEnough = false;
		Economy eco = rtsm.economy;
// 		Debug.Log(nat);
// 		Debug.Log(eco.iron.Count);
		if(eco.iron[nat] >= up.costIron){
			if(eco.gold[nat] >= up.costGold){
				if(eco.lumber[nat] >= up.costLumber){
					if(eco.population[nat] >= up.costPopulation){
						isEnough = true;
					}
				}
			}
		}
		return isEnough;
	}
	
	
	public void AroundBuilding(UnitPars up, Vector3 buildingOffset, float buildingRot){
		NavMeshObstacle nmo = up.GetComponent<NavMeshObstacle>();
		
		float xMax = 0.5f*nmo.size.x - nmo.center.x;
//		float xMin = -0.5f*nmo.size.x - nmo.center.x;
		
		float zMax = 0.5f*nmo.size.z - nmo.center.z;
//		float zMin = -0.5f*nmo.size.z - nmo.center.z;
		
	//	Vector3 pt = new Vector3(3f,0f,3f);
		
		
		float critRotAngl = Mathf.Atan(xMax/zMax) * Mathf.Rad2Deg;
		int N = 50;
		
		for(int i=0; i<N; i++){
			
			float rotat = (1f*i/N)* 90f;
		
			float x1 = 0f;
			float z1 = 0f;
		
			Vector3 outp = new Vector3(0f,0f,0f);
		
			if((rotat > 0f)&&(rotat<critRotAngl)){
				x1 = xMax;
				z1 = x1 * Mathf.Tan(rotat * Mathf.Deg2Rad);
			}
			if((rotat < 90f)&&(rotat>critRotAngl)){
				x1 = zMax * Mathf.Tan((90f-rotat) * Mathf.Deg2Rad);
				z1 = zMax;
			}
		
			outp = new Vector3(x1,0f,z1);
		
			outp = RotAround(-buildingRot, outp, new Vector3(0f,1f,0f));
		
			outp = outp + buildingOffset;
		
			outp = TerrainVector(outp)+ (new Vector3(0f,1f,0f));
		
			GameObject go = (GameObject)Instantiate(projector, outp, projector.transform.rotation);
			go.name = "uuu";
			go.SetActive(true);
		
			go.GetComponent<Projector>().orthographicSize = 0.5f;
		}
		
	//	Debug.Log(outp);
		
		
	}
	
	
	Vector3 RotAround(float rotAngle, Vector3 original, Vector3 direction){
	    
	    
	    
	    Vector3 cross1 = Vector3.Cross(original,direction);
	    
	    Vector3 pr = Vector3.Project(original,direction);
	    Vector3 pr2 = original - pr;
	    
	    
	    Vector3 cross2 = Vector3.Cross(pr2,cross1);
	    
	    Vector3 rotatedVector = (Quaternion.AngleAxis( rotAngle, cross2)*pr2)+pr;
	    
	
		return rotatedVector;
	
	}
	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}
}
