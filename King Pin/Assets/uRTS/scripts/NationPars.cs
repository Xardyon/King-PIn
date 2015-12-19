using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NationPars : MonoBehaviour {
	
	public int nation = 0;
	
	public float nationSize = 0f;
	public float sumOfAllNationsDistances = 0f;
	public float rSafe = 0f;
	
	public List<float> rNations = new List<float>();
	public List<float> neighboursDistanceFrac = new List<float>();
	
	public List<Vector3> safeAttackPoints = new List<Vector3>();
	
	
	public bool isReady = false;
	
	
	
	
	public RTSMaster rtsm = null;
	
	public List<Vector3> nationPos = new List<Vector3>();
	public KDTree nationPosTree = null;
	public List<int> sortedNationNeighbours = new List<int>();
	
	// Use this for initialization
	
	
	void Awake () {
	    for(int i=0; i<rtsm.diplomacy.numberNations; i++){
	    	safeAttackPoints.Add(rtsm.nationCenters[i].transform.position);
	    	nationPos.Add(rtsm.nationCenters[i].transform.position);
	    	sortedNationNeighbours.Add(0);
	    }
	    
		RefreshDistances();
		RefreshStaticPars();
		isReady = true;
	}
	
	
	
	public void RefreshDistances(){
		sumOfAllNationsDistances = 0f;
		for(int i=0; i<rtsm.diplomacy.numberNations; i++){
        	if(i != nation){
        	    rNations[i] = (rtsm.nationCenters[nation].transform.position - rtsm.nationCenters[i].transform.position).magnitude;
        		sumOfAllNationsDistances = sumOfAllNationsDistances + rNations[i];
        	}
        	else{
        		rNations[i] = 0f;
        	}
        	
        }
        
        for(int i=0; i<rtsm.diplomacy.numberNations; i++){
        	if(i != nation){
        	    
        		neighboursDistanceFrac[i] = 1f -
        			(rNations[i] / sumOfAllNationsDistances);
        	}
        	else{
        		neighboursDistanceFrac[i] = 0f;
        	}
        	
        }
        SortNeighbourNations();
        
	}
	
	public void SortNeighbourNations(){
	    for(int i=0; i<rtsm.diplomacy.numberNations; i++){
	    	nationPos[i] = rtsm.nationCenters[i].transform.position;
	    }
	    nationPosTree = KDTree.MakeFromPoints(nationPos.ToArray());
	    
	    sortedNationNeighbours = nationPosTree.FindNearestsK(nationPos[nation], rtsm.diplomacy.numberNations).ToList();
// 	    if(nation == 1){
// 	    	for(int i=0; i<rtsm.diplomacy.numberNations-1; i++){
// 	    		print(
// 	    			(i).ToString()+
// 	    			" "+
// 	    			(sortedNationNeighbours[i]).ToString()+
// 	    			" "+
// 	    			(rNations[sortedNationNeighbours[i]]).ToString()
// 	    		);
// 	    	}
// 	    }
	    
	}
	
	public void RefreshStaticPars(){
	    if(isReady == true){
			rSafe = nationSize + 40f;
			for(int i=0; i<rtsm.diplomacy.numberNations; i++){
				safeAttackPoints[i] = rtsm.nationCenters[i].transform.position;
				if(i != nation){
					safeAttackPoints[i] = GetSafePoint(i);
				}
			}
		}
	}
	
	
	public Vector3 GetSafePoint(int iNat){
    	Vector3 v3 = rtsm.nationCenters[iNat].transform.position;
    	if(rNations[iNat] > (rSafe+rtsm.nationPars[iNat].rSafe)){
    		Vector3 dr = rtsm.nationCenters[iNat].transform.position - rtsm.nationCenters[nation].transform.position;
    		float ratio = (rNations[iNat]-(rSafe+rtsm.nationPars[iNat].rSafe)) / rNations[iNat];
    		Vector3 dr2 = ratio * dr;
    		
    		v3 = TerrainVector(rtsm.nationCenters[nation].transform.position + dr2);
    	}
    	return v3;
    }


	public Vector3 GetSafePointV3(int iNat, Vector3 unitPos){
    	Vector3 v3 = rtsm.nationCenters[iNat].transform.position;
    	float rToNation = (rtsm.nationCenters[iNat].transform.position - unitPos).magnitude;
    	if(rToNation > rtsm.nationPars[iNat].rSafe){
    		Vector3 dr = rtsm.nationCenters[iNat].transform.position - unitPos;
    		float ratio = (rToNation-rtsm.nationPars[iNat].rSafe) / rNations[iNat];
    		Vector3 dr2 = ratio * dr;
    		
    		v3 = TerrainVector(unitPos + dr2);
    	}
    	return v3;
    }
	
	
	
	
	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}
    
    
    
    
//     public static void HeapSortFloat(float[] input, int[] iorig)
//     {
//     	//Build-Max-Heap
//     	int heapSize = input.Length;
//     	for (int p = (heapSize - 1) / 2; p >= 0; p--)
//         	MaxHeapify(input, iorig, heapSize, p);
//  
//     	for (int i = input.Length - 1; i > 0; i--)
//     	{
//         	//Swap
//         	float temp = input[i];
//         	input[i] = input[0];
//         	input[0] = temp;
//         	
//         	int itemp = iorig[i];
//         	iorig[i] = iorig[0];
//         	iorig[0] = itemp;
//  
//         	heapSize--;
//         	MaxHeapify(input, iorig, heapSize, 0);
//     	}
//     }
// 
// 
//     private static void MaxHeapify(float[] input, int[] iorig, int heapSize, int index)
//     {
//     	int left = (index + 1) * 2 - 1;
//     	int right = (index + 1) * 2;
//     	int largest = 0;
//  
//     	if (left < heapSize && input[left] > input[index])
//         	largest = left;
//     	else
//         	largest = index;
//  
//     	if (right < heapSize && input[right] > input[largest])
//         	largest = right;
//  
//     	if (largest != index)
//     	{
//         	float temp = input[index];
//         	input[index] = input[largest];
//         	input[largest] = temp;
//         	
//         	int itemp = iorig[index];
//         	iorig[index] = iorig[largest];
//         	iorig[largest] = itemp;
//  
//         	MaxHeapify(input, iorig, heapSize, largest);
//     	}
//     }	

    
	
	
}
