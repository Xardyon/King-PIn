/*    INFINITY CODE 2013 - 2014     */
/*   http://www.infinity-code.com   */

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
#define NOTES_WITH_LOOK
#endif

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Note))]
public class Note_Editor : Editor 
{
    private GUIStyle _textAreaStyle;
	private Note note;
	private int fastInsertID;
    private float lastTextAreaWidth = 100;
    private readonly string nl = Environment.NewLine;

    private GUIStyle textAreaStyle
    {
        get { return _textAreaStyle ?? (_textAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = note.wordWrap}); }
    }

    private static void CheckInsertLevels(SerializedProperty sp, List<int> levels)
    {
        if (levels.Count > 0) levels[levels.Count - 1] -= 1;

        if (sp.propertyType == SerializedPropertyType.ArraySize) levels.Add(sp.intValue);
        else if (sp.propertyType == SerializedPropertyType.Bounds) levels.Add(2);
        else if (sp.propertyType == SerializedPropertyType.Generic) levels.Add(1);
        else if (sp.propertyType == SerializedPropertyType.Rect) levels.Add(4);
        else if (sp.propertyType == SerializedPropertyType.Vector2) levels.Add(2);
        else if (sp.propertyType == SerializedPropertyType.Vector3) levels.Add(3);

        while (levels.Count > 0)
        {
            int lastIndex = levels.Count - 1;
            if (levels[lastIndex] == 0) levels.RemoveAt(lastIndex);
            else break;
        }
    }

    private void ClearText()
    {
        note.text = "";
        GUI.FocusControl("InsertButton");
        Notes_Manager.RepaintWindow();
    }

    private void ExportText()
    {
        string filename = EditorUtility.SaveFilePanel("Export note", "", note.name + ".txt", "txt");
        if (filename.Length != 0) File.WriteAllText(filename, note.text);
    }

    private List<string> GetComponents(out List<Component> compsInstance)
    {
        Component[] comps = note.GetComponents<Component>();
        List<string> compNames = new List<string>();

        compsInstance = new List<Component>();

        foreach (Component comp in comps.Where(comp => !(comp is Note)))
        {
            string compName;
            if (comp is Transform) compName = "Transform";
            else if (comp is Camera) compName = "Camera";
            else compName = comp.GetType().ToString();

            if (compName != "")
            {
                compNames.Add(compName);
                compsInstance.Add(comp);
            }
        }
        return compNames;
    }

    private void GetComponentValue(SerializedProperty sp, ref string insertValue, int level)
    {
        if (sp.propertyType == SerializedPropertyType.AnimationCurve) GetComponentValueAnimationCurve(sp, ref insertValue, level);
        else if (sp.propertyType == SerializedPropertyType.ArraySize) insertValue += sp.intValue.ToString();
        else if (sp.propertyType == SerializedPropertyType.Boolean) insertValue += sp.boolValue.ToString();
        else if (sp.propertyType == SerializedPropertyType.Color) GetComponentValueColor(sp, ref insertValue, level);
        else if (sp.propertyType == SerializedPropertyType.Enum) insertValue += sp.enumNames[sp.enumValueIndex];
        else if (sp.propertyType == SerializedPropertyType.Float) insertValue += sp.floatValue.ToString();
        else if (sp.propertyType == SerializedPropertyType.Integer) insertValue += sp.intValue.ToString();
        else if (sp.propertyType == SerializedPropertyType.LayerMask) GetComponentValueLayerMask(sp, ref insertValue, level);
        else if (sp.propertyType == SerializedPropertyType.ObjectReference) GetComponentValueObjectReference(sp, ref insertValue, level);
        else if (sp.propertyType == SerializedPropertyType.String) insertValue += sp.stringValue;
        insertValue += "\n";
    }

    private void GetComponentValueAnimationCurve(SerializedProperty sp, ref string insertValue, int level)
    {
        AnimationCurve curve = sp.animationCurveValue;
        insertValue += nl + GetTabs(level + 1) + "Length: " + curve.length;
        insertValue += nl + GetTabs(level + 1) + "PostWrapMode: " + curve.postWrapMode;
        insertValue += nl + GetTabs(level + 1) + "PreWrapMode: " + curve.preWrapMode;
        for (int i = 0; i < curve.keys.Length; i++)
        {
            Keyframe key = curve.keys[i];
            insertValue += nl + GetTabs(level + 1) + "Key " + i + ": " +
                           nl + GetTabs(level + 2) + "inTangent: " + key.inTangent +
                           nl + GetTabs(level + 2) + "outTangent: " + key.outTangent +
                           nl + GetTabs(level + 2) + "tangentMode: " + key.tangentMode +
                           nl + GetTabs(level + 2) + "time: " + key.time +
                           nl + GetTabs(level + 2) + "value: " + key.value;
        }
    }

    private void GetComponentValueColor(SerializedProperty sp, ref string insertValue, int level)
    {
        insertValue += nl + GetTabs(level + 1) + "Color \t\t" + sp.colorValue + nl;
        insertValue += GetTabs(level + 1) + "Color32 \t" + ((Color32) sp.colorValue);
    }

    private void GetComponentValueLayerMask(SerializedProperty sp, ref string insertValue, int level)
    {
        if (sp.intValue == -1) insertValue += "Everything";
        else if (sp.intValue == 0) insertValue += "Nothing";
        else
        {
            for (int i = 0; i < 32; ++i)
            {
                int shifted = 1 << i;
                if ((sp.intValue & shifted) == shifted)
                {
                    string layerName = LayerMask.LayerToName(i);
                    if (!string.IsNullOrEmpty(layerName)) insertValue += nl + GetTabs(level + 1) + layerName;
                }
            }
        }
    }

    private void GetComponentValueObjectReference(SerializedProperty sp, ref string insertValue, int level)
    {
        if (sp.objectReferenceValue != null)
        {
            insertValue += nl + GetTabs(level + 1) + "name: " + sp.objectReferenceValue.name;
            insertValue += nl + GetTabs(level + 1) + "path: " + AssetDatabase.GetAssetOrScenePath(sp.objectReferenceValue);
            insertValue += nl + GetTabs(level + 1) + "type: " + sp.objectReferenceValue.GetType();
        }
        else insertValue += "None";
    }

    private float GetInspectorWidth()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        Rect inspectorSize = GUILayoutUtility.GetLastRect();
        return (inspectorSize.width > 1) ? inspectorSize.width : lastTextAreaWidth;
    }

    private string GetTabs(int level)
    {
        string ret = "";
        for (int i = 0; i <= level; i++) ret += "\t";
        return ret;
    }

    private void ImportText()
    {
        GUI.FocusControl("ImportButton");
        string filename = EditorUtility.OpenFilePanel("Import note", "", "txt");
        if (filename.Length != 0)
        {
            if (new FileInfo(filename).Length > 16000) Debug.LogWarning("The selected file is too large.");
            else
            {
                note.text = File.ReadAllText(filename);
                EditorUtility.SetDirty(note);
            }
        }
    }

    public static bool LoadIcon(Note note)
    {
        if (note.iconPath != "")
        {
            note.icon = AssetDatabase.LoadAssetAtPath(note.iconPath, typeof(Texture2D)) as Texture2D;
            if (note.icon == null)
            {
                note.iconPath = "";
                return false;
            }
            return true;
        }
        return false;
    }
	
	private void OnDestroy()
	{
        Notes_Manager.UpdateNotes();
	}

    private void OnDisable()
	{
        Notes_Manager.UpdateNotes();
	}

    private void OnEnable()
	{
		note = (Note)target;
        Notes_Manager.UpdateNotes();
        if (note.tags != null) note.tagsStr = string.Join("; ", note.tags);
        else note.tagsStr = "";
	}

    private void OnGUIIcon()
    {
        Texture2D icon = note.icon;
        if (note.icon == null && note.iconPath != "")
        {
            if (!LoadIcon(note)) note.iconPath = "";
        }

#if NOTES_WITH_LOOK
        EditorGUIUtility.LookLikeInspector();
#endif
       
		note.icon = (Texture2D)EditorGUILayout.ObjectField("Icon: ", note.icon, typeof(Texture2D), false);

        if (note.icon != icon)
        {
            if (note.icon != null) note.iconPath = AssetDatabase.GetAssetPath(note.icon);
            else note.iconPath = "";
            EditorApplication.RepaintHierarchyWindow();
            Notes_Manager.RepaintWindow();
            EditorUtility.SetDirty(note);
        }

#if NOTES_WITH_LOOK
        EditorGUIUtility.LookLikeControls();
#endif
    }

    private void OnGUIInsert()
    {
        GUI.SetNextControlName("InsertButton");
        if (GUILayout.Button("Insert"))
        {
            GUI.FocusControl("InsertButton");

            List<Component> compsInstance;
            List<string> compNames = GetComponents(out compsInstance);

            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < compNames.Count; i++) menu.AddItem(new GUIContent(compNames[i]), false, OnInsert, compsInstance[i]);
            menu.ShowAsContext();
        }
    }

    private void OnInsert(object compObj)
    {
        OnInsert(compObj as Component);
        Notes_Manager.RepaintWindow();
    }

    private void OnGUIProps()
    {
        OnGUIIcon();
        OnGUIInsert();

        string oldTags = note.tagsStr;
        EditorGUI.BeginChangeCheck();
        note.tagsStr = EditorGUILayout.TextField("Tags (separator \";\"): ", note.tagsStr);
        if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(note);

        if (note.tagsStr != oldTags)
        {
            List<string> tags = note.tagsStr.Split(';').ToList();
            for(int i = 0; i < tags.Count; i++) tags[i] = tags[i].Trim(' ');
            tags.Remove("");
            note.tags = tags.ToArray();
        }

        EditorGUI.BeginChangeCheck();
        note.wordWrap = GUILayout.Toggle(note.wordWrap, "Word wrap");
        if (EditorGUI.EndChangeCheck())
        {
            _textAreaStyle = null;
            GUI.FocusControl("InsertButton");
            Notes_Manager.RepaintWindow();
            EditorUtility.SetDirty(note);
        }

        GUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();
        note.lockHeight = GUILayout.Toggle(note.lockHeight, "Lock text area height" + (note.lockHeight ? ": " : ""));
        if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(note);
        if (note.lockHeight) note.height = GUILayout.HorizontalSlider(note.height, 45, note.maxHeight, GUILayout.MinWidth(80));
        GUILayout.EndHorizontal();
    }

    private void OnGUIText()
    {
        float width = GetInspectorWidth();
        lastTextAreaWidth = width;

        float height = Mathf.Max(textAreaStyle.CalcHeight(new GUIContent(note.text), width - 20), 100);
        note.maxHeight = Mathf.Max(800, height + 50);

        string lastText = note.text;

        OnGUITextArea(height, textAreaStyle);

        if (note.text.Length > 16000) note.text = note.text.Substring(0, 16000);

        if (lastText != note.text)
        {
            EditorUtility.SetDirty(note);
            Notes_Manager.RepaintWindow();
        }
    }

    private void OnGUITextArea(float height, GUIStyle textAreaStyle)
    {
        if (!note.lockHeight)
        {
            note.height = height;
            note.scrollPos = EditorGUILayout.BeginScrollView(note.scrollPos, GUILayout.Height(height + 25));
            note.text = EditorGUILayout.TextArea(note.text, textAreaStyle, GUILayout.Height(height));
            EditorGUILayout.EndScrollView();
        }
        else
        {
            note.scrollPos = EditorGUILayout.BeginScrollView(note.scrollPos, GUILayout.Height(note.height));
            note.text = EditorGUILayout.TextArea(note.text, textAreaStyle, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
    }

    private void OnGUIToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUIStyle toolbarButtonStyle = new GUIStyle(EditorStyles.toolbarButton) {padding = new RectOffset(5, 5, 2, 2)};

        GUI.SetNextControlName("ImportButton");
        if (GUILayout.Button(new GUIContent(Note_Icon_Manager.openIcon, "Import"), toolbarButtonStyle, GUILayout.ExpandWidth(false))) ImportText();
        if (GUILayout.Button(new GUIContent(Note_Icon_Manager.saveIcon, "Export"), toolbarButtonStyle, GUILayout.ExpandWidth(false))) ExportText();

        GUILayout.Label("", EditorStyles.toolbarButton);

        if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false))) ClearText();

        Notes_Manager.OnGUIToolbarSupport();

        GUILayout.EndHorizontal();
    }

    private bool OnInsert(Component comp)
    {
        string insertValue;
        if (comp is Transform) insertValue = OnInsertTransform();
        else insertValue = OnInsertCustom(comp);

        if (insertValue == "") return false;
        if (insertValue.Length > 16000) insertValue = insertValue.Substring(0, 16000).TrimEnd('\t') + "Stopped. Reached the limit on number of characters.";

        if (note.text == "") note.text = insertValue;
        else note.text += nl + insertValue;

        return true;
    }

    private string OnInsertCustom(Component comp)
    {
        SerializedProperty sp = new SerializedObject(comp).GetIterator();

        string insertValue = OnInsertCustomTitle(comp);
        List<int> levels = new List<int>();

        while (sp.NextVisible(true))
        {
            if (sp.name == "m_Script") continue;

            string _name = sp.name;
            if (_name.Length > 2 && _name.Substring(0, 2) == "m_") _name = _name.Substring(2);
            insertValue += GetTabs(levels.Count) + _name + ": ";

            GetComponentValue(sp, ref insertValue, levels.Count);
            CheckInsertLevels(sp, levels);
        }
        return insertValue;
    }

    private string OnInsertCustomTitle(Component comp)
    {
        Type type = comp.GetType();
        string typeName = type.ToString();
        if (typeName.StartsWith("UnityEngine.")) typeName = typeName.Substring(12);
        string insertValue = typeName + nl;
        return insertValue;
    }

    private string OnInsertTransform()
    {
        string insertValue = "Transform" + nl +
                      "\tGlobal position: " + Vector3ToStr(note.transform.position) + nl +
                      "\tLocal position: " + Vector3ToStr(note.transform.localPosition) + nl +
                      "\tGlobal rotation: " + Vector3ToStr(note.transform.rotation.eulerAngles) + nl +
                      "\tLocal rotation: " + Vector3ToStr(note.transform.localRotation.eulerAngles) + nl +
                      "\tScale: " + Vector3ToStr(note.transform.localScale);
        return insertValue;
    }

    public override void OnInspectorGUI()
    {
	    if (note == null) note = (Note) target;

        OnGUIToolbar();
	    OnGUIProps();
	    OnGUIText();
	}

    private string Vector3ToStr(Vector3 v)
    {
        return string.Format("({0}, {1}, {2})", v.x, v.y, v.z);
    }
}