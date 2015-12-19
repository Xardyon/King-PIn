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
[Tooltip("Sets energy bar min and max values.")]
public class SetBarMinMaxValue : FsmStateAction {

    // ===========================================================
    // Constants
    // ===========================================================

    // ===========================================================
    // Fields
    // ===========================================================
    
    [RequiredField]
    [Tooltip("Energy bar object to modify.")]
    [CheckForComponent(typeof(EnergyBar))]
    public FsmGameObject barObject;

    [Tooltip("Minimum allowed value.")]
    public FsmInt minValue;

    [Tooltip("Maximum allowed value.")]
    public FsmInt maxValue;
    
    public bool everyFrame;

    // cache
    GameObject cachedBarObject;
    EnergyBar cachedEnergyBar;
    
    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        DoSetBarValues();
        
        if (!everyFrame) {
            Finish();
        }
    }
    
    public override void OnUpdate() {
        DoSetBarValues();
    }
    
    void DoSetBarValues() {
        UpdateCache();

        if (!minValue.IsNone) {
            cachedEnergyBar.valueMin = minValue.Value;
        }

        if (!maxValue.IsNone) {
            cachedEnergyBar.valueMax = maxValue.Value;
        }
    }
    
    void UpdateCache() {
        if (cachedBarObject != barObject.Value) {
            cachedBarObject = barObject.Value;
            cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
        }
    }
    
    public override void Reset() {
        barObject = null;
        minValue = 0;
        maxValue = 100;
        cachedEnergyBar = null;
        cachedBarObject = null;
    }
    
}

} // namespace