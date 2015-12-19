using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BuildProgressNum : MonoBehaviour {

	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	public ButtonObject buildProgressNum_text;
	
	public ButtonObject bar_done_loc;
	public ButtonObject bar_rem_loc;
	
	public ButtonObject bar_done_gl;
	public ButtonObject bar_rem_gl;
	
	public ResizablePanel bar_rem_loc_rp;
	public ResizablePanel bar_rem_gl_rp;
		
	public List<UnitPars> selectedObjects;
	
	
    public void Build(){
    	mainCanvas = rtsm.mainCanvas;
    	selectedObjects = rtsm.selectionMark.selectedGoPars;
		
		BuildText();
		BuildLocalBar();
		BuildGlobalBar();
		DeActivate();
		RenameButtons();
	}	
	
	
	public void BuildText(){
		buildProgressNum_text = new ButtonObject();
  		buildProgressNum_text.rtsm = rtsm;
  		buildProgressNum_text.buttonCanvas = mainCanvas;
        buildProgressNum_text.isChangeableText = true;
		buildProgressNum_text.textPixelRatio = 2f/3f;
		buildProgressNum_text.textChangeFactor = 0.05f;
  		buildProgressNum_text.SetButton();
  		buildProgressNum_text.MedievalTransparentStyle();
  		buildProgressNum_text.tx_button.text = "0 / 0";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			buildProgressNum_text.rectTransform,
  			0.86f,0.57f,
			0.96f,0.62f,
			0
  		);
  		
	}
	
	public void BuildLocalBar(){
		bar_done_loc = new ButtonObject();
  		bar_done_loc.rtsm = rtsm;
  		bar_done_loc.buttonCanvas = mainCanvas;
  		bar_done_loc.SetButton();
        bar_done_loc.im_button[bar_done_loc.im_button.Count-1].color = Color.green;
  		bar_done_loc.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar_done_loc.rectTransform,
  			0.86f,0.56f,
			0.96f,0.57f,
			0
  		);
  		
		bar_rem_loc = new ButtonObject();
  		bar_rem_loc.rtsm = rtsm;
  		bar_rem_loc.buttonCanvas = mainCanvas;
  		bar_rem_loc.SetButton();
        bar_rem_loc.im_button[bar_rem_loc.im_button.Count-1].color = Color.black;
  		bar_rem_loc.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar_rem_loc.rectTransform,
  			0.94f,0.56f,
			0.96f,0.57f,
			0
  		);
  		
  		bar_rem_loc_rp = rtsm.screenSizeChangeActions.GetResizablePanel(bar_rem_loc.rectTransform);

	}
	
	public void BuildGlobalBar(){
		bar_done_gl = new ButtonObject();
  		bar_done_gl.rtsm = rtsm;
  		bar_done_gl.buttonCanvas = mainCanvas;
  		bar_done_gl.SetButton();
        bar_done_gl.im_button[bar_done_gl.im_button.Count-1].color = Color.green;
  		bar_done_gl.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar_done_gl.rectTransform,
  			0.86f,0.54f,
			0.96f,0.55f,
			0
  		);
  		
		bar_rem_gl = new ButtonObject();
  		bar_rem_gl.rtsm = rtsm;
  		bar_rem_gl.buttonCanvas = mainCanvas;
  		bar_rem_gl.SetButton();
        bar_rem_gl.im_button[bar_rem_gl.im_button.Count-1].color = Color.black;
  		bar_rem_gl.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar_rem_gl.rectTransform,
  			0.94f,0.54f,
			0.96f,0.55f,
			0
  		);
  		
  		bar_rem_gl_rp = rtsm.screenSizeChangeActions.GetResizablePanel(bar_rem_gl.rectTransform);
		
	}
	
	
	
	void RenameButtons(){
    	buildProgressNum_text.buttonGo.name = "BuildProgressNum_text";
    	
    	bar_done_loc.buttonGo.name = "bpn_bar_done_loc";
		bar_rem_loc.buttonGo.name = "bpn_bar_rem_loc";
	
		bar_done_gl.buttonGo.name = "bpn_bar_done_gl";
		bar_rem_gl.buttonGo.name = "bpn_bar_rem_gl";
    	
    }

	
	
	
	public void UpdateLocal(float progress){
		float xmin = 0.86f;
		float xmax = 0.96f;
		float ymin = 0.56f;
		float ymax = 0.57f;
		
		if(progress < 0f){
			progress = 0f;
		}
		if(progress > 1f){
			progress = 1f;
		}
		
		float lowLimit = progress*(xmax-xmin) + xmin;
		rtsm.screenSizeChangeActions.UpdatePanel(
  			bar_rem_loc_rp,
  			lowLimit,ymin,
			xmax,ymax,
			0
  		);
		
	}

	public void UpdateGlobal(float progress){
		float xmin = 0.86f;
		float xmax = 0.96f;
		float ymin = 0.54f;
		float ymax = 0.55f;
		
		if(progress < 0f){
			progress = 0f;
		}
		if(progress > 1f){
			progress = 1f;
		}
		
		float lowLimit = progress*(xmax-xmin) + xmin;
		rtsm.screenSizeChangeActions.UpdatePanel(
  			bar_rem_gl_rp,
  			lowLimit,ymin,
			xmax,ymax,
			0
  		);
		
	}
	
	
	
	

	
	
	void DeActivate(){
		buildProgressNum_text.buttonGo.SetActive(false);
		bar_done_loc.buttonGo.SetActive(false);
		bar_rem_loc.buttonGo.SetActive(false);
		bar_done_gl.buttonGo.SetActive(false);
		bar_rem_gl.buttonGo.SetActive(false);
	}
	
	void Activate(){
		buildProgressNum_text.buttonGo.SetActive(true);
		bar_done_loc.buttonGo.SetActive(true);
		bar_rem_loc.buttonGo.SetActive(true);
		bar_done_gl.buttonGo.SetActive(true);
		bar_rem_gl.buttonGo.SetActive(true);
	}
	
	public void UpdateText(string text){
		buildProgressNum_text.tx_button.text = text;
	}
	
	
	void Awake(){
		selectedObjects = rtsm.selectionMark.selectedGoPars;
	}
	
	
	void Start () {
	
	}
	
	
	void LateUpdate(){
	    bool activeAllowed = false;
		if(selectedObjects.Count == 1){
		    if(selectedObjects[0].isBuilding == true){
				if(selectedObjects[0].thisSpawn != null){
					SpawnPoint sp = selectedObjects[0].thisSpawn;
					if(sp.isSpawning == true){
						if(bar_done_loc.buttonGo.activeSelf == false){
							Activate();
						}
						activeAllowed = true;
						
						UpdateGlobal(sp.progress_gl);
						UpdateLocal(sp.progress_loc);
						
					}
				}
			}
		}
		if(activeAllowed == false){
			if(buildProgressNum_text.buttonGo.activeSelf == true){
				DeActivate();
			}
		}
	}
	
	
	
}
