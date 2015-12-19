using UnityEngine;
using UnityEditor;

public class UETWiz : ScriptableWizard {
	
	[MenuItem("Window/UET/UET Add Batton")]
	static void CreateWizard () {
        ScriptableWizard.DisplayWizard<UETWiz>("Add button", "Apply", "Delete");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }
	
	public string path = "GameObject/Create Empty";
	public string caption = "Create GO";
	
	void OnWizardUpdate () {
        helpString = "Please enter the path and name!";
    }
	
	void OnWizardCreate(){
		UET.paths.Add(path);
		UET.names.Add(caption);
		UET._count_btn++;
		UET.Save();
	}
	
	void OnWizardOtherButton(){
		UET.paths.RemoveAt(UET.names.IndexOf(caption));
		UET.names.Remove(caption);
		UET._count_btn--;
		UET.Save();
	}
}
