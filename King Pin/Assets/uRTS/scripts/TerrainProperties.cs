using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainProperties : MonoBehaviour {

	
	[HideInInspector] public RTSMaster rtsm;
	
	public Terrain terrain;
	
	
	public float xTer = 0f;
    public float zTer = 0f;
    
    public int baseRes = 0;
	
	
	public List<GameObject> gos = new List<GameObject>();
	
	
	void Awake(){
		terrain = rtsm.manualTerrain;
	}
	

	void Start () {
	
	}
	
	
	
	
	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}

	public Vector3 TerrainVector(Vector3 origin, Terrain ter1){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = ter1.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}
	
	
	public float HeightFromTerrain(Vector3 origin){
		Vector3 tv = TerrainVector(origin);
		return (origin.y-tv.y);
	}
	
	public float HeightFromTerrain(Vector3 origin, Terrain ter1){
		Vector3 tv = TerrainVector(origin, ter1);
		return (origin.y-tv.y);
	}
	
	
	
	
	public void Clean(){
		foreach(GameObject go in gos){
			DestroyImmediate(go);
		}
		gos.Clear();
	}
	
	
	
	public void GetTerrainProperties(){
		terrain = Terrain.activeTerrain;
		
		xTer = terrain.terrainData.size.x;
		zTer = terrain.terrainData.size.z;
		
		baseRes = terrain.terrainData.baseMapResolution;
		
		SetNewTerrains();
	}
	
	void SetNewTerrains(){
		gos = new List<GameObject>();
		
// 		Terrain t1 = NewTerrain(new Vector3(-2000f,0f,0f));
// 		Terrain t2 = NewTerrain(new Vector3(-2000f,0f,-2000f));
		
//		ConnectTerrains(t1.terrainData,t2.terrainData);
//		Terrain t2 = NewTerrain();
		
//		t2.SetNeighbors(t1, null, null, null);

	}
	
	
	
	
	public float TerrainSteepness(Vector3 point, float markRadius){
	    float norm_markRadius = markRadius/xTer;
	    
		float ttx = point.x/xTer;
        float ttz = point.z/zTer;
        
        float tileXbase = ttx*baseRes;
        float tileZbase = ttz*baseRes;
        float delta_tileRadius = norm_markRadius*baseRes;
        
        int tilexmin = (int)(tileXbase - delta_tileRadius);
        int tilexmax = (int)(tileXbase + delta_tileRadius);
        int tilezmin = (int)(tileZbase - delta_tileRadius);
        int tilezmax = (int)(tileZbase + delta_tileRadius);
        
        float highestSteepness = 0f;
        
        for(int i=tilexmin; i<=tilexmax; i++){
        	if(i>=0){
        		if(i<baseRes){
        			for(int j=tilezmin; j<=tilezmax; j++){
        				if(j>=0){
							if(j<baseRes){
							
						// if inside the circle		
								if(
									((1f*i/baseRes-ttx)*(1f*i/baseRes-ttx))+
									((1f*j/baseRes-ttz)*(1f*j/baseRes-ttz))
									 <
									norm_markRadius*norm_markRadius
								){
									float steepness = terrain.terrainData.GetSteepness(1f*i/baseRes, 1f*j/baseRes);
									if(highestSteepness < steepness){
										highestSteepness = steepness;
									}
								}
							
								
							}
						}	
        			}
        		}
        	}
        }
        
        return highestSteepness;

	}
	
	
	
	public Terrain NewTerrain(Vector3 tPos){
		GameObject TerrainObj = new GameObject("TerrainObj");
		gos.Add(TerrainObj);
 		 TerrainObj.transform.position = tPos;
		 TerrainData tData = new TerrainData();
 
		 tData.size = new Vector3(125, 600, 125);
		 tData.heightmapResolution = 512;
		 tData.baseMapResolution = 1024;
		 tData.SetDetailResolution(1024, 8);
 
		 RandomizePoints(tData, 0.3f);
 
		 TerrainCollider _TerrainCollider = TerrainObj.AddComponent<TerrainCollider>();
		 Terrain _Terrain2 = TerrainObj.AddComponent<Terrain>();
 
		 _TerrainCollider.terrainData = tData;
		 _Terrain2.terrainData = tData;
		 
		 return _Terrain2;
	}
	
	
	
	void RandomizePoints(TerrainData tData, float strength) { 
         int xRes = tData.heightmapWidth;
         int yRes = tData.heightmapHeight;
         
         int baseRes = tData.baseMapResolution;
         PerlinNoise pnoise = new PerlinNoise();
         float[][] pn = pnoise.GeneratePerlinNoise(baseRes, baseRes, 8);
         
         float[,] heights = tData.GetHeights(0, 0, xRes, yRes);
         
         for (int y = 0; y < yRes; y++) {
             for (int x = 0; x < xRes; x++) {
                 heights[x,y] = 0.05f*pn[x][y];
                 //Random.Range(0.0f, strength) * 0.5f;
             }
         }
         
         tData.SetHeights(0, 0, heights);
     }
	
	 
// 	 void ConnectTerrains(TerrainData tData1, TerrainData tData2){
// 	 	int xRes1 = tData1.heightmapWidth;
//         int yRes1 = tData1.heightmapHeight;
//         
//         int xRes2 = tData2.heightmapWidth;
//         int yRes2 = tData2.heightmapHeight;
//         
//         float[,] heights1a = tData1.GetHeights(0, 0, xRes1, yRes1);
//         float[,] heights1b = tData1.GetHeights(0, 0, xRes1, yRes1);
//         
//         float[,] heights2a = tData2.GetHeights(0, 0, xRes2, yRes2);
//         float[,] heights2b = tData2.GetHeights(0, 0, xRes2, yRes2);
//         
//         for (int y = 0; y < yRes1; y++) {
//              for (int x = 0; x < xRes1; x++) {
//                  float xw = -(2f/xRes1)*x + 1f;
//                  if(xw < 0f){
//                  	xw = 0f;
//                  }
//                  if(xw > 1f){
//                  	xw = 1f;
//                  }
//                  
//                  heights1b[x,y] = (1f-xw) * heights1a[x,y] + xw * heights2a[x,yRes2-1];
//                  //Random.Range(0.0f, strength) * 0.5f;
//              }
//          }
//          
//          tData1.SetHeights(0, 0, heights1b);
//         
//         
// 	 }
	 





//////////////////////////////////////////////////////////////////////////////////////////





















	
	
}
