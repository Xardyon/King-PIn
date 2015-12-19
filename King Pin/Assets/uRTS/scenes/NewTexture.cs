using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewTexture : MonoBehaviour {
	public Camera cam = null;
	public RenderTexture tr = null;
	// Use this for initialization
	public RawImage ri = null;
	void Start () {
		tr = cam.targetTexture;
	}
	
	// Update is called once per frame
	void Update () {
		RenderTexture.active = tr;
		Texture2D tex = new Texture2D(256, 256);
		tex.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
		ri.texture = tr;
	}
	
	void OnGUI(){
//		RenderTexture.active = tr;
//		Texture2D tex = new Texture2D(256, 256);
//		tex.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
//		GUI.Label (new Rect(0, 0, 256, 256), tex);
	}
	
	
}
