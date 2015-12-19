namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Sets the value of many String Variable.")]
	public class SetMultiStringValue : FsmStateAction
	{
		[CompoundArray("Count", "String Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString[] stringVariable;
		public FsmString[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			stringVariable = new FsmString[1];
			values = new FsmString[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetStringValue();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetStringValue();
		}

		public void DoSetStringValue()
		{
			for(int i = 0; i<stringVariable.Length;i++){
				if(!stringVariable[i].IsNone || !stringVariable[i].Value.Equals("")) 
					stringVariable[i].Value = values[i].IsNone ? "": values[i].Value;
			}
		}
	}
}