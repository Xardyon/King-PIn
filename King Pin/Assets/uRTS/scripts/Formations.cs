using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Formations : MonoBehaviour {
	public Texture2D formationMask;
	public RTSMaster rtsm;
	
	public List<Formation> unitFormations = new List<Formation>();
	
	
	void Start(){
		StartCoroutine(RefreshFormation());
	}
	
	
	
	public void CreateNewStrictFormation(List<UnitPars> ups){
		Formation form = new Formation();
		form.rtsm = rtsm;
		rtsm.formations.AddUnitsToFormations(ups,form,true);
		
		form.GetDestinations(ups, form.CurrentMassCentre(ups));
	}
	
	
	
	public IEnumerator RefreshFormation(){
		while(true){
			for(int i=0; i<unitFormations.Count; i++){
				Formation form = unitFormations[i];
				for(int j=0; j<form.units.Count; j++){
					UnitPars up = form.units[j];
			//		up.MoveUnit(form.positions[j], "Walk");
			        if(up.militaryMode == 10){
						rtsm.unitsMover.AddMilitaryAvoider(up,form.positions[j],0);
					}
					if(up.militaryMode == 30){
						form.MoveMassCentreOnly();
					}
				}
			}
			
			yield return new WaitForSeconds(3f);
		}
	}
	
	
	public void AddUnitsToFormationsSC(List<UnitPars> ups, Formation form){
		List<UnitPars> ups1 = new List<UnitPars>();
		for(int i=0; i<ups.Count; i++){
		    UnitPars up = ups[i];
			if(up.formation != null){
				if(up.formation.strictMode != 1){
					ups1.Add(up);
				}
			}
			else{
				ups1.Add(up);
			}
		}
		AddUnitsToFormations(ups1,form);
	}
	
	
	
	public void AddUnitsToFormations(List<UnitPars> ups, Formation form){
		RemoveUnitsFromAnyFormation(ups);
		
		if(form.units == null){
			form.units = ups;
			for(int i=0;i<ups.Count;i++){
				ups[i].formation = form;
			}
		}
		else{
			for(int i=0;i<ups.Count;i++){
				AddUnitToFormations(ups[i], form);
			}
		}
		
		if(!unitFormations.Contains(form)){
			unitFormations.Add(form);
		}
		
	}
	
	
	public void AddUnitsToFormations(List<UnitPars> ups, Formation form, bool strictMode){
		AddUnitsToFormations(ups,form);
		if(strictMode){
			form.strictMode = 1;
		}
	}
	
	
	public void AddUnitToFormations(UnitPars up, Formation form){
		form.units.Add(up);
		up.formation = form;
	}
	
	
	
	public void RemoveUnitsFromAnyFormation(List<UnitPars> unitsToRemove){
		for(int i=0;i<unitsToRemove.Count;i++){
			RemoveUnitFromFormation(unitsToRemove[i]);
		}
	}
	
	public void RemoveUnitFromFormation(UnitPars up){
		if(up.formation != null){
			up.formation.units.Remove(up);
			if(up.formation.units.Count == 0){
				unitFormations.Remove(up.formation);
			}
			up.formation = null;
		}
	}
	
	public void AddToFormations(Formation form){
		unitFormations.Add(form);
	}
	
	public void RemoveFromFormations(Formation form){
		unitFormations.Remove(form);
	}
}


[System.Serializable]
public class Formation {
	public List<UnitPars> units;
	public float size;
	
	public List<Vector3> positions;
	
	Vector3 currentCentre;
	Vector3 destination;
	
	Vector3 formationDirection;
	
	public RTSMaster rtsm;
	
	public Texture2D formationMask;
	
	
	public int strictMode = 0;
	
	
	
	
	public void LoadFormation(List<UnitPars> units1){
	//	units = units1;
		if(rtsm.formations.formationMask != null){
			formationMask = rtsm.formations.formationMask;
		}
		else{
			formationMask = CirclePatern(64);
		}
		size = GetMaxUnitSize();
		if(rtsm == null){
			rtsm = units[0].rtsm;
		}
		
	}
	
