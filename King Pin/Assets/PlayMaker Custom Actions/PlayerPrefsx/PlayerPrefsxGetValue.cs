// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
using UnityEngine;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("PlayerPrefs X")]
	[Tooltip("Gets the value of the preference identified by key.")]
	public class PlayerPrefsxGetValue : FsmStateAction
	{
		[CompoundArray("Count", "Key", "Value")]
		[Tooltip("Case sensitive key.")]
		public FsmString[] keys;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVar[] values;

		public override void Reset()
		{
			keys = new FsmString[1];
			values = new FsmVar[1];
		}
		
		public override string ErrorCheck ()
		{
			for(int i = 0; i<keys.Length;i++){
					string key = keys[i].Value;
					FsmVar fsmVar = values[i];
						
					switch (fsmVar.Type) {
						case VariableType.Int:
						case VariableType.Float:
						case VariableType.Bool:
						case VariableType.Color:
						case VariableType.Quaternion:
						case VariableType.Rect:
						case VariableType.Vector2:
						case VariableType.Vector3:
						case VariableType.String:
							break;
						default:
						return "PlayerPrefsx does not support "+ fsmVar.Type;
					}
			}
			 return "";
		}
		
		public override void OnEnter()
		{
			for(int i = 0; i<keys.Length;i++){
				if(!keys[i].IsNone || !keys[i].Value.Equals("")) 
				{
					string key = keys[i].Value;
					FsmVar fsmVar = values[i];
					string _name = fsmVar.variableName;
					
					switch (fsmVar.Type) {
						case VariableType.Int:
							this.Fsm.Variables.GetFsmInt(_name).Value =  PlayerPrefs.GetInt(key);
							break;
						case VariableType.Float:
							this.Fsm.Variables.GetFsmFloat(_name).Value =  PlayerPrefs.GetFloat(key);
							break;
						case VariableType.Bool:
							this.Fsm.Variables.GetFsmBool(_name).Value =  PlayerPrefsX.GetBool(key);
							break;
						case VariableType.Color:
							this.Fsm.Variables.GetFsmColor(_name).Value =  PlayerPrefsX.GetColor(key);
							break;
						case VariableType.Quaternion:
							this.Fsm.Variables.GetFsmQuaternion(_name).Value =  PlayerPrefsX.GetQuaternion(key);
							break;
						case VariableType.Rect:
							this.Fsm.Variables.GetFsmRect(_name).Value =  PlayerPrefsX.GetRect(key);
							break;
						case VariableType.Vector2:
							this.Fsm.Variables.GetFsmVector2(_name).Value =  PlayerPrefsX.GetVector2(key);
							break;
						case VariableType.Vector3:
							this.Fsm.Variables.GetFsmVector3(_name).Value =  PlayerPrefsX.GetVector3(key);
							break;
						case VariableType.String:
							this.Fsm.Variables.GetFsmString(_name).Value =  PlayerPrefs.GetString(key);
							break;
						default:
							LogError("PlayerPrefsx does not support saving "+ fsmVar.Type);
							break;
					}
					
					
					
					
				}
			}
			
			Finish();
		}
		

	}
}