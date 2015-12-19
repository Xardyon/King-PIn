using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinearNeighbour{
	
	public Vector3[] pts;
	
	public static LinearNeighbour MakeFromPoints(params Vector3[] points) {
		LinearNeighbour root = new LinearNeighbour();
		root.pts = points;
		return root;
	}
	
	public int FindNearest(Vector3 pt){
		int bestIndex = -1;
		float bestSqDist = 1000000000f;
		
		for(int i=0;i<pts.Length;i++){
		    float R = (pts[i]-pt).sqrMagnitude;
			if(R < bestSqDist){
				bestIndex = i;
				bestSqDist = R;
			}
		}
		
		return bestIndex;
	}
	
// 	public void AddPoint(Vector3 pt){
// 		pts.Add(pt);
// 	}
	
}