	public float GetMaxUnitSize(){
	    float maxSize = 0f;
		for(int i=0;i<units.Count;i++){
			if(units[i].thisNMA.radius > maxSize){
				maxSize = units[i].thisNMA.radius;
			}
		}
		return maxSize;
	}
	
	
	public void MoveMassCentreOnly(){
		destination = CurrentMassCentre();
		CalculateSquad(destination);
	}
	
	
// 	public void LoadDestination(Vector3 dest){
// 		
// 	}
	
// 	public List<Vector3> GetDestinations(List<UnitPars> units1, Vector3 curCentre, Vector3 dest){
// 		destination = dest;
// 		LoadFormation(units1);
// 		currentCentre = curCentre;
// 		CalculateSquad(dest);
// 		return positions;
// 	}
	
	public List<Vector3> GetDestinations(List<UnitPars> units1, Vector3 dest){
		destination = dest;
		LoadFormation(units1);
		currentCentre = CurrentMassCentre();
		formationDirection = (destination - currentCentre).normalized;
		CalculateSquad(dest);
		return positions;
	}
	
	public void CalculateSquad(Vector3 dest){
		int nX = (int)(Mathf.Sqrt(1f*units.Count)+0.99f);
	//	int count = units.Count;
		
		int nAcceptedPositions = 0;
		int whileControl = 0;
		
		while((nAcceptedPositions < units.Count)&&(whileControl<1000)){
		    whileControl = whileControl+1;
			nAcceptedPositions = 0;
			
			
		
			List<Vector2> positions2d = new List<Vector2>();
			positions = new List<Vector3>();
		
			Vector2 dest2d = new Vector2(dest.x, dest.z);
		
			float destAngle = AngleFromXAxis(formationDirection);
		
			for(int i=0;i<nX;i++){
				for(int j=0;j<nX;j++){
					Vector2 pos = 4f*size*(new Vector2(1f*i - 0.5f*nX, 1f*j - 0.5f*nX));
					pos = RotAround2d(destAngle,pos);
					pos = pos+dest2d;
					
					Vector3 v3d = TerrainVector(new Vector3(pos.x, 0f, pos.y));
					
// 					if(whileControl == 1){
// 						Debug.Log(MinimumReachDistance(currentCentre, v3d));
// 						Debug.Log(4f*size);
// 					}
					
					if(MinimumReachDistance(currentCentre, v3d) < 4f*size){
						float rr5 = GetPointBrightness(i, j, nX);
						if(rr5 > 0.5f){
							nAcceptedPositions = nAcceptedPositions+1;
						
							positions2d.Add(pos);
						}
						
						
				//		Debug.Log(GetPointBrightness(i, j, nX));
				//		if(formationMask == null){
				//			Debug.Log("qqq");
				//		}
					}
				}
			}
		
			for(int i=0;i<positions2d.Count;i++){
				Vector3 v3d = TerrainVector(new Vector3(positions2d[i].x, 0f, positions2d[i].y));
				positions.Add(v3d);
			}
			
			nX = nX+1;
			
		}
		
	}
	
	
// 	public void CalculateCircle(Vector3 dest){
// 		int nX = (int)()
// 	}
	
	
	
	
	public Vector3 CurrentMassCentre(){
		Vector3 massCentre = new Vector3(0f,0f,0f);
		for(int i=0;i<units.Count;i++){
			massCentre = massCentre + units[i].transform.position;
		}
		massCentre = massCentre/units.Count;
		return massCentre;
	}
	
	public Vector3 CurrentMassCentre(List<UnitPars> ups){
		Vector3 massCentre = new Vector3(0f,0f,0f);
		for(int i=0;i<ups.Count;i++){
			massCentre = massCentre + ups[i].transform.position;
		}
		massCentre = massCentre/units.Count;
		return massCentre;
	}
	
	
	float MinimumReachDistance(Vector3 source, Vector3 dest){
		// calculates minimum distance which unit can reach by walking on NavMesh
//		NavMeshPath path = new NavMeshPath();
		float minDistance = 100000f;
		
		NavMeshHit hit;
		NavMesh.SamplePosition(dest, out hit, 4f*size, NavMesh.AllAreas);
		
		
	//	bool np = NavMesh.CalculatePath(source, dest, NavMesh.AllAreas, path);
	//	Vector3 lastPoint = path.corners[path.corners.Length-1];
	//	Vector3 lastPoint = path.corners[0];
	
	//    Debug.Log(np);
		
		Vector3 lastPoint = hit.position;
		
		minDistance = (dest-lastPoint).magnitude;
		
		return minDistance;
	}
	
	
	
