#if UNITY_EDITOR
#if ! UNITY_WEBPLAYER    
using UnityEditor;
#endif
#endif 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SkMeshRend : MonoBehaviour {
	
	public GameObject model = null;
	public GameObject panel = null;
	
	CreateStaticAnimationState sst = null;
	bool isReady = false;
	float scriptStartTime = 0;
	public List<GameObject> copies = new List<GameObject>();
	
	int md = 1;
	
	
	void Start(){
		StartCoroutine(LoadAnim());
	}
	
	IEnumerator LoadAnim(){
		sst = new CreateStaticAnimationState();
		sst.model = model;
		sst.panel = panel;
		yield return StartCoroutine(sst.Starter());
		if(md == 0){
			isReady = true;
		}
		else{
			CopiesMode();
		}
	}
	
	void LateUpdate () {
		if(isReady == true){
			if(Time.realtimeSinceStartup-scriptStartTime > sst.totalTime){
				scriptStartTime = Time.realtimeSinceStartup;
			}
			float startTime = Time.realtimeSinceStartup-scriptStartTime;
			
			int mode = (int)(startTime*sst.numberOfFrames / sst.totalTime)+1;
		
		
			sst.ShowMesh(mode);
		}
		
	}
	
	public void CopiesMode(){
		for(int i=0; i<sst.N; i++){
			copies.Add((GameObject)Instantiate(model, sst.pos[i], model.transform.rotation));
			copies[i].GetComponent<Animation>()["Walk"].enabled = true;
			model.GetComponent<Animation>()["Walk"].weight = 1f;
			model.GetComponent<Animation>()["Walk"].speed = 1;
			copies[i].GetComponent<Animation>().Play();
		}
	}


}



public class CreateStaticAnimationState {
	
	public float totalTime = 0f;
//	float scriptStartTime = 0;
	public int numberOfFrames = 24;
	
	
	public GameObject model = null;
	
	
	public Animation anim = null;
	
	public List<List<Mesh>> msh = new List<List<Mesh>>();
	
	public List<List<Material>> mats = new List<List<Material>>();
	
	public Vector3[] pos = null;
	public int N = 300;
	
	public GameObject panel = null;
	int numbSubmeshes = 0;
	
//	bool isReady = false;
	
	void Start(){
//		StartCoroutine(Starter());
	}

	public IEnumerator Starter () {
		
		foreach (Transform child in model.transform){
				if(child.GetComponent<SkinnedMeshRenderer>()!= null){
					SkinnedMeshRenderer sss = child.GetComponent<SkinnedMeshRenderer>();
					sss.updateWhenOffscreen = true;
					mats.Add((sss.materials).ToList());
					msh.Add(new List<Mesh>());
					numbSubmeshes++;
				}
		}
//		model.transform.position = new Vector3(0f, 0f, 0f);

	//	msh = new Mesh[numberOfFrames+1];
		
		AnimationState state = model.GetComponent<Animation>()["Walk"];
		totalTime = state.length;
		for(int i=0; i<=numberOfFrames; i++){
			model.GetComponent<Animation>()["Walk"].enabled = true;
			model.GetComponent<Animation>()["Walk"].weight = 1f;
			model.GetComponent<Animation>()["Walk"].speed = 0;
			model.GetComponent<Animation>()["Walk"].time = totalTime*i / numberOfFrames;
			
			int iii = 0;
			foreach (Transform child in model.transform){
				if(child.GetComponent<SkinnedMeshRenderer>()!= null){
					SkinnedMeshRenderer sss = child.GetComponent<SkinnedMeshRenderer>();
				//	if(iii==1){
						msh[iii].Add(new Mesh());
						sss.BakeMesh(msh[iii][i]);
				//		msh[iii][i].Optimize();
				//	}
					
//					string nm = child.name;
// 					AssetDatabase.CreateAsset(mshb, "Assets/uRTS/Resources/temp/"+i.ToString()+nm+"_M" + ".asset");
// 					AssetDatabase.Refresh();
					iii++;
					yield return new WaitForSeconds(0.00003f);
				}
			}
			
			
			
		}
		
		pos = new Vector3[N];
		for(int i=0; i<N; i++){
			pos[i].x = Random.Range(0f,20f);
			pos[i].y = Random.Range(0f,20f);
			pos[i].z = Random.Range(0f,20f);
		}
	//	Destroy(model);
//		scriptStartTime = Time.realtimeSinceStartup;
		panel.SetActive(false);
//		isReady = true;
		
		
		
	}
	
	
	public void ShowMesh(int mode){
		for(int i=0; i<N; i++){
			for(int j=0; j<numbSubmeshes; j++){
				for(int k=0; k<mats[j].Count; k++){
					Graphics.DrawMesh(msh[j][mode], pos[i], Quaternion.identity, mats[j][k], 0, Camera.main, k);
				}
				//Graphics.DrawMesh(msh[j][mode], pos[i], Quaternion.identity, mats[j][1], 0, Camera.main, 1);
			}
		}
	}
	
	
	
	
	
	
	
}
