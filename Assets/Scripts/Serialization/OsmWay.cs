using System;
using System.Collections.Generic;
using System.Xml;
using TMPro;

class OsmWay : BaseNode
{
    public static float levelHightRatio = 3.0f;
    public static float heightRatio = 0.3048f;

    public ulong ID { get; private set; }

    public bool visible { get; private set; }

    public List<ulong> nodeIDs { get; private set; }

    public float Height { get; private set; }

    public bool isBoundary { get; private set; }

    public bool isBuilding { get; private set; }

    public bool isRoad { get; private set; }

    public string name { get; private set; }

    public OsmWay(XmlNode node)
    {
        nodeIDs = new List<ulong>();
        Height = 3.0f;

        ID = GetAttribute<ulong>("id", node.Attributes);
        visible = GetAttribute<bool>("visible", node.Attributes);

        XmlNodeList nds = node.SelectNodes("nd");
        foreach (XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            nodeIDs.Add(refNo);
        }

        if (nodeIDs.Count > 1)
        {
            isBoundary = nodeIDs[0] == nodeIDs[nodeIDs.Count - 1];
        }

        init_setting();
        tagClassification(node);
    }

    private void init_setting()
    {
        name = "Default Game Object";
    }

    private void tagClassification(XmlNode node)
    {
        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode tag in tags)
        {
            string key = GetAttribute<string>("k", tag.Attributes);
            if (key == "building:levels")
            {
                Height = levelHightRatio * GetAttribute<float>("v", tag.Attributes);
            }
            else if (key == "height")
            {
                Height = heightRatio * GetAttribute<float>("v", tag.Attributes);
            }
            else if (key == "building")
            {
                isBuilding = true;
            }
            else if (key == "highway")
            {
                isRoad = true;
            } 
            else if (key == "name")
            {
                name = GetAttribute<string>("v", tag.Attributes);
            }
        }
    }
}

