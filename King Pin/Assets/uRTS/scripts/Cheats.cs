using UnityEngine;
using System.Collections;

public class Cheats : MonoBehaviour {
	
	public RTSMaster rtsm;
	
	public bool useCheats = false;
	public int godMode = 0;
	
	void Start () {
	
	}
	
	void Update () {
		if(useCheats == true){
			if(Input.GetKey("g")){
				if(godMode == 0){
					godMode = 1;
				}
				else{
					godMode = 0;
				}
			}
		}
	}
}
