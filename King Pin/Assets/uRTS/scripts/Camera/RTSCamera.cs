using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {
	
	
	public float zoomSpeed = 1.0f;
	public float moveSpeed = 1.0f;
	public float rotationSpeed = 1.0f;
	

	private float ScrollEdge = 0.01f;

    public float minZoomDist = 20.0f;
    public float maxZoomDist = 500.0f;

    private float camHeight;

	private Transform camTransform;

	private float angleConv = 0f;
	private float sqrt2 = 0f;
	
	public RTSMaster rtsm;
	public Terrain terrain;
		
	public bool followHeightmap = true;


	
	void Awake(){
		terrain = rtsm.manualTerrain;
		camTransform = Camera.main.transform;
	}
	
	
	// Use this for initialization
	void Start () {
		
		angleConv = Mathf.PI/180.0f;
		sqrt2 = Mathf.Sqrt(2f);
	}
	
	// Update is called once per frame
	void Update () {
	    
	    bool terrainSwithced = false;
	    
	    Terrain terrain2 = GetTerrainBellow();
	    if((terrain2 != null)&&(terrain2 != terrain)){
	    	terrain = terrain2;
	    	terrainSwithced = true;
	    }

        float gameSpeed = Time.timeScale;
        float dTime = Time.deltaTime;
        float dT = dTime / gameSpeed;
        if(gameSpeed == 0){
        	dT = 0;
        }
        
        if(terrainSwithced == false){
			
			camHeight = camTransform.position.y - terrain.SampleHeight(camTransform.position) - terrain.transform.position.y;
	
	
			if (Input.GetKey("mouse 2")){

			}
	
			else{
				bool movePassed = false;
				if ( Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge) ){
		
					camTransform.Translate(new Vector3(
						dT * moveSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv),
						0f,
						dT * -moveSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv)),
					Space.World);
					movePassed = true;
				}

				else if ( Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge ){
			
					camTransform.Translate(new Vector3(
						dT * -moveSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv),
						0f,
						dT * moveSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv)),
					Space.World);
					movePassed = true;
				}

	

				if ( Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge) ){

					camTransform.Translate(new Vector3(
						dT * moveSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
						0f,
						dT * moveSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
					Space.World);
					movePassed = true;
				}

				else if ( Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge ){
					camTransform.Translate(new Vector3(
						dT * -moveSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
						0f,
						dT * -moveSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
					Space.World);
					movePassed = true;
				}
			
				if(movePassed == true){
					if(followHeightmap == true){
						camTransform.position = new Vector3(
							camTransform.position.x,
							terrain.SampleHeight(camTransform.position) + terrain.transform.position.y + camHeight,
							camTransform.position.z
						);
						
					}
				}
				

			}
		
			Vector3 fwd = camTransform.TransformDirection (Vector3.forward);
		
			if (Input.GetAxis("Mouse ScrollWheel")> 0){
		
				camTransform.Translate(new Vector3(
					2*dT * zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
					2*dT * -zoomSpeed *camHeight *Mathf.Sin((camTransform.transform.rotation.eulerAngles.x)*angleConv)*sqrt2,
					2*dT * zoomSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
					Space.World
				);
	
		
				if(camTransform.rotation.eulerAngles.x <= 180.0f){
					if (Physics.Raycast (camTransform.position, fwd, minZoomDist)) {
						camTransform.Translate(new Vector3(
							2*dT * -zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
							2*dT * zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.x)*angleConv)*sqrt2,
							2*dT * -zoomSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
							Space.World
						);
					}
				}
				if(camTransform.rotation.eulerAngles.x >= 180.0f){
					if (!(Physics.Raycast (camTransform.position, -fwd, maxZoomDist))) {
						camTransform.Translate(new Vector3(
							2*dT * -zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
							2*dT * zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.x)*angleConv)*sqrt2,
							2*dT * -zoomSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
							Space.World
						);
		  
					}
				}


			}
		
			if (Input.GetAxis("Mouse ScrollWheel")< 0){
	
		
				camTransform.Translate(new Vector3(
					2*dT * -zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
					2*dT * zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.x)*angleConv)*sqrt2,
					2*dT * -zoomSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)), 
					Space.World
				);
		

		
				if(camTransform.rotation.eulerAngles.x >= 180.0f){
					if (Physics.Raycast (camTransform.position, -fwd, minZoomDist)) {
		
						camTransform.Translate(new Vector3(
							2*dT * zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
							2*dT * -zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.x)*angleConv)*sqrt2,
							2*dT * zoomSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
							Space.World
						);
					}
				}	
		
				if(camTransform.rotation.eulerAngles.x <= 180.0f){
					if (!(Physics.Raycast (camTransform.position, fwd, maxZoomDist))) {
		
						camTransform.Translate(new Vector3(
							2*dT * zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
							2*dT * -zoomSpeed *camHeight *Mathf.Sin((camTransform.rotation.eulerAngles.x)*angleConv)*sqrt2,
							2*dT * zoomSpeed *camHeight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
							Space.World
						);
					}
				}
		


			}
			
	//		prevHeight = camHeight;
		}
		
		// ROTATION

    	if (Input.GetMouseButton(1)){
			float h = rotationSpeed * Input.GetAxis ("Mouse X");
			float v = rotationSpeed * Input.GetAxis ("Mouse Y");
			camTransform.Rotate (0, h, 0, Space.World);

			camTransform.Rotate (v, 0, 0);

			if((camTransform.rotation.eulerAngles.x >= 90) &&(camTransform.rotation.eulerAngles.x <= 180)){
	
				camTransform.Rotate (-v, 0, 0);
			}
			if(((camTransform.rotation.eulerAngles.x >= 180)&&(camTransform.rotation.eulerAngles.x <= 270))||(camTransform.rotation.eulerAngles.x < 0) ){
	
				camTransform.Rotate (-v, 0, 0);
			}


    		if((camTransform.rotation.eulerAngles.z >= 160)&&(camTransform.rotation.eulerAngles.z <= 200)){
    			camTransform.Rotate (-v, 0, 0);
    		}
		}
		
		
	}
	
	
	
