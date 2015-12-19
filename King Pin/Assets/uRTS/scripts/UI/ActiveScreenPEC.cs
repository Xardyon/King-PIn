using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class ActiveScreenPEC : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler {

    [HideInInspector] public RTSMaster rtsm = null;


	void Awake(){
//		rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
	}



	public void OnPointerEnter(PointerEventData eventData)
	{ 
		rtsm.cameraOperator.ActiveScreenTrue();
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{  
		rtsm.cameraOperator.ActiveScreenFalse();
	}
}
