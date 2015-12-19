// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
using UnityEngine;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("PlayerPrefs X")]
	[Tooltip("Sets the value of the preference identified by key.")]
	public class PlayerPrefxSetValue : FsmStateAction
	{
		[CompoundArray("Count", "Key", "Value")]
		[Tooltip("Case sensitive key.")]
		public FsmString[] keys;
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
						
					switch (fsmVar.Type) {
						case VariableType.Int:
							PlayerPrefs.SetInt(key, fsmVar.IsNone ? 0 : (int)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.Float:
							PlayerPrefs.SetFloat(key, fsmVar.IsNone ? 0f : (float)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.Bool:
							PlayerPrefsX.SetBool(key, fsmVar.IsNone ? false : (bool)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.Color:
							PlayerPrefsX.SetColor(key, fsmVar.IsNone ? Color.black : (Color)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.Quaternion:
							PlayerPrefsX.SetQuaternion(key, fsmVar.IsNone ? Quaternion.identity : (Quaternion)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.Rect:
							PlayerPrefsX.SetRect(key, fsmVar.IsNone ? new Rect(0f,0f,0f,0f) : (Rect)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.Vector2:
							PlayerPrefsX.SetVector2(key, fsmVar.IsNone ? Vector2.zero : (Vector2)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.Vector3:
							PlayerPrefsX.SetVector3(key, fsmVar.IsNone ? Vector3.zero : (Vector3)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
							break;
						case VariableType.String:
							PlayerPrefs.SetString(key, fsmVar.IsNone ? "" : (string)PlayMakerUtils.GetValueFromFsmVar(this.Fsm,fsmVar));
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