using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ButtonsGrid {
	
	public ButtonsGrid thisObject;
	
	public GameObject canvas;
	public GameObject gridGo;
	
	public GameObject scrollContain;
	public GameObject inactiveScrollContain;
	
	public string gridGoName = "New Grid";
	
	public int size = 4;
	public List<ButtonObject> buttonsPool = new List<ButtonObject>();
	public List<int> buttonsEnableMask = new List<int>();
	
	
	public Vector2 aMin = new Vector3(0.5f, 0.5f);
	public Vector2 aMax = new Vector3(0.5f, 0.5f);
	public Vector2 aPos = new Vector3(0f, 0f);
	public Vector2 sDelt = new Vector3(260f, 230f);
	
	public GridLayoutGroup gr_scrollContain;
	public bool isChangeableWidth = false;
	public bool isChangeableHeight = false;
	public float changeWidth = 1f;
	public float changeHeight = 1f;
	public ResizableGrid rszGrid;
	float width1 = 360f;
	
	public RTSMaster rtsm;
	public bool isChangeableText = false;
	public float textPixelRatio = 1f;
	public float textChangeFactor = 1f;

	
	
	public void SetGrid(){
		
		gridGo = new GameObject(gridGoName);
		
	//	width1 = 2f*Screen.width;
		
		
		Sizes1();
		
		AddFrame(
				ref gridGo,
				ref canvas,
				aMin,
				aMax,
				aPos,
				sDelt,
				1
		);
        gridGo.AddComponent<CanvasRenderer>();
        
        
		scrollContain = new GameObject("Scroll Contain");
		AddFrame(
				ref scrollContain,
				ref gridGo,
				new Vector2(0f,0f),
				new Vector2(1f,1f),
				new Vector2(0f,0f),
				new Vector2(0f,0f),
				1
		);
		scrollContain.AddComponent<CanvasRenderer>();
		gr_scrollContain = scrollContain.AddComponent<GridLayoutGroup>();
	
	// resizable dynamic grid	
		if((isChangeableWidth == true) || (isChangeableHeight == true)){
			rszGrid = new ResizableGrid();
			rszGrid.isChangeableWidth = isChangeableWidth;
			rszGrid.isChangeableHeight = isChangeableHeight;
			rszGrid.changeWidth = changeWidth;
			rszGrid.changeHeight = changeHeight;
			rszGrid.grid = gr_scrollContain;
		}
		
		
		
		gr_scrollContain.cellSize = new Vector2(100f,50f);
	//	gr_scrollContain.cellSize = new Vector2(sDelt.x, sDelt.y/size);
		
		inactiveScrollContain = new GameObject("Inactive Pannels");
		AddFrame(
				ref inactiveScrollContain,
				ref gridGo,
				new Vector2(0f,0f),
				new Vector2(1f,1f),
				new Vector2(0f,0f),
				new Vector2(0f,0f),
				1
		);
		inactiveScrollContain.AddComponent<CanvasRenderer>();
		inactiveScrollContain.SetActive(false);
		
// 		ScrollRect sr_gridGo = gridGo.AddComponent<ScrollRect>();
//         sr_gridGo.elasticity = 0f;
//         
//         sr_gridGo.content = scrollContain.GetComponent<RectTransform>();
		
	}
	
	public void Sizes1(){
		aMin = new Vector3(0.2f, 0.2f);
		aMax = new Vector3(0.8f, 0.8f);
		aPos = new Vector3(0f, 0f);
	//	sDelt = new Vector3(width1, 230f);
	//	sDelt = new Vector3(100f, 100f);
		sDelt = new Vector3(0f, 0f);
	}
	
	public void GridLimits(int width){
	    if(gr_scrollContain != null){
	        gr_scrollContain.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
	        gr_scrollContain.constraintCount = width;
		}
	}
	
	
	public void AddEmptyButton(){
	    ButtonObject bo = new ButtonObject();
	    if(rtsm != null){
	    	bo.rtsm = rtsm;
	    }
	    
	    bo.isChangeableText = isChangeableText;
	    bo.textPixelRatio = textPixelRatio;
	    bo.textChangeFactor = textChangeFactor;
	    
 		bo.buttonCanvas = scrollContain;
 		bo.width1 = width1;
 		
 		bo.SetButton();
 		bo.MedievalTransparentStyle();
 		
		buttonsPool.Add(bo);
		buttonsEnableMask.Add(1);
	}
	
	public void ActivateButton(ButtonObject but){
		if(! buttonsPool.Contains(but)){
			buttonsPool.Add(but);
			buttonsEnableMask.Add(1);
		}
		int id = buttonsPool.IndexOf(but);
		if(buttonsEnableMask[id] == 1){
			but.buttonGo.transform.SetParent(scrollContain.transform);
		}
	}	

	public void DeActivateButton(ButtonObject but){
		but.buttonGo.transform.SetParent(inactiveScrollContain.transform);
		if(! buttonsPool.Contains(but)){
			buttonsPool.Add(but);
			buttonsEnableMask.Add(1);
		}
	}	

	public void ActivateAll(){
		for(int i=0; i<buttonsPool.Count; i++){
			if(buttonsEnableMask[i] == 1){
				buttonsPool[i].buttonGo.transform.SetParent(scrollContain.transform);
			}
		}
	}
	
	public bool IsActiveAny(){
		bool isActive = false;
		for(int i=0; i<buttonsPool.Count; i++){
			if(buttonsPool[i].buttonGo.transform.parent == scrollContain.transform){
				isActive = true;
			}
		}
		return isActive;
	}

	public void FlipAll(){
		for(int i=0; i<buttonsPool.Count; i++){
			if(buttonsPool[i].buttonGo.transform.parent.gameObject == inactiveScrollContain){
				if(buttonsEnableMask[i] == 1){
					buttonsPool[i].buttonGo.transform.SetParent(scrollContain.transform);
				}
			}
			else{
				buttonsPool[i].buttonGo.transform.SetParent(inactiveScrollContain.transform);
				buttonsPool[i].numberOfClicks = 0;
			}
		}
	}

	
	public void DeactivateAll(){
		for(int i=0; i<buttonsPool.Count; i++){
			buttonsPool[i].buttonGo.transform.SetParent(inactiveScrollContain.transform);
		}
	}
	
	
	public IEnumerator ActivateButtonDelay(ButtonObject but, float delayTime){
		ActivateButton(but);
		yield return new WaitForSeconds(delayTime);
		DeActivateButton(but);
	}
	
	
	public void AddFrame(
					ref GameObject go, 
					ref GameObject mCanvas,
					
					Vector2 aMinL,
					Vector2 aMaxL,
					Vector2 aPosL,
					Vector2 sDeltL,
					
					int modeL
	){
		
		go.transform.SetParent(mCanvas.transform);
		
		RectTransform rt_go = null;
		
		if(go.GetComponent<RectTransform>() == null){
			rt_go = go.AddComponent<RectTransform>();
		}
		else{
			rt_go = go.GetComponent<RectTransform>();
		}
		
		go.layer = LayerMask.NameToLayer("UI");
		
		rt_go.anchorMin = aMinL;
		rt_go.anchorMax = aMaxL;
		
		if(modeL == 1){
			rt_go.anchoredPosition = aPosL;
			rt_go.sizeDelta = sDeltL;
		}
		else if(modeL == 2){
			rt_go.offsetMin = aPosL;
			rt_go.offsetMax = sDeltL;
		}
		
	}
	
	
	
	public void MoveLayer(
	// anchor positions
		float aMinX, float aMinY,
		float aMaxX, float aMaxY,
	// offset positions	
		float MinX, float MinY,
		float MaxX, float MaxY		
	){
		
		gridGo.GetComponent<RectTransform>().anchorMin = new Vector2(aMinX,aMinY);
		gridGo.GetComponent<RectTransform>().anchorMax = new Vector2(aMaxX,aMaxY);
		
		gridGo.GetComponent<RectTransform>().offsetMin = new Vector2(MinX,MinY);
		gridGo.GetComponent<RectTransform>().offsetMax = new Vector2(MaxX,MaxY);
		
	}
	
	
	
	

}



