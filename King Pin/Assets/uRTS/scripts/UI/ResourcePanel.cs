using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ResourcePanel : MonoBehaviour {
	
	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	public ButtonObject iron;	
	public ButtonObject gold;	
	public ButtonObject lumber;	
	public ButtonObject population;	
	
	public ButtonObject ironImage;	
	public ButtonObject goldImage;	
	public ButtonObject lumberImage;	
	public ButtonObject populationImage;	
	
	public Text tx_iron;
	public Text tx_gold;
	public Text tx_lumber;
	public Text tx_population;
	
	


    public void Build(){
    	mainCanvas = rtsm.mainCanvas;
		
		
		SetBarImage(ref ironImage,
		    "textures/icons/ironBars",
    		0.1f,0.95f,
			0.15f,1f
    	);
    	SetBarImage(ref goldImage,
    		"textures/icons/goldBars",
    		0.26f,0.95f,
			0.31f,1f
    	);
    	SetBarImage(ref lumberImage,
    		"textures/icons/woodLogs_ico",
    		0.42f,0.95f,
			0.48f,1f
    	);
    	SetBarImage(ref populationImage,
    		"textures/icons/population_ico",
    		0.58f,0.95f,
			0.63f,1f
    	);
		
		
		
		
		SetBar(ref iron,
    		0.15f,0.95f,
			0.22f,1f
    	);
    	SetBar(ref gold,
    		0.31f,0.95f,
			0.38f,1f
    	);
    	SetBar(ref lumber,
    		0.48f,0.95f,
			0.54f,1f
    	);
    	SetBar(ref population,
    		0.63f,0.95f,
			0.7f,1f
    	);
    	
    	tx_iron = iron.tx_button;
    	tx_gold = gold.tx_button;
    	tx_lumber = lumber.tx_button;
    	tx_population = population.tx_button;
    	
    	RenameButtons();
		
	}
	
	
	
	void SetBarImage(
		ref ButtonObject bar,
		string image,
    	float wMin,
    	float hMin,
    	float wMax,
    	float hMax
    ){
    	bar = new ButtonObject();
  		bar.buttonCanvas = mainCanvas;
  		
  		bar.SetButton();
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar.rectTransform,
  			wMin,hMin,
			wMax,hMax,
			1
  		);
  		
  		bar.tx_button.text = "";
  		
  		bar.MedievalTransparentStyle();
		bar.imageLocation.Add(image);
		bar.SetButtonImage();
		
    }
	
	
	
    void SetBar(
    	ref ButtonObject bar,
    	float wMin,
    	float hMin,
    	float wMax,
    	float hMax
    ){
    	bar = new ButtonObject();
  		bar.buttonCanvas = mainCanvas;
  		
  		bar.isChangeableText = true;
  		bar.textPixelRatio = 2f/3f;
  		bar.textChangeFactor = 0.05f;
  		bar.rtsm = rtsm;
  		bar.SetButton();
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar.rectTransform,
  			wMin,hMin,
			wMax,hMax,
			0
  		);
  		
  		bar.MedievalTransparentStyle();
  	//	movementButtonGrid.DeActivateButton(bo);
  		bar.tx_button.text = "0";
  	//	bar.tx_button.fontSize = 14;
  		
  		bar.tx_button.alignment = TextAnchor.MiddleLeft;
  		
  		
  		
  		
  		
//		bar.imageLocation.Add(image);
//		bar.SetButtonImage();
				
//		bar.im_button[1].gameObject.GetComponent<RectTransform>().
		
//		bar.im_button[1].type = Image.Type.Tiled;
    }
	
	
	
	void RenameButtons(){
		ironImage.buttonGo.name = "IronImage";
		goldImage.buttonGo.name = "GoldImage";
		lumberImage.buttonGo.name = "LumberImage";
		populationImage.buttonGo.name = "PopulationImage";
		
		iron.buttonGo.name = "IronValue";
		gold.buttonGo.name = "GoldValue";
		lumber.buttonGo.name = "LumberValue";
		population.buttonGo.name = "PopulationValue";
	}
	
	
	
	void Start () {
	
	}
	
}
