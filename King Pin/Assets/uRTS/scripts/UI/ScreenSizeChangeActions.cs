using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ScreenSizeChangeActions : MonoBehaviour {
	
	public RTSMaster rtsm;
	public int prevScreenWidth = 0;
	public int prevScreenHeight = 0;
	
	public List<ResizableGrid> dynamicGrids = new List<ResizableGrid>();
	public List<ResizablePanel> dynamicPanels = new List<ResizablePanel>();
	public List<ResizableText> dynamicTexts = new List<ResizableText>();
	
	public bool gameMode = false;
	
	
	void Awake(){
		gameMode = true;
	}
	
	// Use this for initialization
	void Start () {
		UpdateGrids();
		UpdatePanels();
		UpdateTexts();
		prevScreenWidth = Screen.width;
		prevScreenHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {

		if(gameMode == true){
			if((Screen.width != prevScreenWidth)||(Screen.height != prevScreenHeight)){
				UpdateGrids();
				UpdatePanels();
				UpdateTexts();
				prevScreenWidth = Screen.width;
				prevScreenHeight = Screen.height;
			}
		}
		
	}
	
	public void UpdateEditor(){
//	    gameMode = false;
		UpdateGrids();
		UpdatePanels();
		UpdateTexts();
//		gameMode = true;
	}
	
	void UpdateGrids(){
		for(int i=0; i<dynamicGrids.Count; i++){
		    ResizableGrid grd = dynamicGrids[i];
		    Vector2 screenSize = new Vector2(1f*Screen.width, 1f*Screen.height);
		    if(gameMode == false){
		    	screenSize = GetMainGameViewSize();
		    }
		    
			if(grd.isChangeableWidth == true){
				grd.grid.cellSize = new Vector2(grd.changeWidth*screenSize[0], grd.grid.cellSize.y);
			}
			if(grd.isChangeableHeight == true){
				grd.grid.cellSize = new Vector2(grd.grid.cellSize.x, grd.changeHeight*screenSize[1]);
			}
		}
	}
	
	void UpdatePanels(){
		for(int i=0; i<dynamicPanels.Count; i++){
			ResizablePanel panel = dynamicPanels[i];
			Vector2 screenSize = new Vector2(1f*Screen.width, 1f*Screen.height);
			if(gameMode == false){
		    	screenSize = GetMainGameViewSize();
		    }
			
			float pMinX = panel.wMin*screenSize[0];
			float pMaxX = -(1f*screenSize[0]-panel.wMax*screenSize[0]);
		
			float pMinY = panel.hMin*screenSize[1];
			float pMaxY = -(1f*screenSize[1]-panel.hMax*screenSize[1]);
			
			if(panel.preserveAspect == 1){
				float dx = 0.5f*(Mathf.Abs(pMaxY+screenSize[1])-pMinY);
				float pxCen = 0.5f*(pMinX+Mathf.Abs(pMaxX+screenSize[0]));
			
				pMinX = pxCen-dx;
				pMaxX = -(screenSize[0]-pxCen-dx);
			}
			if(panel.preserveAspect == 2){
				float dy = 0.5f*(Mathf.Abs(pMaxX+screenSize[0])-pMinX);
				float pyCen = 0.5f*(pMinY+Mathf.Abs(pMaxY+screenSize[1]));
			
				pMinY = pyCen-dy;
				pMaxY = -(screenSize[1]-pyCen-dy);
			}
			
			if(panel.preserveAspect == 3){
				float dx = 0.5f*(Mathf.Abs(pMaxY+screenSize[1])-pMinY);
				float dy = 0.5f*(Mathf.Abs(pMaxX+screenSize[0])-pMinX);
				if(dx<dy){
					float pxCen = 0.5f*(pMinX+Mathf.Abs(pMaxX+screenSize[0]));
			
					pMinX = pxCen-dx;
					pMaxX = -(screenSize[0]-pxCen-dx);
				}
				else{
					float pyCen = 0.5f*(pMinY+Mathf.Abs(pMaxY+screenSize[1]));
			
					pMinY = pyCen-dy;
					pMaxY = -(screenSize[1]-pyCen-dy);
				}
			}
			
//			Debug.Log(pMinX);
			
			panel.rect.offsetMin = new Vector2(pMinX,pMinY);
			panel.rect.offsetMax = new Vector2(pMaxX,pMaxY);
			
// 			if(panel.isChangeableWidth == true){
// 			
// 			}
// 			if(panel.isChangeableHeight == true){
// 			
// 			}
		}
	}
	
	void UpdateTexts(){
		for(int i=0; i<dynamicTexts.Count; i++){
			ResizableText tx = dynamicTexts[i];
			Vector2 screenSize = new Vector2(1f*Screen.width, 1f*Screen.height);
			if(gameMode == false){
		    	screenSize = GetMainGameViewSize();
		    }
			
			if(tx.isChangeable == true){
			    int fSize = (int)(tx.textPixelRatio * tx.changeFactor * screenSize[1]) - 1;
			    if(fSize < 1){
			    	fSize = 1;
			    }
				tx.text.fontSize = fSize;
			}
		}
	}
	
	
	public void AddPanelEditor(RectTransform rect, 
		float wMin,
		float hMin,
		float wMax,
		float hMax,
		int preserveAspect
	){
		ResizablePanel rp = new ResizablePanel();
		rp.rect = rect;
		rp.wMin = wMin;
		rp.wMax = wMax;
		rp.hMin = hMin;
		rp.hMax = hMax;
		rp.preserveAspect = preserveAspect;
		dynamicPanels.Add(rp);
		
		Vector2 screenSize = GetMainGameViewSize();

		float pMinX = 1f*rp.wMin*screenSize[0];
		float pMaxX = -(1f*screenSize[0]-rp.wMax*screenSize[0]);
	
		float pMinY = 1f*rp.hMin*screenSize[1];
		float pMaxY = -(1f*screenSize[1]-rp.hMax*screenSize[1]);
		
		if(preserveAspect == 1){

		    float dx = 0.5f*(Mathf.Abs(pMaxY+screenSize[1])-pMinY);
		    float pxCen = 0.5f*(pMinX+Mathf.Abs(pMaxX+screenSize[0]));
		    
		    pMinX = pxCen-dx;
		    pMaxX = -(screenSize[0]-pxCen-dx);
		}
		if(preserveAspect == 2){
		    
		    float dy = 0.5f*(Mathf.Abs(pMaxX+screenSize[0])-pMinX);
		    float pyCen = 0.5f*(pMinY+Mathf.Abs(pMaxY+screenSize[1]));
		    
		    pMinY = pyCen-dy;
		    pMaxY = -(screenSize[1]-pyCen-dy);
		}
		
		
		rp.rect.anchorMin = new Vector2(0f,0f);
		rp.rect.anchorMax = new Vector2(1f,1f);
		rp.rect.offsetMin = new Vector2(pMinX,pMinY);
		rp.rect.offsetMax = new Vector2(pMaxX,pMaxY);
		
	}
	
	public void RemovePanel(RectTransform rect){
		ResizablePanel rp = GetResizablePanel(rect);
		if(rp != null){
			dynamicPanels.Remove(rp);
		}
	}
	
	
	public ResizablePanel GetResizablePanel(RectTransform rect){
		ResizablePanel rp = null;
		for(int i=0; i<dynamicPanels.Count; i++){
			if(rect == dynamicPanels[i].rect){
				rp = dynamicPanels[i];
			}
		}
		return rp;
	}
	
	
	
	
	public void UpdatePanel(ResizablePanel rp, 
		float wMin,
		float hMin,
		float wMax,
		float hMax,
		int preserveAspect
	){
	//	rp.rect = rect;
		rp.wMin = wMin;
		rp.wMax = wMax;
		rp.hMin = hMin;
		rp.hMax = hMax;
		rp.preserveAspect = preserveAspect;
		
		Vector2 screenSize = new Vector2(prevScreenWidth, prevScreenHeight);
		//GetMainGameViewSize();

		float pMinX = 1f*rp.wMin*screenSize[0];
		float pMaxX = -(1f*screenSize[0]-rp.wMax*screenSize[0]);
	
		float pMinY = 1f*rp.hMin*screenSize[1];
		float pMaxY = -(1f*screenSize[1]-rp.hMax*screenSize[1]);
		
		if(preserveAspect == 1){

		    float dx = 0.5f*(Mathf.Abs(pMaxY+screenSize[1])-pMinY);
		    float pxCen = 0.5f*(pMinX+Mathf.Abs(pMaxX+screenSize[0]));
		    
		    pMinX = pxCen-dx;
		    pMaxX = -(screenSize[0]-pxCen-dx);
		}
		if(preserveAspect == 2){
		    
		    float dy = 0.5f*(Mathf.Abs(pMaxX+screenSize[0])-pMinX);
		    float pyCen = 0.5f*(pMinY+Mathf.Abs(pMaxY+screenSize[1]));
		    
		    pMinY = pyCen-dy;
		    pMaxY = -(screenSize[1]-pyCen-dy);
		}
		
		
		rp.rect.anchorMin = new Vector2(0f,0f);
		rp.rect.anchorMax = new Vector2(1f,1f);
		rp.rect.offsetMin = new Vector2(pMinX,pMinY);
		rp.rect.offsetMax = new Vector2(pMaxX,pMaxY);
		
		
		
		
	}
	
	
	
	public void AddResizableText(
		Text tx_button,
		float textPixelRatio,
		float textChangeFactor
	){
		ResizableText rt = new ResizableText();
		rt.text = tx_button;
		rt.isChangeable = true;
		rt.textPixelRatio = textPixelRatio;
		rt.changeFactor = textChangeFactor;
		
		Vector2 screenSize = GetMainGameViewSize();
		
		int fSize = (int)(rt.textPixelRatio * rt.changeFactor * screenSize[1]) - 1;
		if(fSize < 1){
			fSize = 1;
		}
		rt.text.fontSize = fSize;
//		Debug.Log(fSize);
		
		rtsm.screenSizeChangeActions.dynamicTexts.Add(rt);
		
		
	}
	
	
	
	
	public void AddPanel(RectTransform rect, 
		float wMin,
		float hMin,
		float wMax,
		float hMax
	){
		ResizablePanel rp = new ResizablePanel();
		rp.rect = rect;
		rp.wMin = wMin;
		rp.wMax = wMax;
		rp.hMin = hMin;
		rp.hMax = hMax;
		dynamicPanels.Add(rp);
		
		Vector2 screenSize = new Vector2(1f*Screen.width, 1f*Screen.height);
		
		float pMinX = 1f*rp.wMin*screenSize[0];
		float pMaxX = -(1f*screenSize[0]-rp.wMax*screenSize[0]);
	
		float pMinY = 1f*rp.hMin*screenSize[1];
		float pMaxY = -(1f*screenSize[1]-rp.hMax*screenSize[1]);
		
		rp.rect.anchorMin = new Vector2(0f,0f);
		rp.rect.anchorMax = new Vector2(1f,1f);
		rp.rect.offsetMin = new Vector2(pMinX,pMinY);
		rp.rect.offsetMax = new Vector2(pMaxX,pMaxY);
		
	}
	
	public static Vector2 GetMainGameViewSize(){
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
		return (Vector2)Res;
	}
	
	
	public void PercMoveLayer(
	    RectTransform rect,
		float minX, float minY,
		float maxX, float maxY		
	){
		
		Vector2 screenSize = new Vector2(1f*Screen.width, 1f*Screen.height);
		
		float pMinX = minX*screenSize[0];
		float pMaxX = -(1f*screenSize[0]-maxX*screenSize[0]);
		
		float pMinY = minY*screenSize[1];
		float pMaxY = -(1f*screenSize[1]-maxY*screenSize[1]);
		
		
		
		MoveLayer(
			rect,
			0f,0f,
			1f,1f,
			
			pMinX,pMinY,
			pMaxX,pMaxY
		);
		
	}
	
	public void MoveLayer(
	    RectTransform rect,
	// anchor positions
		float aMinX, float aMinY,
		float aMaxX, float aMaxY,
	// offset positions	
		float MinX, float MinY,
		float MaxX, float MaxY		
	){
		
		rect.anchorMin = new Vector2(aMinX,aMinY);
		rect.anchorMax = new Vector2(aMaxX,aMaxY);
		
		rect.offsetMin = new Vector2(MinX,MinY);
		rect.offsetMax = new Vector2(MaxX,MaxY);
		
	}

	
	
	
}

[System.Serializable]
public class ResizableGrid {
	public GridLayoutGroup grid;
	public bool isChangeableWidth;
	public bool isChangeableHeight;
	public float changeWidth;
	public float changeHeight;
	
	public bool lockRight = false;
}

[System.Serializable]
public class ResizableText {
	public Text text;
	public bool isChangeable = false;
	public float textPixelRatio;
	public float changeFactor;
}

[System.Serializable]
public class ResizablePanel {
	public RectTransform rect;
//	public bool isChangeableWidth;
//	public bool isChangeableHeight;
	public float wMin;
	public float wMax;
	public float hMin;
	public float hMax;
	
	public int preserveAspect;
}