[System.Serializable]
public class ButtonPool {
	
	public List<ButtonObject> poolBO;
	public UIMaster uim;
	public GameObject poolCanvas;
	
	public void SetGridHor(
		int nx, int ny, int n,
		float xmin, float ymin,
		float xmax, float ymax,
		float dx, float dy,
		string text, int aspect
	){
		poolCanvas = uim.SetSubCanvas("grid_"+nx.ToString()+"_"+ny.ToString());
		poolBO = new List<ButtonObject>();
		
		float xmin1 = xmin + 0.5f*dx;
		float ymin1 = ymin + 0.5f*dy;
		
		float xmax1 = xmax - 0.5f*dx;
		float ymax1 = ymax - 0.5f*dy;
		
		float dxc = (xmax1-xmin1) / nx;
		float dyc = (ymax1-ymin1) / ny;
		
		
		for(int j=0; j<ny; j++){
			for(int i=0; i<nx; i++){
			    int k = j*nx + i;
			    if(k<n){
			        
					ButtonObject bo = new ButtonObject();
					bo.rtsm = uim.rtsm;
					bo.buttonCanvas = poolCanvas;
					if(text != ""){
						bo.isChangeableText = true;
						bo.textPixelRatio = 2f/3f;
						bo.textChangeFactor = dy;
					}
					//0.05f;  		
					bo.SetButton();
					bo.MedievalTransparentStyle();
					
					bo.tx_button.text = text;
					
					bo.tx_button.alignment = TextAnchor.MiddleLeft;
					
					float xc = xmin1 + dxc*i + 0.5f*dxc;
					float yc = ymin1 + dyc*j + 0.5f*dyc;
		
					uim.rtsm.screenSizeChangeActions.AddPanelEditor(
						bo.rectTransform,
						xc-0.5f*dx, (1f-yc)-0.5f*dy,
						xc+0.5f*dx, (1f-yc)+0.5f*dy,
						aspect
					);
					poolBO.Add(bo);
				}
			}
  		}
		
	}
	
	
	public void ActivateAll(){
		poolCanvas.SetActive(true);
	}
	
