using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour {
	public UnitPars folowPars;
	
	public Vector3 lastRTSposition;
	public Quaternion lastRTSrotation;
	
	public RTSMaster rtsm;
	
	public RTSCamera rtsCam;
	public RPGCamera rpgCam;
	
	public Transform camTransform;
	
	public int mode = 1;
	
	void Start () {
	    
	    rtsCam = rtsm.rtsCamera;
	    rpgCam = rtsm.rpgCamera;
	    camTransform = Camera.main.transform;
	    
//	    LookAtTransform(Camera.main.transform, new Vector3(585f,6.5f,225f), 80f, 0f, -25f);
// 	    PosRot look = LookAt(new Vector3(585f,6.5f,225f), 50f, 30f, -45f);
// 		Camera.main.transform.position = look.position;
// 		Camera.main.transform.rotation = look.rotation;
	}
	
	public void FlipSwitcher(UnitPars follower){
		if(mode == 1){
			folowPars = follower;
			SwitchToRPG(follower);
		}
		else if(mode == 2){
			SwitchToRTS();
		}
	}
	
	public void SwitchToRPG(UnitPars follower){
	    rtsCam.enabled = false;
		lastRTSposition = camTransform.position;
		lastRTSrotation = camTransform.rotation;
	//	rpgCam.objectToFollow = follower;
		rpgCam.SetLooker(follower);
		rpgCam.enabled = true;
		mode = 2;
	}
	
	public void SwitchToRTS(){
	    rpgCam.enabled = false;
		camTransform.position = lastRTSposition;
		camTransform.rotation = lastRTSrotation;
		rtsCam.enabled = true;
		mode = 1;
	}
	
	public void ResetFromUnit(UnitPars cand){
		if(mode == 2){
			if(cand == folowPars){
				SwitchToRTS();
			}
		}
	}
	
}


public class CameraLooker{

	public void LookAtTransform(Transform transf, Vector3 source, float dist, float hRot, float vRot){
		PosRot look = LookAt(source, dist, hRot, vRot);
		transf.position = look.position;
		transf.rotation = look.rotation;
	}
	
	PosRot LookAt(Vector3 source, float dist, float hRot, float vRot){
//		Quaternion finalRot = Quaternion.identity;
		Vector3 norm = new Vector3(0f,0f,1f);
		norm = RotAround(vRot, norm, new Vector3(1f,0f,0f));
		norm = RotAround(hRot, norm, new Vector3(0f,1f,0f));
		
		norm = norm.normalized;
		
		Vector3 finalPos = source - dist*norm;
		Quaternion finalRot = Quaternion.Euler(-vRot,-hRot,0f);
		
		PosRot pr = new PosRot();
		pr.position = finalPos;
		pr.rotation = finalRot;
		
		return pr;
		
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


public class PosRot {
	public Vector3 position = Vector3.zero;
	public Quaternion rotation = Quaternion.identity;
}