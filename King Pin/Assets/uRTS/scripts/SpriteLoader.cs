using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteLoader : MonoBehaviour {

	[HideInInspector] public int horRotLevels;
	[HideInInspector] public int vertRotLevels;
	
	


	private LinkedSpriteManager lsm;
	private SpritesManagerMaster spritesMaster;
	
	public float spriteSize = 2.0f;
	
	

	[HideInInspector] public int numberOfColumns;
	[HideInInspector] public int numFrames;
	
	[HideInInspector] public int repeatableAnimations;
	
	
	private int numberOfRows;
	

	
	private SpriteM ClientSprite;
	

	
	[HideInInspector] public int animationPlayed = 0;
	[HideInInspector] public int animationToPlay = 0;
	
	[HideInInspector] public int lockDeathAnim = 0;
	


	
	private Vector3 camPos;
	private Vector3 goPos;
	

	
	public string modelName = "knight";
	public string animName = "Walk";
	
	[HideInInspector] public string prevModelName;
	[HideInInspector] public string prevAnimName;
	
	
	
	private float startTime = 0f;
	private float scriptStartTime = 0f;
	

	
	private int updateId = 0;
	private int updateFrequency = 30;
	

	
	public bool changingRotMode = false;
	
	
	
	private float cameraAngleCorrection;
	
	private int smAnimId;
	private int minOffset;
	private int maxOffset;
	
	public float bilboardDistance = 20f;
	
	private bool useRemoval = true;
	
    [HideInInspector] public float t = 0f;
    
    [HideInInspector] public UnitPars spriteUP = null;
	
	
	void Starter(){

		
		prevModelName = modelName;
		prevAnimName = animName;
		
		GetMasterParams();
		
		
	    
	    numberOfRows = numFrames / numberOfColumns;

	    
	    AddSprite();

		

	}
	
	void LateUpdate(){
        
        
		updateId = updateId+1;
		if(updateId>=updateFrequency){
		    
			if(changingRotMode == false){
				
		
								updateId = 0;
	    
	    	
	    	    				UpdateReferencedMode();

	    	
	    	}

	    }

	    
	}
	

	
	
	void UpdateReferencedMode(){
	    	
	    	
	    	

	    	if(animationToPlay!=animationPlayed){
	    	
		        
	    		if(animationToPlay>=0){
	    		    if(prevModelName!=modelName){
	    		    	ReGetMasterParams();
	    		    	prevModelName=modelName;
	    		    	prevAnimName=animName;
	    		    }
	    		    if(prevAnimName!=animName){
	    		    	ReGetMasterParams();
	    		    	prevModelName=modelName;
	    		    	prevAnimName=animName;
	    		    }
	    		    smAnimId = spritesMaster.ReturnAnimationId(modelName, animName);
	    		    
	    		    minOffset = spritesMaster.animIndOffset[smAnimId];
	    		    maxOffset = spritesMaster.animIndOffset[smAnimId+1];
	    		    
	    		    if(animationToPlay>=minOffset){
	    				if(animationToPlay<maxOffset){
	    					changingRotMode = true;
	    					StartCoroutine(ChangeAnimationRefMode());
	    				}
	    			}
	    			else{
	    			
	    			}
	    		}
	    		else{
	    		
	    		}

	    	}
	}
	

	
	
	

	

	
	
	public IEnumerator ChangeAnimationRefMode(){
	    
		startTime = Time.realtimeSinceStartup - scriptStartTime;
		
		t = (startTime/numFrames-((int)(startTime/numFrames)));
				
				
				
				lsm = spritesMaster.smArray[animationPlayed];
      			
      			if(useRemoval == true){
        			lsm.RemoveSprite(ClientSprite);
        		}
        		
        		spritesMaster.smNumArray[animationPlayed] = spritesMaster.smNumArray[animationPlayed] - 1;
        		
        		
        		if(spritesMaster.smNumArray[animationPlayed] <= 0){
        			lsm.gameObject.SetActive(false);
        			if(spritesMaster.smNumArray[animationPlayed] < 0){
        				print(spritesMaster.smNumArray[animationPlayed]);
        			}
        		}
        		
        		
        		lsm = spritesMaster.smArray[animationToPlay];
        		
        		if(spritesMaster.smNumArray[animationToPlay] <= 0){
        			lsm.gameObject.SetActive(true);
        			if(spritesMaster.smNumArray[animationToPlay] < 0){
        				print(spritesMaster.smNumArray[animationToPlay]);
        			}
        		}
        		spritesMaster.smNumArray[animationToPlay] = spritesMaster.smNumArray[animationToPlay]+1;
        		
        		// new animation
        		
        		
        
        	    ClientSprite=(lsm.AddSprite(this.gameObject, spriteSize, spriteSize, new Vector2(0f,(1.0f-1.0f/numberOfRows)), new Vector2(1f/numberOfColumns,1.0f/numberOfRows), Vector3.zero, true));
		
				UVAnimation CurlAnim = new UVAnimation();
				
				
				
				CurlAnim.name = animName;
				CurlAnim.loopCycles = -1;
				
				CurlAnim.framerate = numFrames;
				if(repeatableAnimations==0){
					CurlAnim.loopCycles = 0;
				//	CurlAnim.framerate = (int)(numFrames*4);
				}
		
				CurlAnim.BuildUVAnim(new Vector2(0,(1.0f-1.0f/numberOfRows)), new Vector2(1f/numberOfColumns,1.0f/numberOfRows), numberOfColumns, numberOfRows, numFrames, numFrames);
		        
				ClientSprite.AddAnimation(CurlAnim);
				
				
				
				
			
				ClientSprite.PlayAnim(CurlAnim);
			
				if(repeatableAnimations==0){
					if(lockDeathAnim == 0){
						lockDeathAnim = 1;
						StartCoroutine(SingleAnimCounter(1f));
					}
				}
				
				if(repeatableAnimations!=0){
					ClientSprite.StepAnim(t*numFrames);
				}
				else if(lockDeathAnim == 2){
					ClientSprite.StepAnim(t*numFrames);
				}
				





        
        
        animationPlayed = animationToPlay;
        
        changingRotMode = false;
        
        useRemoval = true;
        
        yield return null;
        

	}
	
	void Start(){
	    Starter();
		scriptStartTime = Time.realtimeSinceStartup;
		
		spritesMaster.clientSpritesGo.Add(spriteUP);
	}
	
	
	public IEnumerator SingleAnimCounter(float tt){
		yield return new WaitForSeconds(tt);
		
		lockDeathAnim = 2;
	
	}
	
	
	
	public void GetMasterParams(){
	    RTSMaster rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
		spritesMaster = rtsm.spritesManagerMaster;
		//GameObject.Find("SMM").GetComponent<SpritesManagerMaster>();
		
		int aId = spritesMaster.ReturnAnimationId(modelName, animName);
		
		animationToPlay = spritesMaster.animIndOffset[aId];
		
		
		horRotLevels = spritesMaster.horRotLevels[aId];
		vertRotLevels = spritesMaster.vertRotLevels[aId];
		
		numberOfColumns = spritesMaster.numberOfColumns[aId];
		numFrames = spritesMaster.numFrames[aId];
		
		repeatableAnimations = spritesMaster.repeatableAnimations[aId];
	}
	
	public void ReGetMasterParams(){
	

		
		int aId = spritesMaster.ReturnAnimationId(modelName, animName);
		
		horRotLevels = spritesMaster.horRotLevels[aId];
		vertRotLevels = spritesMaster.vertRotLevels[aId];
		
		numberOfColumns = spritesMaster.numberOfColumns[aId];
		numFrames = spritesMaster.numFrames[aId];
		
		repeatableAnimations = spritesMaster.repeatableAnimations[aId];
	}
	

	
	public void AddSprite(){
				

				

	    		
	    

	    			GameObject go = this.gameObject;
				
				    int ind = animationToPlay;

				
					GameObject lsmgo = spritesMaster.smGoArray[ind];
				

					LinkedSpriteManager lsm = spritesMaster.smArray[ind];
				
	    		
	    		    
	    	
	    			if(lsmgo.activeSelf == false){
						lsmgo.SetActive(true);
					}
					spritesMaster.smNumArray[ind] = spritesMaster.smNumArray[ind]+1;
	    			ClientSprite = (lsm.AddSprite(go, spriteSize, spriteSize, new Vector2(0f,(1.0f-1.0f/numberOfRows)), new Vector2(1f/numberOfColumns,1.0f/numberOfRows), Vector3.zero, true));
		
					UVAnimation CurlAnim = new UVAnimation();
				
				
				
					CurlAnim.name = animName;
					CurlAnim.loopCycles = -1;
					CurlAnim.framerate = numFrames;
		            
// 		            if(animName=="Death"){
// 						CurlAnim.loopCycles = 0;
// 					}
		            
					CurlAnim.BuildUVAnim(new Vector2(0,(1.0f-1.0f/numberOfRows)), new Vector2(1f/numberOfColumns,1.0f/numberOfRows), numberOfColumns, numberOfRows, numFrames, numFrames);
		
					ClientSprite.AddAnimation(CurlAnim);
					
// 					if(animName=="Death"){
// 						ClientSprite.PlayAnim(CurlAnim);
// 					}
					
					animationPlayed = ind;

	}
	
	public void unsetSprite(){
		updateFrequency = 99999999;
		
		LinkedSpriteManager lsmLocal = spritesMaster.smArray[animationPlayed];
		lsmLocal.RemoveSprite(ClientSprite);
		spritesMaster.clientSpritesGo.Remove(spriteUP);
		this.enabled = false;
	}
	
	public void unsetSprite2(){
		updateFrequency = 99999999;
		
		LinkedSpriteManager lsmLocal = spritesMaster.smArray[animationPlayed];
		lsmLocal.RemoveSprite(ClientSprite);
	//	spritesMaster.clientSpritesGo.Remove(this.gameObject);
		this.enabled = false;
	}
	
	public void unsetSprite3(){
		updateFrequency = 99999999;
		
//		LinkedSpriteManager lsmLocal = spritesMaster.smArray[animationPlayed];
//		lsmLocal.RemoveSprite(ClientSprite);
		spritesMaster.clientSpritesGo.Remove(spriteUP);
		this.enabled = false;
	}
	
	
	public void resetSprite(){
	    updateFrequency = 30;
		changingRotMode = true;
		useRemoval = false;
		
//		LateUpdate();
	    StartCoroutine(ChangeAnimationRefMode());
	    this.enabled = true;
	    
	}


	
}
