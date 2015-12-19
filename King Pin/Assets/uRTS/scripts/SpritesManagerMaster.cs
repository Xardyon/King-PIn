using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;



public class SpritesManagerMaster : MonoBehaviour {
	

	
	[HideInInspector] public List<int> numberOfColumns;// = new List<List<int>>();
	[HideInInspector] public List<int> numFrames;// = new List<List<int>>();

	
	[HideInInspector] public List<int> horRotLevels; // = new List<List<int>>();
	[HideInInspector] public List<int> vertRotLevels;// = new List<List<int>>();
	
	[HideInInspector] public List<int> animIndOffset;
	
	[HideInInspector] public List<int> repeatableAnimations;// = new List<List<int>>();
	
	public List<string> animationName; // = new List<List<string>>();
	public List<string> modelName;
	
	
//	public GameObject pref;
	private GameObject go;
	
	
	private UnitPars goC;
	private SpriteLoader sl;
	
	[HideInInspector] public List<GameObject> smGoArray;
	[HideInInspector] public List<LinkedSpriteManager> smArray;
	
	[HideInInspector] public List<int> smNumArray;
	
	[HideInInspector] public List<UnitPars> clientSpritesGo;
	
	private FPSSelfRegulator fpsReg;
	
	
	[HideInInspector] public float maxPerfCount = 100;
	
	private int nAnim = 0;
	
	private Vector3 camPosition;
//	private LinkedSpriteManager currentLsm = null;
	
	
	[HideInInspector] public RTSMaster rtsm = null;

	
	
	
	
