using System.IO;
using UnityEngine;
#if UNITY_EDITOR  
#if ! UNITY_WEBPLAYER
using UnityEditor;
#endif
#endif
using System.Collections;
using System.Collections.Generic;



public class SpritesCreator : MonoBehaviour {
    
    public GameObject model = null;
    
    
    public string modelName = "knight";
    public string animationName = "Knight_Walk";
    
    public float objectSize = 6f;
    
  #if UNITY_EDITOR 
  #if ! UNITY_WEBPLAYER 
 //   private string objectName;
    
  #endif
  #endif
    
    private int counter = 0;
    
    public int spriteResolution = 128;
    public int numberOfColumns = 8;
    
    
    
    
    public int numberOfFrames = 24;
    
    public int horRotLevels = 4;
    private int hLevel = 0;
    
    public int verRotLevels = 3;
    private int vLevel = 0;
    
    private float Robj;
    
    private Vector3 camInitialPosition;
    private Quaternion camInitialRotation;
    
    
    private bool createSprite = true;
    
    private Texture2D tex;
    
    #if UNITY_EDITOR    
    #if ! UNITY_WEBPLAYER
    private Texture2D texfinMatr;
    #endif
    #endif
    
    public List<Color[]> pix = new List<Color[]>();

    
    private Color[] pixels;
    
    public bool invertSpriteY = true;
    
    public bool repeatAnimation = true;
    public bool movableSprite = true;
    public bool playAnimation = true;
    
	#if UNITY_EDITOR    
	#if ! UNITY_WEBPLAYER  
    private bool deleted = false;
    
    private int largDim;
    
 
    private int maxDim;
    #endif
    #endif
    
    

	void Awake () {
	    

	    
        camInitialPosition = Camera.main.transform.position;
        camInitialRotation = Camera.main.transform.rotation;
        
        Camera.main.orthographic = true;
        
	    pixels = new Color[spriteResolution*numberOfFrames*spriteResolution];
	    
		tex = new Texture2D (spriteResolution, spriteResolution, TextureFormat.ARGB32, false);
		

	    
	    Camera.main.orthographicSize = objectSize*128/spriteResolution;
	    
	    Robj = (Camera.main.transform.position - model.transform.position).magnitude;
	    
	    #if UNITY_EDITOR 
	    #if ! UNITY_WEBPLAYER  
//	    objectName = model.transform.name;
		#endif
		#endif
		
	#if UNITY_EDITOR  
	#if ! UNITY_WEBPLAYER
		texfinMatr = new Texture2D (numberOfColumns*spriteResolution, spriteResolution*numberOfFrames/numberOfColumns, TextureFormat.ARGB32, false);
	#endif
	#endif

		FindTextureMaxDimenion();
		
		RefreshDirectories();
		
		ExportConfig();
		
		

	}
	

