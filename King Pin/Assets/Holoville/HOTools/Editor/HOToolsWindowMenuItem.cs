using Holoville.HOTools;
using UnityEditor;
public class HOToolsWindowMenuItem
{
   [MenuItem ("Window/HOTools")]
   static void ShowWindow() {
       EditorWindow.GetWindow(typeof(HOToolsWindow), false, "HOTools");
   }
}