	public void DeActivateAll(){
		poolCanvas.SetActive(false);
	}
	
	public bool IsActiveAny(){
		return poolCanvas.activeSelf;
	}
	
	public void FlipAll(){
		poolCanvas.SetActive(! poolCanvas.activeSelf);
	}

}



[System.Serializable]
public class ButtonObject {
	
	public ButtonObject thisObject;
	
	public GameObject buttonCanvas;
	public GameObject buttonGo;
	public RectTransform rectTransform;
	
	public string buttonGoName = "New Button";
	
	public List<string> imageLocation = new List<string>();
	
	public Button button;
	public List<Image> im_button = new List<Image>();
	
	public Color col_button = new Color(1f,1f,1f,1f);
	public Text tx_button;
	
	
	public Vector2 aMin = new Vector3(0.5f, 0.5f);
	public Vector2 aMax = new Vector3(0.5f, 0.5f);
	public Vector2 aPos = new Vector3(0f, 0f);
	public Vector2 sDelt = new Vector3(160f, 30f);
	
	public bool isSet = false;
	public int mode = 1;
	
	public int numberOfClicks = 0;
	public int rtsId = -1;
	
	public float width1 = 0f;
	
	public RTSMaster rtsm;
	public bool isChangeableText = false;
	public float textPixelRatio = 1f;
	public float textChangeFactor = 1f;
	
	public bool isPressedIn = false;
	
	
	public void SetButton(){
	
		if(isSet == false){
			buttonGo = new GameObject(buttonGoName);
			
			Sizes1();
			AddFrame(
				ref buttonGo,
				ref buttonCanvas,
				aMin,
				aMax,
				aPos,
				sDelt,
				mode
			);
			buttonGo.AddComponent<CanvasRenderer>();
			
			SetButtonImage();
			
			rectTransform = buttonGo.GetComponent<RectTransform>();
			button = buttonGo.AddComponent<Button>();
			button.transition = Selectable.Transition.None;
			
			SetButtonText();
			
			thisObject = this;
			isSet = true;
			
		}

		
	}
	