	public Texture2D CirclePatern(int resol){
		Texture2D textur = new Texture2D(resol,resol);
		float R2 = 0.25f*resol*resol;
		for (int x = 0; x < textur.height; x++) {
            for (int y = 0; y < textur.width; y++) {
            	
            	if((x-0.5f*resol)*(x-0.5f*resol)+(y-0.5f*resol)*(y-0.5f*resol)<R2){
            		textur.SetPixel(x, y, Color.white);
            	}
            	else{
            		textur.SetPixel(x, y, Color.black);
            	}
            	
            }
        }
        textur.Apply();
        return textur;
	}
	
	
	
	
	public float GetPointBrightness(int ptX, int ptY, int resol){
		float ratMask = 1f*formationMask.width/resol;
		
		int ptXmin = (int)(ratMask*(ptX-0.5f));
		int ptXmax = (int)(ratMask*(ptX+0.5f));
		if(ptXmin < 0){
			ptXmin = 0;
		}
		if(ptXmax > (formationMask.width)){
			ptXmax = formationMask.width;
		}
		
		int ptYmin = (int)(ratMask*(ptY-0.5f));
		int ptYmax = (int)(ratMask*(ptY+0.5f));
		
		if(ptYmin < 0){
			ptYmin = 0;
		}
		if(ptYmax > (formationMask.width)){
			ptYmax = formationMask.width;
		}
		
		float avBrightness = 0f;
		int countBrightness = 0;
		
		for(int x=ptXmin; x<ptXmax; x++){
			for(int y=ptYmin; y<ptYmax; y++){
				Color col = formationMask.GetPixel(x,y);
				float brightness = (col.r + col.g + col.b) / 3f;
				avBrightness = avBrightness + brightness;
				countBrightness = countBrightness + 1;
			}
		}
		
//		Debug.Log((avBrightness).ToString()+" "+(ptXmin).ToString()+" "+(ptXmax).ToString()+" "+(ptYmin).ToString()+" "+(ptYmax).ToString()+" "+(countBrightness).ToString()+" ");
		
		avBrightness = avBrightness/countBrightness;
		
		return avBrightness;
	}
	
	
	
	
	float AngleFromXAxis(Vector3 origin){		
//		Vector3 originPlaneVector = new Vector3(origin.x ,0f ,origin.z);
		
		return (-AngleBetween2d360(new Vector2(1f,0f), new Vector2(origin.x,origin.z)));
	//	return (Vector3.Angle(new Vector3(1f,0f,0f), new Vector3(origin.x ,0f ,origin.z)));
	}
	
	
	
	
	
	
	
	Vector2 RotAround2d(float angle, Vector2 v){
         return Quaternion.Euler(0f, 0f, angle) * v;
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
	
	
	float SignedAngleBetween2d(Vector2 a, Vector2 b){
        
        
    	float angle = Vector2.Angle(a,b);
    	
    	
    	float sign = Mathf.Sign(a.y*b.x-a.x*b.y);

    // angle in [-179,180]
    	float signed_angle = angle * sign;

    // angle in [0,360] (not used but included here for completeness)
     //   float angle360 =  (signed_angle + 180) % 360;

    	return signed_angle;
	}
	
	float AngleBetween2d360(Vector2 a, Vector2 b){
        
        
    	float angle = Vector2.Angle(a,b);
    	
    	
    	float sign = Mathf.Sign(a.y*b.x-a.x*b.y);

    // angle in [-179,180]
    	float signed_angle = angle * sign;

    // angle in [0,360] (not used but included here for completeness)
        float angle360 =  (signed_angle + 180) % 360;

    	return angle360;
	}

	
	
}
