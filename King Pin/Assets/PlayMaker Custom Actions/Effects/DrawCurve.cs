//DrawCurve.cs v1.0.0
//Curve Drawing code from: https://en.wikibooks.org/wiki/Cg_Programming/Unity/B%C3%A9zier_Curves
//Other elements from: DrawLine.cs by AndrewRaphaelLukasik@live.com http://hutonggames.com/playmakerforum/index.php?topic=3943.0
//made by holyfingers : http://hutonggames.com/playmakerforum/index.php?topic=11193.msg52832#msg52832

// __ECO__ __PLAYMAKER__ __ACTION__

using UnityEngine;
namespace HutongGames.PlayMaker.Actions

{
	[ActionCategory(ActionCategory.Effects)]
	[Tooltip("Draws a Quadratic Bézier Curve between three Game Objects using Unity's Line Renderer.")]
	public class DrawCurve : FsmStateAction
	
	{
		[RequiredField]
		[Tooltip("Curve start.")]
		public FsmGameObject start;

		[RequiredField]
		[Tooltip("Curve Middle.")]
		public FsmGameObject middle;

		[RequiredField]
		[Tooltip("Curve End.")]
		public FsmGameObject end;

		[Tooltip("Curve Material (If set to None will automatically use 'Particles/Additive'.")]
		public FsmMaterial material;

		[Tooltip("Curve Colour at Start.")]
		public FsmColor startColour;
		
		[Tooltip("Curve Colour at End.")]
		public FsmColor endColour;

		[Tooltip("Curve Width at Start.")]
		public FsmFloat startWidth = 0.0f;

		[Tooltip("Curve Width at End.")]
		public FsmFloat endWidth = 1.0f;

		[Tooltip("Number of points with which the Curve is drawn.")]
		public FsmInt numberOfPoints = 10;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private FsmGameObject goLineRenderer;
		private FsmGameObject startObjectPosition;
		private FsmGameObject middleObjectPosition;
		private FsmGameObject endObjectPosition;
			
		public override void Reset()
		{
			start = null;
			middle = null;
			end = null;
			material = null;
			startColour = Color.clear;
			endColour = Color.white;
			numberOfPoints = 10;
			startWidth = 0.0f;
			endWidth = 1.0f;
			goLineRenderer = null;
			everyFrame = false;

		}

		public override void OnEnter()

		{	
			//if (goLineRenderer != null) {Object.Destroy(goLineRenderer.Value);}
			goLineRenderer = new GameObject("FSM draw line");
			goLineRenderer.Value.AddComponent<LineRenderer>();
			goLineRenderer.Value.GetComponent<LineRenderer>().material = material.Value;
			if (material.Value == null) {goLineRenderer.Value.GetComponent<LineRenderer>().material.shader = Shader.Find ("Particles/Additive");}

			// update line renderer
			goLineRenderer.Value.GetComponent<LineRenderer>().SetColors(startColour.Value, endColour.Value);
			goLineRenderer.Value.GetComponent<LineRenderer>().SetWidth(startWidth.Value, endWidth.Value);
			if (numberOfPoints.Value > 0)
			{
				goLineRenderer.Value.GetComponent<LineRenderer>().SetVertexCount(numberOfPoints.Value);
			}
			
			// set points of quadratic Bezier curve
			Vector3 p0 = start.Value.transform.position;
			Vector3 p1 = middle.Value.transform.position;
			Vector3 p2 = end.Value.transform.position;
			float t; 
			Vector3 position;
			for(int i = 0; i < numberOfPoints.Value; i++) 
			{
				t = i / (numberOfPoints.Value - 1.0f);
				position = (1.0f - t) * (1.0f - t) * p0 
					+ 2.0f * (1.0f - t) * t * p1
						+ t * t * p2;
				goLineRenderer.Value.GetComponent<LineRenderer>().SetPosition(i, position);
			}

			if (!everyFrame)
			
			{
				Finish();
			}


	}

		public override void OnUpdate()

		{			
			// update line renderer
			goLineRenderer.Value.GetComponent<LineRenderer>().SetColors(startColour.Value, endColour.Value);
			goLineRenderer.Value.GetComponent<LineRenderer>().SetWidth(startWidth.Value, endWidth.Value);
			if (numberOfPoints.Value > 0)
			{
				goLineRenderer.Value.GetComponent<LineRenderer>().SetVertexCount(numberOfPoints.Value);
			}
			
			// set points of quadratic Bezier curve
			Vector3 p0 = start.Value.transform.position;
			Vector3 p1 = middle.Value.transform.position;
			Vector3 p2 = end.Value.transform.position;
			float t; 
			Vector3 position;
			for(int i = 0; i < numberOfPoints.Value; i++) 
			{
				t = i / (numberOfPoints.Value - 1.0f);
				position = (1.0f - t) * (1.0f - t) * p0 
					+ 2.0f * (1.0f - t) * t * p1
						+ t * t * p2;
				goLineRenderer.Value.GetComponent<LineRenderer>().SetPosition(i, position);
			}

		

		}


}
}