	public void Sizes1(){
		aMin = new Vector3(0.5f, 0.5f);
		aMax = new Vector3(0.5f, 0.5f);
		aPos = new Vector3(0f, 0f);
		sDelt = new Vector3(width1, 30f);
	}
	
	public void SetEmpty(){
		if(isSet == false){
			buttonGo = new GameObject(buttonGoName);
			AddFrame(
				ref buttonGo,
				ref buttonCanvas,
				new Vector2(0f,0f),
				new Vector2(1f,1f),
				new Vector2(0f,0f),
				new Vector2(0f,0f),
				1
			);
			buttonGo.AddComponent<CanvasRenderer>();
		}
	}
	
	
	public void SetButtonImage(){
	    GameObject im_buttonGo = new GameObject("Image");
	    
	    AddFrame(
			ref im_buttonGo,
			ref buttonGo,
			new Vector2(0f,0f),
			new Vector2(1f,1f),
			new Vector2(0f,0f),
			new Vector2(0f,0f),
			1
		);
	    
		im_button.Add(im_buttonGo.AddComponent<Image>());
		
		if(imageLocation.Count == (im_button.Count)){
			if(imageLocation[imageLocation.Count-1] != ""){
				im_button[im_button.Count-1].sprite = Resources.Load<Sprite>(imageLocation[imageLocation.Count-1]);
			}
		}
		else{
			imageLocation.Add(null);
		}
		
		im_button[im_button.Count-1].color = col_button;

	}
	


	public void SetButtonImageFrac(
		float minX, float minY,
		float maxX, float maxY
	){
	    GameObject im_buttonGo = new GameObject("Image");
	    
	    AddFrame(
			ref im_buttonGo,
			ref buttonGo,
			new Vector2(0f,0f),
			new Vector2(1f,1f),
			new Vector2(0f,0f),
			new Vector2(0f,0f),
			1
		);
		
		
		im_buttonGo.GetComponent<RectTransform>().anchorMin = new Vector2(0f,0f);
		im_buttonGo.GetComponent<RectTransform>().anchorMax = new Vector2(1f,1f);
		
		float szX = buttonGo.GetComponent<RectTransform>().offsetMax[0] - buttonGo.GetComponent<RectTransform>().offsetMin[0];
		float szY = buttonGo.GetComponent<RectTransform>().offsetMax[1] - buttonGo.GetComponent<RectTransform>().offsetMin[1];
		
		float pminX = szX*minX;
		float pminY = szY*minY;
		float pmaxX = szX*(1f-minX);
		float pmaxY = szY*(1f-minY);
		
		im_buttonGo.GetComponent<RectTransform>().offsetMin = new Vector2(pminX,pminY);
		im_buttonGo.GetComponent<RectTransform>().offsetMax = new Vector2(pmaxX,pmaxY);
		
	    
		im_button.Add(im_buttonGo.AddComponent<Image>());
		
		if(imageLocation.Count == (im_button.Count)){
			if(imageLocation[imageLocation.Count-1] != ""){
				im_button[im_button.Count-1].sprite = Resources.Load<Sprite>(imageLocation[imageLocation.Count-1]);
			}
		}
		else{
			imageLocation.Add(null);
		}
		
		im_button[im_button.Count-1].color = col_button;

	}
	
	
	
	
	
	
	
