namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets the value of many GameObject Variable.")]
	public class SetMultiGameObjectValue : FsmStateAction
	{
		[CompoundArray("Count", "GameObject Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject[] gameobjectVariable;
		public FsmGameObject[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameobjectVariable = new FsmGameObject[1];
			values = new FsmGameObject[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetGameObjectValue();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetGameObjectValue();
		}

		public void DoSetGameObjectValue()
		{
			for(int i = 0; i<gameobjectVariable.Length;i++){
				if(!gameobjectVariable[i].IsNone || !gameobjectVariable[i].Value.Equals("")) 
					gameobjectVariable[i].Value = values[i].Value;
			}
		}
	}
}