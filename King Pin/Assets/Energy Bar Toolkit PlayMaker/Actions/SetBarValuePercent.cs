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
[Tooltip("Sets energy bar value to given percentage value.")]
public class SetBarValuePercent : FsmStateAction {

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
    [Tooltip("Percentage value of energy bar to set.")]
    [HasFloatSlider(0, 1)]
    public FsmFloat value;
    
    public bool everyFrame;

    // cache
    GameObject cachedBarObject;
    EnergyBar cachedEnergyBar;
    
    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        DoSetBarValuePercent();
        
        if (!everyFrame) {
            Finish();
        }
    }
    
    public override void OnUpdate() {
        DoSetBarValuePercent();
    }
    
    void DoSetBarValuePercent() {
        UpdateCache();
        cachedEnergyBar.ValueF = value.Value;
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