	public void SetButtonText(){
		GameObject tx_buttonGo = new GameObject("Text");
		AddFrame(
			ref tx_buttonGo,
			ref buttonGo,
			new Vector2(0f,0f),
			new Vector2(1f,1f),
			new Vector2(0f,0f),
			new Vector2(0f,0f),
			1
		);
		tx_button = tx_buttonGo.AddComponent<Text>();
		tx_button.color = Color.black;
		tx_button.alignment = TextAnchor.MiddleCenter;
		tx_button.text = "New Button";
		
		if(isChangeableText == true){
		    rtsm.screenSizeChangeActions.AddResizableText(
				tx_button,
				textPixelRatio,
				textChangeFactor
			);
// 			ResizableText rt = new ResizableText();
// 			rt.text = tx_button;
// 			rt.isChangeable = true;
// 			rt.textPixelRatio = textPixelRatio;
// 			rt.changeFactor = textChangeFactor;
// 			rtsm.screenSizeChangeActions.dynamicTexts.Add(rt);
		}
	}
	
	
	
	
	public void MedievalStyle(){
	    im_button[im_button.Count-1].color = new Color(0.23f,0.11f,0.0f,1f);
		tx_button.color = new Color(0.61f,0.50f,0.13f,1f);
		tx_button.font = Resources.Load<Font>("Fonts/Rochester/Rochester-Regular");
		if(isChangeableText == false){
			tx_button.fontSize = 30;
		}
		//15;
	//	tx_button.resizeTextForBestFit = true;
	}
	public void MedievalTransparentStyle(){
	    im_button[im_button.Count-1].color = new Color(0f,0f,0f,0f);
		tx_button.color = new Color(0.61f,0.50f,0.13f,1f);
		tx_button.font = Resources.Load<Font>("Fonts/Rochester/Rochester-Regular");
		if(isChangeableText == false){
			tx_button.fontSize = 30;
		}
		//15;
	//	tx_button.resizeTextForBestFit = true;
	}
	
	
	public void SwitchStyle(int mod){
	    // medieval
	    if(mod == 1){
			im_button[im_button.Count-1].color = new Color(0.23f,0.11f,0.0f,1f);
			tx_button.color = new Color(0.61f,0.50f,0.13f,1f);
			if(isChangeableText == false){
				tx_button.fontSize = 30;
			}
		}
		// green
	    if(mod == 2){
			im_button[im_button.Count-1].color = new Color(0.23f,0.11f,0.0f,1f);
			tx_button.color = new Color(0.61f,0.50f,0.13f,1f);
			if(isChangeableText == false){
				tx_button.fontSize = 30;
			}
		}
		// grey
	    if(mod == 3){
			im_button[im_button.Count-1].color = Color.grey;
			tx_button.color = Color.black;
			if(isChangeableText == false){
				tx_button.fontSize = 30;
			}
		}
	}
	
	
	
	
	public void AddFrame(
					ref GameObject go, 
					ref GameObject mCanvas,
					
					Vector2 aMinL,
					Vector2 aMaxL,
					Vector2 aPosL,
					Vector2 sDeltL,
					
					int modeL
		
		
		
	){
		
		go.transform.SetParent(mCanvas.transform);
		
		RectTransform rt_go = go.AddComponent<RectTransform>();
		
		go.layer = LayerMask.NameToLayer("UI");
		
		rt_go.anchorMin = aMinL;
		rt_go.anchorMax = aMaxL;
		
		if(modeL == 1){
			rt_go.anchoredPosition = aPosL;
			rt_go.sizeDelta = sDeltL;
		}
		else if(modeL == 2){
			rt_go.offsetMin = aPosL;
			rt_go.offsetMax = sDeltL;
		}
		
	}
	
	
	public void PercMoveLayer(
		float minX, float minY,
		float maxX, float maxY		
	){
		
		Vector2 screenSize = GetMainGameViewSize();
		
		float pMinX = minX*screenSize[0];
		float pMaxX = -(1f*screenSize[0]-maxX*screenSize[0]);
		
		float pMinY = minY*screenSize[1];
		float pMaxY = -(1f*screenSize[1]-maxY*screenSize[1]);
		
		
		
	//	Debug.Log(GetMainGameViewSize());
	//	Debug.Log(Screen.height);
		
// 		float aMinX = 0.5f*(minX+0f);
// 		float aMaxX = 0.5f*(maxX+1f);
// 		float aMinY = 0.5f*(minY+0f);
// 		float aMaxY = 0.5f*(maxY+1f);
		
// 		Debug.Log(pMinX);
// 		Debug.Log(pMaxX);
// 		Debug.Log(pMinY);
// 		Debug.Log(pMaxY);
		
		
		MoveLayer(
			0f,0f,
			1f,1f,
			
			pMinX,pMinY,
			pMaxX,pMaxY
		);
		
// 		MoveLayer(
// 			aMinX,aMinY,
// 			aMaxX,aMaxY,
// 			
// 			pMinX,pMinY,
// 			pMaxX,pMaxY
// 		);
		
	}
	
	
	public void MoveLayer(
	// anchor positions
		float aMinX, float aMinY,
		float aMaxX, float aMaxY,
	// offset positions	
		float MinX, float MinY,
		float MaxX, float MaxY		
	){
		
		buttonGo.GetComponent<RectTransform>().anchorMin = new Vector2(aMinX,aMinY);
		buttonGo.GetComponent<RectTransform>().anchorMax = new Vector2(aMaxX,aMaxY);
		
		buttonGo.GetComponent<RectTransform>().offsetMin = new Vector2(MinX,MinY);
		buttonGo.GetComponent<RectTransform>().offsetMax = new Vector2(MaxX,MaxY);
		
	}
	
