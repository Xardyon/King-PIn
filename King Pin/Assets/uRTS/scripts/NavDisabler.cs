using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavDisabler : MonoBehaviour {
    public List<GameObject> units = new List<GameObject>();
    public List<NavMeshAgent> unitsNav = new List<NavMeshAgent>();
    
    private int i = 0;
 //   private int maxi1 = 8;
 //   private int maxi2 = 10;
    
    private int maxN = 5;
    private int unitCount;
    
    private int mini;
    private int maxi;
    
    private int pmini = 0;
    private int pmaxi = 0;
    
    
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(i==0){
			GetGos();
		}
		i++;
		
		if(unitCount>0){
			mini = unitCount*(i-1)/maxN;
			maxi = unitCount*i/maxN-1;
		
			for(int j=mini; j<=maxi; j++){
				unitsNav[j].enabled = true;
			}
			for(int j=pmini; j<=pmaxi; j++){
				unitsNav[j].enabled = false;
			}
		
		
			pmini = mini;
			pmaxi = maxi;
		
// 			Debug.Log(" "+
// 				unitCount.ToString()+" "+
// 				i.ToString()+" "+
// 				mini.ToString()+" "+
// 				maxi.ToString()+" "
// 			);
		}
		
		if(i>=maxN){
			i = 0;
		}
		
		
// 		if(i==1){
// 			for(int j=0; j<unitsNav.Count; j++){
// 				unitsNav[j].enabled = false;
// 			}
// 		}
// 		else if((i>=maxi1)&&(i<maxi2)){
// 			for(int j=0; j<unitsNav.Count; j++){
// 				unitsNav[j].enabled = true;
// 			}
// 		}
// 		else if(i>maxi2){
// 			i = 0;
// 		}
		
	}
	
	public void GetGos(){
		units = GameObject.FindGameObjectsWithTag("Unit").ToList();
		unitsNav = new List<NavMeshAgent>();
		unitCount = units.Count;
		for(int i=0; i<units.Count; i++){
			unitsNav.Add(units[i].GetComponent<NavMeshAgent>());
		}
	}
}
