using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;




public class Economy : MonoBehaviour {
    public List<int> iron = new List<int>();
    public List<int> gold = new List<int>();
    public List<int> lumber = new List<int>();
    public List<int> population = new List<int>();
    
    [HideInInspector] public Text textIron = null;
    [HideInInspector] public Text textGold = null;
    [HideInInspector] public Text textLumber = null;
    [HideInInspector] public Text textPopulation = null;
    
    [HideInInspector] public Diplomacy dip = null;
    [HideInInspector] public int numNations = 0;
    
    public float resUpdateFactor = 2.5f;
    
    [HideInInspector] public RTSMaster rtsm;
    
    private List<int> populationDifference = new List<int>();
    
    [HideInInspector] public int playerNation = 0;
    
    
    
    void Awake(){
   // 	rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
   // 	rtsm.economy = this;
    	playerNation = rtsm.diplomacy.playerNation;
    	textIron = rtsm.resourcePanel.tx_iron;
    	textGold = rtsm.resourcePanel.tx_gold;
    	textLumber = rtsm.resourcePanel.tx_lumber;
    	textPopulation = rtsm.resourcePanel.tx_population;
    	
    	dip = rtsm.diplomacy;
		numNations = dip.numberNations;
	    
	    for(int i=0;i<numNations;i++){
			iron.Add(1000);
			gold.Add(1000);
			lumber.Add(1000);
			population.Add(50);
			populationDifference.Add(0);
		}
		if(
			(textIron != null)&&
			(textGold != null)&&
			(textLumber != null)
		){
			textIron.text = iron[playerNation].ToString();
			textGold.text = gold[playerNation].ToString();
			textLumber.text = lumber[playerNation].ToString();
		}
    	
    	
    }
    
	// Use this for initialization
	void Start () {
		
		

		
   //     HugeResources(0);
   //     VeryLargeResources(1);
   //     VeryLargeResources(2);
   //     HugeResources(2);	
		StartCoroutine(UpdateTaxes());
		StartCoroutine(IncreasePopulation());
		
		
	}
	
	
	IEnumerator UpdateTaxes(){
		while(
			(textIron == null)&&
			(textGold == null)&&
			(textLumber == null)
		){
			yield return new WaitForSeconds(0.2f);
		}
		
		while(true){
		    yield return new WaitForSeconds(90f);
		    
		    
		    for(int i=0;i<numNations;i++){
		    	
		        populationDifference[i] = population[i] - (rtsm.numberOfUnitTypes[i][11]+rtsm.numberOfUnitTypes[i][12]);
		        
		        float goldMod = 2f;
		        
		  //      print(rtsm.nationAIs.Count);
		        if(rtsm.nationAIs[i].masterNationId == -1){
					iron[i] = iron[i] + (int) (resUpdateFactor*populationDifference[i]);
					gold[i] = gold[i] + (int) (resUpdateFactor*goldMod*populationDifference[i]);
					lumber[i] = lumber[i] + (int) (resUpdateFactor*populationDifference[i]);
				}
				else{
					if(populationDifference[i]<=0){
						iron[i] = iron[i] + (int) (resUpdateFactor*populationDifference[i]);
						gold[i] = gold[i] + (int) (resUpdateFactor*goldMod*populationDifference[i]);
						lumber[i] = lumber[i] + (int) (resUpdateFactor*populationDifference[i]);
					}
					else{
						iron[i] = iron[i] + (int) (0.5f*resUpdateFactor*populationDifference[i]);
						gold[i] = gold[i] + (int) (0.5f*resUpdateFactor*goldMod*populationDifference[i]);
						lumber[i] = lumber[i] + (int) (0.5f*resUpdateFactor*populationDifference[i]);
						
						int masterId = rtsm.nationAIs[i].masterNationId;
						iron[masterId] = iron[masterId] + (int) (0.5f*resUpdateFactor*populationDifference[i]);
						gold[masterId] = gold[masterId] + (int) (0.5f*resUpdateFactor*goldMod*populationDifference[i]);
						lumber[masterId] = lumber[masterId] + (int) (0.5f*resUpdateFactor*populationDifference[i]);
					}
				}
				
				if(iron[i]<0){
					iron[i] = 0;
				}
				if(gold[i]<0){
					gold[i] = 0;
				}
				if(lumber[i]<0){
					lumber[i] = 0;
				}
			}
			
			if(
				(textIron != null)&&
				(textGold != null)&&
				(textLumber != null)
			){
				textIron.text = iron[playerNation].ToString();
				textGold.text = gold[playerNation].ToString();
				textLumber.text = lumber[playerNation].ToString();
			}
			
			rtsm.scores.UpdateMasterScore();
			
			rtsm.diplomacyMenu.MakeReport("Taxes collected, wages paid");
		
		
			
		}
	}
	
