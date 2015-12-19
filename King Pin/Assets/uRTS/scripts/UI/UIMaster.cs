using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIMaster : MonoBehaviour {

	[HideInInspector] public RTSMaster rtsm;
	
	public GameObject mainCanvas;
	
	[HideInInspector] public List<Sprite> unitsIcons = new List<Sprite>();
	[HideInInspector] public List<Sprite> relationIcons = new List<Sprite>();
	[HideInInspector] public Sprite troopsIcon;
	
	
	
	public GameObject SetSubCanvas(string name){
		GameObject subCanvas = new GameObject(name);
		subCanvas.transform.SetParent(mainCanvas.transform);
		RectTransform rect = subCanvas.AddComponent<RectTransform>();
		
		subCanvas.layer = LayerMask.NameToLayer("UI");
		
		rect.anchorMin = new Vector2(0f,0f);
		rect.anchorMax = new Vector2(1f,1f);
		
		rect.offsetMin = new Vector2(0f,0f);
		rect.offsetMax = new Vector2(0f,0f);
		
		return subCanvas;
	}	
	
	
	public void LoadIcons(){
        int N = rtsm.rtsUnitTypePrefabs.Count;
        for(int i=0; i<N; i++){
    		unitsIcons.Add(Resources.Load<Sprite>("textures/icons/"+rtsm.rtsUnitTypePrefabs[i].GetComponent<UnitPars>().unitName+"_ico"));
    	}
    	troopsIcon = Resources.Load<Sprite>("textures/icons/population_ico");
    	
    // relation icons	
    	relationIcons.Add(Resources.Load<Sprite>("UI/icons/peace"));
    	relationIcons.Add(Resources.Load<Sprite>("UI/icons/war"));
    	relationIcons.Add(Resources.Load<Sprite>("UI/icons/crown"));
    	relationIcons.Add(Resources.Load<Sprite>("UI/icons/bags"));
    	relationIcons.Add(Resources.Load<Sprite>("UI/icons/handshake"));
    	
    }
	
	
	
	void Start () {
	
	}
	
	
	
	
	
}
