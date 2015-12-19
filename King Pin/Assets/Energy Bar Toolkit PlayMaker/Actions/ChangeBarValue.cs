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
[Tooltip("Changed energy bar value by given amount.")]
public class ChangeBarValue : FsmStateAction {

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
    
    [Tooltip("Amount to add to current energy bar value. Set amount below zero to decrease the value.")]
    public FsmInt changeValue;
    
    public bool everyFrame;
    
    // cache
    GameObject cachedBarObject;
    EnergyBar cachedEnergyBar;

    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        DoChangeBarValue();
                
        if (!everyFrame) {
            Finish();
        }
    }
    
    public override void OnUpdate() {
        DoChangeBarValue();
    }
    
    void DoChangeBarValue() {
        UpdateCache();
        
        cachedEnergyBar.valueCurrent =
            Mathf.Clamp(
                cachedEnergyBar.valueCurrent + changeValue.Value, cachedEnergyBar.valueMin, cachedEnergyBar.valueMax);
    }
    
    void UpdateCache() {
        if (cachedBarObject != barObject.Value) {
            cachedBarObject = barObject.Value;
            cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
        }
    }
    
    public override void Reset() {
        barObject = null;
        changeValue = 0;
        cachedEnergyBar = null;
        cachedBarObject = null;
    }
    
}

} // namespace