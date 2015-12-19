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
[Tooltip("Changed energy bar effect smooth change settings.")]
public class ChangeBarEffectSmooth : FsmStateAction {

    // ===========================================================
    // Constants
    // ===========================================================

    // ===========================================================
    // Fields
    // ===========================================================
    
    [RequiredField]
    [Tooltip("Energy bar on which action should be done")]
    [CheckForComponent(typeof(EnergyBarBase))]
    public FsmGameObject barObject;

    public FsmBool effectEnabled;

    public FsmFloat smoothSpeed;
    
    public bool everyFrame;
    
    // cache
    GameObject cachedBarObject;
    EnergyBarBase cachedEnergyBar;

    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        DoChange();
                
        if (!everyFrame) {
            Finish();
        }
    }
    
    public override void OnUpdate() {
        DoChange();
    }
    
    void DoChange() {
        UpdateCache();

        cachedEnergyBar.effectSmoothChange = effectEnabled.Value;
        cachedEnergyBar.effectSmoothChangeSpeed = smoothSpeed.Value;
    }
    
    void UpdateCache() {
        if (cachedBarObject != barObject.Value) {
            cachedBarObject = barObject.Value;
            cachedEnergyBar = cachedBarObject.GetComponent<EnergyBarBase>();
        }
    }
    
    public override void Reset() {
        effectEnabled = true;
        smoothSpeed = 0.5f;
    }
    
}

} // namespace