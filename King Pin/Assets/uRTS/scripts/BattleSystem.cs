using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour {

// BSystem is core component for simulating RTS battles
// It has 6 phases for attack and gets all different game objects parameters inside.
// Attack phases are: Search, Approach target, Attack, Self-Heal, Die, Rot (Sink to ground).
// All 6 phases are running all the time and checking if object is matching criteria, then performing actions
// Movements between different phases are also described

	
	public float attackDistance = 300.0f;

	
	
	private float[] timeloops = new float[7];
	private float[] timeall = new float[7];
	
	private float[] performance = new float[7];

	private string message = " ";
	
	private string message1 = " ";
	private string message2 = " ";
	private string message3 = " ";
	private string message4 = " ";
	private string message5 = " ";
	private string message6 = " ";
	[HideInInspector] public bool displayMessage = false;


	
	private float dist;
	

	
	
	public static int maxIndApproachers = 5;
	public static int maxIndAttackers = 5;
	

	public List<UnitPars> unitssUP = null;

	public List<UnitPars> unitsBuffer = new List<UnitPars>();



    public List<UnitPars> selfHealers = new List<UnitPars>();
    public List<UnitPars> deads = new List<UnitPars>();
    
 

    public List<UnitPars> selfHealersBuffer = new List<UnitPars>();
    public List<UnitPars> deadsBuffer = new List<UnitPars>();

	public List<UnitPars> sinks = new List<UnitPars>();
	
	
	private Scores sc;
	
	public RTSMaster rtsm;
	
	
	
//	static private SelectionMark selM;    

	
	
	void Awake(){
		
//		selM = rtsm.selectionMark;
		sc = rtsm.scores;
		unitssUP = rtsm.allUnits;
	}
	
	
	
	
	// Use this for initialization
	void Start () {
	

		
// starting to add units to main unitss array		
		StartCoroutine(AddBuffer());

		
// Starts all 6 coroutines to start searching for possible units in unitss array.
		StartCoroutine(ApproachTargetPhase());
		StartCoroutine(AttackPhase());
		StartCoroutine(SelfHealingPhase());
		StartCoroutine(DeathPhase());
		StartCoroutine(SinkPhase());
		
		StartCoroutine(ManualRestorer());
		StartCoroutine(PathResetter());
		
		StartCoroutine(UnitsVelocities());
		
		StartCoroutine(TowerAttack());
	
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI (){
	
	// Display performance
		if(displayMessage){
    		GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.05f, 500f, 20f), message);
    		
        	GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.2f, 500f, 20f), message1);
        	GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.3f, 500f, 20f), message2);
        	GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.4f, 500f, 20f), message3);
        	GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.5f, 500f, 20f), message4);
        	GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.6f, 500f, 20f), message5);
        	GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.7f, 500f, 20f), message6);
    	}
	}
	

	
	
	
	
	
	public IEnumerator ApproachTargetPhase(){

// this phase starting attackers to move towards their targets
	    
	    float timeBeg;
	    float twaiter;
	    

	    
	    float Rtarget;
	    
	    float stoppDistance;
	    
	    int pCount = 700;
	    

	    int ii = 0;
	    
	    Diplomacy dip = rtsm.diplomacy;
	    
	    
		while(true){
		    timeBeg = Time.realtimeSinceStartup;
		    
		    
		    int noApproachers = 0;
		    

		    twaiter = 0.0f;
		    
            ii++;
		
		
// checking through main unitss array which units are set to approach (isApproaching)			
		    for(int i = 0; i<unitssUP.Count; i++){
		        
		        ii++;
		        
		 	    UnitPars apprPars = unitssUP[i];
		 		
		 		

		 		if(apprPars.militaryMode == 20){
		    	
				 UnitPars targPars = apprPars.targetUP;		
				 	
				 NavMeshAgent apprNav = apprPars.thisNMA;				
				 NavMeshAgent targNav = apprPars.targetNMA;
				 
				 
		    	 
		    	 bool relationCont = true;
		    	 
		    	 
		    	 if(targPars != null){
					 if(dip.relations[apprPars.nation][targPars.nation]!=1){
						if(apprPars.nation != targPars.nation){
							if(apprPars.strictApproachMode == false){
								relationCont = false;
								ResetSearching(apprPars);
							
								if(ii > pCount){
									yield return new WaitForSeconds(0.05f);
									twaiter = twaiter + 0.05f;
									ii = 0;
								}
								continue;
							}
						}
					 }
		    	 }
		    	 if(relationCont == true){
		    	
					if(targPars != null){
					
						if(targPars.isApproachable == true){
						
				 
						
					
						
							// stopping condition for NavMesh
						
							apprNav.stoppingDistance = apprNav.radius/(apprPars.transform.localScale.x) + targNav.radius/(targPars.transform.localScale.x);
				
						  // distance between approacher and target
					
							Rtarget = (targPars.transform.position - apprPars.transform.position).magnitude;
						
							ii = ii + 2;
					 
					 
							stoppDistance = (apprPars.stopDistOut + apprPars.transform.localScale.x*targPars.transform.localScale.x*apprNav.stoppingDistance);
					
						// counting increased distances (failure to approach) between attacker and target;
						// if counter failedR becomes bigger than critFailedR, preparing for new target search.
					
							if(apprPars.strictApproachMode == true){
							
							
							// for manual approachers
							
							
								float sD = 0.0f;
					
								if(apprPars.isArcher == false){
									sD = stoppDistance;
								}
								else if(apprPars.isArcher == true){
							//		float critRShoot = apprPars.velArrow*apprPars.velArrow/9.81f - (apprPars.stopDistOut-apprPars.stopDistIn);
									if(CanHitCoordinate(apprPars.transform.position, targPars.transform.position, targPars.velocityVector, apprPars.velArrow, 0.4f)==true){
										sD = 1.5f*Rtarget;
									}
									else{
										sD = 0f;
									}
								//	sD = critRShoot;
							
								}
						
						
								if(Rtarget < sD){
									apprPars.StopUnit("Idle");
// 									if(apprNav.enabled == true){
// 										apprNav.SetDestination(apprPars.transform.position);
// 										
// 									}
							
							// pre-setting for attacking
							
							//		apprPars.isApproaching = false;
									apprPars.isAttacking = true;
								
									apprPars.militaryMode = 30;
					
							
								}
								else{
								
				
									if(apprPars.animationToUse == 2){
										if(apprPars.animationMode == 100){
											apprPars.thisAnim.Play("Knight_Walk");
											apprPars.animationMode = 101;
										
										}
								
								
									}
									if(apprPars.animationToUse == 1){
								//		apprPars.PlayAnimation("Walk");
										
// 										SpriteLoader spL = apprPars.thisSL;
// 										if(spL.animName != "Walk"){
// 											spL.animName = "Walk";
// 											rtsm.spritesManagerMaster.SetAnimation(spL,apprPars.rtsUnitId);
// 										}
									
									}
				
			
							// starting to move
						
									if(apprPars.isMovable){
										noApproachers = noApproachers+1;
										
										apprPars.MoveUnit(targPars.transform.position, "Walk");
										
// 										if(apprNav.enabled == true){
// 											apprNav.SetDestination(targPars.transform.position);
// 											apprNav.speed = 1.5f;
// 										}
									}
							
						
								}
								if(ii > pCount){
									yield return new WaitForSeconds(0.05f);
									twaiter = twaiter + 0.05f;
									ii = 0;
								}
								continue;
							
							
							}
					
							if(apprPars.prevR < Rtarget){
						  
								apprPars.failedR = apprPars.failedR + 1;
								if(apprPars.failedR > apprPars.critFailedR){
															
									targPars.noAttackers = targPars.noAttackers - 1;
									if(targPars.isAttackable == false){
										if(targPars.noAttackers < targPars.maxAttackers){
											targPars.isAttackable = true;
										}
									}
								
									SetTarget(apprPars,null);
									apprPars.targetNMA = null;
								
							//	!!!!!!!!!!!!!	apprPars.isApproaching = false;
							
									if(apprPars.onManualControl == false){
									//	apprPars.isReadyBeg = true;
										apprPars.militaryMode = 10;
									}
									apprPars.failedR = 0;
								
									if(apprPars.animationToUse == 2){
										if(apprPars.animationMode == 101){
									   //     appr.GetComponent<Animation>().GetComponent<Animation>().Play("Knight_Walk", PlayMode.StopAll);
											apprPars.thisAnim.Play("Knight_Walk", PlayMode.StopAll);
											apprPars.animationMode = 100;
										
										}
									}
									if(apprPars.animationToUse == 1){
										apprPars.StopUnit("Idle");
// 										SpriteLoader spL = apprPars.thisSL;
// 										if(spL.animName != "Walk"){
// 											spL.animName = "Walk";
// 											rtsm.spritesManagerMaster.SetAnimation(spL,apprPars.rtsUnitId);
// 										}
									}
									if(ii > pCount){
										yield return new WaitForSeconds(0.05f);
										twaiter = twaiter + 0.05f;
										ii = 0;
									}
									continue;
								}
						  
				
							}
				
						
							else{
							// if approachers already close to their targets
							
								float sD = 0.0f;
					
								if(apprPars.isArcher == false){
									sD = stoppDistance;
								}
								else if(apprPars.isArcher == true){
									if(CanHitCoordinate(apprPars.transform.position, targPars.transform.position, targPars.velocityVector, apprPars.velArrow, 0.4f)==true){
										sD = 1.5f*Rtarget;
									}
									else{
										sD = 0f;
									}
								}
						
						
								if(Rtarget < sD){
									apprPars.StopUnit("Idle");
// 									if(apprNav.enabled == true){
// 										
// 										apprNav.SetDestination(apprPars.transform.position);
// 									}
							
							// pre-setting for attacking
							
									apprPars.isAttacking = true;
									apprPars.militaryMode = 30;
					
							
								}
								else{
						
				
									if(apprPars.animationToUse == 2){
										if(apprPars.animationMode == 100){
											apprPars.thisAnim.Play("Knight_Walk");
											apprPars.animationMode = 101;
										
										}
									}
									if(apprPars.animationToUse == 1){
										apprPars.PlayAnimation("Walk");
										
// 										SpriteLoader spL = apprPars.thisSL;
// 										if(spL.animName != "Walk"){
// 											spL.animName = "Walk";
// 											rtsm.spritesManagerMaster.SetAnimation(spL,apprPars.rtsUnitId);
// 										}
									}
				
			
							// starting to move
						
									if(apprPars.isMovable){
										noApproachers = noApproachers+1;
										apprPars.MoveUnit(targPars.transform.position, "Walk");
										
// 										if(apprNav.enabled == true){
// 										
// 									
// 											apprNav.SetDestination(targPars.transform.position);
// 											apprNav.speed = 1.5f;
// 										}
									}
							
						
								}
							}
						// saving previous R
							apprPars.prevR = Rtarget;
							if(ii > pCount){
								yield return new WaitForSeconds(0.05f);
								twaiter = twaiter + 0.05f;
								ii = 0;
							}
							continue;
						}
					
					
					
					// condition for non approachable targets	
						else{
					    
						targPars.noAttackers = targPars.noAttackers - 1;
						if(targPars.isAttackable == false){
				   			if(targPars.noAttackers < targPars.maxAttackers){
				    			targPars.isAttackable = true;
				    		}
						}
						
					    SetTarget(apprPars,null);
					    apprPars.targetNMA = null;
					    
					    apprPars.StopUnit("Idle");
					    
// 					    if(apprNav.enabled == true){
// 							apprNav.SetDestination(apprPars.transform.position);
// 						}
					
						
						if(apprPars.onManualControl == false){
							apprPars.militaryMode = 10;
						}
						
						if(apprPars.animationToUse == 2){
								    if(apprPars.animationMode == 101){
								    	apprPars.thisAnim.Play("Knight_Walk", PlayMode.StopAll);
								        apprPars.animationMode = 100;
								    }
						}
						
//						if(apprPars.animationToUse == 1){
// 							SpriteLoader spL = apprPars.thisSL;
// 							if(spL.animName != "Idle"){
// 									spL.animName = "Idle";
// 									rtsm.spritesManagerMaster.SetAnimation(spL,apprPars.rtsUnitId);
// 							}
//						}
						
					}
					}
				 }
		    	}
		    	
		    	ii = ii + 1;
		    	
		    	
		    	
		    	if(ii > pCount){
		 			yield return new WaitForSeconds(0.05f);
		 			twaiter = twaiter + 0.05f;
		 			ii = 0;
		 		}
		        
		    	
		    }
		    if(ii > pCount){
		 			yield return new WaitForSeconds(0.05f);
		 			twaiter = twaiter + 0.05f;
		 			ii = 0;
		 	}
		    	    
		    
		    
		    
		    
		    
		    
		    
		    
		    
		    
	
// main coroutine wait statement and performance information collection from approach coroutine			
		
		
			
			
			
			twaiter = twaiter + 0.1f;
		
			yield return new WaitForSeconds(0.1f);
		
			
			float curTime = Time.realtimeSinceStartup;
	
			timeloops[2] = curTime - timeBeg - twaiter;
			timeall[2] = curTime - timeBeg;
			
			performance[2] = (curTime-timeBeg-twaiter)*100.0f/(curTime-timeBeg);
			
			message2 = ("Approacher: " + (noApproachers).ToString() + "; "+(noApproachers).ToString() + "; " + (curTime-timeBeg-twaiter).ToString() + "; " + (performance[2]).ToString() + "% ");
		}
	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	public IEnumerator AttackPhase(){
    
    // Attacking phase set attackers to attack their targets and cause damage when they already approached their targets
    
    float timeBeg;
	float timeEnd;
	

	float twaiter;

	
	float Rtarget;
	    
	float stoppDistance;
	
    

    
    float tempRand = 0.0f;
    
    float tempStrength = 1.0f;
    float tempDefence = 1.0f;
    
    int attackersPhaseWaiterCount = 1500;
    int ii=0;
    
    Diplomacy dip = rtsm.diplomacy;
    
    
	while(true){
		timeBeg = Time.realtimeSinceStartup;
	    
        twaiter = 0.0f;
        
        

        
        int noAttackers = 0;
		    

		    
	// checking through main unitss array which units are set to approach (isAttacking)
        
        for(int i = 0; i<unitssUP.Count; i++){
   //     	GameObject att = unitssUP[i].gameObject;
        	UnitPars attPars = unitssUP[i];
        	

        	
        	if(attPars.isAttacking){
        	
        	//		GameObject targ = attPars.target;
        			UnitPars targPars = attPars.targetUP;
        			
        			bool relationCont = true;
		    	 
		    	 	if(dip.relations[attPars.nation][targPars.nation]!=1){
		    	 		if(attPars.nation != targPars.nation){
		    	 			if(attPars.strictApproachMode == false){
		    	 				relationCont = false;
		    	 				ResetSearching(attPars);
		    	 			}
		    	 		}
		    	 	}
		    	 	if(relationCont == true){
		    	
					NavMeshAgent attNav = attPars.thisNMA;
			//		NavMeshAgent attNav = unitssNMA[i];
					NavMeshAgent targNav = attPars.targetNMA;
					
					attNav.stoppingDistance = attNav.radius/(attPars.transform.localScale.x) + targNav.radius/(targPars.transform.localScale.x);
				
				// distance between attacker and target
				
					Rtarget = (targPars.transform.position - attPars.transform.position).magnitude;
					stoppDistance = (attPars.stopDistOut + attPars.transform.localScale.x*targPars.transform.localScale.x*attNav.stoppingDistance);
					
					
				// auto-correction for archers, who can't reach their targets in large enough distance
					
					
					float sD = 0.0f;
					
					if(attPars.isArcher == false){
						sD = stoppDistance;
					}
					else if(attPars.isArcher == true){
						if(CanHitCoordinate(attPars.transform.position, targPars.transform.position, targPars.velocityVector, attPars.velArrow, 0.1f)==true){
							sD = 1.5f*Rtarget;
						}
						else{
							sD = 0f;
						}
					//	sD = critRShoot;
					}
					
				// if target moves away, resetting back to approach target phase
					if(Rtarget > sD){
						attPars.isAttacking = false;
						attPars.militaryMode = 20;
					}
					
					
					
				// if targets becomes immune, attacker is reset to start searching for new target 	
					else if(targPars.isImmune == true){
					
							attPars.isAttacking = false;
							if(attPars.onManualControl == false){
								attPars.militaryMode = 10;
							}
						
							
							
							targPars.attackers.Remove(attPars);
							
							targPars.noAttackers = targPars.noAttackers - 1;
							if(targPars.isAttackable == false){
				   				if(targPars.noAttackers < targPars.maxAttackers){
				    				targPars.isAttackable = true;
				    			}
							}
							
							attPars.PlayAnimation("Idle");
							
// 							if(attPars.animationToUse == 1){
// 							    SpriteLoader spL = attPars.thisSL;
// 								if(spL.animName != "Idle"){
// 									spL.animName = "Idle";
// 									rtsm.spritesManagerMaster.SetAnimation(spL,attPars.rtsUnitId);
// 								}
// 							}
					}
				
				// attacker starts attacking their target	
					else{
						noAttackers = noAttackers + 1;
				
						if(attPars.animationToUse == 1){
						    
						    attPars.PlayAnimation("Attack");
						    
// 						    SpriteLoader spL = attPars.thisSL;
// 							if(spL.animName != "Attack"){
// 									spL.animName = "Attack";
// 									rtsm.spritesManagerMaster.SetAnimation(spL,attPars.rtsUnitId);
// 									
// 							}
						}
						tempRand = Random.value;
						
						tempStrength = attPars.strength;
						tempDefence = attPars.defence;
						
					// if attack passes target through target defence, cause damage to target
						
						
						if(Time.realtimeSinceStartup-attPars.timeMark > attPars.attackWaiter){
						    attPars.timeMark = Time.realtimeSinceStartup;
						    if(attPars.isBuilding == false){
						    	attPars.transform.LookAt(targPars.transform);
						    }
						    if(attPars.isArcher==true){
						        LaunchArrow(attPars,targPars,attPars.transform.position);
						    						    	
						    }
						    else{
								
								if(tempRand > (tempStrength/(tempStrength+tempDefence))){
								    float damage = 2.0f*tempStrength*Random.value;
								    float damageDefended = 2.0f*tempStrength - damage;
								    
								    
								    
			            			targPars.health = targPars.health - damage;
			            			
			            			attPars.levelExp[0] = attPars.levelExp[0] + 0.5f*damage;
			            			attPars.levelExp[1] = attPars.levelExp[1] + damage;
			            			
			            			targPars.levelExp[0] = targPars.levelExp[0] + 0.5f*damageDefended;
			            			targPars.levelExp[2] = targPars.levelExp[2] + damageDefended;
			            			
			            			attPars.UpdateLevel(0);
									attPars.UpdateLevel(1);
									targPars.UpdateLevel(0);
									targPars.UpdateLevel(2);
			            			
			            			sc.damageMade[attPars.nation] = sc.damageMade[attPars.nation] + damage;
			            			sc.damageObtained[targPars.nation] = sc.damageObtained[targPars.nation] + damage;
			            			
			            			if(targPars.health < 0){
			            				if(targPars.health + damage > 0){
			            				    if(targPars.nation != attPars.nation){
			            						rtsm.nationAIs[targPars.nation].beatenUnits[attPars.nation] = rtsm.nationAIs[targPars.nation].beatenUnits[attPars.nation]+1;
			            						UpdateBeatenUnitScores(attPars, targPars);
			            						
			            					}
			            				}
			            			}
			            			
			            	
								}
							}
						}
						
					}
        
        	}
        	
        	
        	}
            
            
            ii = ii + 1;
        	if(ii > attackersPhaseWaiterCount){
        		ii = 0;
        		yield return new WaitForSeconds(0.2f);
        		twaiter = twaiter + 0.2f;
        		
        	}
            
            
        }
        
        
        






	    
	// main coroutine wait statement and performance information collection from attack coroutine	
		
		twaiter = twaiter + 0.2f;
		
		yield return new WaitForSeconds(0.2f);
		
		timeEnd = Time.realtimeSinceStartup;
		
	
		timeloops[3] = timeEnd - timeBeg - twaiter;
		timeall[3] = timeEnd - timeBeg;
		
		performance[3] = (timeEnd-timeBeg - twaiter)*100.0f/(timeEnd-timeBeg);
		
		message3 = ("Attacker: " + (noAttackers).ToString() + "; "+ (noAttackers).ToString() + "; " + (timeEnd-timeBeg - twaiter).ToString() + "; " + (performance[3]).ToString() + "% ");
	}
    
    
    }
	
	
	public void UpdateBeatenUnitScores(UnitPars attPars, UnitPars targPars){
		if(attPars.nation == rtsm.diplomacy.playerNation){
			if(targPars.isBuilding == true){
				rtsm.scores.AddToMasterScoreDiff(0.5f*targPars.totalLevel);
			}
			else if(targPars.isBuilding == false){
				rtsm.scores.AddToMasterScoreDiff(0.05f*targPars.totalLevel);
			}
		}
		else if(targPars.nation == rtsm.diplomacy.playerNation){
			if(targPars.isBuilding == true){
				rtsm.scores.AddToMasterScoreDiff(-1f);
			}
			else if(targPars.isBuilding == false){
				rtsm.scores.AddToMasterScoreDiff(-0.1f);
			}
		}
	}

	
	
	public IEnumerator SelfHealingPhase(){
	
		// Self-Healing phase heals damaged units over time
	
		float timeBeg;
		float timeEnd;
	
		float twaiter;
	
		float t3;
	
		int selfHealingPhaseWaiterCount=1500;
	
	
		while(true){
			timeBeg = Time.realtimeSinceStartup;
		
			int noSelfHealers = 0;
			twaiter = 0.0f;
		
			t3=Time.realtimeSinceStartup;
		
			int ii=0;
		
			selfHealers.Clear();
			
			for(int i = 0; i<selfHealersBuffer.Count; i++){
					UnitPars go = selfHealersBuffer[i];
					selfHealers.Add(go);
					ii = ii + 1;
					if(ii > selfHealingPhaseWaiterCount){
						if(Time.realtimeSinceStartup-t3>0.005f){
							twaiter = twaiter + 0.1f*(Time.realtimeSinceStartup-t3)+0.05f;
							yield return new WaitForSeconds(0.1f*(Time.realtimeSinceStartup-t3)+0.05f);
							t3=Time.realtimeSinceStartup;
							ii = 0;
						}
					}
				
			}
		
		
		// checking which units are damaged	
		
		
			for(int i = 0; i<selfHealers.Count; i++){
		
				UnitPars sheal = selfHealers[i];
			//	UnitPars shealPars = sheal.GetComponent<UnitPars>();
			
				if(sheal.health < sheal.maxHealth){
			
				// if unit has less health than 0, preparing it to die
			
					if(sheal.health < 0.0f){
						sheal.isHealing = false;
						sheal.isImmune = true;
						sheal.isDying = true;
						selfHealersBuffer.Remove(sheal);
						if(!(deadsBuffer.Contains(sheal))){
							deadsBuffer.Add(sheal);
						}
				
					}
				
				// healing unit	
					else{
						sheal.isHealing = true;
						sheal.health = sheal.health + sheal.selfHealFactor;
						noSelfHealers = noSelfHealers + 1;
					
					
					
					 // if unit health reaches maximum, unset self-healing
					
						if(sheal.health >= sheal.maxHealth){
							sheal.health = sheal.maxHealth;
							sheal.isHealing = false;
							noSelfHealers = noSelfHealers - 1;
						}
					}
				
				}
				ii = ii + 1;
					if(ii > selfHealingPhaseWaiterCount){
						if(Time.realtimeSinceStartup-t3>0.005f){
							twaiter = twaiter + 0.1f*(Time.realtimeSinceStartup-t3)+0.05f;
							yield return new WaitForSeconds(0.1f*(Time.realtimeSinceStartup-t3)+0.05f);
							t3=Time.realtimeSinceStartup;
							ii = 0;
						}
					}
			
			}
		
	// main coroutine wait statement and performance information collection from self-healing coroutine
		
			twaiter = twaiter + 3.0f;

			yield return new WaitForSeconds(3.0f);
		
			timeEnd = Time.realtimeSinceStartup;
		
			timeloops[4] = timeEnd - timeBeg - twaiter;
			timeall[4] = timeEnd - timeBeg;
		
			performance[4] = (timeEnd-timeBeg-twaiter)*100.0f/(timeEnd-timeBeg);
		
			message4 = ("SelfHealing: " + (noSelfHealers).ToString() + "; "+ (selfHealers.Count).ToString() + "; " + (timeEnd-timeBeg-twaiter).ToString() + "; "+ (performance[4]).ToString() + "% ");
		
		
		}
	
	
	}
	
	public IEnumerator DeathPhase(){
	
// Death phase unset all unit activity and prepare to die
	
		float timeBeg;
		float timeEnd;
	
		float twaiter;
	
		float t3;
	
	
		int deathPhaseWaiterCount=1500;
	
	
		while(true){
		    timeBeg = Time.realtimeSinceStartup;
		    
			int noDeads = 0;
			twaiter = 0.0f;
			
			t3=Time.realtimeSinceStartup;
			int ii=0;
			
			deads.Clear();
			
			for(int i = 0; i<deadsBuffer.Count; i++){
				UnitPars up = deadsBuffer[i];
				deads.Add(up);
				ii = ii + 1;
				if(ii > deathPhaseWaiterCount){
					if(Time.realtimeSinceStartup-t3>0.005f){
						twaiter = twaiter + 0.1f*(Time.realtimeSinceStartup-t3)+0.05f;
						yield return new WaitForSeconds(0.1f*(Time.realtimeSinceStartup-t3)+0.05f);
						t3=Time.realtimeSinceStartup;
						ii = 0;
					}
				}
				
			}
			
			
	// Getting dying units		
		
			for(int i = 0; i<deads.Count; i++){
			
				UnitPars dead = deads[i];
			//	UnitPars deadPars = dead.GetComponent<UnitPars>();
				
				if(dead.isDying == true){
					    
					// If unit is dead long enough, prepare for rotting (sinking) phase and removing from the unitss list
					    
					    if(dead.deathCalls > dead.maxDeathCalls){
					    
					    	int nation = dead.nation;
							rtsm.nationAIs[nation].UnsetUnit(dead);
							
							dead.isDying = false;
							dead.isSinking = true;
							
							deadsBuffer.Remove(dead);
							deads.Remove(dead);
						    
						    if(dead.isSelected == true){
						        rtsm.cameraSwitcher.ResetFromUnit(dead);
						        rtsm.cameraOperator.DeselectObject(dead);
						    }
						    
						    dead.thisNMA.enabled = false;
							sinks.Add(dead);
							
							int removeIndex = unitssUP.IndexOf(dead);
							
							unitssUP.RemoveAt(removeIndex);
							
						}
						
					// unsetting unit activity and keep it dying	
						else{
							dead.isMovable = false;
							dead.isAttacking = false;
							dead.isApproachable = false;
							dead.isAttackable = false;
							dead.isHealing = false;
							
							dead.militaryMode = -1;
							
							if(dead.animationToUse == 1){
							    dead.PlayAnimation("Death");
// 							    SpriteLoader spL = dead.thisSL;
// 								spL.animName = "Death";
// 								rtsm.spritesManagerMaster.SetAnimation(spL,dead.rtsUnitId);
								
							}
							
							if(dead.deathCalls == 0){
								if(dead.targetUP!=null){
									
									SetTarget(dead,null);
									
								}
								
								rtsm.resourcesCollection[dead.nation].RemoveFromResourcesCollection(dead);
								
							}
							
							SetTarget(dead,null);
							
							dead.targetNMA = null;
							
						
						
						// unselecting deads	
							dead.gameObject.SendMessage("OnUnselected", SendMessageOptions.DontRequireReceiver);
							dead.transform.gameObject.tag = "Untagged";
							
						// unsetting attackers	
						    NavMeshAgent navM = dead.thisNMA;
						    dead.StopUnit();
						    if(navM.enabled == true){
								
// 								navM.SetDestination(dead.transform.position);
								navM.speed = 0.0f;
							}
							dead.deathCalls = dead.deathCalls + 1;
												    
						    if(dead.isSelected == true){
						        rtsm.cameraSwitcher.ResetFromUnit(dead);
						        rtsm.cameraOperator.DeselectObject(dead);
						//        dead.isSelected = false;
								
						//	    selM.selectedGoT.Remove(dead.transform);
						//        selM.selectedGoPars.Remove(dead);

						//		selM.number_Selected_Objects = selM.number_Selected_Objects-1;
								
								
								
							}
						    
							
					    	noDeads = noDeads + 1;
					    }
				}
			}
			
// main coroutine wait statement and performance information collection from death coroutine			

			twaiter = twaiter + 1.0f;
			yield return new WaitForSeconds(1.0f);
			
			timeEnd = Time.realtimeSinceStartup;
			
			timeloops[5] = timeEnd - timeBeg - twaiter;
			timeall[5] = timeEnd - timeBeg;
			
			performance[5] = (timeEnd-timeBeg-twaiter)*100.0f/(timeEnd-timeBeg);
			
			message5 = ("Dead: " + (noDeads).ToString() + "; "+ (deads.Count).ToString() + "; " + (timeEnd-timeBeg-twaiter).ToString() + "; " + (performance[5]).ToString() + "; ");
			
			
		}
	}
	
	
	
	public IEnumerator SinkPhase(){
	
// rotting or sink phase includes time before unit is destroyed: for example to perform rotting animation or sink object into the ground
	
		float timeBeg;
		float timeEnd;
		
		float twaiter;
		
		while(true){
		    
			timeBeg = Time.realtimeSinceStartup;
			twaiter = 0.0f;
			
	// checking in sinks array, which is already different from main units array
			
			for(int i = 0; i<sinks.Count; i++){
			
				UnitPars sink = sinks[i];
				if(sink.isSinking == true){
				
				// moving sinking object down into the ground	
					if(rtsm.terrainProperties.HeightFromTerrain(sink.transform.position)>-2.0f){
			//		if(sink.transform.position.y>-2.0f){
						sink.transform.Translate(0,-0.03f,0,Space.World);
					}
					
				// destroy object if it has sinked enough	
					else if(rtsm.terrainProperties.HeightFromTerrain(sink.transform.position)<-2.0f){
				//	else if(sink.transform.position.y<-2.0f){
						sinks.Remove(sink);
						
						if(sink.thisSL != null){
					        SpriteLoader spL = sink.thisSL;
						    if(spL.enabled == true){
								spL.unsetSprite();
							}
							else{
								spL.unsetSprite3();
							}
						}
						
						if(sink.isBuilding == false){
								sc.nUnits[sink.nation] = sc.nUnits[sink.nation]-1;
								sc.unitsLost[sink.nation] = sc.unitsLost[sink.nation]+1;
						}
						else if(sink.isBuilding == true){
								sc.nBuildings[sink.nation] = sc.nBuildings[sink.nation]-1;
								sc.buildingsLost[sink.nation] = sc.buildingsLost[sink.nation]+1;
						}
                   //     rtsm.numberOfUnitTypes[sink.nation][sink.rtsUnitId] = rtsm.numberOfUnitTypes[sink.nation][sink.rtsUnitId] - 1;
                        
                        rtsm.DestroyUnit(sink);
                        
					//	Destroy(sink.gameObject);
					}
				
				
				}
			}
			
// main coroutine wait statement and performance information collection from sink coroutine			
			twaiter = twaiter + 0.3f;
			yield return new WaitForSeconds(0.3f);
			
			timeEnd = Time.realtimeSinceStartup;
			
			timeloops[6] = timeEnd - timeBeg - twaiter;
			timeall[6] = timeEnd - timeBeg;
			
			performance[6] = (timeEnd-timeBeg-twaiter)*100.0f/(timeEnd-timeBeg);
		
			message6 = ("Sink: " + (sinks.Count).ToString() + "; " + (timeEnd-timeBeg-twaiter).ToString() + "; " + (performance[6]).ToString() + "% ");
		
			
		}
	}
	
	
	
	
	
	public IEnumerator TowerAttack(){
		while(true){
			for(int i = 0; i<unitssUP.Count; i++){
				UnitPars up = unitssUP[i];
				if(up.rtsUnitId == 10){
					if(up.isBuildFinished == true){
						if(up.isDying == false){
							UnitPars targ = rtsm.battleAIs[up.nation].iddles.GetTarget(up.transform.position);
							if(targ != null){
								Vector3 launchPoint = new Vector3(up.transform.position.x, up.transform.position.y+3f*Random.Range(1,4), up.transform.position.z);
								if(CanHitCoordinate(launchPoint, targ.transform.position, targ.velocityVector, up.velArrow, 0.1f)==true){
									LaunchArrow(up,targ,launchPoint);
								}
							}
						}
					}
				}
			}
			
			yield return new WaitForSeconds(Random.Range(0.5f,1.5f));
		}
	}
	
	
	
	
	
	
	
	
	public void LaunchArrow(UnitPars attPars, UnitPars targPars, Vector3 LaunchPoint){
		Quaternion rot = new Quaternion(0.0f,0.0f,0.0f,0.0f);
		GameObject arroww = (GameObject)Instantiate(attPars.arrow, LaunchPoint,rot);
		
		
		float tempRand = Random.value;
						
		float tempStrength = attPars.strength;
		float tempDefence = attPars.defence;
		
		
		Vector3 arrForce2 = LaunchDirection(LaunchPoint, targPars.transform.position, targPars.velocityVector, attPars.velArrow);
		
		
		if((arrForce2.sqrMagnitude > 0.0f)&&(arrForce2.y != -Mathf.Infinity)&&(arrForce2.y != Mathf.Infinity)){
			arroww.GetComponent<Rigidbody>().AddRelativeForce(arrForce2,ForceMode.VelocityChange);
		
			ArrowPars arrPars = arroww.GetComponent<ArrowPars>();
			
		//	arrPars.attacker = attPars.gameObject;
		//	arrPars.target = targPars.gameObject;
			
			arrPars.attPars = attPars;
			arrPars.targPars = targPars;
			
			arrPars.sc = sc;
		
			if(tempRand > (tempStrength/(tempStrength+tempDefence))){
				arrPars.carriedDamage = 2.0f*tempStrength*Random.value;
				arrPars.maxDamage = 2.0f*tempStrength;
	
			}
		}
		else{
			Destroy(arroww.gameObject);
		}
	}
	
	
	

	
	public IEnumerator AddBuffer(){
	
// adding new units from buffer to BSystem : 
// units, which are wanted to be used on BSystem should be placed to unitsBuffer array first	
	
		int setterCount = 0;
		while(true){
			
			int maxbuffer = unitsBuffer.Count;
			
			if(unitsBuffer.Count>0){
			
				for(int i =0; i<maxbuffer; i++){
					UnitPars goPars = unitsBuffer[i];
				
				
					setterCount = setterCount+1;
					if(setterCount>1000){
						yield return new WaitForSeconds(0.2f);
						setterCount=0;
					}
				
		
					if(goPars.onManualControl == false){
						goPars.militaryMode = 10;
					}
					goPars.isApproachable = true;
					goPars.isAttackable = true;
					goPars.statusBS = 1;
						
					goPars.isOnBS = true;
						
					unitssUP.Add(goPars);
				
				}
			
			// cleaning up buffer
				if(unitsBuffer.Count>0){
					for(int i =0; i<unitssUP.Count; i++){
						unitsBuffer.Remove(unitssUP[i]);
				
						setterCount = setterCount+1;
						if(setterCount>1000){
							yield return new WaitForSeconds(0.2f);
							setterCount=0;
						}
					}
				}
			}
					// overall performance calculation from Battle System
		
			timeloops[0] = timeloops[1] + timeloops[2] + timeloops[3] + timeloops[4] + timeloops[5] + timeloops[6];
			timeall[0] = timeall[1] + timeall[2] + timeall[3] + timeall[4] + timeall[5] + timeall[6];
			
		
			                 
			performance[0] = (performance[1] +
			                  performance[2] +
			                  performance[3] +
			                  performance[4] +
			                  performance[5] +
			                  performance[6])/6.0f;
			                  
			
		
		
			message = ("BSystem: " + (unitssUP.Count).ToString() + "; "
			                     + (timeloops[0]).ToString() + "; "
			                     + (timeall[0]).ToString() + "; "
			                     + (performance[0]).ToString() + "% ");
			
			
			
			yield return new WaitForSeconds(0.3f);
		}
	}
	
	
// ManualMover controls unit if it is selected and target is defined by player
	
	public IEnumerator ManualRestorer(){
	float r;
	
		while(true){
			
			for(int i =0; i<unitssUP.Count; i++){
			
				UnitPars goPars = unitssUP[i];
				
				if(goPars.strictApproachMode == false){
				  if(goPars.isMovingMC){
				    
				    
				    r = (goPars.transform.position - goPars.manualDestination).magnitude;
				    
					if(r >= goPars.prevDist){
					
					    if(r<50f){
							goPars.failedDist = goPars.failedDist+1;
							goPars.critFailedDist = (int)(r*0.3f);
							if(goPars.failedDist > goPars.critFailedDist){
								goPars.failedDist = 0;
							//	goPars.critFailedDist = 10;
								goPars.onManualControl = false;
								goPars.isMovingMC = false;
								ResetSearching(goPars);
							}
					    }
						
					}
					goPars.prevDist = r;
				  }
				
			
				}
				
			}
			
			yield return new WaitForSeconds(0.5f);
		}
	}
	

	
	
	
// single action functions	
	
// 	public void AddUnit(UnitPars up){
// 			unitsBuffer.Add(up);
// 	}
	
	public void AddSelfHealer(UnitPars up){
			selfHealersBuffer.Add(up);
	}
	
// 	public void RemoveUnit(UnitPars goPars){
// //		UnitPars goPars = go.GetComponent<UnitPars>();
// 		goPars.statusBS = 2;
// 	}
	
	
	
	public void SetTarget(UnitPars attPars, UnitPars targPars){
//		GameObject prevTarget = attPars.target;
		UnitPars prevTargetPars = attPars.targetUP;
		
		if(prevTargetPars != null){
			prevTargetPars.attackers.Remove(attPars);
			prevTargetPars.noAttackers = prevTargetPars.attackers.Count;
	//		attPars.target = null;
			attPars.targetUP = null;
		}
		
		if(targPars != null){
//			attPars.target = targPars.gameObject;
			attPars.targetUP = targPars;
			targPars.attackers.Add(attPars);
			targPars.noAttackers = targPars.attackers.Count;
		}
	}
	

	
	
	public void ResetSearching(UnitPars goPars){
	    	
				goPars.isAttacking = false;
				
				if(goPars.targetUP != null){
					
					SetTarget(goPars,null);
					goPars.targetNMA = null;
				}
			    
			    goPars.StopUnit("Idle");
			    
// 			    NavMeshAgent goNav = goPars.thisNMA;
// 		        if(goNav.enabled == true){
// 					goNav.SetDestination(goPars.transform.position);
// 				}
// 				
// 		
// 				if(goPars.thisSL != null){
// 			        SpriteLoader spL = goPars.thisSL;
// 					spL.animName = "Idle";
// 					rtsm.spritesManagerMaster.SetAnimation(spL,goPars.rtsUnitId);
// 				}
			
				goPars.militaryMode = 10;
		
	
	}
	
	
	
	public void RemoveUnitFromBS(UnitPars up){
			
			
			if(unitssUP.Contains(up)){
				int ind = unitssUP.IndexOf(up);
				unitssUP.RemoveAt(ind);
			}
			
			for(int i=0;i<unitssUP.Count;i++){
				if(unitssUP[i].targetUP == up){
					ResetSearching(unitssUP[i]);
				}
			}
			
			unitsBuffer.Remove(up);
			selfHealers.Remove(up);
			deads.Remove(up);
			selfHealersBuffer.Remove(up);
			deadsBuffer.Remove(up);
			sinks.Remove(up);
	}
	
	
		

	
	
	public void UnSetSearching(UnitPars goPars){
	
			goPars.isAttacking = false;
			
			goPars.militaryMode = -1;
			
			if(goPars.targetUP != null){
				
				SetTarget(goPars,null);
				goPars.targetNMA = null;
			}
			
			goPars.StopUnit("Idle");
			
// 			NavMeshAgent goNav = goPars.thisNMA;
// 			if(goNav.enabled == true){
// 				goNav.SetDestination(goPars.transform.position);
// 			}
// 	
// 			if(goPars.thisSL != null){
// 				SpriteLoader spL = goPars.thisSL;
// 				spL.animName = "Idle";
// 				rtsm.spritesManagerMaster.SetAnimation(spL,goPars.rtsUnitId);
// 			}
		
	}
	

	
	public IEnumerator PathResetter(){
	// resets paths for stuck units
		while(true){
		    for(int i=0;i<unitssUP.Count;i++){
		    	
		    	UnitPars up_go = unitssUP[i];
		    	NavMeshAgent nav_go = up_go.thisNMA;
		    	
		    	if(up_go.rtsUnitId == 15){
		    // workers walking phases	
		    		if((up_go.chopTreePhase == 1)||(up_go.chopTreePhase == 3)||(up_go.chopTreePhase == 11)||(up_go.chopTreePhase == 3)){
		    		
						if(up_go.fakePathMode == 1){
							up_go.fakePathCount = up_go.fakePathCount+1;
							if(up_go.fakePathCount > up_go.maxFakePathCount){
								up_go.fakePathMode = 0;
								up_go.fakePathCount = 0;
								nav_go.ResetPath();
								up_go.MoveUnit(up_go.restoreTruePath);
					//			nav_go.SetDestination(up_go.restoreTruePath);
								
								
							}
						}
						else if(up_go.fakePathMode == 0){
							float Rsq = (up_go.transform.position - nav_go.destination).sqrMagnitude;
							if(!(Rsq < up_go.remainingPathDistance)){
								up_go.failPath = up_go.failPath + 1;
								if((up_go.failPath > up_go.maxFailPath)&&(up_go.chopTreePhase != 3)){
						            
						            if(up_go.chopTreePhase == 1){
						            	up_go.chopTreePhase = 6;
						            	
						            	up_go.fakePathMode = 0;
						            	up_go.fakePathCount = 0;
										up_go.remainingPathDistance = 1000000000000f;
						            }
						            else{
										up_go.failPath = 0;
										Vector3 dest = nav_go.destination;
										up_go.restoreTruePath = dest;
										nav_go.ResetPath();
										Vector3 curPos = up_go.transform.position;
										float x = curPos.x+Random.Range(-7f,7f);
										float z = curPos.z+Random.Range(-7f,7f);
								//		Vector3 fakePos = new Vector3(x, rtsm.manualTerrain.SampleHeight(new Vector3(x,0f,z)), z);
										Vector3 fakePos = new Vector3(x, 0f, z);
										up_go.MoveUnit(fakePos);
								//		nav_go.SetDestination(fakePos);
										up_go.fakePathMode = 1;
										up_go.remainingPathDistance = 1000000000000f;
									}
							
								}
								else if((up_go.failPath > 80)&&(up_go.chopTreePhase == 3)){
									up_go.chopTreePhase = 6;
						            	
									up_go.fakePathMode = 0;
									up_go.fakePathCount = 0;
									up_go.remainingPathDistance = 1000000000000f;
								}
							}
							up_go.remainingPathDistance = Rsq;
						}
					}
					else{
						if(up_go.fakePathMode == 1){
							up_go.fakePathMode = 0;
							up_go.fakePathCount = 0;
							up_go.remainingPathDistance = 1000000000000f;
						}
					}
					
		    	}
		    	
		    }
			yield return new WaitForSeconds(0.5f);
		}
	}
	
	
	public IEnumerator UnitsVelocities(){
		while(true){
			
			for(int i=0; i<unitssUP.Count; i++){
			    if(unitssUP[i].isSinking == false){
			    	if(unitssUP[i].isDying == false){
						GameObject go = unitssUP[i].gameObject;
						UnitPars up = unitssUP[i];
						up.velocityVector = (go.transform.position-up.lastPosition) / 0.3f;
						up.lastPosition = go.transform.position;
					}
				}
			}
			
			yield return new WaitForSeconds(0.3f);
		}
	}
	

	
	
	public Vector3 LaunchDirection(Vector3 shooterPosition, Vector3 targetPosition, Vector3 targetVolocity, float launchSpeed){
		
			float vini = launchSpeed;
	
	// horizontal plane projections	
			Vector3 shooterPosition2d = new Vector3(shooterPosition.x, 0f, shooterPosition.z);
			Vector3 targetPosition2d = new Vector3(targetPosition.x, 0f, targetPosition.z);
			
			float Rtarget2d = (targetPosition2d - shooterPosition2d).magnitude;
			
	// shooter and target coordinates		
			float ax = shooterPosition.x;
			float ay = shooterPosition.y;
			float az = shooterPosition.z;

			float tx = targetPosition.x;
			float ty = targetPosition.y;
			float tz = targetPosition.z;
			
			float g = 9.81f;
			
			
			
			
			float sqrt = (vini*vini*vini*vini) - (g * (g * (Rtarget2d * Rtarget2d) + 2 * (ty-ay) * (vini*vini)));
			sqrt = Mathf.Sqrt(sqrt);
			
			float angleInRadians = Mathf.Atan((vini*vini + sqrt) / (g * Rtarget2d));
			float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
			
			if(angleInDegrees > 45f){
				angleInDegrees = 90f-angleInDegrees;
			}
			
			if(angleInDegrees < 0f){
				angleInDegrees = -angleInDegrees;
			}
			
			
			Vector3 rotAxis = Vector3.Cross((targetPosition - shooterPosition),new Vector3(0f,1f,0f));
			
			Vector3 arrForce = (RotAround(-angleInDegrees, (targetPosition - shooterPosition), rotAxis)).normalized;
			
			
			
			
			
			
			
			
			
			// shoting time
			
			float shTime = Mathf.Sqrt( ((tx - ax)*(tx - ax)+(tz - az)*(tz - az))/
									((vini*arrForce.x)*(vini*arrForce.x)+(vini*arrForce.z)*(vini*arrForce.z))
			);
			
			Vector3 finalDirection = vini*arrForce + 0.5f*shTime*targetVolocity;
			
			
			
			return finalDirection;

		
	}
	
	public bool CanHitCoordinate(Vector3 shooterPosition, Vector3 targetPosition, Vector3 targetVolocity, float launchSpeed, float distanceIncrement){
		bool canHit = false;
		
		float vini = launchSpeed;
		float g = 9.81f;
		
		Vector3 shooterPosition2d = new Vector3(shooterPosition.x, 0f, shooterPosition.z);
		Vector3 targetPosition2d = new Vector3(targetPosition.x, 0f, targetPosition.z);
		
		float Rtarget2d = (targetPosition2d - shooterPosition2d).magnitude;
		

		Rtarget2d = Rtarget2d + distanceIncrement*Rtarget2d;
		

		float sqrt = (vini*vini*vini*vini) - (g * (g * (Rtarget2d * Rtarget2d) + 2 * (targetPosition.y-shooterPosition.y) * (vini*vini)));
		
		if(sqrt >= 0){
			canHit = true;
		}
		return canHit;
	}
	
	
	
	Vector3 RotAround(float rotAngle, Vector3 original, Vector3 direction){
	    
	    
	    
	    Vector3 cross1 = Vector3.Cross(original,direction);
	    
	    Vector3 pr = Vector3.Project(original,direction);
	    Vector3 pr2 = original - pr;
	    
	    
	    Vector3 cross2 = Vector3.Cross(pr2,cross1);
	    
	    Vector3 rotatedVector = (Quaternion.AngleAxis( rotAngle, cross2)*pr2)+pr;
	    
	
		return rotatedVector;
	
	}

	
	
	
	
	
	
// Heap sort used for sorting before passing to KDTree

    public static void HeapSort(Vector3[] input, int[] iorig)
   {
    	//Build-Max-Heap
    	int heapSize = input.Length;
    	for (int p = (heapSize - 1) / 2; p >= 0; p--)
        	MaxHeapify(input, iorig, heapSize, p);
 
    	for (int i = input.Length - 1; i > 0; i--)
    	{
        	//Swap
        	Vector3 temp = input[i];
        	input[i] = input[0];
        	input[0] = temp;
        	
        	int itemp = iorig[i];
        	iorig[i] = iorig[0];
        	iorig[0] = itemp;
 
        	heapSize--;
        	MaxHeapify(input, iorig, heapSize, 0);
    	}
    }


    private static void MaxHeapify(Vector3[] input, int[] iorig, int heapSize, int index)
    {
    	int left = (index + 1) * 2 - 1;
    	int right = (index + 1) * 2;
    	int largest = 0;
 
    	if (left < heapSize && input[left].x > input[index].x)
        	largest = left;
    	else
        	largest = index;
 
    	if (right < heapSize && input[right].x > input[largest].x)
        	largest = right;
 
    	if (largest != index)
    	{
        	Vector3 temp = input[index];
        	input[index] = input[largest];
        	input[largest] = temp;
        	
        	int itemp = iorig[index];
        	iorig[index] = iorig[largest];
        	iorig[largest] = itemp;
 
        	MaxHeapify(input, iorig, heapSize, largest);
    	}
    }	
	
	
	
	
	

	
	
}
