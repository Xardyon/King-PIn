using UnityEngine;
using System.Collections;

public class ArrowPars : MonoBehaviour {
	
//	[HideInInspector] public GameObject attacker = null;
//	[HideInInspector] public GameObject target = null;
	
	[HideInInspector] public UnitPars attPars = null;
	[HideInInspector] public UnitPars targPars = null;
	
	public float carriedDamage = 0.0f;
	public float maxDamage = 0.0f;
	
	public bool damageApplied = false;
	public bool destrStarted = false;
	
	public float oldVelocity = 1.0f;
	[HideInInspector] public Scores sc = null;
	
	private RTSMaster rtsm;
	
	// Use this for initialization
	void Start () {
	    rtsm = GameObject.Find("RTS Master").GetComponent<RTSMaster>();
		oldVelocity = GetComponent<Rigidbody>().velocity.sqrMagnitude;
	}
	
	// Update is called once per frame
	void Update () {
	    transform.LookAt((GetComponent<Rigidbody>().velocity).normalized+transform.position);
	    
		if(destrStarted == false){
		 if(damageApplied == false){
			if(targPars != null){
				float goDist = (targPars.transform.position - transform.position).sqrMagnitude;
			
				if(goDist < targPars.rEnclosed*targPars.rEnclosed){
					targPars.health = targPars.health - carriedDamage;
					
					float damageDefended = maxDamage - carriedDamage;
					
					attPars.levelExp[0] = attPars.levelExp[0] + 0.5f*carriedDamage;
					attPars.levelExp[1] = attPars.levelExp[1] + carriedDamage;
					
					targPars.levelExp[0] = targPars.levelExp[0] + 0.5f*damageDefended;
					targPars.levelExp[2] = targPars.levelExp[2] + damageDefended;
					
					attPars.UpdateLevel(0);
					attPars.UpdateLevel(1);
					targPars.UpdateLevel(0);
					targPars.UpdateLevel(2);
					
					
					damageApplied = true;
					
					sc.damageMade[attPars.nation] = sc.damageMade[attPars.nation] + carriedDamage;
			        sc.damageObtained[targPars.nation] = sc.damageObtained[targPars.nation] + carriedDamage;
			        
			        if(targPars.health < 0){
						if(targPars.health + carriedDamage > 0){
							if(targPars.nation != attPars.nation){
								rtsm.nationAIs[targPars.nation].beatenUnits[attPars.nation] = rtsm.nationAIs[targPars.nation].beatenUnits[attPars.nation]+1;
							}
						}
					}			
				}
		
		
			}
		 }
    	}
    	
    	if(transform.position.y<-1.0f){
    		Destroy(this.gameObject);
    	}
		
		
		
	}
	
	public IEnumerator DestrPhase(){
		yield return new WaitForSeconds(1.0f);
		Destroy(this.gameObject);
	}
}
