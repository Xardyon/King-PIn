using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TerrainGenerator;


public class Vegetation
{
	public Terrain terrain;
	public TerrainData terrainData;
	
	public TerrainChunk chunk;
	
	private List<TreeInstance> treeList = new List<TreeInstance>();
	
	public List<string> treeName;
	public List<int> numberOfPlants;
	public List<float> minimumCriticalDistance;

	private int currentPlantId = 0;

	public List<Vector3> treePos2D = new List<Vector3>();
	private List<int> treeListPartCount = new List<int>();
	private List<int> treeFirstPartId = new List<int>();
	
	
	
	public List<Vector3> treePositions = new List<Vector3>();
	public List<Vector3> treePositionsNorm = new List<Vector3>();
	
	private LinearNeighbour ln_treePos2D = null;
	
	private TreePrototype[] tps = null;
	
	
	
	private List<List<TreePrototype>> tpsList = new List<List<TreePrototype>>();
	private List<int> treePartCount = new List<int>(); 
	private List<List<int>> treePart1dIndices = new List<List<int>>(); 
	
	public float[,] pn;
	
	public float waterLevel = 16f;
	private float localWaterLevel = 0f;
	


	public void Starter() {
	    
	    localWaterLevel = waterLevel - chunk.Settings.verticalShift;
	    
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
// 		if( !terrain ){
// 			terrain = Terrain.activeTerrain;
// 		}
		
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
//		int baseRes = terrain.terrainData.baseMapResolution;
		
//		float[,] pn = chunk.GetHeightmap(0f);
		
// 			PerlinNoise pnoise = new PerlinNoise();
// 			float[][] pn = pnoise.GeneratePerlinNoise(baseRes, baseRes, 8);



		int kk = 0;
		for(int i=0; i<numberOfPlants[currentPlantId]; i++){

			float rand1 = UnityEngine.Random.Range(0f,1f);
			float rand2 = UnityEngine.Random.Range(0f,1f);
	
			int ix = (int)(rand1*chunk.Settings.HeightmapResolution);
			int iz = (int)(rand2*chunk.Settings.HeightmapResolution);
			
	//		int ix = (int)(rand1*baseRes);
	//		int iz = (int)(rand2*baseRes);
			
	
			if(pn[iz,ix]>0.5f){
				
		//		float height = terrain.SampleHeight(new Vector3(rand1*terrainData.size.x,0f,rand2*terrainData.size.z));
				float height = terrainData.GetHeight(ix, iz);
				
		//		Debug.Log(height);
		

				Vector3 treePos = new Vector3(rand1,(height-1f)/terrainData.size.y,rand2);
			//	Vector3 treePos = new Vector3(rand1,(height-1f),rand2);
				Vector3 new_treePos2D = new Vector3(rand1*terrainData.size.x,0f,rand2*terrainData.size.z);
				
				if(((height-1f)) > localWaterLevel){
		
					if(kk<2){
					
						kk = kk+1;
				
						treePos2D.Add(new Vector3(rand1*terrainData.size.x,0f,rand2*terrainData.size.z));
						treePositions.Add(new Vector3(rand1*terrainData.size.x,(height-1f),rand2*terrainData.size.z));
						treePositionsNorm.Add(treePos);
					
					}
					else{
				//	    kd_treePos2D = KDTree.MakeFromPoints(treePos2D.ToArray());
				//	    int neighId = kd_treePos2D.FindNearest(new_treePos2D);
			
						ln_treePos2D = LinearNeighbour.MakeFromPoints(treePos2D.ToArray());
						int neighId = ln_treePos2D.FindNearest(new_treePos2D);
			
						if((treePos2D[neighId]-new_treePos2D).sqrMagnitude > minimumCriticalDistance[currentPlantId]*minimumCriticalDistance[currentPlantId])
						{
							kk = kk+1;
						
							treePos2D.Add(new_treePos2D);
							treePositions.Add(new Vector3(rand1*terrainData.size.x,(height-1f),rand2*terrainData.size.x));
							treePositionsNorm.Add(treePos);
						
						}
					}
				}
			}
		}

//		kd_treePositions = KDTree.MakeFromPoints(treePositions.ToArray());

	}


	void CalculateTrees(){	
	
		Color cl1 = new Color(0.8f,0.8f,0.8f,1f);
		Color cl2 = new Color(0.6f,0.6f,0.6f,1f);
	//	Color cl3 = new Color(0.4f,0.4f,0.4f,1f);
	
//		kd_treePositions = KDTree.MakeFromPoints(treePositions.ToArray());
	//	kk = 0;
		for(int i=0; i<treePositions.Count; i++){
	
			float rand3 = Random.Range(-1f,1f);
			Vector3 treePos = treePositionsNorm[i];
			
// 			int ix = (int)(treePositionsNorm[i].x*chunk.Settings.HeightmapResolution);
// 			int iz = (int)(treePositionsNorm[i].z*chunk.Settings.HeightmapResolution);
			
	//		Vector3 treePos = new Vector3(treePositionsNorm[i].x, terrainData.GetHeight(ix, iz)/terrainData.size.y, treePositionsNorm[i].z);
			
	//		Debug.Log(terrainData.GetHeight(ix, iz));
		
			if(i<2){
				treeListPartCount.Add(treePartCount[currentPlantId]);
				treeFirstPartId.Add(treeList.Count);
		
		
		
	//			treeHealth.Add(500);
			
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
	
	
	
	//			treeHealth.Add(500);
			
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



}







	
