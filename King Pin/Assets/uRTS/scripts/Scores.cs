using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scores : MonoBehaviour {
	
	[HideInInspector] public List<int> nUnits;
	[HideInInspector] public List<int> nBuildings;
	
	[HideInInspector] public List<int> unitsLost;
	[HideInInspector] public List<int> buildingsLost;
	
	[HideInInspector] public List<float> damageMade;
	[HideInInspector] public List<float> damageObtained;
	
	private List<string> message = new List<string>();
	private string messageTitle = " ";
	
	private int messageId = 2;
	
	private int nNations = 0;
	
	private BattleSystem bs;
	private Diplomacy dip;
	
	public float masterScore = 0;
	public float masterScoreDiff = 0;
	
	
	
	[HideInInspector] public int researchExp = 0;
	public RTSMaster rtsm;
	
	
	void Awake () {
	    
	//    masterScoreDiff = -30f;
	    
	    bs = rtsm.battleSystem;
	    dip = rtsm.diplomacy;
	    
	    nNations = dip.numberNations;
	    
	    
		for(int i=0; i<nNations; i++){
		    
			nUnits.Add(0);
			nBuildings.Add(0);
			
			unitsLost.Add(0);
			buildingsLost.Add(0);
			
			damageMade.Add(0f);
			damageObtained.Add(0f);
			
			message.Add(" ");
		}
	}
	
	// Use this for initialization
	void Start () {
		StartCoroutine(MessageUpdate());
		StartCoroutine(UpdateGlobalExp());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T))
		{
            if(messageId==0){
            	messageId=1;
            	bs.displayMessage = false;
            }
            else if(messageId==1){
            	messageId=2;
            }
            else if(messageId==2){
            	messageId=0;
            	bs.displayMessage = true;
            }
		}
		
		// + "; " + countrf[1] + "; " + (curTime - t1 - twaiter).ToString() + "; " + (performance[1]).ToString() + "% ");
	}
	
	
	public IEnumerator MessageUpdate(){
		while(true){
			messageTitle = (
								"Nation              "+
								"n buildings              "+
								"n units              "+
								
								"lost buildings              "+
								"lost units              "+
								
								"damage made              "+
								"damage got              "
							);
			for(int i=0; i<nNations; i++){
				message[i] = (
				              	(i).ToString() + "                            "  +
				              	(nBuildings[i]).ToString() + "                            " +
				               	nUnits[i] + "                            " +
				               	
				               	
				               	buildingsLost[i] + "                            " +
				               	unitsLost[i] + "                            " +
				               	
				               	damageMade[i] + "                            " +
								damageObtained[i]
				               	
				             );
			}
			
			
			yield return new WaitForSeconds(0.5f);
		}
	
	}
	
	public IEnumerator UpdateGlobalExp(){
// default unlocks	
		for(int i=0; i<nNations; i++){
			rtsm.unitTypeLocking[i][0] = 1;
			rtsm.unitTypeLocking[i][1] = 1;
			rtsm.unitTypeLocking[i][2] = 1;
			rtsm.unitTypeLocking[i][3] = 1;
			rtsm.unitTypeLocking[i][4] = 1;
			rtsm.unitTypeLocking[i][5] = 1;
			
		}
	
    	while(true){
    		
    		researchExp = researchExp+1;
    		if(rtsm.numberOfUnitTypes[0][3]>1){
    		    if(rtsm.unitTypeLocking[0][6] == 0){
    		    	rtsm.buildingButtons.buildingCreationGrid[0].buttonsEnableMask[5] = 1;
    		    	rtsm.unitTypeLocking[0][6] = 1;
    		    }
    		}
    		
    		if(rtsm.numberOfUnitTypes[0][6]>0){
    		    if(rtsm.unitTypeLocking[0][7] == 0){
    		    	rtsm.buildingButtons.buildingCreationGrid[0].buttonsEnableMask[6] = 1;
    		    	rtsm.unitTypeLocking[0][7] = 1;
    		    }
    		}
    		
    		if(rtsm.numberOfUnitTypes[0][7]>0){
    		    if(rtsm.unitTypeLocking[0][8] == 0){
    		    	rtsm.buildingButtons.buildingCreationGrid[0].buttonsEnableMask[7] = 1;
    		    	rtsm.unitTypeLocking[0][8] = 1;
    		    }
    		}
    		
    		
    		
    		
    		
    		if(rtsm.numberOfUnitTypes[0][11]>5){
    		    if(rtsm.unitTypeLocking[0][13] == 0){
    		        rtsm.buildingButtons.buildingCreationGrid[1].buttonsEnableMask[2] = 1;
    		    	rtsm.unitTypeLocking[0][13] = 1;
    		    }
    		}
    		if(rtsm.numberOfUnitTypes[0][12]>5){
    		    if(rtsm.unitTypeLocking[0][14] == 0){
    		        rtsm.buildingButtons.buildingCreationGrid[1].buttonsEnableMask[3] = 1;
    		    	rtsm.unitTypeLocking[0][14] = 1;
    		    }
    		}

    		


    		yield return new WaitForSeconds(1f);
    	}
    }
	
	void OnGUI (){
	
	// Display performance
		if ( messageId == 1 )
    	{
    	    GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.05f, 900f, 20f), messageTitle);
    		for(int i=0; i<nNations; i++){
    			GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * (0.05f*(i+1)+0.1f), 900f, 20f), message[i]);
    		}
    	}
	}
	
	public void UpdateMasterScore(){
		masterScore = masterScore+masterScoreDiff;
		masterScoreDiff = 0f;
//		print(masterScore);
	}
	
	public void AddToMasterScoreDiff(float diff){
		masterScoreDiff = masterScoreDiff+diff;
	}
	
}
