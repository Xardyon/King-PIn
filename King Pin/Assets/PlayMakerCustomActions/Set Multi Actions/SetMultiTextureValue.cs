namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Texture")]
	[Tooltip("Sets the value of many Texture Variable.")]
	public class SetMultiTextureValue : FsmStateAction
	{
		[CompoundArray("Count", "Texture Variable", "Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmTexture[] textureVariable;
		public FsmTexture[] values;
		public bool everyFrame;
		
		public override void Reset()
		{
			textureVariable = new FsmTexture[1];
			values = new FsmTexture[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetTextureValue();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetTextureValue();
		}

		public void DoSetTextureValue()
		{
			for(int i = 0; i<textureVariable.Length;i++){
				if(!textureVariable[i].IsNone) 
					textureVariable[i].Value = values[i].Value;
			}
		}
	}
}