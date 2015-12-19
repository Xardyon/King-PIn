using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateForest : MonoBehaviour {

[HideInInspector] public GameObject model;

[HideInInspector] public bool readynow = true;
[HideInInspector] public float timestep = 0.01f;
[HideInInspector] public int count = 0;

[HideInInspector] public float size = 1.0f;

[HideInInspector] public int nation = 0;


[HideInInspector] public bool addToBS = true;

private GameObject objTerrain;

private Terrain terrain = null;

public List<string> treeName = new List<string>();
public List<int> numberOfPlants = new List<int>();
public List<float> minimumCriticalDistance = new List<float>();

private int currentPlantId = 0;


public RTSMaster rtsm = null;

[HideInInspector] public List<TreeInstance> treeList = new List<TreeInstance>();

[HideInInspector] public List<Vector3> treePos2D = new List<Vector3>();
[HideInInspector] public List<int> treeListPartCount = new List<int>();
[HideInInspector] public List<int> treeFirstPartId = new List<int>();
//private KDTree kd_treePos2D = null;
private LinearNeighbour ln_treePos2D = null;

private List<List<TreePrototype>> tpsList = new List<List<TreePrototype>>();
[HideInInspector] public List<int> treePartCount = new List<int>(); 
[HideInInspector] public List<List<int>> treePart1dIndices = new List<List<int>>(); 

private TreePrototype[] tps = null;



[HideInInspector] public List<Vector3> treePositions = new List<Vector3>();
[HideInInspector] public List<Vector3> treePositionsNorm = new List<Vector3>();
[HideInInspector] public List<int> treeHealth = new List<int>();
[HideInInspector] public KDTree kd_treePositions = null;




//private int currentPrototypeId = 0;



// void Awake(){
// //	rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
// //    rtsm.createForest = this;
// }

void Starter() {

}

void Awake () {
    
    for(int j=0;j<treeName.Count;j++){
        currentPlantId = j;
    	tpsList.Add(new List<TreePrototype>());
    	treePart1dIndices.Add(new List<int>());
    	
    	treePartCount.Add(GetTreePartCount());
    }
    
    
    for(int j=0;j<treeName.Count;j++){
        currentPlantId = j;
		SpawnTrees();
	}
	LoadTreePrototypes();
	
//	currentPrototypeId = 0;
    if(treePositionsNorm.Count == 0){
    	if(numberOfPlants.Count > 0){
    		if(numberOfPlants.Max() > 0){
    			CalculateAllTreePositions();
    		}
    	}
    }
	for(int j=0;j<treeName.Count;j++){
        currentPlantId = j;
		CalculateTrees();
	}

}

int GetTreePartCount(){
	
	string [] lines = new string[1];
	
	string filePath = "trees/"+treeName[currentPlantId]+"/config";
	TextAsset textRes = Resources.Load<TextAsset>(filePath);
	
	StringReader textReader = new StringReader(textRes.text);
	lines[0] = textReader.ReadLine();
	
	int ii = int.Parse(lines[0]);
	
	
	
	
	return ii;
}



void SpawnTrees(){
    if( !terrain ){
		terrain = rtsm.manualTerrain;
	}
		
	for(int i=0; i<treePartCount[currentPlantId]; i++){
		tpsList[currentPlantId].Add(
		    new TreePrototype()
		);
		tpsList[currentPlantId][i].prefab = Resources.Load<GameObject>("trees/"+treeName[currentPlantId]+"/"+treeName[currentPlantId]+" part "+i);
		tpsList[currentPlantId][i].bendFactor = 0.1f;
	}    

}	


void LoadTreePrototypes(){
    int nPrototypes = 0;
    
//    terrain.terrainData.treeInstances = null;
//    terrain.terrainData.treePrototypes = null;
    
	for(int i=0;i<treeName.Count;i++){
		for(int j=0;j<treePartCount[i];j++){
			nPrototypes++;
		}
	}
	
	tps = new TreePrototype[nPrototypes];
	
	nPrototypes = 0;
	for(int i=0;i<treeName.Count;i++){
		for(int j=0;j<treePartCount[i];j++){
		    tps[nPrototypes] = tpsList[i][j];
		    treePart1dIndices[i].Add(nPrototypes);
			nPrototypes++;
		}
	}
	
	terrain.terrainData.treePrototypes = tps;

}


public void CalculateAllTreePositions(){	
	CleanPositions();
	for(int j=0;j<treeName.Count;j++){
        currentPlantId = j;
        CalculateTreePositions();
	}
}


public void CleanPositions(){
	treePos2D.Clear();
	treePositions.Clear();
	treePositionsNorm.Clear();
}



public void CalculateTreePositions(){	
	if( !terrain ){
		terrain = rtsm.manualTerrain;
	}
	int baseRes = terrain.terrainData.baseMapResolution;
	
	PerlinNoise pnoise = new PerlinNoise();
	float[][] pn = pnoise.GeneratePerlinNoise(baseRes, baseRes, 8);
	
	
	
	int kk = 0;
	for(int i=0; i<numberOfPlants[currentPlantId]; i++){
	
	    float rand1 = Random.Range(0f,1f);
 		float rand2 = Random.Range(0f,1f);
 		
 		
 		int ix = (int)(rand1*baseRes);
 		int iz = (int)(rand2*baseRes);
 		
 		
 		if(pn[ix][iz]>0.5f){
 		    kk = kk+1;
			float height = rtsm.manualTerrain.SampleHeight(new Vector3(rand1*terrain.terrainData.size.x,0f,rand2*terrain.terrainData.size.x));
		    
		    

			Vector3 treePos = new Vector3(rand1,(height-1f)/terrain.terrainData.size.y,rand2);
			Vector3 new_treePos2D = new Vector3(rand1*terrain.terrainData.size.x,0f,rand2*terrain.terrainData.size.x);
			
			
			if(kk<2){
			
				treePos2D.Add(new Vector3(rand1*terrain.terrainData.size.x,0f,rand2*terrain.terrainData.size.x));
	            treePositions.Add(new Vector3(rand1*terrain.terrainData.size.x,(height-1f),rand2*terrain.terrainData.size.x));
	            treePositionsNorm.Add(treePos);
	
	        }
	        else{
		//	    kd_treePos2D = KDTree.MakeFromPoints(treePos2D.ToArray());
		//	    int neighId = kd_treePos2D.FindNearest(new_treePos2D);
			    
			    ln_treePos2D = LinearNeighbour.MakeFromPoints(treePos2D.ToArray());
			    int neighId = ln_treePos2D.FindNearest(new_treePos2D);
			    
				if((treePos2D[neighId]-new_treePos2D).sqrMagnitude > minimumCriticalDistance[currentPlantId]*minimumCriticalDistance[currentPlantId])
				{
					treePos2D.Add(new_treePos2D);
					treePositions.Add(new Vector3(rand1*terrain.terrainData.size.x,(height-1f),rand2*terrain.terrainData.size.x));
					treePositionsNorm.Add(treePos);
	            }
	        }
	    }
	}
	
	kd_treePositions = KDTree.MakeFromPoints(treePositions.ToArray());

}

void CalculateTrees(){	
	
	Color cl1 = new Color(0.8f,0.8f,0.8f,1f);
	Color cl2 = new Color(0.6f,0.6f,0.6f,1f);
//	Color cl3 = new Color(0.4f,0.4f,0.4f,1f);
	
	kd_treePositions = KDTree.MakeFromPoints(treePositions.ToArray());
//	kk = 0;
	for(int i=0; i<treePositions.Count; i++){
	
  		float rand3 = Random.Range(-1f,1f);
		Vector3 treePos = treePositionsNorm[i];
		
		if(i<2){
			treeListPartCount.Add(treePartCount[currentPlantId]);
			treeFirstPartId.Add(treeList.Count);
		
		
		
			treeHealth.Add(500);
			
			for(int j=0; j<treePartCount[currentPlantId]; j++) {
				
				TreeInstance tree = new TreeInstance();
				tree.color = cl1;
				tree.heightScale    = 0.5f+0.025f*rand3;
				tree.lightmapColor  = cl2;
				tree.position       = treePos;
				tree.prototypeIndex = treePart1dIndices[currentPlantId][j];
				tree.widthScale     = 0.5f+0.025f*rand3;
				treeList.Add(tree);
				
				treeListPartCount.Add(treePartCount[currentPlantId]);
				
			}
		}
		else{
			treeListPartCount.Add(treePartCount[currentPlantId]);
			treeFirstPartId.Add(treeList.Count);
	
	
	
			treeHealth.Add(500);
			
			for(int j=0; j<treePartCount[currentPlantId]; j++) {
				TreeInstance tree = new TreeInstance();
				tree.color = cl1;
				tree.heightScale    = 0.5f+0.025f*rand3;
				tree.lightmapColor  = cl2;
				tree.position       = treePos;
				tree.prototypeIndex = treePart1dIndices[currentPlantId][j];
				tree.widthScale     = 0.5f+0.025f*rand3;
				treeList.Add(tree);
				
				
				
			}

		}
	}
	
	if(currentPlantId+1 == treeName.Count){
		terrain.terrainData.treeInstances = new TreeInstance[ treeList.Count ];
		terrain.terrainData.treeInstances = treeList.ToArray();
	}
	
	
	
	
}


public void RefreshTrees(){
	terrain.terrainData.treeInstances = new TreeInstance[ treeList.Count ];
	terrain.terrainData.treeInstances = treeList.ToArray();
	
	kd_treePositions = KDTree.MakeFromPoints(treePositions.ToArray());

}

public void SetDefaultSettings(){
	treeName = new List<string>();
	numberOfPlants = new List<int>();
	minimumCriticalDistance = new List<float>();
	
	treeName.Add("Spruce");
	numberOfPlants.Add(4000);
	minimumCriticalDistance.Add(3f);
}

// public int NearestTreeLinearSearch(Vector3 query, int neigh){
// 	int nearestId;
// 	int N = treePositions.Count;
// 	Vector3[] treeDist = new Vector3[N];
// 	for(int i=0;i<N;i++){
// 		treeDist[i] = (treePositions[i]-query).sqrMagnitude;
// 	}
// 	treeDist.Sort();
// 	
// 	
// }



}