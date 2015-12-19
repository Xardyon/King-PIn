using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionMark : MonoBehaviour {
    [HideInInspector] public List<Texture2D> texture = new List<Texture2D>();
    [HideInInspector] public List<Texture2D> healthBar = new List<Texture2D>();
//    private Vector2 offset;
    private float scale;
    
    private float screenHeight;
    
    private bool unlocked = false;
    
    private int colorsCount = 31;
    
    private int healthBarLevels = 60;
    
    private int selMarkMode = 0;
    

    
 //   private Texture2D tex;
    
//    [HideInInspector] public List<Transform> selectedGoT;
    [HideInInspector] public List<UnitPars> selectedGoPars;
    
//    public int number_Selected_Objects = 0;
    
    [HideInInspector] public float remainingSelectedHealth = 0;
    [HideInInspector] public float totalSelectedHealth = 0;
    
    [HideInInspector] public RTSMaster rtsm;
    
	// Use this for initialization
	void Awake () {
  //      rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
  //      rtsm.selectionMark = this;
	    
	    if(texture.Count < colorsCount){
	    	BuildSelectionMarkTextures();
	    }
	    if(healthBar.Count < colorsCount){
	    	BuildHealthBarTextures();
	    }

		screenHeight = Screen.height;
		unlocked = true;
		
	}
	
	public void BuildSelectionMarkTextures(){
		texture.Clear();
		for(int i=0; i<colorsCount; i++){
	        float j = 0f;
	        if(i<0.5f*(colorsCount-1)){
	            j = 2.0f*i/(colorsCount-1);
	    		texture.Add(SetTextures(new Color(1f, j, 0f, 1f)));
	    	}
	    	else if(i==0.5f*(colorsCount-1)){
	    		texture.Add(SetTextures(new Color(1f, 1f, 0f, 1f)));
	    	}
	    	else{
	    	    j = 2.0f*((colorsCount-1)-i)/(colorsCount-1);
	    		texture.Add(SetTextures(new Color(j, 1f, 0f, 1f)));
	    	}
	    }
	    
	    
	}
	
	public void BuildHealthBarTextures(){
		healthBar.Clear();
		for(int i=0; i<healthBarLevels; i++){
	    	healthBar.Add(healthBarSetup(i, healthBarLevels));
	    }
	}
	
	
	void Update(){
		if (Input.GetKeyDown (KeyCode.H)){
			selMarkMode++;
			if(selMarkMode>3){
				selMarkMode = 0;
			}
		}
		
		remainingSelectedHealth = 0f;
		totalSelectedHealth = 0f;
		for(int i=0; i<selectedGoPars.Count; i++){
			remainingSelectedHealth = remainingSelectedHealth+selectedGoPars[i].health;
			totalSelectedHealth = totalSelectedHealth+selectedGoPars[i].maxHealth;
		}
//		if(rtsm.buildDiplomacyMenu.go_selectedObjectInfo.activeSelf == true){	
		if(rtsm.selectedObjectInfo.selectedObjectInfo_barRed.buttonGo.activeSelf == true){
//			rtsm.buildDiplomacyMenu.sld_selectedObjectInfo.value = remainingSelectedHealth/totalSelectedHealth;
			rtsm.selectedObjectInfo.SetHealth(remainingSelectedHealth/totalSelectedHealth);
		}
	}
	
	Texture2D healthBarSetup(int step, int maxStep){
	       int res = 256;
	       Color color = Color.green;
	        
		   Texture2D textureLoc = new Texture2D(res, res, TextureFormat.ARGB32, false);
		   
		   for(int i=0; i<256; i++){
		        for(int j=0; j<256; j++){
					textureLoc.SetPixel(i, j, Color.clear);
				}
			}
			
			for(int i=0; i< res; i++){
		        for(int j=(int)(0.9f*res); j< res; j++){
		  //      for(int j=res; j> (1f-0.1f)*res; j--){
		            if(1.0f*i/res < 1.0f*step/(maxStep-1)){
		            	color = Color.green;
		            }
		            else{
		            	color = Color.red;
		            }
					textureLoc.SetPixel(i, j, color);
				}
			}
			textureLoc.Apply();
			return textureLoc;
			
	}	
	
	Texture2D SetTextures(Color color){
	        int res = 256;
	        
		    Texture2D textureLoc = new Texture2D(res, res, TextureFormat.ARGB32, false);
		    for(int i=0; i<256; i++){
		        for(int j=0; j<256; j++){
					textureLoc.SetPixel(i, j, Color.clear);
				}
			}
		    
		    
	////////////////////////////////////////////////////				    
		    
		    
		    for(int i=0; i< 0.2f*res; i++){
		        for(int j=0; j< 0.06f*res; j++){
					textureLoc.SetPixel(i, j, color);
				}
			}
			
			for(int i=res; i>res*(1f-0.2f); i--){
		        for(int j=0; j<0.06f*res; j++){
					textureLoc.SetPixel(i, j, color);
				}
			}
			
			
			
			for(int i=0; i< 0.2f*res; i++){
		        for(int j=res; j> (1f-0.06f)*res; j--){
					textureLoc.SetPixel(i, j, color);
				}
			}
			
			for(int i=res; i>res*(1f-0.2f); i--){
		        for(int j=res; j> (1f-0.06f)*res; j--){
					textureLoc.SetPixel(i, j, color);
				}
			}
			
			
	////////////////////////////////////////////////////		
			
			
			
			for(int i=0; i< 0.2f*res; i++){
		        for(int j=0; j< 0.06f*res; j++){
					textureLoc.SetPixel(j, i, color);
				}
			}
			
			for(int i=res; i>res*(1f-0.2f); i--){
		        for(int j=0; j<0.06f*res; j++){
					textureLoc.SetPixel(j, i, color);
				}
			}
			
			
			
			for(int i=0; i< 0.2f*res; i++){
		        for(int j=res; j> (1f-0.06f)*res; j--){
					textureLoc.SetPixel(j, i, color);
				}
			}
			
			for(int i=res; i>res*(1f-0.2f); i--){
		        for(int j=res; j> (1f-0.06f)*res; j--){
					textureLoc.SetPixel(j, i, color);
				}
			}
			
			
			
			
			
			
			textureLoc.Apply();
			return textureLoc;
			
	}
	
	
	
	
	void OnGUI(){
	    
	    if(unlocked == true){
	        screenHeight = Screen.height;
	        int index = 0;
	        int indexHb = 0;
			Camera camera = Camera.main;
			Vector3 screenPos;
			
			
			
			for(int i=0; i<selectedGoPars.Count; i++){  
			//  var textu = texture;
				  screenPos = camera.WorldToScreenPoint(selectedGoPars[i].transform.position);
				  if(screenPos.z < 52*selectedGoPars[i].rEnclosed){
					scale = 1000f*selectedGoPars[i].rEnclosed/screenPos.z;
				  }
				  else{
					scale = 1000f*selectedGoPars[i].rEnclosed/(52f*selectedGoPars[i].rEnclosed);
				  }

				  if(selectedGoPars[i].health < 0){
					 index = 0;
					 indexHb = 0;
				 
				  }
				  else if(selectedGoPars[i].health > selectedGoPars[i].maxHealth){
					 index = colorsCount-1;
					 indexHb = healthBarLevels-1;
				  }
				  else{
					 index = (int) ((selectedGoPars[i].health / selectedGoPars[i].maxHealth)*(colorsCount-1));
					 indexHb = (int) ((selectedGoPars[i].health / selectedGoPars[i].maxHealth)*(healthBarLevels-1));
				  }
				  if(selMarkMode == 0){
					GUI.Label (new Rect(screenPos.x-0.5f*scale, screenHeight-(screenPos.y+0.5f*scale), scale, scale), texture[index]);
					GUI.Label (new Rect(screenPos.x-0.5f*scale, screenHeight-(screenPos.y+0.7f*scale), scale, scale), healthBar[indexHb]);
				  }
				  else if(selMarkMode == 1){
					GUI.Label (new Rect(screenPos.x-0.5f*scale, screenHeight-(screenPos.y+0.5f*scale), scale, scale), texture[index]);
				  }
				  else if(selMarkMode == 2){
					GUI.Label (new Rect(screenPos.x-0.5f*scale, screenHeight-(screenPos.y+0.7f*scale), scale, scale), healthBar[indexHb]);
				  }
				  else if(selMarkMode == 3){
			  
				  }
			  
			}
		}  
		  
	
	}
}