	void Update () {
		
		if(counter<=numberOfFrames+1){
		    if(counter==0){
	      
		    }
			SpriteSetup();
		}
		else{
			if(hLevel<horRotLevels-1){
				counter = 0;
				hLevel = hLevel+1;
				
				model.transform.Rotate (0, 360f/horRotLevels, 0, Space.World);
				createSprite = true;
				
				pix.Clear();
			}
			else if(hLevel==horRotLevels-1){
				if(vLevel<verRotLevels-1){
					counter = 0;
					hLevel = 0;
					vLevel = vLevel+1;
				
					model.transform.Rotate (0, 360f/horRotLevels, 0, Space.World);
					createSprite = true;
				
					pix.Clear();
					
					Camera.main.transform.position = camInitialPosition;
					Camera.main.transform.rotation = camInitialRotation;
					
					float rotAngle = 90f*(vLevel)/(verRotLevels-1);
					
					Camera.main.transform.Translate (
					                                 0,
					                                 Robj*Mathf.Sin(rotAngle*Mathf.PI/180.0f),
					                                 Robj*(1f-Mathf.Cos(rotAngle*Mathf.PI/180.0f)),
					                                 Space.World);
					Camera.main.transform.Rotate (rotAngle, 0, 0, Space.World);
				}
			}
		}
	

	}
	
	
	void SpriteSetup(){
		if(numberOfFrames%numberOfColumns != 0){
	    	
	    	if(createSprite == true){
        	
 
        			print("Correct number of frames that sprite sheet would be fulfilled");
        			createSprite = false;
        	}
	    }
	    else{
	    
			counter = counter+1;
			

		
			if(counter<numberOfFrames+1){
    			
				model.GetComponent<Animation>().GetComponent<Animation>()[animationName].enabled = true;
	    		model.GetComponent<Animation>().GetComponent<Animation>()[animationName].weight = 1f;
	    		model.GetComponent<Animation>().GetComponent<Animation>()[animationName].time = ((float)(counter)/numberOfFrames)*model.GetComponent<Animation>().GetComponent<Animation>()[animationName].length;
	    		model.GetComponent<Animation>().GetComponent<Animation>()[animationName].speed = 0;    			
    			
    			StartCoroutine(MakeSprite());
    		

			}
        	if(counter>=numberOfFrames+1){
        		if(createSprite == true){
        	
 
        			MakeSpriteMatrix();
        			createSprite = false;
        		}
        	}
        }
	}
	
	
	

	
	public IEnumerator MakeSprite () {

        yield return new WaitForEndOfFrame();
        
        
		int width = Screen.width;
		int height = Screen.height;
		
		int centralW = (int)(width/2.0f);
		int centralH = (int)(height/2.0f);
		
        int xBeg = centralW-spriteResolution/2;
        int yBeg = centralH-spriteResolution/2;
        
        int xEnd = centralW+spriteResolution/2;
        int yEnd = centralH+spriteResolution/2;
        

		
			tex.ReadPixels (new Rect(xBeg, yBeg, xEnd, yEnd), 0, 0);
			tex.Apply ();

        	var pixels1 = tex.GetPixels();
        	

        	
        	pix.Add(pixels1);
        
       

	}
	
	
	
	
	void MakeSpriteMatrix(){

	    
	    int matrixRes = spriteResolution*numberOfColumns;
        
        
	    
		for(int i = 0; i < pixels.Length; i++){
			
		    
		    int ind1 = i+1;
		    int imax1 = matrixRes;
		    

		    int j1 = (ind1+imax1-1)/imax1;
		    int i1 = ind1 - (j1-1)*imax1;
		    

		    
		    
		    int i2 = ((i1-1)/spriteResolution)+1;
		    
		
		    int j2 = ((j1-1)/spriteResolution)+1;
		
		    int jinv2 = numberOfFrames/numberOfColumns-((j1-1)/spriteResolution)+1;
		
		    
		
		    int ind2 = (j2-1)*numberOfColumns + i2;
		
		    int indinv2 = (jinv2-2)*numberOfColumns + i2;
		
		   
		  
		    
		    
		    int iof2 = spriteResolution*(i2-1);
		    int jof2 = spriteResolution*(j2-1);
		    
		    int i3 = i1 - iof2;
		    int j3 = j1 - jof2;
		    
		    int ind3 = (j3-1)*spriteResolution + i3;
		    

			
			if(invertSpriteY==false){
				pixels[i]=pix[ind2-1][ind3-1];
			}
			else{
				pixels[i]=pix[indinv2-1][ind3-1];
			}
			
		}

#if UNITY_EDITOR	
#if ! UNITY_WEBPLAYER	
		texfinMatr.SetPixels(pixels);
		texfinMatr.Apply ();
		
		var bytes = texfinMatr.EncodeToPNG();
		
		int hL = hLevel+1;
		int vL = vLevel+1;
		
		
		string filePath = Application.dataPath +"/Resources/3dSprites/"+modelName +"/"+animationName+"/png/";
		string sfilePath = "Assets" +"/Resources/3dSprites/"+modelName +"/"+animationName+"/png/";
		
		string fileName = modelName+"_"+animationName+"_"+vL+"_"+hL+".png";
		


		File.WriteAllBytes(filePath+fileName, bytes);
		
		
		AssetDatabase.Refresh();
		
		
		string matfilePath = "Assets" + "/Resources/3dSprites/"+modelName +"/"+animationName+"/mat/";
		string matfileName = modelName+"_"+animationName+"_"+vL+"_"+hL+".mat";



	
     //   var material = new Material (Shader.Find("Sprites/Default"));
    //    var material = new Material (Shader.Find ("Unlit/Transparent Cutout"));
		var material = new Material (Shader.Find("Particles/Alpha Blended"));
		
		
		TextureImporter tImporter = AssetImporter.GetAtPath( sfilePath+fileName ) as TextureImporter;
	//	tImporter.textureType = TextureImporterType.Sprite;
		tImporter.maxTextureSize = maxDim;
		
		material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(sfilePath+fileName);

		

		AssetDatabase.CreateAsset(material, matfilePath+matfileName);
		
		AssetDatabase.Refresh();
		
		
		
		

#endif	
#endif
	
	}
	
	
	public void ExportConfig(){
#if UNITY_EDITOR
#if ! UNITY_WEBPLAYER
		string [] lines = new string[7];
		
		lines[0] = string.Empty + numberOfColumns;
		lines[1] = string.Empty + numberOfFrames;
		lines[2] = string.Empty + horRotLevels;
		lines[3] = string.Empty + verRotLevels;
		
				
		if(repeatAnimation == true){
			lines[4] = "1";
		}
		else{
			lines[4] = "0";
		}
		
		if(movableSprite == true){
			lines[5] = "1";
		}
		else{
			lines[5] = "0";
		}
		
		if(playAnimation == true){
			lines[6] = "1";
		}
		else{
			lines[6] = "0";
		}
		
		
		System.IO.File.WriteAllLines(@Application.dataPath + "/Resources/3dSprites/"+modelName +"/"+animationName+"/config.txt", lines);
#endif	
#endif		
	}
	
	
	
