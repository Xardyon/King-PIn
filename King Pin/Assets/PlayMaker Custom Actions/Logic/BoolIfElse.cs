// BoolIfElse.cs v1.0
// Modified By __Darkhitori__
// __ECO__ __PLAYMAKER__ __ACTION__

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends Events based on the value of a Boolean Variable.")]
	public class BoolIfElse : FsmStateAction
	{
		[ActionSection("If")]
		[RequiredField]
		[CompoundArray("Bools", "Bool Variable", "Is True")]
		[UIHint(UIHint.Variable)]
        [Tooltip("The Bool variable to test.")]
		public FsmBool[] boolVariable;
		[Tooltip("Event to send if the Bool variable is True.")]
		public FsmEvent[] isTrue;
		
		[ActionSection("Else")]
		[Tooltip("Event to send if the Bool variable is False.")]
		public FsmEvent isFalse;

        [Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = new FsmBool[1];
			isTrue = new FsmEvent[1];
			isFalse = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoBoolTests();
			
			if (!everyFrame)
				{
				    Finish();
				}
		}
		
		public override void OnUpdate()
		{
			DoBoolTests();
		}
		void DoBoolTests()
		{
			if (boolVariable.Length == 0) return;
			
			for (int i = 0; i < boolVariable.Length; i++)
			{
				if (boolVariable[i].Value == true)
				{
					Fsm.Event(isTrue[i]);
					return;
				}
				
			}
			Fsm.Event(isFalse);
		}
	}
}
