using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsMover : MonoBehaviour {
	
	public RTSMaster rtsm;
	
	public List<UnitPars> staticMovers = new List<UnitPars>();
	public List<UnitPars> followers = new List<UnitPars>();
	public List<UnitPars> militaryAvoiders = new List<UnitPars>();
	
	void Start () {
		StartCoroutine(MoveUnits());
	}
	
	public void AddMover(UnitPars up, Vector3 pos, int completionMark){
		AddMover(up,pos,"Walk","Idle", up.thisNMA.radius/(up.transform.localScale.x), completionMark);
	}
	
	public void AddMilitaryAvoider(UnitPars up, Vector3 pos, int completionMark){
		AddMilitaryAvoider(up,pos,"Walk","Idle", up.thisNMA.radius/(up.transform.localScale.x), completionMark);
	}
	
	
	
	
	public void AddMover(UnitPars up, Vector3 pos, string animation, string animationOnComplete, float stopDistance, int completionMark){
		CompleteMovent(up);
	//	up.MoveUnit(pos, animation);
		up.MoveUnit(pos);
		up.um_staticPosition = pos;
		up.um_animationOnMove = animation;
		up.um_animationOnComplete = animationOnComplete;
		up.um_stopDistance = stopDistance;
		up.um_completionMark = completionMark;
		staticMovers.Add(up);
	}
	
	public void AddFollower(UnitPars up, Transform follower, string animation, string animationOnComplete, float stopDistance, int completionMark){
		CompleteMovent(up);
	//	up.MoveUnit(follower.position, animation);
		up.MoveUnit(follower.position);
		up.um_animationOnMove = animation;
		up.um_animationOnComplete = animationOnComplete;
		up.um_stopDistance = stopDistance;
		up.um_completionMark = completionMark;
		followers.Add(up);
	}
	
	public void AddMilitaryAvoider(UnitPars up, Vector3 pos, string animation, string animationOnComplete, float stopDistance, int completionMark){
		CompleteMovent(up);
	//	up.MoveUnit(pos, animation);
		up.MoveUnit(pos);
		up.um_staticPosition = pos;
		up.um_animationOnMove = animation;
		up.um_animationOnComplete = animationOnComplete;
		up.um_stopDistance = stopDistance;
		up.um_completionMark = completionMark;
		militaryAvoiders.Add(up);
	}
	
	
	public void CompleteMovent(UnitPars up){
		if(staticMovers.Contains(up)){
			FinishMover(up);
		}
		if(followers.Contains(up)){
			FinishFollower(up);
		}
		if(militaryAvoiders.Contains(up)){
			FinishMilitaryAvoider(up);
		}
		
	}
	
	public void FinishMover(UnitPars up){
		staticMovers.Remove(up);
		up.StopUnit(up.um_animationOnComplete);
		up.um_complete = up.um_completionMark;
		up.um_completionMark = 0;
		up.um_staticPosition = Vector3.zero;
		up.um_stopDistance = 0f;
		up.um_animationOnComplete = "";
	}

	public void FinishFollower(UnitPars up){
		followers.Remove(up);
		up.StopUnit(up.um_animationOnComplete);
		up.um_complete = up.um_completionMark;
		up.um_completionMark = 0;
		up.um_staticPosition = Vector3.zero;
		up.um_stopDistance = 0f;
		up.um_animationOnComplete = "";
	}

	public void FinishMilitaryAvoider(UnitPars up){
		militaryAvoiders.Remove(up);
		up.StopUnit(up.um_animationOnComplete);
		up.um_complete = up.um_completionMark;
		up.um_completionMark = 0;
		up.um_staticPosition = Vector3.zero;
		up.um_stopDistance = 0f;
		up.um_animationOnComplete = "";
	}

	
	
	public IEnumerator MoveUnits(){
		int i1 = 0;
		while(true){
			for(int i=0; i<staticMovers.Count; i++){
				UnitPars up = staticMovers[i];
				CheckMovementAnimations(up,up.um_animationOnMove,up.um_animationOnComplete);
				float rStop = up.um_stopDistance;
				if((up.transform.position - up.um_staticPosition).sqrMagnitude < rStop*rStop){
					FinishMover(up);
				}
				i1=i1+1;
				if(i1 > 300){
				    i1=0;
					yield return new WaitForSeconds(0.1f);
				}
			}
			for(int i=0; i<followers.Count; i++){
				UnitPars up = staticMovers[i];
				CheckMovementAnimations(up,up.um_animationOnMove,up.um_animationOnComplete);
				float rStop = up.um_stopDistance;
				if((up.transform.position - up.um_Follower.position).sqrMagnitude < rStop*rStop){
					FinishFollower(up);
				}
				else{
					up.MoveUnit(up.um_Follower.position);
				}
				i1=i1+2;
				if(i1 > 300){
				    i1=0;
					yield return new WaitForSeconds(0.1f);
				}
			}
			
			for(int i=0; i<militaryAvoiders.Count; i++){
				UnitPars up = militaryAvoiders[i];
				CheckMovementAnimations(up,up.um_animationOnMove,up.um_animationOnComplete);
				float rStop = up.um_stopDistance;
				if(up.militaryMode == 10){
					if((up.transform.position - up.um_staticPosition).sqrMagnitude < rStop*rStop){
						FinishMilitaryAvoider(up);
					}
					else{
						up.MoveUnit(up.um_staticPosition);
					}
				}
				i1=i1+2;
				if(i1 > 300){
				    i1=0;
					yield return new WaitForSeconds(0.1f);
				}
			}
			
			
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	
	public void CheckMovementAnimations(UnitPars up, string movingAnim, string idleAnim){
		if(up.velocityVector.magnitude > 0.5f*up.thisNMA.speed){
			if(up.rtsUnitId != 15){
				if(up.thisSL.animName == idleAnim){
					up.PlayAnimation(movingAnim);
				}
			}
		}
		if(up.velocityVector.magnitude < 0.5f*up.thisNMA.speed){
			if(up.rtsUnitId != 15){
				if(up.thisSL.animName == movingAnim){
					up.PlayAnimation(idleAnim);
				}
			}
		}
	}
	
	
}
