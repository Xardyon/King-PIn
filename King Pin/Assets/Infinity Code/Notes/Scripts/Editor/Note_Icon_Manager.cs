/*    INFINITY CODE 2013 - 2014     */
/*   http://www.infinity-code.com   */

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad] 
public class Note_Icon_Manager
{
    private const float iconSize = 16;

    private static Texture2D _minusIcon;
    private static Texture2D _openIcon;
    private static Texture2D _plusIcon;
    private static Texture2D _plusIcon2;
    private static Texture2D _saveIcon;
    private static Texture2D _searchIcon;

    private static bool needLoad;
    private static string nodeString;
    private static Vector2 managerScroll;
    private static bool restoreScroll;
    private static bool noDefaultIcon;
    private static Type typeOfHierarchy2;
    private static Type typeOfFavoritesTab;
    private static EditorWindow hierarchyWindow;

    private static Texture2D minusIcon
    {
        get { return _minusIcon ?? (_minusIcon = GetIcon("MinusIcon.png")); }
    }

    public static Texture2D openIcon
    {
        get { return _openIcon ?? (_openIcon = GetIcon("OpenIcon.png")); }
    }

    private static Texture2D plusIcon
    {
        get { return _plusIcon ?? (_plusIcon = GetIcon("PlusIcon.png")); }
    }

    public static Texture2D plusIcon2
    {
        get { return _plusIcon2 ?? (_plusIcon2 = GetIcon("PlusIcon2.png")); }
    }

    public static Texture2D saveIcon
    {
        get { return _saveIcon ?? (_saveIcon = GetIcon("SaveIcon.png")); }
    }

    public static Texture2D searchIcon
    {
        get { return _searchIcon ?? (_searchIcon = GetIcon("SearchIcon.png")); }
    }
	
    static Note_Icon_Manager()
    {
        //EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;

        typeOfHierarchy2 =
                typeof(Note_Icon_Manager).Assembly.GetType("vietlabs.Hierarchy2")
                ?? typeof(Note_Icon_Manager).Assembly.GetType("Hierarchy2");

        typeOfFavoritesTab = typeof(Note_Icon_Manager).Assembly.GetType("FavoritesTab");
    }

    private static bool IsRenaming()
    {
        if (hierarchyWindow == null) hierarchyWindow = EditorWindow.GetWindow(Types.GetType("UnityEditor.HierarchyWindow", "UnityEditor"));
        var type = Types.GetType("UnityEditor.BaseProjectWindow", "UnityEditor");
        if (type == null) return false;
        var field = type.GetField("m_RealEditNameMode", BindingFlags.Default | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        if (field == null) return false;
        return (int)field.GetValue(hierarchyWindow) == 2;
    }

    public static Texture2D GetGroupTexture(bool expanded)
    {
        return expanded ? minusIcon : plusIcon;
    }

    public static Texture2D GetIcon(string iconName)
    {
        string[] path = Directory.GetFiles(Application.dataPath, iconName, SearchOption.AllDirectories);
        if (path.Length == 0) return null;
        string iconFile = "Assets" + path[0].Substring(Application.dataPath.Length).Replace('\\', '/');
        return AssetDatabase.LoadAssetAtPath(iconFile, typeof(Texture2D)) as Texture2D;
    }

    public static string GetXMLString()
    {
        Note[] notes = Resources.FindObjectsOfTypeAll(typeof(Note)) as Note[];

        XmlDocument doc = new XmlDocument();
        XmlElement notesNode = doc.CreateElement("Notes");
        if (Notes_Manager.wnd != null)
        {
            notesNode.SetAttribute("managerScrollX", Notes_Manager.wnd.scrollPos.x.ToString());
            notesNode.SetAttribute("managerScrollY", Notes_Manager.wnd.scrollPos.y.ToString());
}
        if (notes != null) foreach (Note note in notes) notesNode.AppendChild(note.GetNode(doc));
        doc.AppendChild(notesNode);
        return doc.InnerXml;
    }

   /* private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        if ((typeOfFavoritesTab != null || typeOfHierarchy2 != null) && IsRenaming()) return;

        if (Notes_Manager.notes == null || Notes_Manager.notes.Any(n => n == null || n.gameObject == null)) Notes_Manager.UpdateNotes();

        Note note = Notes_Manager.notes.FirstOrDefault(n => n.instanceID == instanceID);
        if (note == null) return;

        if (note.icon == null && note.iconPath != "") Note_Editor.LoadIcon(note);

        Texture2D icon = note.icon;
        if (icon == null)
        {
            if (noDefaultIcon) return;
            if (Note.defaultIcon == null)
            {
                if ((Note.defaultIcon = GetIcon("NoteIcon.png")) == null)
                {
                    noDefaultIcon = true;
                    return;
                }
            }
            icon = Note.defaultIcon;
        }

        float x = selectionRect.xMax - iconSize - 5;

        if (typeOfFavoritesTab != null || typeOfHierarchy2 != null) x = GUI.skin.label.CalcSize(new GUIContent(note.gameObject.name)).x + 20;

        if (GUI.Button(new Rect(x, selectionRect.center.y - (iconSize / 2f),
                        iconSize, iconSize), icon, GUIStyle.none))
        {
            Selection.activeGameObject = note.gameObject;
            Notes_Manager.ShowNote(note);
        }
    }
*/
    private static void OnPlaymodeStateChanged()
    {
        if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            needLoad = true;
            nodeString = GetXMLString();
        }
        else if (needLoad)
        {
            needLoad = false;
            SetXMLString(nodeString);
            Notes_Manager.needUpdate = true;
            EditorApplication.RepaintHierarchyWindow();
        }
    }

    public static void SetXMLString(string str)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(str);
		
        XmlElement notesNode = doc.FirstChild as XmlElement;
        if (notesNode == null) return;

        if (Notes_Manager.wnd != null)
        {
            Single.TryParse(notesNode.GetAttribute("managerScrollX"), out Notes_Manager.wnd.scrollPos.x);
            Single.TryParse(notesNode.GetAttribute("managerScrollY"), out Notes_Manager.wnd.scrollPos.y);
        }

        foreach (XmlElement el in notesNode.ChildNodes)
        {
            int id = int.Parse(el.GetAttribute("id"));
            GameObject go = EditorUtility.InstanceIDToObject(id) as GameObject;
            if (go != null)
            {
                Note note = go.GetComponent<Note>() ?? go.AddComponent<Note>();
                note.SetNode(el);
            }
        }
    }
}