// 	void MoveCamera(float speed, float cheight, float dTim){
// 		camTransform.Translate(new Vector3(
// 			dTim * speed *cheight *Mathf.Sin((camTransform.rotation.eulerAngles.y)*angleConv),
// 			0.0f,
// 			dTim * speed *cheight *Mathf.Cos((camTransform.rotation.eulerAngles.y)*angleConv)),
// 		Space.World);
// 	}
	
	
	
	Terrain GetTerrainBellow(){
	// Gets terrain bellow camera using raycast
		Terrain locTerrain = null;
		
		Ray ray = new Ray();
		ray.direction = new Vector3(0f,-1f,0f);
		ray.origin = camTransform.position;
		
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit)){
			if(hit.collider != null){
				if(hit.collider.gameObject.GetComponent<Terrain>()!=null){
					locTerrain = hit.collider.gameObject.GetComponent<Terrain>();
					
					camHeight = hit.distance;
			//		float height = locTerrain.SampleHeight(camTransform.position);
					
				}
			}
		}
		
		return locTerrain;
		
	}
	
	float AverageHeight(){
		float avHeight = 0f;
		
		float dx = 10f;
		
//		float simpleHeight = camTransform.position.y - terrain.SampleHeight(camTransform.position) - terrain.transform.position.y;
		
	//	avHeight = simpleHeight;
		
		float kk = 0f;
		
		for(int i=-1;i<2;i++){
			for(int j=-1;j<2;j++){
			    Vector3 pos = new Vector3(camTransform.position.x + i*dx, 0f, camTransform.position.z + j*dx);
				avHeight = avHeight + camTransform.position.y - terrain.SampleHeight(pos) - terrain.transform.position.y;
				kk=kk+1f;
			}
		}
		
		avHeight = avHeight/kk;
		
		return avHeight;
	}
	
	
}
