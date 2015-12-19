namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets the value of many Float Variable.")]
	public class SetMultiFloatValue : FsmStateAction
	{
		[CompoundArray("Count", "Float Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat[] floatVariable;
		public FsmFloat[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			floatVariable = new FsmFloat[1];
			values = new FsmFloat[1];
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFloatValue();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetFloatValue();
		}
		
		public void DoSetFloatValue()
		{
			for(int i = 0; i<floatVariable.Length;i++){
				if(!floatVariable[i].IsNone || !floatVariable[i].Value.Equals("")) 
					floatVariable[i].Value = values[i].IsNone ? 0f : values[i].Value;
			}
		}
	}
}