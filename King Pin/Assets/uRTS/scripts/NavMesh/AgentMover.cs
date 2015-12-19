using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentMover : MonoBehaviour {
	
	public RTSMaster rtsm;
	
	public bool useManualAgentsSystem = false;
	
	public List<AgentMover1> movers = new List<AgentMover1>();
	
	public KDTree kd_MovableUnits;
	public List<Vector3> allUnitsPos;
	
	void Start () {
//		StartCoroutine(MoveAgents());
	}
	
	public void AddAgent(UnitPars up, Transform ag_transform, Vector3 destination, float stopDistance, float moveVelocity, float angularSpeed, float height, float size){
		RemoveAgent(ag_transform);
		
		AgentMover1 n_mover = new AgentMover1();
		n_mover.rtsm = rtsm;
		n_mover.agentUP = up;
		n_mover.ag_transform = ag_transform;
		n_mover.destination = destination;
		n_mover.stopDistance = stopDistance;
		n_mover.moveVelocity = moveVelocity;
		n_mover.angularSpeed = angularSpeed;
		n_mover.height = height;
		n_mover.size = size;
		n_mover.currentCorner = 0;
		n_mover.CalculatePath(Time.time);
		n_mover.lastTime = Time.time;
		n_mover.prevTime = Time.time;
		movers.Add(n_mover);
		
	}
	
	public void RemoveAgent(Transform ag_transform){
		for(int i=0;i<movers.Count;i++){
			if(movers[i].ag_transform == ag_transform){
				movers.Remove(movers[i]);
			}
		}
	}
	
	public IEnumerator MoveAgents(){
		int loopCounter = 0;
		while(true){
			for(int i=0;i<movers.Count;i++){
				AgentMover1 mov = movers[i];
				
				if(mov.isInDestination()==false){
					mov.MoveStep(Time.time);
				}
				else{
					RemoveAgent(mov.ag_transform);
				}
				
				mov.prevTime = Time.time;
				
				loopCounter = loopCounter+1;
				if(loopCounter > 500){
					loopCounter = 0;
					yield return new WaitForSeconds(0.02f);
				}
			}
			yield return new WaitForSeconds(0.02f);
		}
	}
	
	void LateUpdate() {
	    this.enabled = useManualAgentsSystem;
	    RefreshMovablesKDTree();
//	    MoveCloseNeigbours();
	    
		for(int i=0;i<movers.Count;i++){
			AgentMover1 mov = movers[i];
			
			
			
			if(mov.isInDestination()==false){
			//	mov.MoveStep(Time.time);
			    float rNearest = kd_MovableUnits.FindNearestK_R(mov.ag_transform.position,1);
			    float rNearestFurther = kd_MovableUnits.FindNearestK_R(mov.GetStep(Time.time),2);
			    
			    if(rNearest >= 2.5f*mov.size){
		//		if((rNearest >= mov.size)&&(rNearestFurther >= mov.size)){
					mov.MoveStep(Time.time);
				}
				else if((rNearest < 2.5f*mov.size)&&(rNearestFurther > rNearest)){
					mov.MoveStep(Time.time);
				}
				else{
					mov.lastTime = mov.lastTime + (Time.time - mov.prevTime);
			//		mov.CalculatePath(Time.time);
			        if(Random.value < 0.1f){
			//			mov.RecalculatePath(Time.time);
					}
			//		mov.agentUP.MoveUnit(mov.destination);
					
				}
				
				if(mov.ag_transform.gameObject.GetComponent<UnitPars>().nation == 0){
// 					Debug.Log(rNearest.ToString()+" "+rNearestFurther);
// 					Debug.DrawLine(mov.path.corners[ie], mov.path.corners[ie+1], Color.red);
					for (int ie = 0; ie < mov.path.corners.Length-1; ie++){
						Debug.DrawLine(mov.path.corners[ie], mov.path.corners[ie+1], Color.red);
					}
				}
				
				mov.prevTime = Time.time;
			}
			else{
				RemoveAgent(mov.ag_transform);
			}
			
			
			
		}
		
		
		
		
		
	}
	
	
	void RefreshMovablesKDTree(){
		List<UnitPars> allUnits = rtsm.allUnits;
		allUnitsPos = new List<Vector3>();
		for(int i=0;i<allUnits.Count;i++){
			if(allUnits[i].isBuilding == false){
				allUnitsPos.Add(allUnits[i].transform.position);
			}
		}
		
		if(allUnitsPos.Count > 0){
			kd_MovableUnits = KDTree.MakeFromPoints(allUnitsPos.ToArray());
		}
	}
	
	
	void MoveCloseNeigbours(){
		List<UnitPars> allUnits = rtsm.allUnits;
		List<UnitPars> allUnitsNonBuild = new List<UnitPars>();
		for(int i=0;i<allUnits.Count;i++){
			if(allUnits[i].isBuilding == false){
				allUnitsNonBuild.Add(allUnits[i]);
			}
		}
		if(allUnitsPos.Count > 1){
			for(int i=0;i<allUnitsNonBuild.Count;i++){
				int iNearest = kd_MovableUnits.FindNearestK(allUnitsNonBuild[i].transform.position,1);
				if((iNearest > 0)&&(iNearest<allUnitsPos.Count)){
					float rNearest = (allUnitsPos[iNearest]-allUnitsNonBuild[i].transform.position).magnitude;
					if(rNearest < 10f){
						if(allUnitsNonBuild[i].nation == 0){
				//			Debug.Log(rNearest);
						}
					}
					if(rNearest < 2f*allUnitsNonBuild[i].thisNMA.radius){
						Vector3 bounceVector = (allUnitsNonBuild[i].transform.position - allUnitsPos[iNearest]).normalized;
						float bv_size = allUnitsNonBuild[i].thisNMA.speed * Time.deltaTime;
						bounceVector = bv_size * bounceVector;
						
						Vector3 nextMove = TerrainVector(allUnitsNonBuild[i].transform.position + bounceVector + new Vector3(0f,0.5f*allUnitsNonBuild[i].thisNMA.height,0f));
						
						NavMeshHit hit;
						if (NavMesh.SamplePosition(nextMove, out hit, 2f*allUnitsNonBuild[i].thisNMA.height, NavMesh.AllAreas)) {
							if((TerrainVector(hit.position)-nextMove).magnitude < allUnitsNonBuild[i].thisNMA.radius){
							
								float rNearestFurter = kd_MovableUnits.FindNearestK_R(allUnitsNonBuild[i].transform.position + bounceVector,2);
								if(rNearestFurter > rNearest){
									allUnitsNonBuild[i].transform.position = allUnitsNonBuild[i].transform.position + bounceVector;
								}
							}
						}
					}
				}
			}
		}
	}
	
	
	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}
	
	
	
}