	public void FindTextureMaxDimenion(){
		#if UNITY_EDITOR
		#if ! UNITY_WEBPLAYER
		if(spriteResolution*numberOfColumns > spriteResolution*numberOfFrames/numberOfColumns){
			largDim = spriteResolution*numberOfColumns;
		}
		else{
			largDim = spriteResolution*numberOfFrames/numberOfColumns;
		}
		
		
	    
	
		if(largDim<=32){
			maxDim = 32;
		}
		else if((largDim>32)&&(largDim<=64)){
			maxDim = 64;
		}
		else if((largDim>64)&&(largDim<=128)){
			maxDim = 128;
		}
		else if((largDim>128)&&(largDim<=256)){
			maxDim = 256;
		}
		else if((largDim>256)&&(largDim<=512)){
			maxDim = 512;
		}
		else if((largDim>512)&&(largDim<=1024)){
			maxDim = 1024;
		}
		else if((largDim>1024)&&(largDim<=2048)){
			maxDim = 2048;
		}
		else if((largDim>2048)&&(largDim<=4096)){
			maxDim = 4096;
		}
		
		else{
			print("Maximum resolution size is exceded! Resize texture and try again");
			maxDim = 1024;
		}
		#endif
		#endif

	}
	
	
	
	
	void RefreshDirectories(){
		#if UNITY_EDITOR
		#if ! UNITY_WEBPLAYER
		
		AssetDatabase.Refresh();
		(new FileInfo(@Application.dataPath + "/Resources/3dSprites/"+modelName +"/"+animationName+"/png/")).Directory.Create();
		AssetDatabase.Refresh();
		(new FileInfo(@Application.dataPath + "/Resources/3dSprites/"+modelName +"/"+animationName+"/mat/")).Directory.Create();
		AssetDatabase.Refresh();
		
		
		if(deleted == false){
			AssetDatabase.Refresh();
			Directory.Delete(@Application.dataPath +"/Resources/3dSprites/"+modelName +"/"+animationName+"/png", true);
			AssetDatabase.Refresh();
			Directory.Delete(@Application.dataPath +"/Resources/3dSprites/"+modelName +"/"+animationName+"/mat", true);
			
			deleted = true;
		}
		
		AssetDatabase.Refresh();
		(new FileInfo(@Application.dataPath + "/Resources/3dSprites/"+modelName +"/"+animationName+"/png/")).Directory.Create();
		AssetDatabase.Refresh();
		(new FileInfo(@Application.dataPath + "/Resources/3dSprites/"+modelName +"/"+animationName+"/mat/")).Directory.Create();
		AssetDatabase.Refresh();
		
		
		
		UpdateDirectoriesList();
		
		
		
		
		
		

		
		
		#endif
		#endif
	}
	
	void UpdateDirectoriesList(){
		#if UNITY_EDITOR
		#if ! UNITY_WEBPLAYER
		
		string dirfile = @Application.dataPath + "/Resources/3dSprites/directories.txt";
		
		List<string> aname = new List<string>();
		
		int nEntries = 0;
		
		
		string pth = @Application.dataPath+"/Resources/3dSprites";
		foreach (string s in Directory.GetDirectories(pth)){
			foreach (string ss in Directory.GetDirectories(s)){
				aname.Add(s.Remove(0,pth.Length+1));
				aname.Add(ss.Remove(0,s.Length+1));
				nEntries = nEntries+1;
				
			}
		}
		
		aname.Insert(0,string.Empty + nEntries);


		
		
		
		
		
		string[] arraname = new string[aname.Count];
		
		for(int i=0;i<aname.Count;i++){
			arraname[i] = aname[i];
		}
		
		System.IO.File.WriteAllLines(dirfile, arraname);
		
		
		#endif
		#endif
	}


	

	
}
