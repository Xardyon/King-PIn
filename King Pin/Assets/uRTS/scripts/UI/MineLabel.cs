using UnityEngine;
using System.Collections;

public class MineLabel : MonoBehaviour {
	
	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	public ButtonObject mineLabel_im;
	public ButtonObject mineLabel_tx;
	
	public Sprite iron;
	public Sprite gold;
	
	
	
	public void Build(){
		mainCanvas = rtsm.mainCanvas;
		BuildImage();
		BuildText();
		DeActivate();
		RenameButtons();
	}
	
	
	void BuildImage(){
		
		iron = Resources.Load<Sprite>("textures/icons/ironBars");
		gold = Resources.Load<Sprite>("textures/icons/goldBars");
		
		mineLabel_im = new ButtonObject();
  		mineLabel_im.rtsm = rtsm;
  		mineLabel_im.buttonCanvas = mainCanvas;
  		mineLabel_im.SetButton();
  		mineLabel_im.MedievalTransparentStyle();
  		mineLabel_im.tx_button.text = "";
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			mineLabel_im.rectTransform,
  			0.88f,0.5f,
			0.92f,0.55f,
			1
  		);
  		
  		mineLabel_im.imageLocation.Add("textures/icons/ironBars");
		mineLabel_im.SetButtonImage();

	}
	
	void BuildText(){
		mineLabel_tx = new ButtonObject();
  		mineLabel_tx.rtsm = rtsm;
  		mineLabel_tx.buttonCanvas = mainCanvas;
        mineLabel_tx.isChangeableText = true;
		mineLabel_tx.textPixelRatio = 2f/3f;
		mineLabel_tx.textChangeFactor = 0.05f;  		
  		mineLabel_tx.SetButton();
  		mineLabel_tx.MedievalTransparentStyle();
  		mineLabel_tx.tx_button.text = "0";
  		mineLabel_tx.tx_button.alignment = TextAnchor.MiddleLeft;
  		
  		
  		rtsm.screenSizeChangeActions.AddPanelEditor(
  			mineLabel_tx.rectTransform,
  			0.92f,0.5f,
			0.99f,0.55f,
			0
  		);
	}
	
	void RenameButtons(){
    	mineLabel_im.buttonGo.name = "MineLabel_im";
    	mineLabel_tx.buttonGo.name = "MineLabel_tx";
    }
	
	
	
	void Activate(){
		mineLabel_im.buttonGo.SetActive(true);
		mineLabel_tx.buttonGo.SetActive(true);
	}
	
	public void DeActivate(){
		mineLabel_im.buttonGo.SetActive(false);
		mineLabel_tx.buttonGo.SetActive(false);
	}
	
	
	
	
	public void SelectIron(int amount){
		mineLabel_tx.tx_button.text = amount.ToString();
		mineLabel_im.im_button[1].sprite = iron;
		Activate();
		
	}
	public void SelectGold(int amount){
		mineLabel_tx.tx_button.text = amount.ToString();
		mineLabel_im.im_button[1].sprite = gold;
		Activate();
	}
	
	public void UpdateAmount(int amount){
		mineLabel_tx.tx_button.text = amount.ToString();
	}
	
	
	void Start () {
	
	}
	
}
