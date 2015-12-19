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
[Tooltip("Sets energy bar value to given value.")]
public class SetBarValue : FsmStateAction {

    // ===========================================================
    // Constants
    // ===========================================================

    // ===========================================================
    // Fields
    // ===========================================================
    
    [RequiredField]
    [Tooltip("Energy bar on which action should be done.")]
    [CheckForComponent(typeof(EnergyBar))]
    public FsmGameObject barObject;

    [RequiredField]
    [Tooltip("Value of energy bar to set.")]
    public FsmInt value;
    
    public bool everyFrame;

    // cache
    GameObject cachedBarObject;
    EnergyBar cachedEnergyBar;
    
    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        DoSetBarValue();
        
        if (!everyFrame) {
            Finish();
        }
    }
    
    public override void OnUpdate() {
        DoSetBarValue();
    }
    
    void DoSetBarValue() {
        UpdateCache();
        cachedEnergyBar.valueCurrent = Mathf.Clamp(value.Value, cachedEnergyBar.valueMin, cachedEnergyBar.valueMax);
    }
    
    void UpdateCache() {
        if (cachedBarObject != barObject.Value) {
            cachedBarObject = barObject.Value;
            cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
        }
    }
    
    public override void Reset() {
        barObject = null;
        value = 0;
        cachedEnergyBar = null;
        cachedBarObject = null;
    }
    
}

} // namespace