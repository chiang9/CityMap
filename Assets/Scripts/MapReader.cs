using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


class MapReader : MonoBehaviour
{

    public string maxlat;
    public string minlat;
    public string maxlon;
    public string minlon;

    public Terrain terrain;


    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public OsmBound bounds;

    [HideInInspector]
    public List<OsmWay> ways;

    [HideInInspector]
    public List<OsmRelation> relations;

    //[Tooltip("The resouce file for OSM data")]
    //public string resourceFile;

    public bool isReady { get; private set; }

    public OsmHttpRequest OsmHttpRequest = new OsmHttpRequest();

    void Start()
    {
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();

        XmlDocument doc = OsmHttpRequest.HttpGetOsm(maxlat, minlat, maxlon, minlon);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        SetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));
        GetRelation(doc.SelectNodes("/osm/relation"));

        isReady = true;

        init_setting();
        Debug.Log(terrain.terrainData.size);
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

    private void create_terrain()
    {
        GameObject TerrainObj = new GameObject("TerrainObj");

        TerrainData _TerrainData = new TerrainData();

        _TerrainData.size = new Vector3(10, 600, 10);
        _TerrainData.heightmapResolution = 512;
        _TerrainData.baseMapResolution = 1024;
        _TerrainData.SetDetailResolution(1024, 16);

        int _heightmapWidth = _TerrainData.heightmapWidth;
        int _heightmapHeight = _TerrainData.heightmapHeight;

        TerrainCollider _TerrainCollider = TerrainObj.AddComponent<TerrainCollider>();
        Terrain _Terrain2 = TerrainObj.AddComponent<Terrain>();

        _TerrainCollider.terrainData = _TerrainData;
        _Terrain2.terrainData = _TerrainData;
    }

    private void init_setting()
    {
        terrain.transform.position = new Vector3(0-bounds.size.x/2,(float)-0.01,0-bounds.size.z/2);
        terrain.terrainData.size = bounds.size;
        //create_terrain();
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

    private void GetRelation(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmRelation rel = new OsmRelation(node);
            ways.Add(rel);
        }
    }

}
