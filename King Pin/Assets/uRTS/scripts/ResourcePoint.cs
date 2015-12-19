using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcePoint : MonoBehaviour {
    
    public int resourceMin = 7000;
    public int resourceMax = 32000;
    
    public int numberIronNodes = 50;
    public int numberGoldNodes = 50;
    
    public GameObject ironPrefab = null;
    public GameObject goldPrefab = null;
    
    public float ironPrefabScaleMin = 0.01f;
    public float ironPrefabScaleMax = 0.1f;
    public float goldPrefabScaleMin = 0.01f;
    public float goldPrefabScaleMax = 0.1f;
    
    public int numberIronPrefabsMin = 1;
    public int numberIronPrefabsMax = 10;
    public int numberGoldPrefabsMin = 1;
    public int numberGoldPrefabsMax = 10;

    
    [HideInInspector] public RTSMaster rtsm = null;
    [HideInInspector] public Terrain terrain = null;
    
    [HideInInspector] public List<Vector3> allResLocations = new List<Vector3>();
    [HideInInspector] public List<Vector3> ironLocations = new List<Vector3>();
    [HideInInspector] public List<Vector3> goldLocations = new List<Vector3>();
    
    public KDTree kd_allResLocations = null;
    public KDTree kd_ironLocations = null;
    public KDTree kd_goldLocations = null;
    
//    [HideInInspector] public List<ResourceObject> resObject = new List<ResourceObject>();
    
    [HideInInspector] public List<int> resTypes = new List<int>();     // types of resources: 0 - iron, 1 - gold
    [HideInInspector] public List<int> resTypeId = new List<int>();    // reference to iron or gold lists indices
    [HideInInspector] public List<int> resAmount = new List<int>();    // amount of 'resType' resource
    
    [HideInInspector] public List<GameObject> resGo = new List<GameObject>();    // resource gameObject (used for destroy when resources are depleted)
    
	// Use this for initialization
	void Awake(){
//		rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
//    	rtsm.resourcePoint = this;
    	terrain = rtsm.manualTerrain;
    	SpawnResources(0, numberIronNodes);
		SpawnResources(1, numberGoldNodes);
	}
	
	void Start () {
// 		terrain = rtsm.manualTerrain;
// 		SpawnResources(0, numberIronNodes);
// 		SpawnResources(1, numberGoldNodes);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SpawnResources(int resType, int N){
	    
	    GameObject res = null;
	    if(resType == 0){
	    	res = ironPrefab;
	    }
	    else{
	    	res = goldPrefab;
	    }
	    
		for(int i=0; i<N; i++){
		    float rand1 = Random.Range(0f,1f);
		    float rand2 = Random.Range(0f,1f);
		    
		    
		    
		    float randResource = Random.Range(1f*resourceMin,1f*resourceMax);
		    
		    Vector3 planeVect = new Vector3(rand1*terrain.terrainData.size.x,0f,rand2*terrain.terrainData.size.x);

		    Vector3 mainPos = new Vector3(
							   rand1*terrain.terrainData.size.x,
							   terrain.SampleHeight(planeVect),
							   rand2*terrain.terrainData.size.x
		                   );
            
//            resObject.Add(new ResourceObject());
            
            if(resType == 0){
            	ironLocations.Add(mainPos);
            	resTypeId.Add(ironLocations.Count-1);
            }
            else if(resType == 1){
            	goldLocations.Add(mainPos);
            	resTypeId.Add(goldLocations.Count-1);
            }
            allResLocations.Add(mainPos);
            resTypes.Add(resType);
            resAmount.Add((int)randResource);
            
            int j = 0;
            
			
            GameObject goMain = new GameObject("resource"+i.ToString());
            
            resGo.Add(goMain);
            
            
            int min_maxj = 1;
            int max_maxj = 8;
            
            
            if(resType == 0){
                if(numberIronPrefabsMin<numberIronPrefabsMax){
            		min_maxj = numberIronPrefabsMin;
            		max_maxj = numberIronPrefabsMax;
            	}
            }
            else if(resType == 1){
                if(numberGoldPrefabsMin<numberGoldPrefabsMax){
            		min_maxj = numberGoldPrefabsMin;
            		max_maxj = numberGoldPrefabsMax;
            	}
            }
            
            
            
            
            float av_maxj = (1f*min_maxj+1f*max_maxj)/2f;
            
            float averageRes = (1f*resourceMin+1f*resourceMax)/2f;
            float averageShift = 1f+(randResource - averageRes) / averageRes;
            int maxj = (int)(av_maxj*averageShift);
            
            
            
            if(maxj < min_maxj){
            	maxj = min_maxj;
            }
            else if(maxj > max_maxj){
                maxj = max_maxj;
            }
            
            
            Material mat = null;
            while(j<maxj){
		        float radius = 1f;
		    
		        float randj1 = radius*Random.Range(-1f,1f);
		        float randj2 = radius*Random.Range(-1f,1f);
		        
		        Vector3 planeVectj = new Vector3(rand1*terrain.terrainData.size.x+randj1,0f,rand2*terrain.terrainData.size.x+randj2);
		        
		        if((planeVectj-planeVect).sqrMagnitude < radius*radius){
		            j++;
					Vector3 spawnPos = new Vector3(
									   (rand1*terrain.terrainData.size.x+randj1),
									   terrain.SampleHeight(planeVectj),
									   (rand2*terrain.terrainData.size.x+randj2)
								   );
						   
			
						   
			
					float randAngle1 = Random.Range(0f, 360f);
					float randAngle2 = Random.Range(0f, 360f);
					float randAngle3 = Random.Range(0f, 360f);
			
					Quaternion rot = Quaternion.Euler( randAngle1, randAngle2, randAngle3);
			
					GameObject go = (GameObject)Instantiate(res, spawnPos, rot);
				    
				    go.transform.parent = goMain.transform;
				    
				    
				    
			
					float scaleFactor = 1f;
					if(resType == 0){
						scaleFactor = Random.Range(ironPrefabScaleMin,ironPrefabScaleMax);
					}
					else if(resType == 1){
						scaleFactor = Random.Range(goldPrefabScaleMin,goldPrefabScaleMax);
					}
			
					go.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
					mat = go.GetComponent<MeshRenderer>().material;
				}
			}
			
			MergeMeshesToParent(goMain, mat);
			
			
		}
		
		if(resType == 0){
	    	kd_ironLocations = KDTree.MakeFromPoints(ironLocations.ToArray());
	    }
	    else if(resType == 1){
	    	kd_goldLocations = KDTree.MakeFromPoints(goldLocations.ToArray());
	    }
		kd_allResLocations = KDTree.MakeFromPoints(allResLocations.ToArray());
		
	}
	
	
	void MergeMeshesToParent(GameObject go, Material mat){
		MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        
        go.AddComponent<MeshFilter>();
        go.GetComponent<MeshFilter>().mesh = new Mesh();
        go.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        
        go.AddComponent<MeshRenderer>();
        go.GetComponent<MeshRenderer>().material = mat;
        go.SetActive(true);
	}
	
	
	public void UnsetResourcePoint(int id){
	    int resPointId = id;
		if(resTypes[resPointId] == 0){
		   int indivResId = resTypeId[resPointId];
			ironLocations.RemoveAt(indivResId);
			kd_ironLocations = KDTree.MakeFromPoints(ironLocations.ToArray());
			for(int jj=0;jj<resTypeId.Count;jj++){
			    if(resTypes[jj] == 0){
					if(resTypeId[jj]>indivResId){
						resTypeId[jj] = resTypeId[jj]-1;
					}
				}
			}
		}
		else if(resTypes[resPointId] == 1){
			int indivResId = resTypeId[resPointId];
			goldLocations.RemoveAt(indivResId);
			kd_goldLocations = KDTree.MakeFromPoints(goldLocations.ToArray());
			for(int jj=0;jj<resTypeId.Count;jj++){
			    if(resTypes[jj] == 1){
					if(resTypeId[jj]>indivResId){
						resTypeId[jj] = resTypeId[jj]-1;
					}
				}
			}
		}
		
		allResLocations.RemoveAt(resPointId);
		resTypes.RemoveAt(resPointId);
		resTypeId.RemoveAt(resPointId);
		resAmount.RemoveAt(resPointId);
		
		GameObject go = resGo[resPointId];
		Destroy(go);
		resGo.RemoveAt(resPointId);
		
		kd_allResLocations = KDTree.MakeFromPoints(allResLocations.ToArray());
		
		
	}
	
	
	
	
}



// public class ResourceObject {
// 
// 	public int resType = -1;
// 	public int resTypeId = -1;
// 	public int resAmount = -1;
// 	
// 	public GameObject resGo = null;
// 	
// 	public Vector3 position = Vector3.zero;
// 	
// 	
// 	
// 	
// 
// }


