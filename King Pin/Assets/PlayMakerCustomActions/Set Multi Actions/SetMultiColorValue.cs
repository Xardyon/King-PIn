namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Sets the value of many Color Variable.")]
	public class SetMultiColorValue : FsmStateAction
	{
		[CompoundArray("Count", "Color Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor[] colorVariable;
		public FsmColor[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			colorVariable = new FsmColor[1];
			values = new FsmColor[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetColorValue();
			
			if (!everyFrame)
				Finish();
		}

		
		public override void OnUpdate()
		{
			DoSetColorValue();
		}

		public void DoSetColorValue()
		{
			for(int i = 0; i<colorVariable.Length;i++){
				if(!colorVariable[i].IsNone) 
					colorVariable[i].Value = values[i].Value;
			}
		}
	}
}