	IEnumerator IncreasePopulation(){
	    while(
			(textPopulation == null)
		){
			yield return new WaitForSeconds(0.2f);
		}
		
	    while(true){
	        for(int i=0;i<numNations;i++){
	            if(population[i] < (5*rtsm.numberOfUnitTypes[i][3]+20)){
					population[i] = population[i]+1;
				}
				populationDifference[i] = population[i] - (rtsm.numberOfUnitTypes[i][11]+rtsm.numberOfUnitTypes[i][12]);
			}
			
			if((textPopulation != null)){
				textPopulation.text = population[dip.playerNation].ToString();
				if(populationDifference[dip.playerNation] < 0){
					textPopulation.color = new Color(0.79f,0.22f,0.22f,1f);
				}
				else{
					textPopulation.color = new Color(0.61f,0.50f,0.13f,1f);
				}
			}
		
			yield return new WaitForSeconds(3f);
		}
	}
	
	
	
	public void AddIron(int nationId, int amount){
	    int finalAmount = amount;
	    if(rtsm.nationAIs[nationId].masterNationId == -1){
			iron[nationId] = iron[nationId] + amount;
		}
		else{
		    int master = rtsm.nationAIs[nationId].masterNationId;
		    
		    int half = (int) (0.5f*amount);
		    int half2 = amount - half;
		    
			iron[nationId] = iron[nationId]+half;
			iron[master] = iron[master]+half2;
			finalAmount = half;
			
		}
		if(nationId == playerNation){
		    rtsm.scores.AddToMasterScoreDiff(0.005f*finalAmount);
			textIron.text = iron[playerNation].ToString();
		}
	}
	public void AddGold(int nationId, int amount){
	    int finalAmount = amount;
	    if(rtsm.nationAIs[nationId].masterNationId == -1){
			gold[nationId] = gold[nationId] + amount;
		}
		else{
		    int master = rtsm.nationAIs[nationId].masterNationId;
		    
		    int half = (int) (0.5f*amount);
		    int half2 = amount - half;
		    
			gold[nationId] = gold[nationId]+half;
			gold[master] = gold[master]+half2;
			finalAmount = half;
			
		}
		if(nationId == playerNation){
		    rtsm.scores.AddToMasterScoreDiff(0.005f*finalAmount);
			textGold.text = gold[playerNation].ToString();
		}
	}
	public void AddLumber(int nationId, int amount){
	    int finalAmount = amount;
	    if(rtsm.nationAIs[nationId].masterNationId == -1){
			lumber[nationId] = lumber[nationId] + amount;
		}
		else{
		    int master = rtsm.nationAIs[nationId].masterNationId;
		    
		    int half = (int) (0.5f*amount);
		    int half2 = amount - half;
		    
			lumber[nationId] = lumber[nationId]+half;
			lumber[master] = lumber[master]+half2;
			finalAmount = half;
			
		}
		if(nationId == playerNation){
		    rtsm.scores.AddToMasterScoreDiff(0.005f*finalAmount);
			textLumber.text = lumber[playerNation].ToString();
		}
	}
	
	
	
	
	
	
	public void HugeResources(int nationId){
		iron[nationId] = 100000;
		gold[nationId] = 100000;
		lumber[nationId] = 100000;
		population[nationId] = 50000;

	}

	public void VeryLargeResources(int nationId){
		iron[nationId] = 50000;
		gold[nationId] = 50000;
		lumber[nationId] = 50000;
		population[nationId] = 5000;

	}
	public void LargeResources(int nationId){
		iron[nationId] = 30000;
		gold[nationId] = 30000;
		lumber[nationId] = 30000;
		population[nationId] = 1000;
		if(nationId == playerNation){
			RefreshResources();
		}

	}
	
	public void RefreshResources(){
		textIron.text = iron[dip.playerNation].ToString();
		textGold.text = gold[dip.playerNation].ToString();
		textLumber.text = lumber[dip.playerNation].ToString();
		textPopulation.text = population[dip.playerNation].ToString();
	}


	
	
}
