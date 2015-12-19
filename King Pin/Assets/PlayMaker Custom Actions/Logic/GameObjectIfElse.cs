// GameObjectIfElse.cs v1.0
// Modified By __Darkhitori__
// __ECO__ __PLAYMAKER__ __ACTION__


using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compares 2 Game Objects and sends Events based on the result.")]
	public class GameObjectIfElse : FsmStateAction
	{
		[ActionSection("If")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("A Game Object variable to compare.")]
		public FsmGameObject gameObjectVariable;
		[CompoundArray("GameObjects", "Compare to", "Equal Event")]
		[RequiredField]
		[Tooltip("Compare the variable with this Game Object")]
		public FsmGameObject[] compareTo;
		[Tooltip("Send this event if Game Objects are equal")]
		public FsmEvent[] equalEvent;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result of the check in a Bool Variable. (True if equal, false if not equal).")]
		public FsmBool storeResult;
		[ActionSection("Else")]
		[Tooltip("Send this event if Game Objects are not equal")]
		public FsmEvent notEqualEvent;
		
		[Tooltip("Repeat every frame. Useful if you're waiting for a true or false result.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObjectVariable = null;
			compareTo = new FsmGameObject[1];
			equalEvent = new FsmEvent[1];
			notEqualEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGameObjectCompare();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoGameObjectCompare();
		}
		
		void DoGameObjectCompare()
		{
			if (gameObjectVariable == null || compareTo == null) return;
			
			for (int i = 0; i < compareTo.Length; i++) 
			{
				bool equal = gameObjectVariable.Value == compareTo[i].Value;
				
				if (gameObjectVariable.Value == compareTo[i].Value)
				{
					Fsm.Event(equalEvent[i]);
					storeResult.Value = equal;
					return;
				}
			}
			if (notEqualEvent != null)
			{
				Fsm.Event(notEqualEvent);
				storeResult.Value = false;
			}
				
			
		}
		
	}
}
