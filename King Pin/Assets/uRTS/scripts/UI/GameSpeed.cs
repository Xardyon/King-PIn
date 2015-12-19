using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSpeed : MonoBehaviour {
    private Slider slider;
    public bool usePowerLaw = false;
	void Awake () {
		slider = this.gameObject.GetComponent<Slider>();
	}
	
	
	public void ChangeGameSpeed(){
		
		if(usePowerLaw == true){
			Time.timeScale = Mathf.Pow(10f,slider.value);
		}
		else{
			Time.timeScale = slider.value;
		}
		if(slider.value == slider.minValue){
			Time.timeScale = 0f;
		}
		
	}
	
}