public class AgentMover1{
    public RTSMaster rtsm;
    
    public UnitPars agentUP;
    
	public Transform ag_transform;
	public Vector3 destination;
	public float stopDistance;
	public float moveVelocity;
	public float angularSpeed;
	public float height;
	public float size;
	
	public int currentCorner;
	
	public NavMeshPath path;
	
	private float prevSumDist = 0f;
	
	public float lastTime;
	public float prevTime;
	
	public float pathRecalculationStartTime = -1f;
	int isNMOEnabled = 0;
	
	int prePathMode = 0;
	
	public void RecalculatePath(float newTime){
		if(pathRecalculationStartTime < 0f){
			pathRecalculationStartTime = newTime;
			if(agentUP.thisNMO != null){
				if(agentUP.thisNMO.enabled == true){
					agentUP.thisNMO.enabled = false;
					isNMOEnabled = 1;
				}
			}

		}
		else{
			if((newTime-pathRecalculationStartTime) < 0.3f){
				pathRecalculationStartTime = -1f;
				CalculatePath(newTime);
				if(isNMOEnabled == 1){
		 //			agentUP.thisNMO.enabled = true;
		 			isNMOEnabled = 0;
		 		}
			}
		}
	}
	
	
	
	public void CalculatePath(float newTime){
	    path = new NavMeshPath();
// 	    isNMOEnabled = 0;
// 	    if(agentUP.thisNMO != null){
// 	    	if(agentUP.thisNMO.enabled == true){
// 	    		agentUP.thisNMO.enabled = false;
// 	    		isNMOEnabled = 1;
// 	    	}
// 	    }
		NavMesh.CalculatePath(ag_transform.position, destination, NavMesh.AllAreas, path);
		currentCorner = 0;
		prevSumDist = 0f;
		lastTime = newTime;
		prevTime = newTime;
		
		prePathMode = 1;
// 		if(isNMOEnabled == 1){
// 			agentUP.thisNMO.enabled = true;
// 		}


// 		if(agentUP.nation==0){
// 			if(agentUP.thisNMO!=null){
// 				if(agentUP.thisNMO.enabled == true){
// 					Debug.Log("qqq");
// 				}
// 			}
// 		}
	}
	
	
	public void MoveStep(float newTime){
		if(pathRecalculationStartTime >= 0f){
			if((newTime-pathRecalculationStartTime) < 0.3f){
				pathRecalculationStartTime = -1f;
				CalculatePath(newTime);
				if(isNMOEnabled == 1){
		// 			agentUP.thisNMO.enabled = true;
		 			isNMOEnabled = 0;
		 		}
			}
		}
		else{
			if(path!=null){
				if(prePathMode == 1){
					MovePrePathStep(newTime);
				}
				else{
					MovePathStep(newTime);
				}
			}
		}
	}
	
