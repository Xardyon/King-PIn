// FloatIfElse.cs v1.0
//http://hutonggames.com/playmakerforum/index.php?topic=11243.0
// __ECO__ __PLAYMAKER__ __ACTION__


using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Performs comparision on 2 Floats: ==, >,<,>=,<=,half,twice,opposite, OppositeSign. Allow the result to be saved in a FsmBool, and can send events.")]
	public class FloatIfElse : FsmStateAction
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
		
		[CompoundArray("Floats", "Float 1", "FLoat 2")]
		[RequiredField]
		[Tooltip("Number of Ifs Must Match Operations & Send Events")]
		[ActionSection("If")]
		[Title("Float 1")]
		public FsmFloat[] ifFloat1;
		[RequiredField]
		[Title("Float 2")]
		public FsmFloat[] ifFloat2;
		[CompoundArray("Operations & Send Events", "Operation", "Send Event")]
		[Title("Operation")]
		public Operation[] ifOperation;
		[Title("Send Event")]
		public FsmEvent[] ifEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool result;
		
		[Tooltip("Event sent if comparison is false")]
		[ActionSection("Else")]
		[Title("Send Event")]
		public FsmEvent elseEvent;
		
		[Tooltip("Performs comparison everyframe, usefull when value is changing")]
		public bool everyFrame;
		
		public override void Reset()
		{
			ifFloat1 = new FsmFloat[1];
			ifFloat2 = new FsmFloat[1];
			ifOperation = new Operation[1];
			result = null;
			ifEvent = new FsmEvent[1];
			elseEvent = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoFloatCompare();
			
			if (!everyFrame)
				Finish();
		}
		
		// NOTE: very frame rate dependent!
		public override void OnUpdate()
		{
			DoFloatCompare();
				
		}
		
		void DoFloatCompare()
		{
			if (ifFloat1.Length == 0 || ifOperation.Length == 0) return;
			if (ifFloat1.Length != ifOperation.Length) return;
			
			for (int i = 0; i < ifFloat1.Length; i++)
			{
				float v1 = ifFloat1[i].Value;
				float v2 = ifFloat2[i].Value;
				
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
