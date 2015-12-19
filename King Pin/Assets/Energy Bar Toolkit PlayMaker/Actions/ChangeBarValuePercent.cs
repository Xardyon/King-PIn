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
[Tooltip("Changed energy bar value by given percentage amount.")]
public class ChangeBarValuePercent : FsmStateAction {

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

    [RequiredField]
    [Tooltip("Amount to add to current energy bar value. Set amount below zero to decrease the value.")]
    [HasFloatSlider(-1, 1)]
    public FsmFloat changeValue;
    
    public bool everyFrame;
    
    // cache
    GameObject cachedBarObject;
    EnergyBar cachedEnergyBar;

    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        DoChangeBarValuePercent();
        
        if (!everyFrame) {
            Finish();
        }
    }
    
    public override void OnUpdate() {
        DoChangeBarValuePercent();
    }
    
    void DoChangeBarValuePercent() {
        UpdateCache();
        cachedEnergyBar.ValueF += changeValue.Value;
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