/*    INFINITY CODE 2013 - 2014     */
/*   http://www.infinity-code.com   */

using UnityEngine;
using System.Xml;

[AddComponentMenu("Infinity Code/Note")]
public class Note: MonoBehaviour 
{
    public static Texture2D defaultIcon;

    public bool expanded = true;
    public float height = 45;
    public Texture2D icon;
    public string iconPath = "";
    public bool isPrefab;
    public bool lockHeight;
    public float managerHeight = 45;
    public Vector2 managerScrollPos;
    public float maxHeight = 800;
    public Vector2 scrollPos;
    public string[] tags;
    public string tagsStr = "";
	public string text = "";
    public bool wordWrap;

    private int _instanceID = int.MinValue;

    public string title
    {
        get
        {
            string _name = gameObject.name;
            if (isPrefab) _name = "Prefab: " + _name;
            return _name;
        }
    }

    public int instanceID
    {
        get
        {
            if (_instanceID == int.MinValue) _instanceID = gameObject.GetInstanceID();
            return _instanceID;
        }
    }
	
	public XmlElement GetNode(XmlDocument doc)
	{
		XmlElement node = doc.CreateElement("Note");
		XmlCDataSection nodeText = doc.CreateCDataSection(text);
		node.SetAttribute("id", gameObject.GetInstanceID().ToString());
        node.SetAttribute("expanded", expanded.ToString());
        node.SetAttribute("height", height.ToString());
		node.SetAttribute("icon", iconPath);
	    node.SetAttribute("lockHeight", lockHeight.ToString());
        node.SetAttribute("managerHeight", managerHeight.ToString());
        node.SetAttribute("managerScrollPosX", managerScrollPos.x.ToString());
        node.SetAttribute("managerScrollPosY", managerScrollPos.y.ToString());
        node.SetAttribute("scrollPosX", scrollPos.x.ToString());
        node.SetAttribute("scrollPosY", scrollPos.y.ToString());
        node.SetAttribute("wordWrap", wordWrap.ToString());
        node.SetAttribute("tags", (tags != null)?string.Join(";", tags): "");
        node.AppendChild(nodeText);
		return node;
	}
	
	public void SetNode(XmlElement node)
	{
		text = node.InnerText;
		string _iconPath = node.GetAttribute("icon");
	    if (_iconPath != iconPath)
	    {
	        iconPath = _iconPath;
	        icon = null;
	    }
	    bool.TryParse(node.GetAttribute("expanded"), out expanded);
        float.TryParse(node.GetAttribute("height"), out height);
        bool.TryParse(node.GetAttribute("lockHeight"), out lockHeight);
        float.TryParse(node.GetAttribute("managerHeight"), out managerHeight);
        float.TryParse(node.GetAttribute("managerScrollPosX"), out scrollPos.x);
        float.TryParse(node.GetAttribute("managerScrollPosY"), out scrollPos.y);
        float.TryParse(node.GetAttribute("scrollPosX"), out scrollPos.x);
		float.TryParse(node.GetAttribute("scrollPosY"), out scrollPos.y);
        bool.TryParse(node.GetAttribute("wordWrap"), out wordWrap);
	    tags = node.GetAttribute("tags").Split(';');
	    tagsStr = string.Join("; ", tags);
	}
}