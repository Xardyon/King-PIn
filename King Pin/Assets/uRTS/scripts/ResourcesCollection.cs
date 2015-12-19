using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesCollection : MonoBehaviour {
    
	[HideInInspector] public RTSMaster rtsm = null;
	
//	[HideInInspector] public List<GameObject> workers = new List<GameObject>();
	[HideInInspector] public List<UnitPars> up_workers = new List<UnitPars>();
//	[HideInInspector] public List<NavMeshAgent> nav_workers = new List<NavMeshAgent>();
//	[HideInInspector] public List<SpriteLoader> sl_workers = new List<SpriteLoader>();
	[HideInInspector] public List<int> workers_resType = new List<int>();
	[HideInInspector] public List<int> workers_resAmount = new List<int>();
	
	
//	[HideInInspector] public List<Vector3> treePositions = new List<Vector3>();
//	[HideInInspector] public List<int> treeHealth = new List<int>();
	
//	[HideInInspector] public List<GameObject> sawmills = new List<GameObject>();
	[HideInInspector] public List<UnitPars> up_sawmills = new List<UnitPars>();	
	[HideInInspector] public List<Vector3> sawmillsPos = new List<Vector3>();
//	[HideInInspector] public List<NavMeshAgent> nav_sawmills = new List<NavMeshAgent>();

//	[HideInInspector] public List<GameObject> mines = new List<GameObject>();
	[HideInInspector] public List<UnitPars> up_mines = new List<UnitPars>();	
	[HideInInspector] public List<Vector3> minesPos = new List<Vector3>();
	[HideInInspector] public List<int> minesResource = new List<int>();
	[HideInInspector] public List<int> minesResourceType = new List<int>();	
	[HideInInspector] public List<int> minesResPoints = new List<int>();
	
	
//	[HideInInspector] public List<GameObject> castle = new List<GameObject>();
	[HideInInspector] public List<UnitPars> up_castle = new List<UnitPars>();
	[HideInInspector] public List<Vector3> castlePos = new List<Vector3>();

	
	private Terrain ter = null;
	
//	[HideInInspector] public KDTree kd_treePositions = null;
	[HideInInspector] public KDTree kd_sawmills = null;
	[HideInInspector] public KDTree kd_minesPos = null;
	[HideInInspector] public KDTree kd_castle = null;

      

	// Use this for initialization
	void Start () {
		
		ter = rtsm.manualTerrain;
		StartCoroutine(WorkerChopPhases());
		StartCoroutine(WorkerMinePhases());
		

	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonUp(1)){
		//     Vector3 hitLocation = Vector3.zero;
		     bool hitted = false;
		     
		     RaycastHit hit;
			 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (ter.GetComponent<Collider>().Raycast (ray, out hit, 10000f)) {
                 hitted = true;
             }
             if(hitted == true){
                if(rtsm.createForest.treePositions.Count>0){
					rtsm.createForest.kd_treePositions = KDTree.MakeFromPoints(rtsm.createForest.treePositions.ToArray());
					int treeId = rtsm.createForest.kd_treePositions.FindNearest(hit.point);
					if(
						(rtsm.createForest.treePositions[treeId]-hit.point).sqrMagnitude
						<
						25f
					){
					    ChopTree(treeId);
					
					
					}
					else if(minesPos.Count>0){
						kd_minesPos = KDTree.MakeFromPoints(minesPos.ToArray());
						int mineId = kd_minesPos.FindNearest(hit.point);
						if(
							(minesPos[mineId]-hit.point).sqrMagnitude
							<
							45f
						){
							MineResource(mineId);
					
					
						}
						else{
							WalkTo(hit.point);
					
						}
				
					}
					
					
					
					else{
             	        WalkTo(hit.point);
					
					}
					
             	}
             	else if(minesPos.Count>0){
             		kd_minesPos = KDTree.MakeFromPoints(minesPos.ToArray());
             		int mineId = kd_minesPos.FindNearest(hit.point);
             		if(
						(minesPos[mineId]-hit.point).sqrMagnitude
						<
						1.3f*up_mines[mineId].rEnclosed*up_mines[mineId].rEnclosed
					//	45f
					){
					    MineResource(mineId);
					
					
					}
					else{
             	        WalkTo(hit.point);
					
					}
             	
             	}
             	
             	else{
             	    
             	    WalkTo(hit.point);
             	    
             	}
             	
             	
             }

		}
		
		
	}
    
    public void WalkTo(Vector3 dest){
	   for(int i=0;i<up_workers.Count;i++){
			if(up_workers[i].isSelected==true){
// 			    float R1 = (up_workers[i].transform.position-dest).magnitude;
// 			    if(R1>150f){
// 			    	print("bbb1");
// 			    	print(R1);
// 			    }
				
				up_workers[i].MoveUnit(dest, "Walk");
				
// 				up_workers[i].thisNMA.SetDestination(
// 					dest
// 				);
				up_workers[i].chopTreeId = -1;
				up_workers[i].chopTreePhase = -1;
// 				up_workers[i].thisSL.animName = "Walk";
// 				rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
				
				workers_resType[i] = -1;
			}
		
		}

    }
    
    void ChopTree(int treeId){
    	for(int i=0;i<up_workers.Count;i++){
			if(up_workers[i].isSelected==true){
// 			    float R1 = (up_workers[i].transform.position-rtsm.createForest.treePositions[treeId]).magnitude;
// 			    if(R1>150f){
// 			    	print("bbb2");
// 			    	print(R1);
// 			    }
// 				up_workers[i].thisNMA.SetDestination(
// 					rtsm.createForest.treePositions[treeId]
// 				);
                up_workers[i].MoveUnit(rtsm.createForest.treePositions[treeId],"Walk");
                
				up_workers[i].chopTreeId = treeId;
				up_workers[i].chopTreePhase = 1;
// 				up_workers[i].thisSL.animName = "Walk";
// 				rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
				
				workers_resType[i] = 2;
			}
		}
    }
    
    void MineResource(int mineId){
    	for(int i=0;i<up_workers.Count;i++){
			if(up_workers[i].isSelected==true){
			    up_workers[i].MoveUnit(minesPos[mineId], "Walk");
// 				up_workers[i].thisNMA.SetDestination(
// 					minesPos[mineId]
// 				);
				up_workers[i].chopTreeId = mineId;
				up_workers[i].chopTreePhase = 11;
// 				up_workers[i].thisSL.animName = "Walk";
// 				rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
				
				workers_resType[i] = minesResourceType[mineId];
			}
		}
    }

    
    

    IEnumerator WorkerChopPhases(){
    	while(true){
    	    bool kd_sawmill_ready = false;
    	    bool kd_treePositions_ready = false;
    	    
    		for(int i=0; i<up_workers.Count; i++){
    			if(up_workers[i].chopTreePhase == 1){
    			    if(up_workers[i].chopTreeId < rtsm.createForest.treeList.Count){
						if(
							(rtsm.createForest.treePositions[up_workers[i].chopTreeId] - up_workers[i].transform.position).sqrMagnitude
							<
							16f
						){
							up_workers[i].StopUnit("Attack");
// 							up_workers[i].thisNMA.ResetPath();
// 							up_workers[i].thisSL.animName = "Attack";
// 							rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
							up_workers[i].chopTreePhase = 2;
							up_workers[i].maxChopHits = 150;
						}
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 2){
    				up_workers[i].chopHits = up_workers[i].chopHits + 1;
    				if(up_workers[i].chopHits > up_workers[i].maxChopHits){
    				
    					int chTreeId = up_workers[i].chopTreeId;
    					workers_resAmount[i] = 50;
    					if(rtsm.createForest.treeHealth[chTreeId]-50 < 0){
    						workers_resAmount[i] = rtsm.createForest.treeHealth[chTreeId];
    					}
    					rtsm.createForest.treeHealth[chTreeId] = rtsm.createForest.treeHealth[chTreeId]-50;
    					
    					
						if(rtsm.createForest.treeHealth[chTreeId]<0){	
						    
						    RemoveTree(chTreeId);
						    
					 		
						}
    					
    					if(up_sawmills.Count > 0){
    					    if(kd_sawmill_ready == false){
    					    	kd_sawmills = KDTree.MakeFromPoints(sawmillsPos.ToArray());
    					    	kd_sawmill_ready = true;
    					    }
    					    
    					    int neighId = kd_sawmills.FindNearest(up_workers[i].transform.position);
    					    Vector3 dest = sawmillsPos[neighId];
             	        	    	
    				//	    up_workers[i].thisNMA.SetDestination(dest);
             	        	
             	        	up_workers[i].MoveUnit(dest, "WalkLog");
             	        	
    					    up_workers[i].targetSawmillId = neighId;
    				//		up_workers[i].thisSL.animName = "WalkLog";
					//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    						up_workers[i].chopTreePhase = 3;
    						up_workers[i].chopHits = 0;
    						
    						
    						
    						
    					}
    					else{
    					    up_workers[i].StopUnit("IdleLog");
//     						up_workers[i].thisSL.animName = "IdleLog";
// 							rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    						up_workers[i].chopTreePhase = 4;
    						up_workers[i].chopHits = 0;
    					}
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 3){
    			    
    			    if(up_sawmills.Count>0){
    			        if(up_workers[i].targetSawmillId < up_sawmills.Count){
							float rEncl = up_sawmills[up_workers[i].targetSawmillId].thisNMA.radius + up_workers[i].thisNMA.radius;
						//	float rEncl = up_sawmills[up_workers[i].targetSawmillId].rEnclosed;
							if(
								(sawmillsPos[up_workers[i].targetSawmillId] - up_workers[i].transform.position).sqrMagnitude
								<
								(rEncl*rEncl*2.5f)
							//	(0.3f*rEncl*rEncl)
							){
							
								int nationId = up_sawmills[up_workers[i].targetSawmillId].nation;
							//	rtsm.economy.lumber[nationId] = rtsm.economy.lumber[nationId] + workers_resAmount[i];
								
								rtsm.economy.AddLumber(nationId, workers_resAmount[i]);
								workers_resAmount[i] = 0;
							//	if(nationId == 0){
							//		rtsm.economy.textLumber.text = rtsm.economy.lumber[0].ToString();
							//	}
						
								if(rtsm.createForest.treePositions.Count>0){
									if((up_workers[i].chopTreeId < rtsm.createForest.treePositions.Count)&&(up_workers[i].chopTreeId >= 0)){
										float R1 = (up_workers[i].transform.position-rtsm.createForest.treePositions[up_workers[i].chopTreeId]).magnitude;
										if(R1>300f){
										    up_workers[i].StopUnit("Idle");
									//		up_workers[i].thisNMA.ResetPath();
									//		up_workers[i].chopTreePhase = 5;
									//		up_workers[i].thisSL.animName = "Idle";
									//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
	// 								    	print("bbb3");
	// 								    	print(R1);
	// 								    	print(up_workers[i].chopTreeId);
										}
										else{
										    up_workers[i].MoveUnit(rtsm.createForest.treePositions[up_workers[i].chopTreeId],"Walk");
								//			up_workers[i].thisNMA.SetDestination(
								//					rtsm.createForest.treePositions[up_workers[i].chopTreeId]
								//				);
											up_workers[i].chopTreePhase = 1;
								//			up_workers[i].thisSL.animName = "Walk";
								//			rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
										}
									}
								}
								else{
								    up_workers[i].StopUnit("Idle");
							//		up_workers[i].thisNMA.ResetPath();
									up_workers[i].chopTreePhase = 5;
							//		up_workers[i].thisSL.animName = "Idle";
							//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
								}
						
							}
						}
    				}
    				else{
    				    up_workers[i].StopUnit("IdleLog");
    			//		up_workers[i].thisSL.animName = "IdleLog";
				//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    					up_workers[i].chopTreePhase = 4;
    					up_workers[i].chopHits = 0;
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 4){
    				if(up_sawmills.Count > 0){
    					if(kd_sawmill_ready == false){
							kd_sawmills = KDTree.MakeFromPoints(sawmillsPos.ToArray());
							kd_sawmill_ready = true;
						}
						
						int neighId = kd_sawmills.FindNearest(up_workers[i].transform.position);
						
						up_workers[i].MoveUnit(sawmillsPos[neighId],"WalkLog");
						
// 						up_workers[i].thisNMA.SetDestination(
// 							sawmillsPos[
// 								neighId
// 							]
// 						);
						up_workers[i].targetSawmillId = neighId;
//						up_workers[i].thisSL.animName = "WalkLog";
					//	if(sl_workers[i].enabled == false){
//						rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
					//	}
						up_workers[i].chopTreePhase = 3;
						up_workers[i].chopHits = 0;
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 5){
    				if(rtsm.createForest.treePositions.Count>0){
    				    if(kd_treePositions_ready == false){
    						rtsm.createForest.kd_treePositions = KDTree.MakeFromPoints(rtsm.createForest.treePositions.ToArray());
    						kd_treePositions_ready = true;
    					}
    					int neighId = rtsm.createForest.kd_treePositions.FindNearest(up_workers[i].transform.position);
    					
//     					float R2 = (rtsm.createForest.treePositions[neighId] - up_workers[i].transform.position).magnitude;
//     					if(R2>150f){
// 							print("aaa2");
// 							print((rtsm.createForest.treePositions[neighId] - up_workers[i].transform.position).magnitude);
//     					}

						up_workers[i].MoveUnit(rtsm.createForest.treePositions[neighId],"Walk");
						
//     					up_workers[i].thisNMA.SetDestination(
//              	        	    rtsm.createForest.treePositions[
//              	        	    	neighId
//              	        	    ]
//              	        	);
             	        up_workers[i].chopTreeId = neighId;
             	        up_workers[i].chopTreePhase = 1;
//              	        up_workers[i].thisSL.animName = "Walk";
// 						rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);    					
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 6){
    				if(rtsm.createForest.treePositions.Count>0){
    				    if(kd_treePositions_ready == false){
    						rtsm.createForest.kd_treePositions = KDTree.MakeFromPoints(rtsm.createForest.treePositions.ToArray());
    						kd_treePositions_ready = true;
    					}
    		            int rand = 1;
    		            if(rtsm.createForest.treePositions.Count>6){
    		            	rand = Random.Range(1,6);
    		            }
    		            else{
    		            	rand = Random.Range(1,rtsm.createForest.treePositions.Count);
    		            }
    		            	
    					int neighId = rtsm.createForest.kd_treePositions.FindNearestK(up_workers[i].transform.position,rand);
    					
//     					float R2 = (rtsm.createForest.treePositions[neighId] - up_workers[i].transform.position).magnitude;
//     					if(R2>150f){
// 							print("aaa3");
// 							print((rtsm.createForest.treePositions[neighId] - up_workers[i].transform.position).magnitude);
//              	        }
                        
                        up_workers[i].MoveUnit(rtsm.createForest.treePositions[neighId],"Walk");
                        	    	
//     					up_workers[i].thisNMA.ResetPath();
//     					up_workers[i].thisNMA.SetDestination(
//              	        	    rtsm.createForest.treePositions[
//              	        	    	neighId
//              	        	    ]
//              	        	);
             	        
             	        up_workers[i].chopTreeId = neighId;
             	        up_workers[i].chopTreePhase = 1;
//              	        up_workers[i].thisSL.animName = "Walk";
// 						rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    					
    				}
    			}
    			

    			
    		}
    		yield return new WaitForSeconds(0.1f);
    	}
    
    
    }
    
    
    
    
    
    
    IEnumerator WorkerMinePhases(){
    	while(true){
    	    bool kd_castle_ready = false;
    		bool kd_mines_ready = false;
    		
    		for(int i=0; i<up_workers.Count; i++){
    			if(up_workers[i].chopTreePhase == 11){
    			    if(up_workers[i].chopTreeId < minesPos.Count){
						if(
							(up_workers[i].TerrainVector(minesPos[up_workers[i].chopTreeId]) - up_workers[i].TerrainVector(up_workers[i].transform.position)).sqrMagnitude
						//	(minesPos[up_workers[i].chopTreeId] - up_workers[i].transform.position).sqrMagnitude
							<
							0.8f*up_mines[up_workers[i].chopTreeId].rEnclosed*up_mines[up_workers[i].chopTreeId].rEnclosed
						){
						    up_workers[i].StopUnit("IdlePouch");
					//		up_workers[i].thisNMA.ResetPath();
					//		up_workers[i].thisSL.animName = "IdlePouch";
					//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
							up_workers[i].chopTreePhase = 12;
							up_workers[i].maxChopHits = 2;
						}
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 12){
    				up_workers[i].chopHits = up_workers[i].chopHits + 1;
    				if(up_workers[i].chopHits > up_workers[i].maxChopHits){
    				
    					int chTreeId = up_workers[i].chopTreeId;
    					
    					if(chTreeId < minesResource.Count){
							workers_resAmount[i] = 100;
							if(minesResource[chTreeId]-100 < 0){
								workers_resAmount[i] = minesResource[chTreeId];
							}
							minesResource[chTreeId] = minesResource[chTreeId]-100;
							up_mines[chTreeId].remainingResources = minesResource[chTreeId];
							if(up_mines[chTreeId].remainingResources < 0){
								up_mines[chTreeId].remainingResources = 0;
							}
							if(up_mines[chTreeId].isSelected == true){
						//		rtsm.cameraOperator.mineLabel.text = up_mines[chTreeId].remainingResources.ToString();
								rtsm.mineLabel.UpdateAmount(up_mines[chTreeId].remainingResources);
							}
						
							int resPointId = minesResPoints[chTreeId];
						
							rtsm.resourcePoint.resAmount[resPointId] = rtsm.resourcePoint.resAmount[resPointId] - 100;
							if(minesResource[chTreeId]<0){	
							
								up_mines[chTreeId].selfHealFactor = -(int)(0.1f*up_mines[chTreeId].maxHealth)-1;
								up_mines[chTreeId].health = up_mines[chTreeId].health +2*up_mines[chTreeId].selfHealFactor;
							
						// 		rtsm.nationCenters[up_mines[chTreeId].nation].GetComponent<NationAI>().UnsetUnit(mines[chTreeId]);
							
						//		mines.RemoveAt(chTreeId);
								up_mines.RemoveAt(chTreeId);
								minesPos.RemoveAt(chTreeId);
								minesResource.RemoveAt(chTreeId);
								minesResourceType.RemoveAt(chTreeId);
								minesResPoints.RemoveAt(chTreeId);
							
							
							
							// removal of resource point from map	
								if(rtsm.resourcePoint.resAmount[resPointId]<0){
									for(int jj=0;jj<minesResPoints.Count;jj++){
										if(minesResPoints[chTreeId]>resPointId){
											minesResPoints[chTreeId] = minesResPoints[chTreeId]-1;
										}
									}
								
									rtsm.resourcePoint.UnsetResourcePoint(resPointId);
								
								
								
								
								
								
								}
							
							
								if(minesPos.Count>0){
									kd_minesPos = KDTree.MakeFromPoints(minesPos.ToArray());
							
									for(int j=0; j<up_workers.Count; j++){
										if(up_workers[j].chopTreeId == chTreeId){
									
											int neighTreeId = kd_minesPos.FindNearest(up_workers[j].transform.position);
											up_workers[j].chopTreeId = neighTreeId;
											if(j != i){
												if(up_workers[j].chopTreePhase == 11){
													up_workers[j].MoveUnit(minesPos[neighTreeId]);
											//		up_workers[j].thisNMA.SetDestination(
											//			minesPos[neighTreeId]
											//		);
												}
												else if(up_workers[j].chopTreePhase == 12){
											//		up_workers[j].thisNMA.SetDestination(
											//			minesPos[neighTreeId]
											//		);
											        up_workers[j].MoveUnit(minesPos[neighTreeId],"Walk");
													up_workers[j].chopTreePhase = 11;
											//		up_workers[j].thisSL.animName = "Walk";
											//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
												}
											}
									
										}
										else if(up_workers[j].chopTreeId > chTreeId){
											up_workers[j].chopTreeId = up_workers[j].chopTreeId-1;
										}
									}
								}
								else{
									for(int j=0; j<up_workers.Count; j++){
										if(j != i){
											if(up_workers[j].chopTreePhase == 11){
											    up_workers[j].StopUnit("Idle");
										//		up_workers[j].thisNMA.ResetPath();
										//		up_workers[j].thisSL.animName = "Idle";
										//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
												up_workers[j].chopTreePhase = 15;
											}
											else if(up_workers[j].chopTreePhase == 12){
												up_workers[j].PlayAnimation("Idle");
										//		up_workers[j].thisSL.animName = "Idle";
										//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
												up_workers[j].chopTreePhase = 15;
											}
										
										}
									
									}
								}
							
							}
    					
    					}
    					if(up_castle.Count > 0){
    					    if(kd_castle_ready == false){
    					    	kd_castle = KDTree.MakeFromPoints(castlePos.ToArray());
    					    	kd_castle_ready = true;
    					    }
    					    
    					    int neighId = kd_castle.FindNearest(up_workers[i].transform.position);
    					    
    					    up_workers[i].MoveUnit(castlePos[neighId], "WalkPouch");
    					    
//     					    up_workers[i].thisNMA.SetDestination(
//              	        	    castlePos[
//              	        	    	neighId
//              	        	    ]
//              	        	);
    					    up_workers[i].targetSawmillId = neighId;
//     						up_workers[i].thisSL.animName = "WalkPouch";
// 							rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    						up_workers[i].chopTreePhase = 13;
    						up_workers[i].chopHits = 0;
    						
    						
    						
    					}
    					else{
    					    up_workers[i].StopUnit("IdlePouch");
//     						up_workers[i].thisSL.animName = "IdlePouch";
// 							rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    						up_workers[i].chopTreePhase = 14;
    						up_workers[i].chopHits = 0;
    					}
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 13){
    				if(up_castle.Count>0){
    				    if(up_castle.Count > up_workers[i].targetSawmillId){
							float rEncl = up_castle[up_workers[i].targetSawmillId].rEnclosed;
							if(
								(castlePos[up_workers[i].targetSawmillId] - up_workers[i].transform.position).sqrMagnitude
								<
								(1.0f*rEncl*rEncl)
							){
								int nationId = up_castle[up_workers[i].targetSawmillId].nation;
						
								if(workers_resType[i] == 0){
							//		rtsm.economy.iron[nationId] = rtsm.economy.iron[nationId] + workers_resAmount[i];
									rtsm.economy.AddIron(nationId, workers_resAmount[i]);
									workers_resAmount[i] = 0;
							//		if(nationId == 0){
							//			rtsm.economy.textIron.text = rtsm.economy.iron[0].ToString();
							//		}
								}
								if(workers_resType[i] == 1){
							//		rtsm.economy.gold[nationId] = rtsm.economy.gold[nationId] + workers_resAmount[i];
									rtsm.economy.AddGold(nationId, workers_resAmount[i]);
									workers_resAmount[i] = 0;
							//		if(nationId == 0){
							//			rtsm.economy.textGold.text = rtsm.economy.gold[0].ToString();
							//		}
								}
						
						
						
								if(minesPos.Count>0){
									if(up_workers[i].chopTreeId < minesPos.Count){
										up_workers[i].MoveUnit(minesPos[up_workers[i].chopTreeId],"Walk");
									//	up_workers[i].thisNMA.SetDestination(
									//			minesPos[up_workers[i].chopTreeId]
									//		);
										up_workers[i].chopTreePhase = 11;
									//	up_workers[i].thisSL.animName = "Walk";
									//	rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
									}
								}
								else{
								    up_workers[i].StopUnit("Idle");
							//		up_workers[i].thisNMA.ResetPath();
									up_workers[i].chopTreePhase = 15;
							//		up_workers[i].thisSL.animName = "Idle";
							//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
								}
							}
						}
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 14){
    				if(up_castle.Count > 0){
    					if(kd_castle_ready == false){
							kd_castle = KDTree.MakeFromPoints(castlePos.ToArray());
							kd_castle_ready = true;
						}
						
						int neighId = kd_castle.FindNearest(up_workers[i].transform.position);
						
						up_workers[i].MoveUnit(castlePos[neighId],"WalkPouch");
						
				//		up_workers[i].thisNMA.SetDestination(
				//			castlePos[
				//				neighId
				//			]
				//		);
						up_workers[i].targetSawmillId = neighId;
				//		up_workers[i].thisSL.animName = "WalkPouch";
				//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
						up_workers[i].chopTreePhase = 13;
						up_workers[i].chopHits = 0;
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 15){
    				if(minesPos.Count>0){
    				    if(kd_mines_ready == false){
    						kd_minesPos = KDTree.MakeFromPoints(minesPos.ToArray());
    						kd_mines_ready = true;
    					}
    					int neighId = kd_minesPos.FindNearest(up_workers[i].transform.position);
    					
    					up_workers[i].MoveUnit(minesPos[neighId],"Walk");
    					
    			//		up_workers[i].thisNMA.SetDestination(
             	//        	    minesPos[
             	//        	    	neighId
             	//        	    ]
             	//        	);
             	        up_workers[i].chopTreeId = neighId;
             	        up_workers[i].chopTreePhase = 11;
             	//        up_workers[i].thisSL.animName = "Walk";
				//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    					
    				}
    			}
    			else if(up_workers[i].chopTreePhase == 16){
    				if(minesPos.Count>0){
    				    if(kd_mines_ready == false){
    						kd_minesPos = KDTree.MakeFromPoints(minesPos.ToArray());
    						kd_mines_ready = true;
    					}
    					int neighId = Random.Range(0,up_mines.Count);
    					//kd_minesPos.FindNearest(workers[i].transform.position,1);
    					
    					up_workers[i].MoveUnit(minesPos[neighId],"Walk");
    					
    			//		up_workers[i].thisNMA.SetDestination(
             	//        	    minesPos[
             	//        	    	neighId
             	//        	    ]
             	//        	);
             	        up_workers[i].chopTreeId = neighId;
             	        up_workers[i].chopTreePhase = 11;
             	//        up_workers[i].thisSL.animName = "Walk";
				//		rtsm.spritesManagerMaster.SetAnimation(up_workers[i].thisSL,up_workers[i].rtsUnitId);
    					
    				}
    			}
    			
    			
    		}
    		yield return new WaitForSeconds(0.1f);
    	}
    
    
    }
    
    
    
    
    
    
    
    public void SetAutoChopper(UnitPars goPars){
 //   	UnitPars goPars = go.GetComponent<UnitPars>();
    	
    	if(up_workers.Contains(goPars)){
    	    
    		int i = up_workers.IndexOf(goPars);
    		
    		workers_resType[i] = 2;
    		
    		up_workers[i].chopTreePhase = 5;
    		int neighTreeId = rtsm.createForest.kd_treePositions.FindNearest(up_workers[i].transform.position);
//     		float R2 = (rtsm.createForest.treePositions[neighTreeId] - up_workers[i].transform.position).magnitude;
//     		if(R2>150f){
// 				print("aaa4");
// 				print((rtsm.createForest.treePositions[neighTreeId] - up_workers[i].transform.position).magnitude);
//     		}
    		up_workers[i].chopTreeId = neighTreeId;
    		
    		up_workers[i].targetSawmillId = 0;
    		
    		
    	}
    }
    


    public void SetAutoMiner(UnitPars goPars, int resTp){
  //  	UnitPars goPars = go.GetComponent<UnitPars>();
    	if(up_workers.Contains(goPars)){
    		if(up_mines.Count > 0){
    		    
    		    List<int> suitableMines = new List<int>();
    		    for(int ii=0; ii<up_mines.Count; ii++){
    		    	if(up_mines[ii].resourceType == resTp){
    		    		suitableMines.Add(ii);
    		    	}
    		    }
    		    
    		    if(suitableMines.Count > 0){
					int i = up_workers.IndexOf(goPars);
				// finding random mine    
					int j = suitableMines[Random.Range(0, suitableMines.Count)];
					up_workers[i].chopTreeId = j;
				//    print(j);
					workers_resType[i] = up_mines[j].resourceType;
					up_workers[i].chopTreePhase = 16;
    		    }
    		    
    		}
    	}
    }


	
	IEnumerator AssignScripts(){
		bool cont = true;
		while(cont == true){
			
			if(rtsm.diplomacy != null){
			    up_workers.Clear();
				int NN = rtsm.diplomacy.numberNations;
		
				for(int i=0;i<NN;i++){
		//			workers.Add(new List<GameObject>());
				}
				cont = false;
			}
			
			yield return new WaitForSeconds(0.02f);
		}
	}
	
	
	
	
	
	
	public void RemoveTree(int chTreeId){
		
		
// 		for(int j=0; j<up_workers.Count; j++){
// 			float r77 = (up_workers[j].transform.position - rtsm.createForest.treePositions[up_workers[j].chopTreeId]).magnitude;
// 			if(r77>350f){
// 				print("ddd0");
// 				print(r77);
// 			}
// 		}
		
						    
		int removalCount = rtsm.createForest.treeListPartCount[chTreeId];
		int removalIndex = rtsm.createForest.treeFirstPartId[chTreeId];
		
		
		for(int j=0;j<removalCount;j++){
			rtsm.createForest.treeList.RemoveAt(removalIndex);
		}
		
		rtsm.createForest.treePos2D.RemoveAt(chTreeId);
		for(int j=chTreeId+1;j<rtsm.createForest.treeFirstPartId.Count;j++){
			rtsm.createForest.treeFirstPartId[j] = rtsm.createForest.treeFirstPartId[j]-removalCount;
		}
		
		rtsm.createForest.treeListPartCount.RemoveAt(chTreeId);
		rtsm.createForest.treeFirstPartId.RemoveAt(chTreeId);
		
		rtsm.createForest.RefreshTrees();
		
		rtsm.createForest.treePositions.RemoveAt(chTreeId);
		rtsm.createForest.treeHealth.RemoveAt(chTreeId);
		
// 		print("uuu1");
// 		print(chTreeId);
		
		if(rtsm.createForest.treePositions.Count>0){
			rtsm.createForest.kd_treePositions = KDTree.MakeFromPoints(rtsm.createForest.treePositions.ToArray());
		
			for(int j=0; j<up_workers.Count; j++){
				if(up_workers[j].chopTreeId == chTreeId){
				
					int neighTreeId = rtsm.createForest.kd_treePositions.FindNearest(up_workers[j].transform.position);
					
// 					float R1 = (rtsm.createForest.treePositions[neighTreeId] - up_workers[j].transform.position).magnitude;
// 					if(R1>350f){
// 						print("aaa1");
// 						print((rtsm.createForest.treePositions[neighTreeId] - up_workers[j].transform.position).magnitude);
// 					}
					
					up_workers[j].chopTreeId = neighTreeId;
			//		if(j != i){
						if(up_workers[j].chopTreePhase == 1){
						    up_workers[j].MoveUnit(rtsm.createForest.treePositions[neighTreeId]);
			//				up_workers[j].thisNMA.SetDestination(
			//					rtsm.createForest.treePositions[neighTreeId]
			//				);
						}
						else if(up_workers[j].chopTreePhase == 2){
							up_workers[j].MoveUnit(rtsm.createForest.treePositions[neighTreeId],"Walk");
			//				up_workers[j].thisNMA.SetDestination(
			//					rtsm.createForest.treePositions[neighTreeId]
			//				);
							up_workers[j].chopTreePhase = 1;
			//				up_workers[j].thisSL.animName = "Walk";
			//				rtsm.spritesManagerMaster.SetAnimation(up_workers[j].thisSL,up_workers[j].rtsUnitId);
						}
			//		}
				
				}
				else if(up_workers[j].chopTreeId > chTreeId){
				    if(up_workers[j].chopTreePhase < 10){
						up_workers[j].chopTreeId = up_workers[j].chopTreeId-1;
	// 					float R11 = (up_workers[j].transform.position-rtsm.createForest.treePositions[up_workers[j].chopTreeId]).magnitude;
	// 					if(R11>150f){
	// 						print("ccc1");
	// 						print(R11);
	// 					}
					}
				}
			}
		}
		else{
			for(int j=0; j<up_workers.Count; j++){
		//		if(j != i){
					if(up_workers[j].chopTreePhase == 1){
					    up_workers[j].StopUnit("Idle");
				//		up_workers[j].thisNMA.ResetPath();
				//		up_workers[j].thisSL.animName = "Idle";
				//		rtsm.spritesManagerMaster.SetAnimation(up_workers[j].thisSL,up_workers[j].rtsUnitId);
						up_workers[j].chopTreePhase = 5;
					}
					else if(up_workers[j].chopTreePhase == 2){
						up_workers[j].PlayAnimation("Idle");
				//		up_workers[j].thisSL.animName = "Idle";
				//		rtsm.spritesManagerMaster.SetAnimation(up_workers[j].thisSL,up_workers[j].rtsUnitId);
						up_workers[j].chopTreePhase = 5;
					}
					
		//		}
				
			}
		}
			
			
// 		for(int j=0; j<up_workers.Count; j++){
// 			float r77 = (up_workers[j].transform.position - rtsm.createForest.treePositions[up_workers[j].chopTreeId]).magnitude;
// 			if(r77>350f){
// 				print("ddd1");
// 				print(r77);
// 			}
// 		}			 		
						
	}
	
	
	
	
	
	
	public void AddToResourcesCollection(GameObject go){
		
		UnitPars goPars = go.GetComponent<UnitPars>();
		if(goPars.rtsUnitId == 15){
		//	workers.Add(goPars);
			up_workers.Add(goPars);
		//	nav_workers.Add(go.GetComponent<NavMeshAgent>());
		//	sl_workers.Add(go.GetComponent<SpriteLoader>());
			workers_resType.Add(-1);
			workers_resAmount.Add(0);
		}
		else if(goPars.rtsUnitId == 2){
	//		sawmills.Add(go);
			up_sawmills.Add(goPars);
			sawmillsPos.Add(
				go.transform.position + RotAround(-go.transform.rotation.eulerAngles.y+180f,(new Vector3(0f,0f,goPars.rEnclosed)),(new Vector3(0f,1f,0f)))
			);
	//		nav_sawmills.Add(go.GetComponent<NavMeshAgent>());
			
		}
		else if(goPars.rtsUnitId == 5){
		//	mines.Add(go);
			up_mines.Add(goPars);
			minesPos.Add(
				goPars.TerrainVector(go.transform.position + RotAround(-go.transform.rotation.eulerAngles.y,(new Vector3(0f,0f,goPars.rEnclosed)),(new Vector3(0f,1f,0f))))
			);
			
			int neigh = rtsm.resourcePoint.kd_allResLocations.FindNearest(go.transform.position);
			float r = (rtsm.resourcePoint.allResLocations[neigh] - go.transform.position).magnitude;
			int resAmount = rtsm.resourcePoint.resAmount[neigh];
			int resToAdd = (int)((1f-0.6f*(r/7f))*resAmount);
			if(resToAdd < 500){
				resToAdd = 500;
				if(resToAdd>resAmount){
					resToAdd = resAmount;
				}
			}
	//		print(resToAdd);
			minesResource.Add(resToAdd);
			goPars.resourceType = rtsm.resourcePoint.resTypes[neigh];
			goPars.remainingResources = resToAdd;
			minesResourceType.Add(rtsm.resourcePoint.resTypes[neigh]);
			minesResPoints.Add(neigh);
			
		}
		else if(goPars.rtsUnitId == 0){
	//		castle.Add(go);
			up_castle.Add(goPars);
			castlePos.Add(
				go.transform.position + RotAround(-go.transform.rotation.eulerAngles.y+90f,(new Vector3(0f,0f,goPars.rEnclosed)),(new Vector3(0f,1f,0f)))
			);
			
		}
		
		
	}
	
	public void RemoveFromResourcesCollection(UnitPars goPars){
	//	UnitPars goPars = go.GetComponent<UnitPars>();
	//	GameObject go = goPars.gameObject;
		if(goPars.rtsUnitId == 15){
			if(up_workers.Contains(goPars)){
				int iii = up_workers.IndexOf(goPars);
		
			//	workers.RemoveAt(iii);
				up_workers.RemoveAt(iii);
			//	nav_workers.RemoveAt(iii);
			//	sl_workers.RemoveAt(iii);
				workers_resType.RemoveAt(iii);
				workers_resAmount.RemoveAt(iii);
			}
		}
		else if(goPars.rtsUnitId == 2){
			if(up_sawmills.Contains(goPars)){
				int iii = up_sawmills.IndexOf(goPars);
	
			//	sawmills.RemoveAt(iii);
				up_sawmills.RemoveAt(iii);
				sawmillsPos.RemoveAt(iii);
	//			nav_sawmills.RemoveAt(iii);
				
				if(sawmillsPos.Count > 0){
					kd_sawmills = KDTree.MakeFromPoints(sawmillsPos.ToArray());
				}
				
				for(int iw=0; iw<up_workers.Count; iw++){
					if(workers_resType[iw]==2){
						if(up_workers[iw].targetSawmillId > iii){
							up_workers[iw].targetSawmillId = up_workers[iw].targetSawmillId-1;
						}
					}
				}
				
			}
		}
		else if(goPars.rtsUnitId == 5){
			if(up_mines.Contains(goPars)){
				int iii = up_mines.IndexOf(goPars);
				
				int resTypeLoc = up_mines[iii].resourceType;
				
				for(int iw=0; iw<up_workers.Count; iw++){
					if(workers_resType[iw]==resTypeLoc){
						if(up_workers[iw].chopTreeId == iii){
							up_workers[iw].chopTreeId = -1;
						//	print("a");
							up_workers[iw].chopTreePhase = 16;
						//	if(up_workers[iw].chopTreePhase == 11){
						//		up_workers[iw].chopTreePhase = 16;
						//	}
						}
						if(up_workers[iw].chopTreeId > iii){
							up_workers[iw].chopTreeId = up_workers[iw].chopTreeId-1;
						}
					}
				}
				
		//		mines.RemoveAt(iii);
				up_mines.RemoveAt(iii);
				minesPos.RemoveAt(iii);
				minesResource.RemoveAt(iii);
				minesResourceType.RemoveAt(iii);
				minesResPoints.RemoveAt(iii);
				
				if(minesPos.Count > 0){
					kd_minesPos = KDTree.MakeFromPoints(minesPos.ToArray());
				}
				
				
			}
		}
		else if(goPars.rtsUnitId == 0){
			if(up_castle.Contains(goPars)){
				int iii = up_castle.IndexOf(goPars);
			//	castle.RemoveAt(iii);
				up_castle.RemoveAt(iii);
				castlePos.RemoveAt(iii);
			}
		}
		
	
	}
	
	
	
	
	
	
    Vector3 RotAround(float rotAngle, Vector3 original, Vector3 direction){
	    
	    Vector3 cross1 = Vector3.Cross(original,direction);
	    
	    Vector3 pr = Vector3.Project(original,direction);
	    Vector3 pr2 = original - pr;
	    
	    
	    Vector3 cross2 = Vector3.Cross(pr2,cross1);
	    
	    Vector3 rotatedVector = (Quaternion.AngleAxis( rotAngle, cross2)*pr2)+pr;
	    
	
		return rotatedVector;
	
	}
	
	
	
}


// public class MineObject {
// 	public RTSMaster rtsm = null;
// 	public GameObject mineGo = null;
// 	public UnitPars minePars = null;
// 	
// 	public Vector3 minesPos = Vector3.zero;
// 	public int minesResourceType = -1;
// 	public int minesResPoints = -1;
// 	
// 	
// 	
// 	
// 	
// }





