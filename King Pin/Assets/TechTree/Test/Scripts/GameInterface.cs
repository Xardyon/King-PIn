using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TechTree;
using System.Linq;

public class GameInterface : MonoBehaviour
{

    public BlueprintModelController bmc;
    public Rect rect = new Rect (0, 0, 512, 512);
    public Color teamColor;
    List<BlueprintBuildRequest> buildRequests = new List<BlueprintBuildRequest> ();
    Vector2 scroll;
    Vector2 buildScroll;

    void Start ()
    {
        rect.height = Screen.height;
    }

    TechTreeUnit selectedUnit = null;

    void DrawResourceValues ()
    {
        GUILayout.BeginHorizontal ("box");
        GUILayout.Label ("Resources: ");
        foreach (var i in bmc.resources.Values) {
            GUILayout.Label (string.Format ("{0} {1}", i.resource.ID, (int)i.qty));
        }
        GUILayout.EndHorizontal ();
    }

    void DrawUnits ()
    {
        buildScroll = GUILayout.BeginScrollView (buildScroll, GUILayout.Width (256));
        GUILayout.Label ("Select a Unit");
        foreach (var i in bmc.units.ToArray ()) {
            if (i == null) {
                bmc.units.Remove (i);
                continue;
            }
            GUILayout.BeginHorizontal (selectedUnit == i ? "box" : "");
            if (GUILayout.Button (i.bpc.blueprint.ID)) {
                selectedUnit = i;
            }
            if (GUILayout.Button ("X", GUILayout.Width (32))) {
                Destroy (i.gameObject);
            }
            GUILayout.EndHorizontal ();
        }
        GUILayout.EndScrollView ();
    }

    void DrawSelectedUnit ()
    {
        if (selectedUnit != null) {
            scroll = GUILayout.BeginScrollView (scroll);
            GUILayout.Label (selectedUnit.bpc.blueprint.isFactory ? "Select a Unit to Build" : "The selected unit is not a factory.");
            if (selectedUnit.bpc.blueprint.isFactory) {
                var factory = selectedUnit.GetComponent<TechTreeFactory> ();
                foreach (var b in factory.blueprints) {
                    if (factory.CanBuild (b)) {
                        if (GUILayout.Button (b.blueprint.ID)) {
                            var buildRequest = factory.Build (b.blueprint.ID);
                            buildRequests.Add (buildRequest);
                        }
                    }
                }
            }

            GUILayout.Label (selectedUnit.bpc.blueprint.isUpgradeable ? "This unit is upgradeable." : "This unit can NOT be upgraded.");
            if (selectedUnit.bpc.blueprint.isUpgradeable) {
                GUILayout.Label ("Level: " + selectedUnit.Level);
                GUILayout.Label ("Is Upgrading? " + selectedUnit.IsUpgrading.ToString ());
                GUILayout.Label ("Percent Complete: " + selectedUnit.UpgradeProgress.ToString ());
                if (selectedUnit.CanUpgrade) {
                    if (GUILayout.Button ("Upgrade")) {
                        selectedUnit.PerformUpgrade ();
                    }
                } else {
                    GUILayout.Label ("Max level reached.");
                }
            }
            foreach (var s in selectedUnit.Stats) {
                GUILayout.BeginHorizontal ();
                GUILayout.Label (s);
                GUILayout.Label (selectedUnit.GetStat (s).ToString());
                GUILayout.EndHorizontal ();
            }

            GUILayout.EndScrollView ();
        }
    }

    void DrawBuildQueue ()
    {
        for (var i=0; i<Mathf.Min(buildRequests.Count, 5); i++) {
            var br = buildRequests [buildRequests.Count - i - 1];
            GUILayout.BeginHorizontal ("box");
            GUILayout.Label (br.status.ToString ());
            if (br.status == BuildStatus.Success) {
                GUILayout.Label (br.blueprint.ID);
                if (br.Complete) {
                    GUILayout.Label ("Complete");
                } else {
                    GUILayout.Label (((int)(br.percentComplete * 100)).ToString () + "%");
                    if (GUILayout.Button ("Cancel")) {
                        br.Cancel ();
                    }
                }
            }
            GUILayout.EndHorizontal ();
        }

    }
   
    void OnGUI ()
    {
        GUI.backgroundColor = teamColor;
        GUILayout.BeginArea (rect);
        DrawResourceValues ();
        GUILayout.BeginVertical ();
        GUILayout.BeginHorizontal ();
        DrawUnits ();
        DrawSelectedUnit ();
        GUILayout.EndVertical ();
        DrawBuildQueue ();
        GUILayout.EndHorizontal ();
        GUILayout.EndArea ();
    }
    
    
}
