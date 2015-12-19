/*
* Energy Bar Toolkit by Mad Pixel Machine
* http://www.madpixelmachine.com
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{

[ActionCategory("Energy Bar Toolkit")]
[Tooltip("Reads current Energy Bar min/max value")]
public class GetBarMinMaxValue : FsmStateAction {

    // ===========================================================
    // Constants
    // ===========================================================

    // ===========================================================
    // Fields
    // ===========================================================
    
    [RequiredField]
    [Tooltip("Energy bar on which action should be done")]
    [CheckForComponent(typeof(EnergyBar))]
    public FsmGameObject barObject;

    [Tooltip("Variable to store minimum bar value")]
    [UIHint(UIHint.Variable)]
    public FsmInt storeMinValue;

    [Tooltip("Variable to store maximum bar value")]
    [UIHint(UIHint.Variable)]
    public FsmInt storeMaxValue;
    
    // cache
    GameObject cachedBarObject;
    EnergyBar cachedEnergyBar;

    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        UpdateCache();

        if (!storeMinValue.IsNone) {
            storeMinValue.Value = cachedEnergyBar.valueMin;
        }

        if (!storeMaxValue.IsNone) {
            storeMaxValue.Value = cachedEnergyBar.valueMax;
        }

        Finish();
    }
    
    void UpdateCache() {
        if (cachedBarObject != barObject.Value) {
            cachedBarObject = barObject.Value;
            cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
        }
    }
    
    public override void Reset() {
        barObject = null;
        cachedEnergyBar = null;
        cachedBarObject = null;
    }

}

} // namespace