	public void MovePrePathStep(float newTime){
		if(path.corners.Length > 0){
			if(newTime > prevTime){
				float distToMove = moveVelocity*(newTime-prevTime);
				Vector3 lookAtVector = (path.corners[0] - ag_transform.position).normalized;
		
				lookAtVector = distToMove*lookAtVector;
				if(lookAtVector.magnitude > (path.corners[0] - ag_transform.position).magnitude){
					lookAtVector = path.corners[0] - ag_transform.position;
			
					prePathMode = 0;
					lastTime = newTime;
				}
				if((new Vector3(path.corners[0].x,0f,path.corners[0].z) - new Vector3(ag_transform.position.x,0f,ag_transform.position.z)).magnitude < distToMove){
					prePathMode = 0;
					lastTime = newTime;
				}
				if(agentUP.nation == 0){
		//			Debug.Log(lookAtVector.ToString()+" "+prePathMode);
				}
				
				ag_transform.position = TerrainVector(ag_transform.position + lookAtVector);
			}
		}
	}

	public void MovePathStep(float newTime){
	    if(newTime > lastTime){
			float distToMove = moveVelocity*(newTime-lastTime);
			float sumDist = prevSumDist;
			int newCorner = currentCorner;
		
	// 		if(ag_transform.gameObject.GetComponent<UnitPars>().nation == 0){
	// 			Debug.Log(path.corners.Length);
	// 		}
		
	//		Debug.Log(newCorner);
			int iwhile4 = 0;
			while((sumDist < distToMove)&&(newCorner < path.corners.Length)){
				iwhile4 = iwhile4+1;
			//	newCorner = newCorner + 1;
				int i3 = newCorner + 1;			
				if(i3 >= path.corners.Length){
					i3 = newCorner;
				}
				int i4 = i3-1;
				if(i4<0){
					i4 = 0;
				}
				if(i4 >= path.corners.Length){
					i4 = newCorner;
				}
				prevSumDist = sumDist;
				currentCorner = newCorner;
			
				sumDist = sumDist + (path.corners[i3]-path.corners[i4]).magnitude;
				newCorner = newCorner + 1;
			}
		
// 			if(ag_transform.gameObject.GetComponent<UnitPars>().nation == 0){
// 				Debug.Log(sumDist.ToString()+" "+distToMove+" "+newCorner+" "+path.corners.Length);
// 			}
	//		Debug.Log(iwhile4);
		
			int i1 = newCorner-1;
			int i2 = newCorner;
		
			if(i1 < 0){
				i1 = 0;
				i2 = 1;
			}
			if(i2 >= path.corners.Length){
				i2 = path.corners.Length-1;
			//	i1 = i2-1;
			}
		
		//	Debug.Log(i1);
		
			if(i1<path.corners.Length){
				Vector3 prevCorner = path.corners[i1];
				Vector3 nextCorner = path.corners[i2];
		
				float sumDist2 = 0f;
				int ifor4 = 0;
				for(int i=currentCorner;i<i2;i++){
					sumDist2 = sumDist2 + (path.corners[i+1]-path.corners[i]).magnitude;
					ifor4 = ifor4+1;
				}
			
	// 			if((iwhile4>1)||(ifor4>1)){
	// 				Debug.Log(iwhile4.ToString()+" "+ifor4);
	// 			}
		
				float dist3 = (path.corners[i2] - path.corners[i1]).magnitude - (sumDist-distToMove);
			
			//	float dist3 = distToMove;
			//	Debug.Log(((path.corners[i2] - path.corners[i1]).magnitude).ToString()+" "+sumDist2+" "+sumDist+" "+distToMove);
			
				Vector3 lookAtVector = (nextCorner - prevCorner).normalized;
				Vector3 finalDest = prevCorner + (dist3*lookAtVector);
		
				ag_transform.position = TerrainVector(finalDest) + new Vector3(0f,height,0f);
			
				// rotation
				Vector3 rotVect = ag_transform.rotation * new Vector3(0f,0f,1f);
				float rotAngle = -SignedAngleBetween2d(new Vector2(lookAtVector.x,lookAtVector.z), new Vector2(rotVect.x,rotVect.z));
			
				int angleSign = 0;
				if(rotAngle < 0){
					angleSign = -1;
				}
				else if(rotAngle > 0){
					angleSign = 1;
				}
			
				float angleToRotate = angularSpeed * (newTime-prevTime);
				if(Mathf.Abs(angleToRotate) > Mathf.Abs(rotAngle)){
					angleToRotate = rotAngle;
				}
			
				angleToRotate = angleToRotate*angleSign;
				if(currentCorner == i2){
					angleToRotate = 0f;
				}
			
			//	Debug.Log(angularSpeed * (newTime-prevTime));
				rotVect = RotAround(angleToRotate, rotVect, new Vector3(0f,1f,0f));
			
				ag_transform.rotation = ag_transform.rotation * Quaternion.Euler(0f,angleToRotate,0f);
			
			}
		
	//  		currentCorner = i2-1;
	//  		if(currentCorner < 0){
	//  			currentCorner = 0;
	//  		}
	//  		prevSumDist = sumDist;
	//		lastTime = newTime;
		}
	}
	
	

