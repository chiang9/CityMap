using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    Dictionary<ulong, OsmNode> nodes;

    [Tooltip("The resouce file for OSM data")]
    public string resourceFile;

    void Start()
    {
        nodes = new Dictionary<ulong, OsmNode>();

        var txtAsset = Resources.Load<TextAsset>(resourceFile);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset.text);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        SetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));


    }

    private void GetWays(XmlNodeList xmlNodeList)
    {

    }

    private void SetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;

        }
    }

    private void SetBounds(XmlNode xmlNode)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
