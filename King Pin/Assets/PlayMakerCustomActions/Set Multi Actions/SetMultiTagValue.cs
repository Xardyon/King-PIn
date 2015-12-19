using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets the value of many Game Object's Tag.")]
	public class SetMultiTagValue : FsmStateAction
	{
		[CompoundArray("Count", "GameObject", "Tag")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmOwnerDefault[] gameobjectVariable;
		[UIHint(UIHint.Tag)]
		[Title("Tag")]
		public FsmString[] tag;
		public bool everyFrame;

		private GameObject go;
		
		public override void Reset()
		{
			gameobjectVariable = new FsmOwnerDefault[1];
			tag =  new FsmString[1];
			everyFrame = false;
		}
		

		public override void OnEnter()
		{
			DoSetGameObjectTagValue();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetGameObjectTagValue();
		}

		public void DoSetGameObjectTagValue()
		{
			for(int i = 0; i<gameobjectVariable.Length;i++){
				if(tag[i].Value != "Untagged" || tag[i].Value != "" || !tag[i].IsNone) {
	
				go = Fsm.GetOwnerDefaultTarget(gameobjectVariable[i]);
				
				if (go != null)
						go.tag = tag[i].Value;
				}
			}
		}
	}
}