	public Vector3 GetStep(float newTime){
		Vector3 nextStep = ag_transform.position;
		if(newTime > lastTime){
			float distToMove = moveVelocity*(newTime-lastTime);
			float sumDist = prevSumDist;
			int newCorner = currentCorner;
		
			
		
	//		Debug.Log(newCorner);
			int iwhile4 = 0;
			while((sumDist < distToMove)&&(newCorner < path.corners.Length)){
				iwhile4 = iwhile4+1;
			//	newCorner = newCorner + 1;
				int i3 = newCorner + 1;			
				if(i3 >= path.corners.Length){
					i3 = newCorner;
				}
				int i4 = i3-1;
				if(i4<0){
					i4 = 0;
				}
				if(i4 >= path.corners.Length){
					i4 = newCorner;
				}
		//		prevSumDist = sumDist;
		//		currentCorner = newCorner;
			
				sumDist = sumDist + (path.corners[i3]-path.corners[i4]).magnitude;
				newCorner = newCorner + 1;
			}
		
	//		Debug.Log(sumDist.ToString()+" "+distToMove+" "+newCorner+" "+path.corners.Length);
		
	//		Debug.Log(iwhile4);
		
			int i1 = newCorner-1;
			int i2 = newCorner;
		
			if(i1 < 0){
				i1 = 0;
				i2 = 1;
			}
			if(i2 >= path.corners.Length){
				i2 = path.corners.Length-1;
			//	i1 = i2-1;
			}
		
		//	Debug.Log(i1);
		
			if(i1<path.corners.Length){
				Vector3 prevCorner = path.corners[i1];
				Vector3 nextCorner = path.corners[i2];
		
				float sumDist2 = 0f;
				int ifor4 = 0;
				for(int i=currentCorner;i<i2;i++){
					sumDist2 = sumDist2 + (path.corners[i+1]-path.corners[i]).magnitude;
					ifor4 = ifor4+1;
				}
			
	// 			if((iwhile4>1)||(ifor4>1)){
	// 				Debug.Log(iwhile4.ToString()+" "+ifor4);
	// 			}
		
				float dist3 = (path.corners[i2] - path.corners[i1]).magnitude - (sumDist-distToMove);
			
			//	float dist3 = distToMove;
			//	Debug.Log(((path.corners[i2] - path.corners[i1]).magnitude).ToString()+" "+sumDist2+" "+sumDist+" "+distToMove);
			
				Vector3 lookAtVector = (nextCorner - prevCorner).normalized;
				Vector3 finalDest = prevCorner + (dist3*lookAtVector);
		
				nextStep = TerrainVector(finalDest) + new Vector3(0f,height,0f);
			
	// rotation
	// 			Vector3 rotVect = ag_transform.rotation * new Vector3(0f,0f,1f);
	// 			float rotAngle = -SignedAngleBetween2d(new Vector2(lookAtVector.x,lookAtVector.z), new Vector2(rotVect.x,rotVect.z));
	// 			
	// 			int angleSign = 0;
	// 			if(rotAngle < 0){
	// 				angleSign = -1;
	// 			}
	// 			else if(rotAngle > 0){
	// 				angleSign = 1;
	// 			}
	// 			
	// 			float angleToRotate = angularSpeed * (newTime-prevTime);
	// 			if(Mathf.Abs(angleToRotate) > Mathf.Abs(rotAngle)){
	// 				angleToRotate = rotAngle;
	// 			}
	// 			
	// 			angleToRotate = angleToRotate*angleSign;
	// 			if(currentCorner == i2){
	// 				angleToRotate = 0f;
	// 			}
	// 			
	// 		//	Debug.Log(angularSpeed * (newTime-prevTime));
	// 			rotVect = RotAround(angleToRotate, rotVect, new Vector3(0f,1f,0f));
	// 			
	// 			ag_transform.rotation = ag_transform.rotation * Quaternion.Euler(0f,angleToRotate,0f);
			
			}
		
		}
		
		return nextStep;
//  		currentCorner = i2-1;
//  		if(currentCorner < 0){
//  			currentCorner = 0;
//  		}
//  		prevSumDist = sumDist;
//		lastTime = newTime;
		
	}

	





	
	
