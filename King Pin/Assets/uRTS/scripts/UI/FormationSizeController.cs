using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormationSizeController : MonoBehaviour {
	public RTSMaster rtsm;
	public Slider slider;
	public Text tx_slider;
	
	public SpawnPoint spawnPoint;
	
	void Start () {
	
	}
	
// 	public void ChangeValue(){
// //		Debug.Log(slider.value);
// 		tx_slider.text = "Formation ("+slider.value.ToString()+")";
// 	}
	
	public void GetSelectedSpawnPoint(){
		SelectionMark selM = rtsm.selectionMark;
		if(selM.selectedGoPars.Count > 0){
			if(selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>() != null){
				spawnPoint = selM.selectedGoPars[0].gameObject.GetComponent<SpawnPoint>();
			}
		}
	}
	
	public void LoadSpawnPointToSlider(){
		slider.value = spawnPoint.formationSize;
		tx_slider.text = "Formation ("+slider.value.ToString()+")";
	}
	
	public void LoadSliderToSpawnPoint(){
		spawnPoint.formationSize = (int)(slider.value);
		tx_slider.text = "Formation ("+slider.value.ToString()+")";
	}
}
