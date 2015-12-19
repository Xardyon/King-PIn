// (c) Copyright Darkhitori, LLC 2015. All rights reserved.
// GetRightTrigger v1.0
//http://hutonggames.com/playmakerforum/index.php?topic=11288.0
// __ECO__ __PLAYMAKER__ __ACTION__

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the value of the specified Input Axis and stores it in a Float Variable. See Unity Input Manager docs.")]
	public class GetRightTrigger : FsmStateAction
	{
		
		
		[RequiredField]
		[Tooltip("The name of the axis. Must be set to 10th Axis in the Unity Input Manager.")]
		public FsmString rightTrigger;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Title("Store Right Trigger")]
		[Tooltip("Store the result in a float variable.")]
		public FsmFloat rtStore;
		
		[Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range.")]
		public FsmFloat rtMultiplier;
		
		[Tooltip("Event to send if the button is pressed.")]
		public FsmEvent rtSendEvent;
		
		[Tooltip("Set to True if the button is pressed.")]
		[UIHint(UIHint.Variable)]
		[Title("Store Result")]
		public FsmBool rtStoreResult;
		
		[Tooltip("Repeat every frame. Typically this would be set to True.")]
		public bool everyFrame;
		
		
		
		public override void Reset()
		{
			rightTrigger = "";
			rtMultiplier = 1.0f;
			rtStore = null;
			rtSendEvent = null;
			rtStoreResult = null;
			everyFrame = true;
		}
		
		public override void OnEnter()
		{
			DoGetTriggers();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetTriggers();
		}
		
		void DoGetTriggers()
		{
			var axisValue2 = Input.GetAxis(rightTrigger.Value);
			
			// if variable set to none, assume multiplier of 1
			if (!rtMultiplier.IsNone)
			{
				axisValue2 *= rtMultiplier.Value;
				
			}
			rtStore.Value = axisValue2;
			
			if(Input.GetAxis(rightTrigger.Value) == 1)
			{
				Fsm.Event(rtSendEvent);
				rtStoreResult.Value = true;
			}
			else
				rtStoreResult.Value = false;
			
		}
	}
}