	public void HideButton(){
		buttonGo.SetActive(false);
	}
	public void ShowButton(){
		buttonGo.SetActive(true);
	}

    
    
    public static Vector2 GetMainGameViewSize(){
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
		return (Vector2)Res;
	}
    
	
}




[System.Serializable]
public class DiplomacyButtonsGrid : ButtonsGrid {
	
	public List<ButtonObject> warSug = new List<ButtonObject>();              // 1
	public List<ButtonObject> mercySug = new List<ButtonObject>();            // 2
	public List<ButtonObject> mercyAcceptSug = new List<ButtonObject>();      // 3
	public List<ButtonObject> mercyDeclineSug = new List<ButtonObject>();     // 4
	public List<ButtonObject> escapeSlaverySug = new List<ButtonObject>();    // 5
	public List<ButtonObject> allianceSug = new List<ButtonObject>();         // 6
	public List<ButtonObject> allianceAcceptSug = new List<ButtonObject>();   // 7
	public List<ButtonObject> allianceDeclineSug = new List<ButtonObject>();  // 8
	public List<ButtonObject> allianceLeaveSug = new List<ButtonObject>();    // 9
	
	
	
	
	public void AddButton_war(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "war"+(warSug.Count).ToString();
		DeActivateButton(but);
		warSug.Add(but);
		
	}
	public void AddButton_mercy(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "mercy"+(warSug.Count).ToString();
		DeActivateButton(but);
		mercySug.Add(but);
	}
	public void AddButton_mercyAccept(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "mercyAccept"+(warSug.Count).ToString();
		DeActivateButton(but);
		mercyAcceptSug.Add(but);
	}
    public void AddButton_mercyDecline(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "mercyDecline"+(warSug.Count).ToString();
		DeActivateButton(but);
		mercyDeclineSug.Add(but);
	}
	public void AddButton_escapeSlavery(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "escapeSlavery"+(warSug.Count).ToString();
		DeActivateButton(but);
		escapeSlaverySug.Add(but);
	}
	public void AddButton_alliance(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "alliance"+(warSug.Count).ToString();
		DeActivateButton(but);
		allianceSug.Add(but);
	}
	public void AddButton_allianceAccept(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "allianceAccept"+(warSug.Count).ToString();
		DeActivateButton(but);
		allianceAcceptSug.Add(but);
	}
	public void AddButton_allianceDecline(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "allianceDecline"+(warSug.Count).ToString();
		DeActivateButton(but);
		allianceDeclineSug.Add(but);
	}
	public void AddButton_allianceLeave(){
	    ButtonObject but = CreateMedievalButton();
	    but.buttonGo.name = "allianceLeave"+(allianceLeaveSug.Count).ToString();
		DeActivateButton(but);
		allianceLeaveSug.Add(but);
	}
	
	
	
	
	
