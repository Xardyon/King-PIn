//SetGameObjectasViewportLabel.cs v1.0.0
//Face Camera code from: http://wiki.unity3d.com/index.php?title=CameraFacingBillboard#Alternative_Mod
//made by holyfingers: http://hutonggames.com/playmakerforum/index.php?topic=11358.0

// __ECO__ __PLAYMAKER__ __ACTION__

using UnityEngine;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets the Position of a Game Object in Viewport space relative to another Game Object in World space.")]
	public class SetGameObjectAsViewportLabel : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to follow.")]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[Tooltip("The GameObject to transform.")]
		public FsmGameObject labelObject;
		[Tooltip("The GameObject to act as a Label.")]
		public Camera referenceCamera;
		[Tooltip("Offset the Label position in Viewport X.")]
		public FsmFloat viewportOffsetX;
		[Tooltip("Offset the Label position in Viewport Y.")]
		public FsmFloat viewportOffsetY;
		[Tooltip("Offset the Label position in Viewport Z.")]
		public FsmFloat viewportOffsetZ;
		[Tooltip("Aligns the Label object with the Viewport.")]
		public bool faceCamera = false;
		[Tooltip("Flips the Label.")]
		public bool flipLabel = false;
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;


		public override void Reset()
		{
			labelObject = null;
			referenceCamera = null;
			// default axis to variable dropdown with None selected.
			viewportOffsetX = new FsmFloat { UseVariable = true };
			viewportOffsetY = new FsmFloat { UseVariable = true };
			viewportOffsetZ = 1;
			faceCamera = false;
			flipLabel = false;
			everyFrame = false;
			lateUpdate = false;
		}

		public override void OnEnter()
		{
			if (!everyFrame && !lateUpdate)
			{
				DoSetPositionInViewport();
				Finish();
			}		
		}
		
		public override void OnUpdate()
		{
			if (!lateUpdate)
			{ 
				DoSetPositionInViewport();
			}
		}
		
		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoSetPositionInViewport();
			}
			
			if (!everyFrame)
			{ 
				Finish();
			}
		}
		
		void DoSetPositionInViewport()
		
		{
			Vector3 worldPosition;
			Vector3 viewPosition;
			Vector3 newWorldPosition;

			if (referenceCamera == null) 
			
			{
				referenceCamera = Camera.main;
			}

			//Get World position from target gameObject

			var go = Fsm.GetOwnerDefaultTarget (gameObject);

			if (go == null) {

				return;

			}

			worldPosition = go.transform.position;

			//Get View Position

			viewPosition = referenceCamera.WorldToViewportPoint(worldPosition);

			//Set New World Position
			
			if (!viewportOffsetX.IsNone) viewPosition.x = viewPosition.x + viewportOffsetX.Value;
			if (!viewportOffsetY.IsNone) viewPosition.y = viewPosition.y + viewportOffsetY.Value;
			if (!viewportOffsetZ.IsNone) viewPosition.z = viewportOffsetZ.Value;
			
			newWorldPosition = referenceCamera.ViewportToWorldPoint(viewPosition);

			labelObject.Value.transform.position = newWorldPosition;

			if (faceCamera ==true) {

				// rotates the object relative to the camera

				Vector3 targetPos = labelObject.Value.transform.position + referenceCamera.transform.rotation * (flipLabel ? Vector3.back : Vector3.forward);
				Vector3 targetOrientation = referenceCamera.transform.rotation * Vector3.up;
				labelObject.Value.transform.LookAt (targetPos, targetOrientation);

				}

			}

	}
}
