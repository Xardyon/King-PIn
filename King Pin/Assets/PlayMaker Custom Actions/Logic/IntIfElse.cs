// IntIfElse.cs v1.0
// __ECO__ __PLAYMAKER__ __ACTION__

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Performs comparision on 2 Integers: ==, >,<,>=,<=,half,twice,opposite, OppositeSign. Allow the result to be saved in a FsmBool, and can send events.")]
	public class IntIfElse : FsmStateAction
	{
		public enum Operation
		{
			Equal,
			Greater,
			GreaterOrEqual,
			Less,
			LessOrEqual,
			OppositeSign,
			Opposite,
			Half,
			Double,
		}
		
		[CompoundArray("Ints", "Int 1", "Int 2")]
		[RequiredField]
		[Tooltip("Number of Ifs Must Match Operations & Send Events")]
		[ActionSection("If")]
		[Title("Integer 1")]
		public FsmInt[] ifInt1;
		[RequiredField]
		[Title("Integer 2")]
		[Tooltip("The second integer to compare")]
		public FsmInt[] ifInt2;
		[CompoundArray("Operations & Send Events", "Operation", "Send Event")]
		[Tooltip("The comparison method")]
		[Title("Operation")]
		public Operation[] ifOperation;
		[Tooltip("Event sent if comparison is true")]
		[Title("Send Event")]
		public FsmEvent[] ifEvent;
		[UIHint(UIHint.Variable)]
		[Tooltip("The comparison result")]
		public FsmBool result;
		
		[Tooltip("Event sent if comparison is false")]
		[ActionSection("Else")]
		[Title("Send Event")]
		public FsmEvent elseEvent;
		
		[Tooltip("Performs comparison everyframe, usefull when value is changing")]
		public bool everyFrame;
		
		public override void Reset()
		{
			ifInt1 = new FsmInt[1];
			ifInt2 = new FsmInt[1];
			ifOperation = new Operation[1];
			result = null;
			ifEvent = new FsmEvent[1];
			elseEvent = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoIntCompare();
			
			if (!everyFrame)
				Finish();
		}
		
		// NOTE: very frame rate dependent!
		public override void OnUpdate()
		{
			DoIntCompare();
			
			
		}
		
		void DoIntCompare()
		{
			if (ifInt1.Length == 0 || ifOperation.Length == 0) return;
			if (ifInt1.Length != ifOperation.Length) return;
			
			for (int i = 0; i < ifInt1.Length; i++)
			{
				float v1 = ifInt1[i].Value;
				float v2 = ifInt2[i].Value;
				
				bool _result = false;
				
				switch (ifOperation[i])
				{
				case Operation.Equal:
					
					_result = v1 == v2;
					break;
					
				case Operation.Greater:
					_result = v1 > v2;
					break;
					
				case Operation.GreaterOrEqual:
					_result = v1 >= v2;
					break;
					
				case Operation.Less:
					_result = v1 < v2;
					break;
					
				case Operation.LessOrEqual:
					_result = v1 <= v2;
					break;
					
				case Operation.Opposite:
					_result = v1 == v2*-1;
					break;
					
				case Operation.OppositeSign:
					_result = v1*v2 < 0;
					break;
				}
				
				result.Value = _result;
				if (_result)
				{
					Fsm.Event(ifEvent[i]);
					return;
				}
			}
			Fsm.Event(elseEvent);
		}
		
	}
}
