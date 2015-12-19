using UnityEngine;
using System.Collections;

public class RPGCamera : MonoBehaviour {
	
	public UnitPars followPars = null;
	public Transform objectToFollow = null;
	public CameraLooker camLook = null;
	
	public RTSMaster rtsm = null;
	public Transform camTransform = null;
	
	public float distance = 50f;
	public float distanceOffset = 0f;
	
	public float hAngleOffest = 0f;
	public float vAngleOffest = 0f;
	
	void Awake(){
		camTransform = Camera.main.transform;
	}
	
	void Start () {
	
	}
	
	public void SetLooker(UnitPars follower){
		camLook = new CameraLooker();
		followPars = follower;
		objectToFollow = follower.transform;
		camTransform = Camera.main.transform;
		distance = 5*follower.thisNMA.radius;
		distanceOffset = 0f;
		hAngleOffest = 0f;
	    vAngleOffest = 0f;
	}
	
	void Update () {
		camLook.LookAtTransform(camTransform, objectToFollow.transform.position, distance+distanceOffset, -objectToFollow.rotation.eulerAngles.y+hAngleOffest, -25f+vAngleOffest);
	//	hAngleOffest = 0f;
	//	vAngleOffest = 0f;
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			if((distance+distanceOffset) > followPars.thisNMA.radius){
				distanceOffset = distanceOffset - 0.05f*(distance+distanceOffset);
			}
		}
		else if(Input.GetAxis("Mouse ScrollWheel") < 0){
			if((distance+distanceOffset) < 300f){
				distanceOffset = distanceOffset + 0.05f*(distance+distanceOffset);
			}
		}
		
		if (Input.GetKey("d")){
			hAngleOffest = hAngleOffest+1f;
			if(hAngleOffest > 360f){
				hAngleOffest = hAngleOffest-360f;
			}
		}
		if(Input.GetKey("a")){
			hAngleOffest = hAngleOffest-1f;
			if(hAngleOffest < -360f){
				hAngleOffest = hAngleOffest+360f;
			}
		}
		if (Input.GetKey("s")){
			if((vAngleOffest-25f) < 0f){
				vAngleOffest = vAngleOffest+1f;
			}
		}
		if(Input.GetKey("w")){
			if((vAngleOffest-25f) > -90f){
				vAngleOffest = vAngleOffest-1f;
			}
		}
		
	}
}
