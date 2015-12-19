// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
// http://hutonggames.com/playmakerforum/index.php?topic=8452
//--- __ECO__ __ACTION__ ---//
using UnityEngine;
using UnityEngine.EventSystems;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Sets the EventSystem's currently select GameObject.")]
	public class uGuiSetSelectedGameObject : FsmStateAction
	{


		//[RequiredField]
		[UIHint(UIHint.FsmGameObject)]

		public FsmGameObject UguiObject;


		public override void Reset()
		{
			UguiObject = null;
		}

		public override void OnEnter()
		{

		    DoSetSelectedGameObject();
			
			Finish();	
		}
		
		void DoSetSelectedGameObject()
		{
			EventSystem.current.SetSelectedGameObject (UguiObject.Value);
		}

	}
}
