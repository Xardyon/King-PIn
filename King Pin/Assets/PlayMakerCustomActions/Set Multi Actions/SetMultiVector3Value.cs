namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Sets the value of many Vector3 Variable.")]
	public class SetMultiVector3Value : FsmStateAction
	{
		[CompoundArray("Count", "Vector3 Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3[] vector3Variable;
		public FsmVector3[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			vector3Variable = new FsmVector3[1];
			values = new FsmVector3[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetVector3Value();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetVector3Value();
		}

		public void DoSetVector3Value()
		{
			for(int i = 0; i<vector3Variable.Length;i++){
				if(!vector3Variable[i].IsNone) 
					vector3Variable[i].Value = values[i].Value;
			}
		}
	}
}