using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsGrouping : MonoBehaviour {
	
	[HideInInspector] public RTSMaster rtsm;
	
//	[HideInInspector] public int nGroups = 0;
//	[HideInInspector] public List<int> groupCount = new List<int>();
	
	[HideInInspector] public List<UnitsGroup> unitsGroups = new List<UnitsGroup>();
	
	[HideInInspector] public List<UnitPars> allUnits = null;
	
	
	
	void Start(){
		allUnits = rtsm.battleSystem.unitssUP;
	}
	
	
	public void GroupSelected(){
		
		UnitsGroup ug = new UnitsGroup();
		unitsGroups.Add(ug);
		for(int i=0;i<allUnits.Count;i++){
			UnitPars goPars = allUnits[i];
			if(goPars.isSelected){
				if(goPars.unitsGroup != null){
					if(goPars.unitsGroup.members.Count > 1){
						goPars.unitsGroup.members.Remove(goPars);
					}
					else{
						CollapseGroup(goPars.unitsGroup);
					}
				}
				
				goPars.unitsGroup = ug;
				ug.members.Add(goPars);
			}
		}
		CheckGroupFormation(ug);
		
	}
	
	public void CheckGroupFormation(UnitsGroup ug){
		if(rtsm.groupingMenu.GetFormationMode()){
		    Formation form = new Formation();
		    form.rtsm = rtsm;
			rtsm.formations.AddUnitsToFormations(ug.members,form,true);
			ug.formationMode = 1;
			
			form.GetDestinations(ug.members, form.CurrentMassCentre(ug.members));
		}
	}
	
	
	public void CollapseGroup(UnitsGroup ug){
		for(int i=0;i<allUnits.Count;i++){
			UnitPars goPars = allUnits[i];
			if(goPars.unitsGroup == ug){
				goPars.unitsGroup.members.Remove(goPars);
				goPars.unitsGroup = null;
				
			}
		}
		int iGroup = unitsGroups.IndexOf(ug);
		if(iGroup+1 > 0){
// 		    Debug.Log(iGroup.ToString()+" b");
			rtsm.groupingMenu.RemoveGroup(iGroup);
		}
		unitsGroups.Remove(ug);
		
	}
	
	public void CleanUpGroups(){
		
		for(int i=0;i<allUnits.Count;i++){
			UnitPars goPars = allUnits[i];
			if(goPars.unitsGroup != null){
				goPars.unitsGroup.members.Remove(goPars);
				goPars.unitsGroup = null;
			}
			
		}
		unitsGroups.Clear();
	}
	
	public void SelectGroup(int iGroup){
		rtsm.cameraOperator.UnsetPropertiesButton();
		
		for(int i=0;i<allUnits.Count;i++){
			UnitPars goPars = allUnits[i];
			if(goPars.isSelected == true){
				rtsm.cameraOperator.DeselectObject(goPars);
			}
		}
		
		
		for(int i=0;i<allUnits.Count;i++){
			UnitPars goPars = allUnits[i];
			if(goPars.unitsGroup != null){
//			    Debug.Log(unitsGroups.IndexOf(goPars.unitsGroup).ToString()+" "+iGroup);
			    if((unitsGroups.IndexOf(goPars.unitsGroup)+1) == iGroup){
					if(goPars.isSelected == false){
						rtsm.cameraOperator.SelectObject(goPars);
						rtsm.cameraOperator.ActivateUnitsMenu();
						rtsm.cameraOperator.SelectedUnitsInfo();
					}
				}
			}
		}
		
	}
	
	public void AssignGroupButton(ButtonObject bo, int iGroup){
		UnitsGroup ug = GetUnitsGroup(iGroup);
		
		if(ug != null){
			ug.groupButton = bo;
		}
	}
	
	public UnitsGroup GetUnitsGroup(int iGroup){
		UnitsGroup ug = null;
		for(int i=0;i<allUnits.Count;i++){
			UnitPars goPars = allUnits[i];
			if(goPars.unitsGroup != null){
				if((unitsGroups.IndexOf(goPars.unitsGroup)+1) == iGroup){
					ug = goPars.unitsGroup;
				}
			}
		}
		return ug;
	}
	
	public void RemoveUnitFromGroup(UnitPars up){
		UnitsGroup ug = up.unitsGroup;
		if(ug != null){
			if(ug.members != null){
				ug.members.Remove(up);
				if(ug.members.Count == 0){
					CollapseGroup(ug);
				}
			}
		}
		up.unitsGroup = null;
	}
	
}

[System.Serializable]
public class UnitsGroup{
	public List<UnitPars> members = new List<UnitPars>();
	public int mode = 0;
	public ButtonObject groupButton;
	public int formationMode = 0;
}
