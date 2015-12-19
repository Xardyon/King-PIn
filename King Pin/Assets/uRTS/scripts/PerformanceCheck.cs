using System.IO;
using UnityEngine;
using System.Collections;

public class PerformanceCheck : MonoBehaviour {
	
	public int N = 10000;
	private float fN;
	
	public float incrementFactor = 1.01f;
	
	private string [] lines ; 
	
	private float tFor;
	private float tIf;
	
	public int maxIterations = 500;
	private int count = 0;


	void Start () {
	    lines = new string [maxIterations+1];
	    
	    fN = (float)N;
	    
	    for(count=0;count<maxIterations;count++){
			EmptyForCheck();
			IfCheck();
			AsciiExporter();
			fN = fN*incrementFactor;
			N = (int)fN;
		}
	}
	
	
	void EmptyForCheck(){
		float t1 = Time.realtimeSinceStartup;
	
		for(int i=1;i<N;i++){
	
		}
		float t2 = Time.realtimeSinceStartup;
		tFor = t2-t1;

	}

	void IfCheck(){
		float t1 = Time.realtimeSinceStartup;
	
		for(int i=1;i<N;i++){
			if(i<0){
		
			}
		}
		float t2 = Time.realtimeSinceStartup;
		tIf = t2-t1;

	}
	
	
	void AsciiExporter(){
	    int c = count+1;
	    if(c == 1){
	    	lines[0] = string.Empty + "#"+"\t"+"i"+"\t"+"N"+"\t"+"tFor"+"\t"+"tIf";
	    }
	    
	    lines[c] = string.Empty + c + "\t" + N + "\t" + tFor + "\t" + tIf;
	    
		if(c == maxIterations){
	#if ! UNITY_WEBPLAYER  
			System.IO.File.WriteAllLines(@Application.dataPath + "/../data.txt", lines);
	#endif
		}
	
	}
	
	
}
