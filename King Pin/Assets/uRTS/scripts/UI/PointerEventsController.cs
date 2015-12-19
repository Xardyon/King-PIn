using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PointerEventsController : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler {

//	public int mouseOnCount = 0;
	[HideInInspector] public int pointerEventsControllerId = 0;
	[HideInInspector] public string buttonTitle = "";
	[HideInInspector] public RTSMaster rtsm = null;
	[HideInInspector] public UnitPars spawnGoUP = null;
	
	
	void Awake(){
		rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{ 
//		rtsm.buildDiplomacyMenu.EnableBBI(spawnGoUP);
		rtsm.bottomBarInfo.EnableBBI(spawnGoUP);
//		mouseOnCount = mouseOnCount+1;
//		Debug.Log(mouseOnCount);
        
	}
 
	public void OnPointerExit(PointerEventData eventData)
	{  
//		rtsm.buildDiplomacyMenu.DisableBBI();
		rtsm.bottomBarInfo.DisableBBI();
	}
    
}