	public void DeActivateButton_war(int id){
		DeActivateButton(warSug[id]);
	}
	public void DeActivateButton_mercy(int id){
		DeActivateButton(mercySug[id]);
	}
	public void DeActivateButton_mercyAccept(int id){
		DeActivateButton(mercyAcceptSug[id]);
	}
	public void DeActivateButton_mercyDecline(int id){
		DeActivateButton(mercyDeclineSug[id]);
	}	
	public void DeActivateButton_escapeSlavery(int id){
		DeActivateButton(escapeSlaverySug[id]);
	}
	public void DeActivateButton_alliance(int id){
		DeActivateButton(allianceSug[id]);
	}
	public void DeActivateButton_allianceAccept(int id){
		DeActivateButton(allianceAcceptSug[id]);
	}
	public void DeActivateButton_allianceDecline(int id){
		DeActivateButton(allianceDeclineSug[id]);
	}
    public void DeActivateButton_allianceLeave(int id){
		DeActivateButton(allianceLeaveSug[id]);
	}

    
    
    
    


	public IEnumerator ActivateButton_war(int id){
		ActivateButton(warSug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(warSug[id]);
	}
	public IEnumerator ActivateButton_mercy(int id){
		ActivateButton(mercySug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(mercySug[id]);
	}
	public IEnumerator ActivateButton_mercyAccept(int id){
		ActivateButton(mercyAcceptSug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(mercyAcceptSug[id]);
	}
	public IEnumerator ActivateButton_mercyDecline(int id){
		ActivateButton(mercyDeclineSug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(mercyDeclineSug[id]);
	}	
	public IEnumerator ActivateButton_escapeSlavery(int id){
		ActivateButton(escapeSlaverySug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(escapeSlaverySug[id]);
	}
	public IEnumerator ActivateButton_alliance(int id){
		ActivateButton(allianceSug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(allianceSug[id]);
	}
	public IEnumerator ActivateButton_allianceAccept(int id){
		ActivateButton(allianceAcceptSug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(allianceAcceptSug[id]);
	}
	public IEnumerator ActivateButton_allianceDecline(int id){
		ActivateButton(allianceDeclineSug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(allianceDeclineSug[id]);
	}
    public IEnumerator ActivateButton_allianceLeave(int id){
		ActivateButton(allianceLeaveSug[id]);
		yield return new WaitForSeconds(5f);
		DeActivateButton(allianceLeaveSug[id]);
	}
	
	
	public ButtonObject CreateMedievalButton(){
		ButtonObject bo = new ButtonObject();
		bo.rtsm = rtsm;
		bo.buttonCanvas = canvas;
		bo.isChangeableText = true;
		bo.textPixelRatio = 2f/3f;
		bo.textChangeFactor = 0.1f;
		bo.imageLocation.Add(null);
		bo.SetButton();
		bo.MedievalTransparentStyle();
	//	bo.tx_button.fontSize = 30;
		return bo;
	}


	
	
	

}




[System.Serializable]
public class NationIconButton : ButtonObject {
	
	public List<Image> other_images = new List<Image>();
	public List<string> other_images_loc = new List<string>();
	
	
	
	public void Add_OtherImage(){
		other_images.Add(null);
		other_images_loc.Add(null);
	}
	
	
	public void Set_OtherImage(int id){
// 		GameObject im_buttonGo = new GameObject("Image");
// 	    
// 	    AddFrame(
// 			ref im_buttonGo,
// 			ref buttonGo,
// 			new Vector2(0f,0f),
// 			new Vector2(1f,1f),
// 			new Vector2(0f,0f),
// 			new Vector2(0f,0f),
// 			1
// 		);
// 	    
// 		im_button = im_buttonGo.AddComponent<Image>();
// 			
// 		if(imageLocation != ""){
// 			im_button.sprite = Resources.Load<Sprite>(imageLocation);
// 		}
// 		
// 		im_button.color = col_button;

	}

}




