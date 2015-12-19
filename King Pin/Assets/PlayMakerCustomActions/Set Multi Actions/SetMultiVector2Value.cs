namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Sets the value of many Vector2 Variable.")]
	public class SetMultiVector2Value : FsmStateAction
	{
		[CompoundArray("Count", "Vector2 Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector2[] vector2Variable;
		public FsmVector2[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			vector2Variable = new FsmVector2[1];
			values = new FsmVector2[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetVector2Value();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetVector2Value();
		}

		public void DoSetVector2Value()
		{
			for(int i = 0; i<vector2Variable.Length;i++){
				if(!vector2Variable[i].IsNone) 
					vector2Variable[i].Value = values[i].Value;
			}
		}
	}
}