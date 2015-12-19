/*    INFINITY CODE 2013 - 2014     */
/*   http://www.infinity-code.com   */

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
#define NOTES_WITH_LOOK
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Notes_Manager : EditorWindow
{
    public static bool needUpdate = true;
    public static Note[] notes;
    public static Notes_Manager wnd;

    private static GUIStyle _expandedStyle;
    private static Texture2D _selectedTexture;
    private static bool clearFocus;
    private static long lastClick;
    private static string scenePath;
    //private static int searchIndex = -1;
    //private static Note searchNote;
    //private static string searchText = "";
    private static Note selectNote;
    private static bool setSearchFocus;

    public Vector2 scrollPos;

    private List<string> allTags;
    private GameObject[] lastSelection;
    //private bool showSearch;
    
    private int tagsMask = -1;
    private static bool updateSelection;

    private static GUIStyle expandedStyle
    {
        get
        {
            return _expandedStyle ??
                   (_expandedStyle =
                       new GUIStyle { fixedWidth = 16, fixedHeight = 16, margin = new RectOffset(4, 0, 4, 0) });
        }
    }

    private static Texture2D selectedTexture
    {
        get { return _selectedTexture ?? (_selectedTexture = Note_Icon_Manager.GetIcon("SelectedNoteItem.png")); }
    }

    private static void AddNoteToSelected()
    {
        GUI.FocusControl("AddToSelectedButton");
        foreach (GameObject go in Selection.gameObjects.Where(go => go.GetComponent<Note>() == null)) go.AddComponent<Note>();
    }

    private static void ExportNodes()
    {
        string filename = EditorUtility.SaveFilePanel("Export notes", "", "notes.xml", "xml");
        if (filename.Length != 0)
            File.WriteAllBytes(filename, Encoding.UTF8.GetBytes(Note_Icon_Manager.GetXMLString()));
    }

    private static void ImportNodes()
    {
        string filename = EditorUtility.OpenFilePanel("Import notes", "", "xml");
        if (filename.Length != 0)
            Note_Icon_Manager.SetXMLString(Encoding.UTF8.GetString(File.ReadAllBytes(filename)));
    }

    private void OnDestroy()
    {
        wnd = null;
    }

    private void OnDisable()
    {
        wnd = null;
    }

    private void OnEnable()
    {
        wnd = this;
    }

	private void OnGUI()
	{
		if (notes == null || needUpdate || notes.Any(n => n == null || n.gameObject == null)) 
		{
            UpdateNotes();
			needUpdate = false;
		}

		OnGUIToolbar();

	    /*if (showSearch)
	    {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            searchText = EditorGUILayout.TextField("Search: ", searchText);
	        if (GUILayout.Button("Find", GUILayout.ExpandWidth(false)) && searchText != "")
	        {
	            foreach (Note note in notes)
	            {
	                searchIndex = note.text.IndexOf(searchText);
	                if (searchIndex != -1)
	                {
	                    searchNote = note;
	                    setSearchFocus = true;
	                }
	            }
	        }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
	    }*/

        BitArray bArray = new BitArray(BitConverter.GetBytes(tagsMask));

	    scrollPos = GUILayout.BeginScrollView(scrollPos);
	    foreach (Note note in notes)
	    {
            if (tagsMask == 0) continue;
	        
	        if (tagsMask != -1)
	        {
	            bool finded = note.tags.Any(tag => bArray.Get(allTags.IndexOf(tag)));
                if (bArray.Get(0) && (note.tags == null || note.tags.Length == 0)) finded = true;
                
	            if (!finded) continue;
	        }
	        if (OnGUIItem(note)) break;
	    }
	    GUILayout.EndScrollView();
	}

    private bool OnGUIItem(Note note)
    {
        if (OnGUIItemHeader(note)) return true;
        if (note.expanded)
        {
            OnGUIItemProps(note);
            OnGUIItemText(note);

            GUILayout.Space(5);
        }

        return false;
    }

    private bool OnGUIItemHeader(Note note)
    {
        expandedStyle.normal.background = Note_Icon_Manager.GetGroupTexture(note.expanded);

        GUIStyle groupStyle = new GUIStyle { fixedHeight = 25 };
        if (Selection.gameObjects.Contains(note.gameObject)) groupStyle.normal.background = selectedTexture;

        GUILayout.BeginHorizontal(groupStyle);

        if (GUILayout.Button("", expandedStyle, GUILayout.Width(20))) note.expanded = !note.expanded;

        OnGUIItemHeaderTitle(note);

        GUI.SetNextControlName("ClearButton");
        if (note.expanded && GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
        {
            GUI.FocusControl("ClearButton");
            note.text = "";
            return true;
        }

        if (clearFocus)
        {
            clearFocus = false;
            GUI.FocusControl("ClearButton");
        }

        if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false))) return RemoveNote(note);

        GUILayout.EndHorizontal();
        return false;
    }

    private void OnGUIItemHeaderTitle(Note note)
    {
        if (selectNote && note == selectNote)
        {
            Rect noteRect = GUILayoutUtility.GetLastRect();
            if (Mathf.FloorToInt(noteRect.y) > 0)
            {
                scrollPos.y = Mathf.FloorToInt(noteRect.y) - 5;
                selectNote = null;
            }
        }

        if (GUILayout.Button(note.title, EditorStyles.objectFieldThumb))
        {
            long curTicks = DateTime.Now.Ticks;
            if (curTicks - lastClick < 3000000)
            {
                Selection.activeGameObject = note.gameObject;
            }
            else EditorGUIUtility.PingObject(note.gameObject);
            lastClick = curTicks;
        }
    }

    private static void OnGUIItemIcon(Note note)
    {
        Texture2D icon = note.icon;
        if (note.icon == null && note.iconPath != "")
        {
            if (!Note_Editor.LoadIcon(note)) note.iconPath = "";
        }

#if NOTES_WITH_LOOK
        EditorGUIUtility.LookLikeInspector();
#endif
        note.icon = (Texture2D)EditorGUILayout.ObjectField("Icon: ", note.icon, typeof(Texture2D), false);
#if NOTES_WITH_LOOK
        EditorGUIUtility.LookLikeControls();
#endif

        if (note.icon != icon)
        {
            if (note.icon != null) note.iconPath = AssetDatabase.GetAssetPath(note.icon);
            else note.iconPath = "";
            EditorApplication.RepaintHierarchyWindow();
        }
    }

    private static void OnGUIItemProps(Note note)
    {
        OnGUIItemIcon(note);

        string oldTags = note.tagsStr;
        note.tagsStr = EditorGUILayout.TextField("Tags (separator \";\"): ", note.tagsStr, EditorStyles.textField);


        if (note.tagsStr != oldTags)
        {
            List<string> tags = note.tagsStr.Split(';').ToList();
            for (int i = 0; i < tags.Count; i++) tags[i] = tags[i].Trim(' ');
            tags.Remove("");
            note.tags = tags.ToArray();
        }

        GUILayout.BeginHorizontal();
        note.wordWrap = GUILayout.Toggle(note.wordWrap, "Word wrap", GUILayout.ExpandWidth(false));
        note.lockHeight = GUILayout.Toggle(note.lockHeight, "Lock text area height" + (note.lockHeight ? ": " : ""),
            GUILayout.ExpandWidth(false));

        if (note.lockHeight)
            note.managerHeight = GUILayout.HorizontalSlider(note.managerHeight, 45, note.maxHeight, GUILayout.MinWidth(80));
        GUILayout.EndHorizontal();
    }

    private static void OnGUIItemText(Note note)
    {
        GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = note.wordWrap};

        const int textAreaMinHeight = 50;
        float height = Mathf.Max(textAreaStyle.CalcHeight(new GUIContent(note.text), wnd.position.width), textAreaMinHeight);
        note.maxHeight = Mathf.Max(800, height + 50);
        string lastText = note.text;

        OnGUIItemTextArea(note, height, textAreaStyle, textAreaMinHeight);

        if (note.text.Length > 16000) note.text = note.text.Substring(0, 16000);
        if (note.text != lastText) EditorUtility.SetDirty(note);
    }

    private static void OnGUIItemTextArea(Note note, float height, GUIStyle textAreaStyle, int textAreaMinHeight)
    {
        string noteName = "noteText" + notes.ToList().IndexOf(note);

        if (!note.lockHeight)
        {
            note.managerHeight = height;
            note.managerScrollPos = EditorGUILayout.BeginScrollView(note.managerScrollPos, GUILayout.Height(height + 25));
            
            GUI.SetNextControlName(noteName);
            note.text = EditorGUILayout.TextArea(note.text, textAreaStyle, GUILayout.MinHeight(textAreaMinHeight),
                GUILayout.Height(height));
            EditorGUILayout.EndScrollView();
        }
        else
        {
            note.managerScrollPos = GUILayout.BeginScrollView(note.managerScrollPos,
                GUILayout.Height(note.managerHeight));

            GUI.SetNextControlName(noteName);
            note.text = EditorGUILayout.TextArea(note.text, textAreaStyle, GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();
        }

        /*if (setSearchFocus && searchNote == note)
        {
            GUI.FocusControl(noteName);
            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            editor.selectPos = searchIndex;
            editor.pos = searchIndex;
            setSearchFocus = false;
            updateSelection = true;
        }
        else if (updateSelection)
        {
            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            editor.selectPos = searchIndex;
            editor.pos = searchIndex;
            updateSelection = false;
        }*/
    }

    private void OnGUIToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUIStyle toolbarButtonStyle = new GUIStyle(EditorStyles.toolbarButton) {padding = new RectOffset(5, 5, 2, 2)};

        if (GUILayout.Button(new GUIContent(Note_Icon_Manager.openIcon, "Import"), toolbarButtonStyle, GUILayout.ExpandWidth(false))) ImportNodes();

        if (notes.Length > 0)
        {
            if (GUILayout.Button(new GUIContent(Note_Icon_Manager.saveIcon, "Export"), toolbarButtonStyle, GUILayout.ExpandWidth(false))) ExportNodes();
        }

        /*if (GUILayout.Button(new GUIContent(Note_Icon_Manager.searchIcon, "Search"), toolbarButtonStyle,
            GUILayout.ExpandWidth(false)))
        {
            showSearch = !showSearch;
        }*/

        GUI.SetNextControlName("AddToSelectedButton");
        if (GUILayout.Button(new GUIContent("Add note to selected object", Note_Icon_Manager.plusIcon2), toolbarButtonStyle)) AddNoteToSelected();

        allTags = new List<string> {"Empty"};

        foreach (Note note in notes)
        {
            if (note.tags != null)
            {
                foreach (string tag in note.tags)
                {
                    string lowerTag = tag.ToLower();
                    if (allTags.All(t => t.ToLower() != lowerTag))
                    {
                        allTags.Add(tag);
                        if (allTags.Count == 32) break;
                    }
                }
                if (allTags.Count == 32) break;
            }
        }

        if (allTags.Count > 1)
        {
            EditorGUI.BeginChangeCheck();
            tagsMask = EditorGUILayout.MaskField("Tags:", tagsMask, allTags.ToArray(), EditorStyles.toolbarDropDown);
            if (EditorGUI.EndChangeCheck()) GUI.FocusControl("AddToSelectedButton");
        }
        else tagsMask = -1;

        bool collapsed = notes.All(n => !n.expanded);
        if (GUILayout.Button(collapsed ? "Expand all" : "Collapse all", EditorStyles.toolbarButton, GUILayout.Width(80))) foreach (Note note in notes) note.expanded = collapsed;

        OnGUIToolbarSupport();

        GUILayout.EndHorizontal();
    }

    public static void OnGUIToolbarSupport()
    {
        if (GUILayout.Button("Help", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("View online documentation"), false, OnViewDocs);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Open product page"), false, OnProductPage);
            menu.AddItem(new GUIContent("Mail to support"), false, OnSendMail);

            menu.ShowAsContext();
        }
    }

    public static void OnProductPage()
    {
        Process.Start("http://infinity-code.com/products/notes");
    }

    public static void OnSendMail()
    {
        Process.Start("mailto:support@infinity-code.com?subject=Notes");
    }

    public static void OnViewDocs()
    {
        Process.Start("http://infinity-code.com/docs/notes");
    }

    [MenuItem("Window/Infinity Code/Notes Manager", false, 2)]
    private static void OpenWindow()
    {
        wnd = GetWindow<Notes_Manager>(false, "Notes Manager");
        wnd.autoRepaintOnSceneChange = true;
        DontDestroyOnLoad(wnd);
        scenePath = EditorApplication.currentScene;
    }

    private static bool RemoveNote(Note note)
    {
        needUpdate = true;
        DestroyImmediate(note, true);
        return true;
    }

    public static void RepaintWindow()
    {
        if (wnd != null) wnd.Repaint();
    }

    public static void ShowNote(Note note)
    {
        selectNote = note;
        if (wnd == null) OpenWindow();
        else RepaintWindow();
    }

    private void Update()
    {
        if (EditorApplication.currentScene != scenePath)
        {
            scenePath = EditorApplication.currentScene;
            clearFocus = true;
            lastSelection = null;
            UpdateNotes();
            Repaint();
        }
        if (!Selection.gameObjects.Equals(lastSelection))
        {
            lastSelection = Selection.gameObjects;
            Repaint();
        }
    }

    public static void UpdateNotes()
    {
        notes = (Note[]) Resources.FindObjectsOfTypeAll(typeof (Note));
        notes = notes.Where(n => n.gameObject != null).OrderBy(n => n.title).ToArray();
        foreach (Note note in notes) note.isPrefab = PrefabUtility.GetPrefabParent(note.gameObject) == null && PrefabUtility.GetPrefabObject(note.gameObject);
    }
}