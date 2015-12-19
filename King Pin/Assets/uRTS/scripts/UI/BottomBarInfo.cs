using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BottomBarInfo : MonoBehaviour {

	[HideInInspector] public RTSMaster rtsm;

	public GameObject mainCanvas;
	
	public ButtonObject description;
	
	

	public ButtonObject image1;
	public ButtonObject image2;
	public ButtonObject image3;
	public ButtonObject image4;

	
	public ButtonObject text1;
	public ButtonObject text2;
	public ButtonObject text3;
	public ButtonObject text4;
	
    public List<Sprite> sprites = new List<Sprite>();
	
	
	public void Build(){
    	mainCanvas = rtsm.mainCanvas;
		BuildMenu();
		RenameButtons();
	}
	
	void BuildMenu(){
    	
    	sprites.Add(Resources.Load<Sprite>("textures/icons/ironBars"));
    	sprites.Add(Resources.Load<Sprite>("textures/icons/goldBars"));
    	sprites.Add(Resources.Load<Sprite>("textures/icons/woodLogs_ico"));
    	sprites.Add(Resources.Load<Sprite>("textures/icons/population_ico"));
    	
    	
    	SetBarImage(ref image1,
    		"",
    		0.26f,0f,
			0.31f,0.03f
    	);
    	SetBarImage(ref image2,
    		"",
    		0.42f,0f,
			0.48f,0.03f
    	);
    	SetBarImage(ref image3,
    		"",
    		0.58f,0f,
			0.63f,0.03f
    	);
    	SetBarImage(ref image4,
    		"",
    		0.73f,0f,
			0.78f,0.03f
    	);

    	
    	
		SetBar(ref description,
    		0.15f,0f,
			0.22f,0.04f
    	);
    	SetBar(ref text1,
    		0.31f,0f,
			0.38f,0.04f
    	);
    	SetBar(ref text2,
    		0.48f,0f,
			0.54f,0.04f
    	);
    	SetBar(ref text3,
    		0.63f,0f,
			0.7f,0.04f
    	);
    	SetBar(ref text4,
    		0.78f,0f,
			0.85f,0.04f
    	);
    	
    	description.tx_button.text = "building";
    	
    	image1.im_button[1].sprite = sprites[0];
    	image2.im_button[1].sprite = sprites[1];
    	image3.im_button[1].sprite = sprites[2];
    	image4.im_button[1].sprite = sprites[3];
    	
    	DisableBBI();

	}
	
	void RenameButtons(){
    	description.buttonGo.name = "bbi_info";
    	
    	image1.buttonGo.name = "bbi_image1";
    	image2.buttonGo.name = "bbi_image2";
    	image3.buttonGo.name = "bbi_image3";
    	image4.buttonGo.name = "bbi_image4";
    	
    	text1.buttonGo.name = "bbi_text1";
    	text2.buttonGo.name = "bbi_text2";
    	text3.buttonGo.name = "bbi_text3";
    	text4.buttonGo.name = "bbi_text4";
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
  		bar.textChangeFactor = 0.04f;
  		bar.rtsm = rtsm;
  		bar.SetButton();
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			bar.rectTransform,
  			wMin,hMin,
			wMax,hMax,
			0
  		);
  		
  		bar.MedievalTransparentStyle();
  		bar.tx_button.text = "0";
  		
  		bar.tx_button.alignment = TextAnchor.LowerLeft;
  		
    }
	
	
	
	public void EnableBBI(UnitPars up){
		DisableBBI();
		if(up != null){
			description.buttonGo.SetActive(true);
			description.tx_button.text = "Build "+up.unitName;
			
			List<ButtonObject> images = new List<ButtonObject>();
			List<ButtonObject> texts = new List<ButtonObject>();
			
			images.Add(image1);
			images.Add(image2);
			images.Add(image3);
			images.Add(image4);
			
			texts.Add(text1);
			texts.Add(text2);
			texts.Add(text3);
			texts.Add(text4);
			
			int i = 0;
			
			if(up.costIron > 0){
				images[i].buttonGo.SetActive(true);
				images[i].im_button[1].sprite = sprites[0];
				
				texts[i].buttonGo.SetActive(true);
				texts[i].tx_button.text = up.costIron.ToString();
				i=i+1;
			}
			if(up.costGold > 0){
				images[i].buttonGo.SetActive(true);
				images[i].im_button[1].sprite = sprites[1];
				
				texts[i].buttonGo.SetActive(true);
				texts[i].tx_button.text = up.costGold.ToString();
				i=i+1;
			}
			if(up.costLumber > 0){
				images[i].buttonGo.SetActive(true);
				images[i].im_button[1].sprite = sprites[2];
				
				texts[i].buttonGo.SetActive(true);
				texts[i].tx_button.text = up.costLumber.ToString();
				i=i+1;
			}
			if(up.costPopulation > 0){
				images[i].buttonGo.SetActive(true);
				images[i].im_button[1].sprite = sprites[3];
				
				texts[i].buttonGo.SetActive(true);
				texts[i].tx_button.text = up.costPopulation.ToString();
				i=i+1;
			}
		}
	}
	
	public void DisableBBI(){
		image1.buttonGo.SetActive(false);
		image2.buttonGo.SetActive(false);
		image3.buttonGo.SetActive(false);
		image4.buttonGo.SetActive(false);
		
		description.buttonGo.SetActive(false);
		text1.buttonGo.SetActive(false);
		text2.buttonGo.SetActive(false);
		text3.buttonGo.SetActive(false);
		text4.buttonGo.SetActive(false);
	}
	
	
	
	


	void Start () {
	
	}
	
}
