namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets the value of many Bool Variable.")]
	public class SetMultiBoolValue : FsmStateAction
	{
		[CompoundArray("Count", "Bool Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool[] boolVariable;
		public FsmBool[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			boolVariable = new FsmBool[1];
			values = new FsmBool[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetBoolValue();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetBoolValue();
		}

		public void DoSetBoolValue()
		{
			for(int i = 0; i<boolVariable.Length;i++){
				if(!boolVariable[i].IsNone || !boolVariable[i].Value.Equals(false)) 
					boolVariable[i].Value = values[i].Value;
			}
	
		}
	}
}