using UnityEngine;
using System.Collections;

public class CreateForestEdit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void Generate(){
	    if(GetComponent<CreateForest>()!=null){
			CreateForest cf = GetComponent<CreateForest>();
			cf.CalculateAllTreePositions();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
