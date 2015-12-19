using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollCountController : MonoBehaviour {
	public int counter = 1;
	public int counter2 = 1;
	
	private Text txt = null;
	public SpawnPoint spawner = null;
	
	public GameObject model = null;
	private RTSMaster rtsm = null;
	
	void Awake(){
		rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
	}
	
	// Use this for initialization
	void Start () {
		counter = 1;
		counter2 = 1;
		if(spawner == null){
			SelectionMark selM = rtsm.selectionMark;
			if(selM.selectedGoPars.Count > 0){
				if(selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>() != null){
					spawner = selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>();
				}
			}
		}
		
		if(spawner != null){
			counter = spawner.formationSize;
		}
	//	txt = this.gameObject.GetComponent<Text>();
		txt = rtsm.buildingButtons.butCounter.tx_button;
		if(txt != null){
			txt.text = counter.ToString();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	    
		if (Input.GetAxis("Mouse ScrollWheel")> 0){
		    int increm = 1;
		    if(spawner == null){
		        SelectionMark selM = rtsm.selectionMark;
		    	if(selM.selectedGoPars.Count > 0){
		    		if(selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>() != null){
		    			spawner = selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>();
		    		}
		    	}
		    }
		    if(spawner != null){
				increm = spawner.formationSize;
			}
			else{
				Debug.Log("empty");
			}
			counter2 = counter2+1;
			counter = counter2*increm;
			if(counter>255){
			    counter2 = 1;
				counter = increm;
			}
			
			if(txt != null){
				txt.text = counter.ToString();
			}
			
		}
		
		if (Input.GetAxis("Mouse ScrollWheel")< 0){
		    int increm = 1;
		    if(spawner == null){
		        SelectionMark selM = rtsm.selectionMark;
		    	if(selM.selectedGoPars.Count > 0){
		    		if(selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>() != null){
		    			spawner = selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>();
		    		}
		    	}
		    }
		    if(spawner != null){
				increm = spawner.formationSize;
			}
		    
		    counter2 = counter2-1;
		    counter = counter2*increm;
			if(counter<1){
				counter = GetCurrentMaximumSize(increm,255);
				counter2=(int)(1f*counter/increm);
			}
			
			if(txt != null){
				txt.text = counter.ToString();
			}
		}
	}
	
	
	public int GetCurrentMaximumSize(int increm, int fulMax){
		int cIncrem = increm;
		int i = 0;
		if(increm < fulMax){
			while((cIncrem+increm <= fulMax)&&(i<1000)){
				if(cIncrem+increm <= fulMax){
					cIncrem = cIncrem + increm;
					i++;
				}
				else{
					i++;
				}
			}
		}
		else{
			cIncrem = fulMax;
		}
		return cIncrem;
	}
	
	
	
	
	public void StartSpawnng(){
	    rtsm.bottomBarInfo.DisableBBI();
	    SelectionMark selM = rtsm.selectionMark;
		spawner = selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>();
		if(spawner != null){
			spawner.numberOfObjects = counter;
			
			if(model != null){
				spawner.model = model;
				
			}
			spawner.StartSpawning();
//			rtsm.buildProgressNum.Activate();
	//		rtsm.buildDiplomacyMenu.go_cancelSpawnButton.SetActive(true);
			rtsm.cancelSpawnButton.Activate();
		//	rtsm.buildDiplomacyMenu.tx_buildProgressNum.text = (0.ToString()+"/"+counter.ToString());
			rtsm.buildProgressNum.UpdateText(1.ToString()+"/"+counter.ToString());
//			UnitPars up = spawner.gameObject.GetComponent<UnitPars>();
		//	if(unitPropertiesButton[selCandidate_Pars.propertiesMenuList] != null){
//			rtsm.cameraOperator.unitPropertiesButton[up.propertiesMenuList].SetActive(false);
		}
		counter = 1;
		if(txt != null){
			txt.text = counter.ToString();
		}
		spawner = null;
	}
	
}
