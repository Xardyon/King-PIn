using UnityEngine;
using System.Collections;

public class ParticlesMeshRenderer : MonoBehaviour {
	
	ParticleSystem ptSystem = null;
	ParticleSystemRenderer pr = null;
	
	public Mesh msh = null;
	public Material mat = null;
	
	private ParticleSystem.Particle[] pool = null;
	int N = 2000;
	private Vector3[] pos = null;
	
	int updId = 0;
	
	// Use this for initialization
	
	void Start () {
		Starter ();
	}
	
	void Starter () {
		ptSystem = this.gameObject.AddComponent<ParticleSystem>();
		
		ptSystem.enableEmission = false;
		
		
		pr = (ParticleSystemRenderer)ptSystem.GetComponent<Renderer>();
	//	pr.mesh = msh;
		
		pr.renderMode = ParticleSystemRenderMode.Mesh;
		pr.material = mat;
		
		pool = new ParticleSystem.Particle[N];
		pos = new Vector3[N];
		
		for(int i=0; i<N; i++){
			pos[i].x = Random.Range(0f,20f);
			pos[i].y = Random.Range(0f,20f);
			pos[i].z = Random.Range(0f,20f);
		}
		
		for(int i=0; i<N; i++){
			pool[i].position = pos[i];
			pool[i].startLifetime = 0.2f;
			pool[i].lifetime = 100000f;
			pool[i].size = 1f;
			pool[i].color = Color.white;
		}
	//	ptSystem.SetParticles(pool, N);
		
	}
	
	// Update is called once per frame
	void Update () {
		updId++;
		if(updId>3){
//			ptSystem.SetParticles(pool, N);
			updId = 0;
			
			
			
		}
		
		for(int i=0; i<N; i++){
			Graphics.DrawMesh(msh, pos[i], Quaternion.identity, mat, 0);
		}
		
		
	}
}
