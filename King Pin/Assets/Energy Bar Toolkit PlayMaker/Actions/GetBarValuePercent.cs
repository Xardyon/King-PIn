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
[Tooltip("Reads current Energy Bar Value percentage.")]
public class GetBarValuePercent : FsmStateAction {

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
    [UIHint(UIHint.Variable)]
    public FsmFloat storeValue;

    public bool everyFrame;

    // cache
    GameObject cachedBarObject;
    EnergyBar cachedEnergyBar;
    
    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    public override void OnEnter() {
        Execute();

        if (!everyFrame) {
            Finish();
        }
    }

    private void Execute() {
        UpdateCache();
        storeValue.Value = cachedEnergyBar.ValueF;
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