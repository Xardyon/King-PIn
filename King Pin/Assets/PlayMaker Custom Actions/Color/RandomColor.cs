// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

// http://answers.unity3d.com/questions/204958/random-colors-as-distinct-as-possible.html

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Pick a random color. Optionnaly leave the alpha channel alone")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1148")]
	public class RandomColor : FsmStateAction
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The random Color")]
		public FsmColor storeResult;
		
		[Tooltip("Includes a random alpha value ( ranging from 0.1 to 1 to avoid totally invisible colors)")]
		public FsmBool includeAlpha;
		
		public override void Reset()
		{
			includeAlpha = false;
			storeResult = null;
		}

		public override void OnEnter()
		{
			
			Color _col = new Color(Random.value, Random.value, Random.value);
			ColorHSV colorhsv = new ColorHSV(_col);
			colorhsv.h += Random.Range(0f,360f);
			Color result = colorhsv.ToColor();
			
			if (includeAlpha.Value)
			{
				result.a = Random.Range(0.1f,1f);
			}
			
			storeResult.Value = result;
			
			Finish();
		}
		
	}
}