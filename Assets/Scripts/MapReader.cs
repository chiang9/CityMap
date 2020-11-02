using System.Collections.Generic;
using System.Xml;
using UnityEngine;

class MapReader : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public OsmBound bounds;

    [HideInInspector]
    public List<OsmWay> ways;

    [Tooltip("The resouce file for OSM data")]
    public string resourceFile;

    public bool isReady { get; private set; }

    void Start()
    {
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();

        var txtAsset = Resources.Load<TextAsset>(resourceFile);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset.text);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        SetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));

        /*
        float minx = (float)MercatorProjection.lonToX(bounds.minlon);
        float maxx = (float)MercatorProjection.lonToX(bounds.maxlon);
        float miny = (float)MercatorProjection.latToY(bounds.minlat);
        float maxy = (float)MercatorProjection.latToY(bounds.maxlat);
        */

        isReady = true;

    }

    void Update()
    {
        foreach(OsmWay w in ways)
        {
            if (w.visible)
            {
                Color c = Color.cyan;
                if (!w.isBoundary) c = Color.red;

                for (int i = 1; i<w.nodeIDs.Count; i++)
                {
                    OsmNode p1 = nodes[w.nodeIDs[i - 1]];
                    OsmNode p2 = nodes[w.nodeIDs[i]];

                    Vector3 v1 = p1 - bounds.centre;
                    Vector3 v2 = p2 - bounds.centre;

                    Debug.DrawLine(v1, v2, c);
                }
            }
        }
    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach(XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
    }

    void SetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;

        }
    }

    void SetBounds(XmlNode xmlNode)
    {
        bounds = new OsmBound(xmlNode);
    }

}