	void Awake(){
		
		
//	  rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
//	  rtsm.spritesManagerMaster = this;	

//	  animationName.Add(new List<string>());
	  
//	  numberOfColumns.Add(new List<int>());
//	  numFrames.Add(new List<int>());
	  
//	  horRotLevels.Add(new List<int>());
//	  vertRotLevels.Add(new List<int>());
		
		
	  string filePath = "3dSprites/";
	  string fileName = "directories";

	  TextAsset textRes = Resources.Load<TextAsset>(filePath+fileName);
		
	  StringReader textReader = new StringReader(textRes.text);
		
	  string snumEntries = textReader.ReadLine();
	  
	  int numEntries = int.Parse(snumEntries);
	  nAnim = numEntries;
	  
	  for(int i=0;i<numEntries;i++){
	  	string entrie = textReader.ReadLine();
	  	modelName.Add(entrie);
	  	entrie = textReader.ReadLine();
	  	animationName.Add(entrie);
	  }
	  
	  	
		

	   
	   for(int ia=0;ia<=nAnim;ia++){
	   		animIndOffset.Add(0);
	   }
	   
	   for(int ia=0;ia<nAnim;ia++){
	   		numberOfColumns.Add(6);
	   		numFrames.Add(24);
	   
	   		horRotLevels.Add(32);
	   		vertRotLevels.Add(8);
	   		
	   		repeatableAnimations.Add(1);
	   		
	   }
		

	   this.gameObject.name = "SMM";
	   
	   
	   fpsReg = rtsm.fpsSelfRegulator;
	   //GameObject.Find("PerformanceManager").GetComponent<FPSSelfRegulator>();
	   fpsReg.smm = this;
	   
	   
	   ImportConfig();
	   
	   
	   animIndOffset[0] = 0;
	   
	   for(int ia=1;ia<=nAnim;ia++){

	        	animIndOffset[ia] = animIndOffset[ia-1]+vertRotLevels[ia-1]*horRotLevels[ia-1];

	    }
	   
	   for(int ia=0;ia<nAnim;ia++){
	        
	   		for(int j=0;j<vertRotLevels[ia];j++){
	    		int jj=j+1;
	    		for(int i=0;i<horRotLevels[ia];i++){
	        		int ii=i+1;
	        	
	        	


	        	
					InstMultipleSpriteManagers(ia, jj, ii);

	    		}
	    	}
	    }
	    

	    
	    
	}
	
	
	public IEnumerator CalcRotIndices(){
		
		int perfCount = 0;
		
		while(true){
			
			camPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
			
			Vector2 camForwardXZ = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
			
			float camVertRot = Camera.main.transform.rotation.eulerAngles.y;
			
			
			for(int i=0; i<clientSpritesGo.Count; i++){
			
				
				
				goC = clientSpritesGo[i];
				sl = goC.thisSL;
				//goC.GetComponent<SpriteLoader>();
		//		currentLsm = smArray[i];
				
				CheckBilboarding();
				
				
				
				if(sl.changingRotMode == false){
					
					perfCount = perfCount+1;
					
	    	
	  
	    	
	    	
	    	
	    	
					Vector2 goCamVec = new Vector2((goC.transform.position - camPosition).x, (goC.transform.position - camPosition).z);
					
					
	    	        float cameraAngleCorrection = SignedAngleBetween2d(camForwardXZ, goCamVec);
	    	
	    	

	    	
	    			int aId = ReturnAnimationId(sl.modelName, sl.animName);
	    			
	    			int finYindex = (int)(((goC.transform.rotation.eulerAngles.y-camVertRot)-cameraAngleCorrection)/(360f/horRotLevels[aId]));
	    			
	    			
	    			
	    
	    			if(finYindex>horRotLevels[aId]-1){
	    				finYindex = finYindex-horRotLevels[aId];
	    			}
	    			else if(finYindex<0){
	    				finYindex = finYindex+horRotLevels[aId];
	    				if(finYindex<0){
	    					finYindex = finYindex+horRotLevels[aId];
	    				}
	    		
	    			}
		

	    
// camera vertical rotation
        			Vector3 camPos = camPosition - goC.transform.position;
        			float vertAngle = Vector3.Angle(camPos, new Vector3(0f,1f,0f));
        
        
        			int vertRotIndexCam = vertRotLevels[aId]-1 - (int) (vertAngle / (90f/vertRotLevels[aId]));
        	
        			if(vertRotIndexCam<0){
        				vertRotIndexCam = 0;
        			}
        
					sl.animationToPlay = vertRotIndexCam*horRotLevels[aId]+finYindex+animIndOffset[aId];
				}
				
				perfCount = perfCount+1;
				
				if(perfCount > maxPerfCount){
					yield return new WaitForSeconds(0.02f);
					perfCount = 0;
				}
				
			}
			yield return new WaitForSeconds(0.02f);
		
		}
	
	
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
	
	
	
	
	
	
	void ImportConfig(){
		string [] lines = new string[7];
		
		for(int i=0;i<nAnim;i++){
			string locAnimationName = animationName[i];
			string filePath = "3dSprites/"+modelName[i] +"/"+locAnimationName+"/";
	        
	            
	       
	    	string fileName = "config";
		

			TextAsset textRes = Resources.Load<TextAsset>(filePath+fileName);

		
			StringReader textReader = new StringReader(textRes.text);
		
			lines[0] = textReader.ReadLine();
			lines[1] = textReader.ReadLine();
			lines[2] = textReader.ReadLine();
			lines[3] = textReader.ReadLine();
			
			lines[4] = textReader.ReadLine();
			lines[5] = textReader.ReadLine();
			lines[6] = textReader.ReadLine();
		
		
			numberOfColumns[i] = int.Parse(lines[0]);
			numFrames[i] = int.Parse(lines[1]);
			horRotLevels[i] = int.Parse(lines[2]);
			vertRotLevels[i] = int.Parse(lines[3]);
			
			
			
			repeatableAnimations[i] = int.Parse(lines[4]);
			
		}
		
	
		
	}
	
	
	

	
	
	void InstMultipleSpriteManagers(int ia, int jj, int ii){
				
	
				
			    go = new GameObject("SM_"+modelName[ia]+"_"+animationName[ia]+"_"+jj+"_"+ii);
			//    go = (GameObject)Instantiate(pref, pref.transform.position, pref.transform.rotation);
			    
	        	go.SetActive(false);
	        	
	        	smNumArray.Add(0);
	        	smGoArray.Add(go);
	        	
	     //   	go.transform.Translate(new Vector3(0f,9999f,0f));
	        	go.transform.position = new Vector3(0f,9999f,0f);
	        	

	        	
	        	go.name = "SM_"+modelName[ia]+"_"+animationName[ia]+"_"+jj+"_"+ii;
	        	
	   //     	LinkedSpriteManager lsm = go.GetComponent<LinkedSpriteManager>();
	        	LinkedSpriteManager lsm = go.AddComponent<LinkedSpriteManager>();
	     //   	lsm.Launch();
	        	smArray.Add(lsm);
	        	lsm.allocBlockSize = 1;
	        	lsm.autoUpdateBounds = true;
	        	lsm.enabled = true;
	        	
	        	string matfilePath = "3dSprites/"+modelName[ia] +"/"+animationName[ia]+"/mat/";
	        
	            
	            string matfileName = modelName[ia]+"_"+animationName[ia]+"_"+jj+"_"+ii;
	        	
	        	lsm.material = Resources.Load<Material>(matfilePath+matfileName);
	        	
	     //   	go.AddComponent<MeshRenderer>().renderer.material = Resources.Load<Material>(matfilePath+matfileName);
	    			
	        
	}
	
	
	
	public int ReturnAnimationId(string askedModel, string askedAnimation){
		int aId = -1;
		for(int ia=0;ia<nAnim;ia++){
		    if(modelName[ia]==askedModel){
				if(animationName[ia]==askedAnimation){
					aId = ia;
				}
			}
		}
		return aId;
	
	}
	
	
	
	public void CheckBilboarding(){
		if((camPosition-goC.transform.position).sqrMagnitude < sl.bilboardDistance*sl.bilboardDistance){
			if(sl.enabled == true){
			    sl.unsetSprite2();
			    
			    SetAnimation(sl, goC.rtsUnitId);
			    
			    
			}
			
		}
		else{
			if(sl.enabled == false){
			    sl.resetSprite();
				foreach (Transform child in goC.transform){
					child.gameObject.SetActive(false);
				}
				sl.enabled = true;
			}
		}
	}
	
	
	
	string[] GetClipsList(Animation anim){
	    List<string> tmpList = new List<string>();
	    
		foreach (AnimationState state in anim) {
			 // add name to tmpList
			 tmpList.Add(state.name);
		 }
		 string[] list = tmpList.ToArray();
		 return list;
	}
	
	
	
	
	// Use this for initialization
	void Start () {
		StartCoroutine(CalcRotIndices());
  
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	public void SetAnimation(SpriteLoader spL, int unitId){
	    
	    GameObject tgo = spL.gameObject;
	    
	    if(spL.enabled == false){
	        int iiii = animationName.IndexOf(spL.animName);
			if(iiii>=0){
				spL.repeatableAnimations = repeatableAnimations[iiii];
				if(repeatableAnimations[iiii] == 0){
					StartCoroutine(spL.SingleAnimCounter(0f));
				
				}
			}
			if(unitId == 15){
					
					
					
				foreach (Transform child in tgo.transform){
					child.gameObject.SetActive(false);
				}
			
				GameObject relevanChild = null;
				Animation relevantAnimation = null;
			
				foreach (Transform child in tgo.transform){
					GameObject tempChild = child.gameObject;
					
					if(tempChild.GetComponent<Animation>() != null){
						Animation tempAnim = tempChild.GetComponent<Animation>();
						foreach (AnimationState state in tempAnim) {
							if(state.name == spL.animName){
								relevanChild = tempChild;
								relevantAnimation = tempAnim;
							}
						}
					}
				}
			
				relevanChild.SetActive(true);
				if(spL.repeatableAnimations != 0){
					relevantAnimation.wrapMode = WrapMode.Loop;
					relevantAnimation[spL.animName].time = spL.t;
					relevantAnimation.Play(spL.animName);
				}
				else if(spL.repeatableAnimations==0){
					if(spL.lockDeathAnim < 2){
						relevantAnimation.wrapMode = WrapMode.Once;
						relevantAnimation[spL.animName].time = spL.t;
						relevantAnimation.Play(spL.animName);
					}
					else{
						relevantAnimation.wrapMode = WrapMode.Once;
						relevantAnimation[spL.animName].time = relevantAnimation[spL.animName].length;
						relevantAnimation.Play(spL.animName);
					}
				}
			
			
			
			
			}
			else{
				foreach (Transform child in tgo.transform){
					child.gameObject.SetActive(true);
				}
				if(tgo.GetComponent<Animation>() != null){
					Animation anim = tgo.GetComponent<Animation>();
					if(spL.repeatableAnimations != 0){
						anim.wrapMode = WrapMode.Loop;
						anim[spL.animName].time = spL.t;
						anim.Play(spL.animName);
					}
					else if(spL.repeatableAnimations==0){
						if(spL.lockDeathAnim < 2){
							anim.wrapMode = WrapMode.Once;
							anim[spL.animName].time = spL.t;
							anim.Play(spL.animName);
						}
						else{
							anim.wrapMode = WrapMode.Once;
							anim[spL.animName].time = anim[spL.animName].length;
							anim.Play(spL.animName);
						}
					}
				}
			
			}
		}
	}
	
	
	
}
