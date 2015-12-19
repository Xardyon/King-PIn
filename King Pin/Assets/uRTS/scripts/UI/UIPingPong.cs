using UnityEngine;
using System.Collections;

public class UIPingPong : MonoBehaviour {
    
    
    [HideInInspector] public int helpInteger1 = 0;
    [HideInInspector] public int helpInteger2 = 0;
    [HideInInspector] public int helpInteger3 = 0;
    
    [HideInInspector] public string helpString1 = null;
    [HideInInspector] public string helpString2 = null;
    [HideInInspector] public string helpString3 = null;
    
    [HideInInspector] public int selGroupButtonId = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
//	void Update () {
	
//	}
	
	public void PingPong(){
		if(this.gameObject.activeSelf == true){
			this.gameObject.SetActive(false);
		}
		else if(this.gameObject.activeSelf == false){
			this.gameObject.SetActive(true);
		}
	}
	
	public void Activate(){
		this.gameObject.SetActive(true);
	}
	
	public void DeActivate(){
		this.gameObject.SetActive(false);
	}
	
	
	
	public void SetInteger1(int i){
		helpInteger1 = i;
	}
	public void SetString1(string str){
		helpString1 = str;
	}
	
	
	
// 	public void QuitGame(){
// 	//	Application.Quit();
// 	//	Application.LoadLevel (0);
// 	}
	
// 	public void EnterFullScreen(){
// 		Screen.fullScreen = !Screen.fullScreen;
// 	}
	
}