	public bool isInDestination(){
		bool inDestination = false;
		if((ag_transform.position-destination).magnitude < stopDistance){
			inDestination = true;
		}
		return inDestination;
	}
	
	
	
	
	
	
	public Vector3 TerrainVector(Vector3 origin){
		Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
		float y1 = rtsm.manualTerrain.SampleHeight(planeVect);
		
		Vector3 tv = new Vector3(origin.x, y1, origin.z);
		return tv;
	}
	
	
	float SignedAngleBetween2d(Vector2 a, Vector2 b){
        
        
    	float angle = Vector2.Angle(a,b);
    	
    	
    	float sign = Mathf.Sign(a.y*b.x-a.x*b.y);

    // angle in [-179,180]
    	float signed_angle = angle * sign;

    // angle in [0,360] (not used but included here for completeness)
     //   float angle360 =  (signed_angle + 180) % 360;

    	return signed_angle;
	}
	
	
	Vector3 RotAround(float rotAngle, Vector3 original, Vector3 direction){
	    
	    
	    
	    Vector3 cross1 = Vector3.Cross(original,direction);
	    
	    Vector3 pr = Vector3.Project(original,direction);
	    Vector3 pr2 = original - pr;
	    
	    
	    Vector3 cross2 = Vector3.Cross(pr2,cross1);
	    
	    Vector3 rotatedVector = (Quaternion.AngleAxis( rotAngle, cross2)*pr2)+pr;
	    
	
		return rotatedVector;
	
	}

	
	
	
	
}


